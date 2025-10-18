using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Template.CrossCutting.ExceptionHandler.Extensions;
using Training.Application.Interfaces;
using Training.Application.ViewModels.ExerciseViewModels;
using Training.Application.ViewModels.MuscleGroupViewModels;
using Training.Domain.Entities;
using Training.Domain.Interfaces;

namespace Training.Application.Services
{
    public class ExerciseService : IExerciseService
    {

        private readonly IClientProfessionalRepository clientProfessionalRepository;
        private readonly IUserServiceBase<Professional> userServiceBaseProfessional;
        private readonly IUserServiceBase<Client> userServiceBaseClient;
        private readonly IProfessionalRepository professionalRepository;
        private readonly IExerciseRepository exerciseRepository;
        private readonly IMapper mapper;

        public ExerciseService(IUserServiceBase<Professional> userServiceBaseProfessional, IExerciseRepository exerciseRepository, IClientProfessionalRepository clientProfessionalRepository,
                               IProfessionalRepository professionalRepository, IUserServiceBase<Client> userServiceBaseClient, IMapper mapper)
        {
            this.clientProfessionalRepository = clientProfessionalRepository;
            this.userServiceBaseProfessional = userServiceBaseProfessional;
            this.userServiceBaseClient = userServiceBaseClient;
            this.professionalRepository = professionalRepository;
            this.exerciseRepository = exerciseRepository;
            this.mapper = mapper;
        }

        public List<ExerciseViewModel> Get(string tokenId)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            // Recebe o tipo do usuário logado e instância profissional logado
            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            try
            {
                List<Exercise> _exercises;

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _exercises = [.. this.exerciseRepository.Query(x => !x.IsDeleted)];

                    if (_exercises.Count == 0)
                        throw new ApiException("Exercises not found", HttpStatusCode.NotFound);
                }
                else
                {
                    _exercises = [.. this.exerciseRepository.Query(x => x.ProfessionalId == _professionalLogged.Id && !x.IsDeleted)];

                    if (_exercises.Count == 0)
                        throw new ApiException("No exercises found for this professional", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<ExerciseViewModel>>(_exercises);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }

        }

