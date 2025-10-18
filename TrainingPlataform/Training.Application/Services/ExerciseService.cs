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
using Training.Application.ViewModels;
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
    }
}
