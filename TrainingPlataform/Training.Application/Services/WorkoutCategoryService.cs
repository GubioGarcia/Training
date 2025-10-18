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
using Training.Application.ViewModels.WorkoutCategoryViewModels;
using Training.Domain.Entities;
using Training.Domain.Interfaces;

namespace Training.Application.Services
{
    public class WorkoutCategoryService : IWorkoutCategoryService
    {
        private readonly IUserServiceBase<Professional> userServiceBaseProfessional;
        private readonly IUserServiceBase<Client> userServiceBaseClient;
        private readonly IWorkoutCategoryRepository workoutCategoryRepository;
        private readonly IProfessionalRepository professionalRepository;
        private readonly IClientProfessionalRepository clientProfessionalRepository;
        private readonly IMapper mapper;

        public WorkoutCategoryService(IUserServiceBase<Professional> userServiceBaseProfessional, IUserServiceBase<Client> userServiceBaseClient,
                                  IWorkoutCategoryRepository workoutCategoryRepository, IMapper mapper, IProfessionalRepository professionalRepository,
                                  IClientProfessionalRepository clientProfessionalRepository)
        {
            this.userServiceBaseProfessional = userServiceBaseProfessional;
            this.userServiceBaseClient = userServiceBaseClient;
            this.professionalRepository = professionalRepository;
            this.clientProfessionalRepository = clientProfessionalRepository;
            this.workoutCategoryRepository = workoutCategoryRepository;
            this.mapper = mapper;
        }

        public List<WorkoutCategoryViewModel> Get(string tokenId)
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
                List<WorkoutCategory> _workoutCategory;

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _workoutCategory = [.. this.workoutCategoryRepository.Query(x => !x.IsDeleted)];

                    if (_workoutCategory.Count == 0)
                        throw new ApiException("No workout category found", HttpStatusCode.NotFound);
                }
                else
                {
                    _workoutCategory = [.. this.workoutCategoryRepository.Query(x => x.ProfessionalId == _professionalLogged.Id && !x.IsDeleted)];

                    if (_workoutCategory.Count == 0)
                        throw new ApiException("No workout category found for this professional", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<WorkoutCategoryViewModel>>(_workoutCategory);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public WorkoutCategoryViewModel GetById(Guid id, string tokenId)
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

            WorkoutCategory _workoutCategory = this.workoutCategoryRepository.Find(x => x.Id == id && !x.IsDeleted)
                                           ?? throw new ApiException("Workout Category not found", HttpStatusCode.NotFound);

            if (_userTypeLogged != "Admin") // deverá ser usuário 'Professional' ou 'Client'
            {
                // verifica se há professional vinculado no muscleGroup
                if (_workoutCategory.ProfessionalId != null && _userTypeLogged == "Professional")
                {
                    Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == tokenValidId && !x.IsDeleted)
                                             ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

                    if (_professionalLogged.Id != _workoutCategory.ProfessionalId)
                        throw new ApiException("Workout Category not found", HttpStatusCode.NotFound);
                }
                else if (_workoutCategory.ProfessionalId != null && _userTypeLogged == "Client")
                {
                    // verifica se usuário possui relacionamento com o 'professional_id' declarado no musclegroup
                    ClientProfessional _clientProfessional = this.clientProfessionalRepository.Find(x => x.ProfessionalId == _workoutCategory.ProfessionalId
                                                            && x.ClientId == tokenValidId && !x.IsDeleted) ?? throw new ApiException("Client not found", HttpStatusCode.NotFound);

                    if (_clientProfessional.ProfessionalId != _workoutCategory.ProfessionalId)
                        throw new ApiException("Workout Category not found", HttpStatusCode.NotFound);
                }
            }

            return mapper.Map<WorkoutCategoryViewModel>(_workoutCategory);
        }

        public List<WorkoutCategoryViewModel> GetByName(string name, string tokenId)
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
                List<WorkoutCategory> _workoutCategory = [];

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _workoutCategory = [.. this.workoutCategoryRepository.Query(x => EF.Functions.Like(x.Name, $"%{name}%")
                                                                                       && !x.IsDeleted)];