        public ExerciseViewModel GetById(Guid id, string tokenId)
        {
            if (!Guid.TryParse(tokenId, out Guid tokenValidId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Recebe tipo do usuário logado
            string _userTypeLogged = userServiceBaseClient.LoggedInUserType(tokenId);
            if (string.IsNullOrEmpty(_userTypeLogged))
                _userTypeLogged = userServiceBaseProfessional.LoggedInUserType(tokenId);
            if (string.IsNullOrEmpty(_userTypeLogged))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            // Valida se usuário logado possui acesso liberado ao método
            if (_userTypeLogged != "Admin" && _userTypeLogged != "Client" && _userTypeLogged != "Professional")
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            Exercise _exercise = this.exerciseRepository.Find(x => x.Id == id && !x.IsDeleted)
                                           ?? throw new ApiException("Exercise not found", HttpStatusCode.NotFound);

            if (_userTypeLogged != "Admin")
            {
                // verifica se há professional vinculado no exercise
                if (_exercise.ProfessionalId != null && _userTypeLogged == "Professional")
                {
                    Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == tokenValidId && !x.IsDeleted)
                                             ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

                    if (_professionalLogged.Id != _exercise.ProfessionalId)
                        throw new ApiException("Muscle Group not found", HttpStatusCode.NotFound);
                }
                else if (_exercise.ProfessionalId != null && _userTypeLogged == "Client")
                {
                    // verifica se usuário possui relacionamento com o 'professional_id' declarado no exercise
                    ClientProfessional _clientProfessional = this.clientProfessionalRepository.Find(x => x.ProfessionalId == _exercise.ProfessionalId
                                                            && x.ClientId == tokenValidId && !x.IsDeleted) ?? throw new ApiException("Client not found", HttpStatusCode.NotFound);

                    if (_clientProfessional.ProfessionalId != _exercise.ProfessionalId)
                        throw new ApiException("Exercise not found", HttpStatusCode.NotFound);
                }
            }

            return mapper.Map<ExerciseViewModel>(_exercise);
        }

        public List<ExerciseViewModel> GetByName(string name, string tokenId)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            // Recebe o tipo do usuário logado e instância profissional logado
            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            try
            {
                List<Exercise> _exercises = [];

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _exercises = [.. this.exerciseRepository.Query(x => EF.Functions.Like(x.Name, $"%{name}%")
                                                                                       && !x.IsDeleted)];

                    if (_exercises.Count == 0)
                        throw new ApiException("No exercises with this name were found", HttpStatusCode.NotFound);
                }
                else
                {
                    _exercises = [.. this.exerciseRepository.Query(x => !x.IsDeleted && EF.Functions.Like(x.Name, $"%{name}%") &&
                                                                          (x.ProfessionalId == validId || x.ProfessionalId == null))];

                    if (_exercises.Count == 0)
                        throw new ApiException("No exercises with this name were found", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<ExerciseViewModel>>(_exercises);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public List<ExerciseViewModel> GetByProfessional(Guid id, string tokenId)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            // Recebe o tipo do usuário logado e instância profissional logado
            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            try
            {
                List<Exercise> _exercises = [];

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _exercises = [.. this.exerciseRepository.Query(x => x.ProfessionalId == id && !x.IsDeleted)];

                    if (_exercises.Count == 0)
                        throw new ApiException("No exercise found for this professional", HttpStatusCode.NotFound);
                }
                else
                {
                    _exercises = [.. this.exerciseRepository.Query(x => x.ProfessionalId == _professionalLogged.Id && !x.IsDeleted)];

                    if (_exercises.Count == 0)
                        throw new ApiException("No exercise found for this professional", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<ExerciseViewModel>>(_exercises);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public List<ExerciseViewModel> GetByMuscleGroup(Guid id, string tokenId)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            // Recebe o tipo do usuário logado e instância profissional logado
            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            try
            {
                List<Exercise> _exercises = [];

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _exercises = [.. this.exerciseRepository.Query(x => x.MuscleGroupId == id && !x.IsDeleted)];

                    if (_exercises.Count == 0)
                        throw new ApiException("No exercise found for this muscle group", HttpStatusCode.NotFound);
                }
                else
                {
                    _exercises = [.. this.exerciseRepository.Query(x => x.ProfessionalId == _professionalLogged.Id
                                                                   && x.MuscleGroupId == id && !x.IsDeleted)];

                    if (_exercises.Count == 0)
                        throw new ApiException("No exercise found for this muscle group", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<ExerciseViewModel>>(_exercises);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public List<ExerciseViewModel> GetByWorkoutCategory(Guid id, string tokenId)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            // Recebe o tipo do usuário logado e instância profissional logado
            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            try
            {
                List<Exercise> _exercises = [];

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _exercises = [.. this.exerciseRepository.Query(x => x.WorkoutCategoryId == id && !x.IsDeleted)];

                    if (_exercises.Count == 0)
                        throw new ApiException("No exercise found for this workout category", HttpStatusCode.NotFound);
                }
                else
                {
                    _exercises = [.. this.exerciseRepository.Query(x => x.ProfessionalId == _professionalLogged.Id
                                                                   && x.WorkoutCategoryId == id && !x.IsDeleted)];

                    if (_exercises.Count == 0)
                        throw new ApiException("No exercise found for this workout category", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<ExerciseViewModel>>(_exercises);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public ExerciseViewModel Post(string tokenId, ExerciseRequestViewModel exerciseRequestViewModel)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);
            if (exerciseRequestViewModel == null)
                throw new ApiException("Exercise is required", HttpStatusCode.BadRequest);
            if (string.IsNullOrWhiteSpace(exerciseRequestViewModel.Name))
                throw new ApiException("Name is required", HttpStatusCode.BadRequest);
            if (string.IsNullOrWhiteSpace(exerciseRequestViewModel.Description))
                throw new ApiException("Description is required", HttpStatusCode.BadRequest);

            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);
            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);

            // Verifica se já existe registro (global ou associado a um profissional)
            Exercise _exerciseExisting = this.exerciseRepository.Find(x => !x.IsDeleted &&
                                                                      x.Name.ToLower() == exerciseRequestViewModel.Name.ToLower());

            if (_exerciseExisting != null)
            {
                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase) && _exerciseExisting.ProfessionalId == null)
                {
                    throw new ApiException("Exercise already exists");
                }
                else if (_exerciseExisting.ProfessionalId == null || _exerciseExisting.ProfessionalId == validId)
                {
                    throw new ApiException("Exercise already exists");
                }
            }

            // Criar o registro Exercise
            Exercise _exercise = new()
            {
                WorkoutCategoryId = exerciseRequestViewModel.WorkoutCategoryId,
                MuscleGroupId = exerciseRequestViewModel.MuscleGroupId,
                Name = exerciseRequestViewModel.Name,
                Description = exerciseRequestViewModel.Description,
                UrlMedia = exerciseRequestViewModel.UrlMedia,
                ProfessionalId = loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase) ? null : validId,
                DateUpdated = DateTime.UtcNow,
                IsDeleted = false
            };

            try
            {
                exerciseRepository.Create(_exercise);
                return mapper.Map<ExerciseViewModel>(_exercise);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public ExerciseViewModel Put(string tokenId, ExerciseUpdateViewModel exerciseUpdateViewModel)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);
            if (exerciseUpdateViewModel == null)
                throw new ApiException("Exercise is required", HttpStatusCode.BadRequest);

            // Verifica se o usuário logado é Admin ou Professional
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.Forbidden);

            // Busca o Professional logado e verifica se ele existe e não está deletado
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            // Busca o exercício e garante que ele não esteja deletado
            Exercise _exercise = this.exerciseRepository.Find(x => x.Id == exerciseUpdateViewModel.Id && !x.IsDeleted)
                ?? throw new ApiException("Exercise not found", HttpStatusCode.BadRequest);

            // Obtém o tipo do usuário logado
            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);

            if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Admin pode modificar se professionalId for null ou pertencer ao mesmo usuário
                if (_exercise.ProfessionalId == null || _exercise.ProfessionalId == validId)
                {
                    _exercise.WorkoutCategory = exerciseUpdateViewModel.WorkoutCategoryId != Guid.Empty ? _exercise.WorkoutCategory : _exercise.WorkoutCategory;
                    _exercise.MuscleGroupId = exerciseUpdateViewModel.MuscleGroupId != Guid.Empty ? _exercise.MuscleGroupId : _exercise.MuscleGroupId;
                    _exercise.Name = exerciseUpdateViewModel.Name ?? _exercise.Name;
                    _exercise.Description = exerciseUpdateViewModel.Description ?? _exercise.Description;
                    _exercise.UrlMedia = exerciseUpdateViewModel.UrlMedia ?? _exercise.UrlMedia;
                    _exercise.DateUpdated = DateTime.UtcNow;

                    this.exerciseRepository.Update(_exercise);
                }
                else
                {
                    throw new ApiException("You are not authorized to modify this record", HttpStatusCode.Forbidden);
                }
            }
            else if (loggedInUserType.Equals("Professional", StringComparison.OrdinalIgnoreCase))
            {
                // Professional só pode modificar se for o dono do registro
                if (_exercise.ProfessionalId == validId)
                {
                    _exercise.WorkoutCategory = exerciseUpdateViewModel.WorkoutCategoryId != Guid.Empty ? _exercise.WorkoutCategory : _exercise.WorkoutCategory;
                    _exercise.MuscleGroupId = exerciseUpdateViewModel.MuscleGroupId != Guid.Empty ? _exercise.MuscleGroupId : _exercise.MuscleGroupId;
                    _exercise.Name = exerciseUpdateViewModel.Name ?? _exercise.Name;
                    _exercise.Description = exerciseUpdateViewModel.Description ?? _exercise.Description;
                    _exercise.UrlMedia = exerciseUpdateViewModel.UrlMedia ?? _exercise.UrlMedia;
                    _exercise.DateUpdated = DateTime.UtcNow;

                    this.exerciseRepository.Update(_exercise);
                }
                else
                {
                    throw new ApiException("You are not authorized to modify this record", HttpStatusCode.Forbidden);
                }
            }

