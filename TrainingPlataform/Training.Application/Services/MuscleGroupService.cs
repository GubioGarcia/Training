using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Template.CrossCutting.ExceptionHandler.Extensions;
using Training.Application.Interfaces;
using Training.Application.ViewModels;
using Training.Application.ViewModels.ClientViewModels;
using Training.Application.ViewModels.ProfessionalViewModels;
using Training.Domain.Entities;
using Training.Domain.Interfaces;

namespace Training.Application.Services
{
    public class MuscleGroupService : IMuscleGroupService
    {
        private readonly IUserServiceBase<Professional> userServiceBaseProfessional;
        private readonly IUserServiceBase<Client> userServiceBaseClient;
        private readonly IMuscleGroupRepository muscleGroupRepository;
        private readonly IProfessionalRepository professionalRepository;
        private readonly IClientProfessionalRepository clientProfessionalRepository;
        private readonly IMapper mapper;

        public MuscleGroupService(IUserServiceBase<Professional> userServiceBaseProfessional, IUserServiceBase<Client> userServiceBaseClient,
                                  IMuscleGroupRepository muscleGroupRepository, IMapper mapper, IProfessionalRepository professionalRepository,
                                  IClientProfessionalRepository clientProfessionalRepository)
        {
            this.userServiceBaseProfessional = userServiceBaseProfessional;
            this.userServiceBaseClient = userServiceBaseClient;
            this.professionalRepository = professionalRepository;
            this.clientProfessionalRepository = clientProfessionalRepository;
            this.muscleGroupRepository = muscleGroupRepository;
            this.mapper = mapper;
        }

        public List<MuscleGroupViewModel> Get(string tokenId)
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
                List<MuscleGroup> _muscleGroups;

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _muscleGroups = [.. this.muscleGroupRepository.Query(x => !x.IsDeleted)];

                    if (_muscleGroups.Count == 0)
                        throw new ApiException("No muscle groups found", HttpStatusCode.NotFound);
                }
                else
                {
                    _muscleGroups = [.. this.muscleGroupRepository.Query(x => x.ProfessionalId == _professionalLogged.Id && !x.IsDeleted)];

                    if (_muscleGroups.Count == 0)
                        throw new ApiException("No muscle groups found for this professional", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<MuscleGroupViewModel>>(_muscleGroups);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public MuscleGroupViewModel GetById(Guid id, string tokenId)
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

            MuscleGroup _muscleGroup = this.muscleGroupRepository.Find(x => x.Id == id && !x.IsDeleted)
                                           ?? throw new ApiException("Muscle Group not found", HttpStatusCode.NotFound);

            if (_userTypeLogged != "Admin") // deverá ser usuário 'Professional' ou 'Client'
            {
                // verifica se há professional vinculado no muscleGroup
                if (_muscleGroup.ProfessionalId != null && _userTypeLogged == "Professional")
                {
                    Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == tokenValidId && !x.IsDeleted)
                                             ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

                    if (_professionalLogged.Id != _muscleGroup.ProfessionalId)
                        throw new ApiException("Muscle Group not found", HttpStatusCode.NotFound);
                }
                else if (_muscleGroup.ProfessionalId != null && _userTypeLogged == "Client")
                {
                    // verifica se usuário possui relacionamento com o 'professional_id' declarado no musclegroup
                    ClientProfessional _clientProfessional = this.clientProfessionalRepository.Find(x => x.ProfessionalId == _muscleGroup.ProfessionalId
                                                            && x.ClientId == tokenValidId && !x.IsDeleted) ?? throw new ApiException("Client not found", HttpStatusCode.NotFound);

                    if (_clientProfessional.ProfessionalId != _muscleGroup.ProfessionalId)
                        throw new ApiException("Muscle Group not found", HttpStatusCode.NotFound);
                }
            }

            return mapper.Map<MuscleGroupViewModel>(_muscleGroup);
        }

        public List<MuscleGroupViewModel> GetByName(string name, string tokenId)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin","Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            // Recebe o tipo do usuário logado e instância profissional logado
            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            try
            {
                List<MuscleGroup> _muscleGroups = [];

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _muscleGroups = [.. this.muscleGroupRepository.Query(x => EF.Functions.Like(x.Name, $"%{name}")
                                                                                       && !x.IsDeleted)];

                    if (_muscleGroups.Count == 0)
                        throw new ApiException("No muscle groups with this name were found", HttpStatusCode.NotFound);
                }
                else
                {
                    _muscleGroups = [.. this.muscleGroupRepository.Query(x => !x.IsDeleted && EF.Functions.Like(x.Name, $"%{name}%") &&
                                                                          (x.ProfessionalId == validId || x.ProfessionalId == null))];
                    
                    if (_muscleGroups.Count == 0)
                        throw new ApiException("No muscle groups with this name were found", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<MuscleGroupViewModel>>(_muscleGroups);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public List<MuscleGroupViewModel> GetByProfessional(Guid id, string tokenId)
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
                List<MuscleGroup> _muscleGroups = [];

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _muscleGroups = [.. this.muscleGroupRepository.Query(x => x.ProfessionalId == id && !x.IsDeleted)];

                    if (_muscleGroups.Count == 0)
                        throw new ApiException("No muscle groups found for this professional", HttpStatusCode.NotFound);
                }
                else
                {
                    _muscleGroups = [.. this.muscleGroupRepository.Query(x => x.ProfessionalId == _professionalLogged.Id && !x.IsDeleted)];

                    if (_muscleGroups.Count == 0)
                        throw new ApiException("No muscle groups found for this professional", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<MuscleGroupViewModel>>(_muscleGroups);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