                    if (_workoutCategory.Count == 0)
                        throw new ApiException("No Workout Category with this name were found", HttpStatusCode.NotFound);
                }
                else
                {
                    _workoutCategory = [.. this.workoutCategoryRepository.Query(x => !x.IsDeleted && EF.Functions.Like(x.Name, $"%{name}%") &&
                                                                          (x.ProfessionalId == validId || x.ProfessionalId == null))];

                    if (_workoutCategory.Count == 0)
                        throw new ApiException("No Workout Category with this name were found", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<WorkoutCategoryViewModel>>(_workoutCategory);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public List<WorkoutCategoryViewModel> GetByProfessional(Guid id, string tokenId)
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
                List<WorkoutCategory> _workoutCategory = [];

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _workoutCategory = [.. this.workoutCategoryRepository.Query(x => x.ProfessionalId == id && !x.IsDeleted)];

                    if (_workoutCategory.Count == 0)
                        throw new ApiException("No Workout Category found for this professional", HttpStatusCode.NotFound);
                }
                else
                {
                    _workoutCategory = [.. this.workoutCategoryRepository.Query(x => x.ProfessionalId == _professionalLogged.Id && !x.IsDeleted)];

                    if (_workoutCategory.Count == 0)
                        throw new ApiException("No Workout Category found for this professional", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<WorkoutCategoryViewModel>>(_workoutCategory);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public WorkoutCategoryViewModel Post(string tokenId, WorkoutCategoryRequestViewModel workoutCategoryRequestViewModel)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);
            if (workoutCategoryRequestViewModel == null)
                throw new ApiException("Workout Category is required", HttpStatusCode.BadRequest);
            if (string.IsNullOrWhiteSpace(workoutCategoryRequestViewModel.Name))
                throw new ApiException("Name is required", HttpStatusCode.BadRequest);
            if (string.IsNullOrWhiteSpace(workoutCategoryRequestViewModel.Description))
                throw new ApiException("Description is required", HttpStatusCode.BadRequest);

            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            // Verifica se a categoria já existe (global ou associada a um profissional)
            WorkoutCategory _workoutCategoryExisting = this.workoutCategoryRepository.Find(x => !x.IsDeleted &&
                                                            x.Name.ToLower() == workoutCategoryRequestViewModel.Name.ToLower());

            if (_workoutCategoryExisting != null)
            {
                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase) && _workoutCategoryExisting.ProfessionalId == null)
                {
                    throw new ApiException("Workout Category already exists");
                }
                else if (_workoutCategoryExisting.ProfessionalId == null || _workoutCategoryExisting.ProfessionalId == validId)
                {
                    throw new ApiException("Workout Category already exists");
                }
            }

            // Criar nova categoria
            WorkoutCategory _workoutCategory = new()
            {
                Name = workoutCategoryRequestViewModel.Name,
                Description = workoutCategoryRequestViewModel.Description,
                ProfessionalId = loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase) ? null : validId,
                DateUpdated = DateTime.UtcNow,
                IsDeleted = false
            };

            try
            {
                workoutCategoryRepository.Create(_workoutCategory);
                return mapper.Map<WorkoutCategoryViewModel>(_workoutCategory);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public WorkoutCategoryViewModel Put(string tokenId, WorkoutCategoryUpdateViewModel workoutCategoryUpdateViewModel)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);
            if (workoutCategoryUpdateViewModel == null)
                throw new ApiException("Workout Category is required", HttpStatusCode.BadRequest);
            if (string.IsNullOrWhiteSpace(workoutCategoryUpdateViewModel.Name))
                throw new ApiException("Name is required", HttpStatusCode.BadRequest);
            if (string.IsNullOrWhiteSpace(workoutCategoryUpdateViewModel.Description))
                throw new ApiException("Description is required", HttpStatusCode.BadRequest);

            // Verifica se o usuário logado é Admin ou Professional
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            // Busca o Professional logado, verifica se ele existe e não está deletado
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            // Busca o WorkoutCategory a ser atualizado, verifica se ele existe e não está deletado
            WorkoutCategory _workoutCategory = this.workoutCategoryRepository.Find(x => x.Id == workoutCategoryUpdateViewModel.Id && !x.IsDeleted)
                                                ?? throw new ApiException("Workout Category not found", HttpStatusCode.NotFound);
            
            // Verifica se o usuário logado tem permissão para atualizar a categoria
            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);

            if (!loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Admin pode modificar se professionalId for null ou pertencer ao mesmo usuário
                if (_workoutCategory.ProfessionalId == null || _workoutCategory.ProfessionalId != validId)
                {
                    _workoutCategory.Name = workoutCategoryUpdateViewModel.Name;
                    _workoutCategory.Description = workoutCategoryUpdateViewModel.Description;
                    _workoutCategory.DateUpdated = DateTime.UtcNow;

                    this.workoutCategoryRepository.Update(_workoutCategory);
                }
                else
                {
                    throw new ApiException("You are not authorized to update this workout category", HttpStatusCode.BadRequest);
                }
            }
            else if (loggedInUserType.Equals("Professional", StringComparison.OrdinalIgnoreCase))
            {
                // Professional só pode modificar se professionalId pertencer ao mesmo usuário
                if (_workoutCategory.ProfessionalId == validId)
                {
                    _workoutCategory.Name = workoutCategoryUpdateViewModel.Name;
                    _workoutCategory.Description = workoutCategoryUpdateViewModel.Description;
                    _workoutCategory.DateUpdated = DateTime.UtcNow;

                    this.workoutCategoryRepository.Update(_workoutCategory);
                }
                else
                {
                    throw new ApiException("You are not authorized to update this workout category", HttpStatusCode.BadRequest);
                }
            }

            return mapper.Map<WorkoutCategoryViewModel>(_workoutCategory);
        }

        public void Delete(string tokenId, Guid id)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Verifica se o usuário logado é Admin ou Professional
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            // Busca o Professional logado, verifica se ele existe e não está deletado
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            // Busca o WorkoutCategory a ser deletado, garante que não está deletado
            WorkoutCategory _workoutCategory = this.workoutCategoryRepository.Find(x => x.Id == id)
                                                ?? throw new ApiException("Workout Category not found", HttpStatusCode.NotFound);
            if (_workoutCategory.IsDeleted)
                throw new ApiException("Workout Category not found", HttpStatusCode.NotFound);

            // Obtém o tipo do usuário logado
            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);

            if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Admin pode deletar se professionalId for null ou pertencer ao mesmo usuário
                if (_workoutCategory.ProfessionalId == null || _workoutCategory.ProfessionalId == validId)
                    this.workoutCategoryRepository.IsDeleted(_workoutCategory);
                else
                    throw new ApiException("You are not authorized to delete this workout category", HttpStatusCode.BadRequest);
            }
            else if (loggedInUserType.Equals("Professional", StringComparison.OrdinalIgnoreCase))
            {
                // Professional só pode deletar se professionalId pertencer ao mesmo usuário
                if (_workoutCategory.ProfessionalId == validId)
                    this.workoutCategoryRepository.IsDeleted(_workoutCategory);
                else
                    throw new ApiException("You are not authorized to delete this workout category", HttpStatusCode.BadRequest);
            }
        }
    }
}