            return mapper.Map<ExerciseViewModel>(_exercise);
        }

        public void Delete(string tokenId, Guid id)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Verifica se o usuário logado é Admin ou Professional
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.Forbidden);

            // Busca o Professional logado e verifica se ele existe e não está deletado
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            // Busca o exercício e garante que ele não esteja deletado
            Exercise _exercise = this.exerciseRepository.Find(x => x.Id == id)
                ?? throw new ApiException("Exercise not found", HttpStatusCode.BadRequest);
            if (_exercise.IsDeleted == true)
                throw new ApiException("Exercise already deleted", HttpStatusCode.BadRequest);

            // Obtém o tipo do usuário logado
            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);

            if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Admin pode deletar se professionalId for null ou pertencer ao mesmo usuário
                if (_exercise.ProfessionalId == null || _exercise.ProfessionalId == validId)
                    this.exerciseRepository.IsDeleted(_exercise);
                else
                    throw new ApiException("You are not authorized to modify this record", HttpStatusCode.Forbidden);
            }
            else if (loggedInUserType.Equals("Professional", StringComparison.OrdinalIgnoreCase))
            {
                // Professional só pode deletar se for o dono do registro
                if (_exercise.ProfessionalId == validId)
                    this.exerciseRepository.IsDeleted(_exercise);
                else
                    throw new ApiException("You are not authorized to modify this record", HttpStatusCode.Forbidden);
            }
        }
    }
}
