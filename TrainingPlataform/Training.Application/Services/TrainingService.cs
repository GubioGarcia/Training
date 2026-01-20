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
    }
}
