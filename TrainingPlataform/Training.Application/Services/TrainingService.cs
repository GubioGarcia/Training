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
using Training.Application.ViewModels.TrainingViewModels;
using Training.Application.ViewModels.WorkoutCategoryViewModels;
using Training.Domain.Entities;
using Training.Domain.Interfaces;

namespace Training.Application.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly IUserServiceBase<Professional> userServiceBaseProfessional;
        private readonly IUserServiceBase<Client> userServiceBaseClient;
        private readonly IClientProfessionalRepository clientProfessionalRepository;
        private readonly ITrainingRepository trainingRepository;
        private readonly IProfessionalRepository professionalRepository;
        private readonly IMapper mapper;

        public TrainingService(IUserServiceBase<Professional> userServiceBaseProfessional, IUserServiceBase<Client> userServiceBaseClient, IMapper mapper,
                               IProfessionalRepository professionalRepository, ITrainingRepository trainingRepository, IClientProfessionalRepository clientProfessionalRepository)
        {
            this.userServiceBaseProfessional = userServiceBaseProfessional;
            this.userServiceBaseClient = userServiceBaseClient;
            this.trainingRepository = trainingRepository;
            this.professionalRepository = professionalRepository;
            this.clientProfessionalRepository = clientProfessionalRepository;
            this.mapper = mapper;
        }

        public List<TrainingViewModel> Get(string tokenId)
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
                List<Training.Domain.Entities.Training> _training;

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _training = [.. this.trainingRepository.Query(x => !x.IsDeleted)];

                    if (_training.Count == 0)
                        throw new ApiException("No training found", HttpStatusCode.NotFound);
                }
                else
                {
                    _training = [.. this.trainingRepository.Query(x => x.ProfessionalId == _professionalLogged.Id && !x.IsDeleted)];

                    if (_training.Count == 0)
                        throw new ApiException("No training found for this professional", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<TrainingViewModel>>(_training);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public TrainingViewModel GetById(Guid id, string tokenId)
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
            Training.Domain.Entities.Training _training = this.trainingRepository.Find(x => x.Id == id && !x.IsDeleted)
                                           ?? throw new ApiException("Training not found", HttpStatusCode.NotFound);

            if (_userTypeLogged != "Admin") // deverá ser usuário 'Professional' ou 'Client'
            {
                // verifica se há professional vinculado no treino
                if (_training.ProfessionalId != null && _userTypeLogged == "Professional")
                {
                    Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == tokenValidId && !x.IsDeleted)
                                             ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

                    if (_professionalLogged.Id != _training.ProfessionalId)
                        throw new ApiException("Training not found", HttpStatusCode.NotFound);
                }
                else if (_training.ProfessionalId != null && _userTypeLogged == "Client")
                {
                    // verifica se usuário possui relacionamento com o 'professional_id' declarado no treino
                    ClientProfessional _clientProfessional = this.clientProfessionalRepository.Find(x => x.ProfessionalId == _training.ProfessionalId
                                                            && x.ClientId == tokenValidId && !x.IsDeleted) ?? throw new ApiException("Client not found", HttpStatusCode.NotFound);

                    if (_clientProfessional.ProfessionalId != _training.ProfessionalId)
                        throw new ApiException("Training not found", HttpStatusCode.NotFound);
                }
            }

            return mapper.Map<TrainingViewModel>(_training);
        }

        public List<TrainingViewModel> GetByName(string name, string tokenId)
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
                List<Training.Domain.Entities.Training> _training = [];

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _training = [.. this.trainingRepository.Query(x => EF.Functions.Like(x.Name, $"%{name}%")
                                                                                       && !x.IsDeleted)];

                    if (_training.Count == 0)
                        throw new ApiException("No Training with this name were found", HttpStatusCode.NotFound);
                }
                else
                {
                    _training = [.. this.trainingRepository.Query(x => !x.IsDeleted && EF.Functions.Like(x.Name, $"%{name}%") &&
                                                                          (x.ProfessionalId == validId || x.ProfessionalId == null))];

                    if (_training.Count == 0)
                        throw new ApiException("No Training with this name were found", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<TrainingViewModel>>(_training);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public List<TrainingViewModel> GetByProfessional(Guid id, string tokenId)
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
                List<Domain.Entities.Training> training = [];

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    training = [.. this.trainingRepository.Query(x => x.ProfessionalId == id && !x.IsDeleted)];

                    if (training.Count == 0)
                        throw new ApiException("No Training found for this professional", HttpStatusCode.NotFound);
                }
                else
                {
                    training = [.. this.trainingRepository.Query(x => x.ProfessionalId == _professionalLogged.Id && !x.IsDeleted)];

                    if (training.Count == 0)
                        throw new ApiException("No Training found for this professional", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<TrainingViewModel>>(training);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public TrainingViewModel Post(string tokenId, TrainingRequestViewModel trainingRequestViewModel)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);
            if (trainingRequestViewModel == null)
                throw new ApiException("Training is required", HttpStatusCode.BadRequest);
            if (string.IsNullOrWhiteSpace(trainingRequestViewModel.Name))
                throw new ApiException("Name is required", HttpStatusCode.BadRequest);
            if (string.IsNullOrWhiteSpace(trainingRequestViewModel.Description))
                throw new ApiException("Description is required", HttpStatusCode.BadRequest);

            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            // Verifica se o treino já existe (global ou associado a um profissional)
            Training.Domain.Entities.Training _trainingExisting = this.trainingRepository.Find(x => !x.IsDeleted &&
                                                            x.Name.ToLower() == trainingRequestViewModel.Name.ToLower());

            if (_trainingExisting != null)
            {
                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase) && _trainingExisting.ProfessionalId == null)
                {
                    throw new ApiException("Training already exists");
                }
                else if (_trainingExisting.ProfessionalId == null || _trainingExisting.ProfessionalId == validId)
                {
                    throw new ApiException("Training already exists");
                }
            }

            // Criar novo treino
            Training.Domain.Entities.Training _training = new()
            {
                Name = trainingRequestViewModel.Name,
                Description = trainingRequestViewModel.Description,
                ProfessionalId = loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase) ? null : validId,
                DateUpdated = DateTime.UtcNow,
                IsDeleted = false
            };

            try
            {
                trainingRepository.Create(_training);
                return mapper.Map<TrainingViewModel>(_training);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public TrainingViewModel Put(string tokenId, TrainingUpdateViewModel trainingUpdateViewModel)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);
            if (trainingUpdateViewModel == null)
                throw new ApiException("Training is required", HttpStatusCode.BadRequest);
            if (string.IsNullOrWhiteSpace(trainingUpdateViewModel.Name))
                throw new ApiException("Name is required", HttpStatusCode.BadRequest);
            if (string.IsNullOrWhiteSpace(trainingUpdateViewModel.Description))
                throw new ApiException("Description is required", HttpStatusCode.BadRequest);

            // Verifica se o usuário logado é Admin ou Professional
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            // Busca o Professional logado, verifica se ele existe e não está deletado
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            // Busca o treino a ser atualizado, verifica se ele existe e não está deletado
            Training.Domain.Entities.Training _training = this.trainingRepository.Find(x => x.Id == trainingUpdateViewModel.Id && !x.IsDeleted)
                                                ?? throw new ApiException("Training not found", HttpStatusCode.NotFound);

            // Verifica se o usuário logado tem permissão para atualizar o treino
            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);

            if (!loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Admin pode modificar se professionalId for null ou pertencer ao mesmo usuário
                if (_training.ProfessionalId == null || _training.ProfessionalId != validId)
                {
                    _training.Name = trainingUpdateViewModel.Name;
                    _training.Description = trainingUpdateViewModel.Description;
                    _training.DateUpdated = DateTime.UtcNow;

                    this.trainingRepository.Update(_training);
                }
                else
                {
                    throw new ApiException("You are not authorized to update this training", HttpStatusCode.BadRequest);
                }
            }
            else if (loggedInUserType.Equals("Professional", StringComparison.OrdinalIgnoreCase))
            {
                // Professional só pode modificar se professionalId pertencer ao mesmo usuário
                if (_training.ProfessionalId == validId)
                {
                    _training.Name = trainingUpdateViewModel.Name;
                    _training.Description = trainingUpdateViewModel.Description;
                    _training.DateUpdated = DateTime.UtcNow;

                    this.trainingRepository.Update(_training);
                }
                else
                {
                    throw new ApiException("You are not authorized to update this training", HttpStatusCode.BadRequest);
                }
            }

            return mapper.Map<TrainingViewModel>(_training);
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

            // Busca o treino a ser deletado, garante que não está deletado
            Training.Domain.Entities.Training _training = this.trainingRepository.Find(x => x.Id == id)
                                                ?? throw new ApiException("Training not found", HttpStatusCode.NotFound);
            if (_training.IsDeleted)
                throw new ApiException("Training not found", HttpStatusCode.NotFound);

            // Obtém o tipo do usuário logado
            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);

            if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Admin pode deletar se professionalId for null ou pertencer ao mesmo usuário
                if (_training.ProfessionalId == null || _training.ProfessionalId == validId)
                    this.trainingRepository.IsDeleted(_training);
                else
                    throw new ApiException("You are not authorized to delete this training", HttpStatusCode.BadRequest);
            }
            else if (loggedInUserType.Equals("Professional", StringComparison.OrdinalIgnoreCase))
            {
                // Professional só pode deletar se professionalId pertencer ao mesmo usuário
                if (_training.ProfessionalId == validId)
                    this.trainingRepository.IsDeleted(_training);
                else
                    throw new ApiException("You are not authorized to delete this training", HttpStatusCode.BadRequest);
            }
        }
    }
}
