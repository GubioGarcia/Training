using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Template.CrossCutting.ExceptionHandler.Extensions;
using Training.Application.Interfaces;
using Training.Application.ViewModels.PeriodizationViewModels;
using Training.Application.ViewModels.TrainingViewModels;
using Training.Domain.Entities;
using Training.Domain.Interfaces;

namespace Training.Application.Services
{
    public class PeriodizationService : IPeriodizationService
    {

        private readonly IUserServiceBase<Professional> userServiceBaseProfessional;
        private readonly IClientProfessionalRepository clientProfessionalRepository;
        private readonly IUserServiceBase<Client> userServiceBaseClient;
        private readonly IPeriodizationRepository periodizationRepository;
        private readonly IProfessionalRepository professionalRepository;
        private readonly IMapper mapper;

        public PeriodizationService(IUserServiceBase<Professional> userServiceBaseProfessional, IUserServiceBase<Client> userServiceBaseClient, IClientProfessionalRepository clientProfessionalRepository,
                                     IMapper mapper, IPeriodizationRepository periodizationRepository, IProfessionalRepository professionalRepository)
        {
            this.clientProfessionalRepository = clientProfessionalRepository;
            this.userServiceBaseProfessional = userServiceBaseProfessional;
            this.userServiceBaseClient = userServiceBaseClient;
            this.periodizationRepository = periodizationRepository;
            this.professionalRepository = professionalRepository;
            this.mapper = mapper;
        }

        public List<PeriodizationViewModel> Get(string tokenId)
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
                List<Periodization> _periodizations;

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _periodizations = [.. this.periodizationRepository.Query(x => !x.IsDeleted)];

                    if (_periodizations.Count == 0)
                        throw new ApiException("Periodizations not found", HttpStatusCode.NotFound);
                }
                else
                {
                    _periodizations = [.. this.periodizationRepository.Query(x => x.ProfessionalId == _professionalLogged.Id && !x.IsDeleted)];

                    if (_periodizations.Count == 0)
                        throw new ApiException("No periodizations found for this professional", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<PeriodizationViewModel>>(_periodizations);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public PeriodizationViewModel GetById(string tokenId, Guid id)
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

            Periodization _periodization = this.periodizationRepository.Find(x => x.Id == id && !x.IsDeleted)
                                         ?? throw new ApiException("Periodization not found", HttpStatusCode.NotFound);

            if (_userTypeLogged != "Admin")
            {
                if (_periodization.ProfessionalId != null && _userTypeLogged == "Professional")
                {
                    Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == tokenValidId && !x.IsDeleted)
                                             ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

                    if (_professionalLogged.Id != _periodization.ProfessionalId)
                        throw new ApiException("Periodization not found", HttpStatusCode.NotFound);
                }
                else if (_periodization.ProfessionalId != null && _userTypeLogged == "Client")
                {
                    // verifica se usuário possui relacionamento com o 'professional_id' declarado na periodization
                    ClientProfessional _clientProfessional = this.clientProfessionalRepository.Find(x => x.ProfessionalId == _periodization.ProfessionalId
                                                            && x.ClientId == tokenValidId && !x.IsDeleted) ?? throw new ApiException("Client not found", HttpStatusCode.NotFound);

                    if (_clientProfessional.ProfessionalId != _periodization.ProfessionalId)
                        throw new ApiException("Periodization not found", HttpStatusCode.NotFound);
                }
            }

            return mapper.Map<PeriodizationViewModel>(_periodization);
        }

        public List<PeriodizationViewModel> GetByProfessional(string tokenId, Guid id)
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
                List<Periodization> _periodizations = [];

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _periodizations = [.. this.periodizationRepository.Query(x => x.ProfessionalId == id && !x.IsDeleted)];

                    if (_periodizations.Count == 0)
                        throw new ApiException("No periodization found for this professional", HttpStatusCode.NotFound);
                }
                else
                {
                    _periodizations = [.. this.periodizationRepository.Query(x => x.ProfessionalId == _professionalLogged.Id && !x.IsDeleted)];

                    if (_periodizations.Count == 0)
                        throw new ApiException("No periodization found for this professional", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<PeriodizationViewModel>>(_periodizations);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public List<PeriodizationViewModel> GetByClient(string tokenId, Guid id)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Identifica tipo de usuário logado
            string _userTypeLogged = userServiceBaseClient.LoggedInUserType(tokenId);
            if (string.IsNullOrEmpty(_userTypeLogged))
                _userTypeLogged = userServiceBaseProfessional.LoggedInUserType(tokenId);
            if (string.IsNullOrEmpty(_userTypeLogged))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            // Valida se é Client ou Professional
            if (_userTypeLogged != "Professional" && _userTypeLogged != "Client")
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            try
            {
                List<Periodization> _periodizations = [];

                if (_userTypeLogged == "Client")
                {
                    if (validId != id)
                        throw new ApiException("You are not authorized to access this client's data", HttpStatusCode.BadRequest);

                    _periodizations = [.. this.periodizationRepository.Query(x => x.ClientId == id && !x.IsDeleted)];

                    if (_periodizations.Count == 0)
                        throw new ApiException("No periodizations found for this client", HttpStatusCode.NotFound);
                }
                else if (_userTypeLogged == "Professional")
                {
                    ClientProfessional _clientProfessional = this.clientProfessionalRepository.Find(x => x.ClientId == id && x.ProfessionalId == validId && !x.IsDeleted)
                        ?? throw new ApiException("You are not authorized to access this client's data", HttpStatusCode.BadRequest);

                    _periodizations = [.. this.periodizationRepository.Query(x => x.ClientId == id && !x.IsDeleted)];

                    if (_periodizations.Count == 0)
                        throw new ApiException("No periodizations found for this client", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<PeriodizationViewModel>>(_periodizations);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public List<PeriodizationViewModel> GetByName(string name, string tokenId)
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
                List<Periodization> _periodization = [];

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _periodization = [.. this.periodizationRepository.Query(x => EF.Functions.Like(x.Name, $"%{name}%")
                                                                                       && !x.IsDeleted)];

                    if (_periodization.Count == 0)
                        throw new ApiException("No periodization with this name were found", HttpStatusCode.NotFound);
                }
                else
                {
                    _periodization = [.. this.periodizationRepository.Query(x => !x.IsDeleted && EF.Functions.Like(x.Name, $"%{name}%") &&
                                                                          (x.ProfessionalId == validId || x.ProfessionalId == null))];

                    if (_periodization.Count == 0)
                        throw new ApiException("No periodization with this name were found", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<PeriodizationViewModel>>(_periodization);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public List<PeriodizationViewModel> GetByDateRange( string tokenId, DateTime dateStart, DateTime dateEnd)
        {
            if (!Guid.TryParse(tokenId, out Guid loggedUserId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Identifica tipo de usuário logado
            string userType = userServiceBaseClient.LoggedInUserType(tokenId) ?? userServiceBaseProfessional.LoggedInUserType(tokenId);
            if (string.IsNullOrEmpty(userType))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            if (userType != "Client" && userType != "Professional")
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            try
            {
                IQueryable<Periodization> query = periodizationRepository.Query(x => !x.IsDeleted && x.DateStart <= dateEnd &&  x.DateEnd >= dateStart);

                if (userType == "Client") query = query.Where(x => x.ClientId == loggedUserId);
                else query = query.Where(x => x.ProfessionalId == loggedUserId); // Professional

                List<Periodization> periodizations = query.ToList();

                if (periodizations.Count == 0)
                    throw new ApiException("No periodizations found for this period", HttpStatusCode.NotFound);

                return mapper.Map<List<PeriodizationViewModel>>(periodizations);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public List<PeriodizationViewModel> GetByWeeklyFrequency(string tokenId, int weeklyTrainingFrequency)
        {
            if (!Guid.TryParse(tokenId, out Guid loggedUserId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            if (weeklyTrainingFrequency <= 0)
                throw new ApiException("Weekly training frequency must be greater than zero", HttpStatusCode.BadRequest);

            // Identifica tipo de usuário logado
            string userType = userServiceBaseClient.LoggedInUserType(tokenId) ?? userServiceBaseProfessional.LoggedInUserType(tokenId);
            if (string.IsNullOrEmpty(userType))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            if (userType != "Client" && userType != "Professional")
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            try
            {
                IQueryable<Periodization> query = periodizationRepository.Query(x => !x.IsDeleted && x.WeeklyTrainingFrequency == weeklyTrainingFrequency);

                if (userType == "Client") query = query.Where(x => x.ClientId == loggedUserId);
                else query = query.Where(x => x.ProfessionalId == loggedUserId); // Professional

                List<Periodization> periodizations = query.ToList();

                if (periodizations.Count == 0)
                    throw new ApiException("No periodizations were found for this weekly frequency", HttpStatusCode.NotFound);

                return mapper.Map<List<PeriodizationViewModel>>(periodizations);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public PeriodizationViewModel Post(string tokenId, PeriodizationRequestViewModel periodizationRequestViewModel)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);
            if (periodizationRequestViewModel == null)
                throw new ApiException("Periodization is required", HttpStatusCode.BadRequest);
            if (string.IsNullOrWhiteSpace(periodizationRequestViewModel.Name))
                throw new ApiException("Name is required", HttpStatusCode.BadRequest);
            if (string.IsNullOrWhiteSpace(periodizationRequestViewModel.Description))
                throw new ApiException("Description is required", HttpStatusCode.BadRequest);
            if (periodizationRequestViewModel.DateStart == default)
                throw new ApiException("Start date is required", HttpStatusCode.BadRequest);
            if (periodizationRequestViewModel.DateEnd == default)
                throw new ApiException("End date is required", HttpStatusCode.BadRequest);
            if (periodizationRequestViewModel.DateEnd < periodizationRequestViewModel.DateStart)
                throw new ApiException("The end date must be greater than the start date", HttpStatusCode.BadRequest);
            if (periodizationRequestViewModel.DateStart < DateTime.UtcNow.Date)
                throw new ApiException("The start date is invalid", HttpStatusCode.BadRequest);
            if (periodizationRequestViewModel.WeeklyTrainingFrequency < 1 || periodizationRequestViewModel.WeeklyTrainingFrequency > 7)
                throw new ApiException("Weekly training frequency must be between 1 and 7", HttpStatusCode.BadRequest);

            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);
            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);

            // Verifica se já existe registro (global ou associado a um profissional)
            Periodization _periodizationExisting = this.periodizationRepository.Find(x => !x.IsDeleted &&
                                                        x.Name.ToLower() == periodizationRequestViewModel.Name.ToLower());

            if (_periodizationExisting != null)
            {
                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase) && _periodizationExisting.ProfessionalId == null)
                {
                    throw new ApiException("Periodization already exists");
                }
                else if (_periodizationExisting.ProfessionalId == null || _periodizationExisting.ProfessionalId == validId
                         && periodizationRequestViewModel.ClientId == _periodizationExisting.ClientId)
                {
                    throw new ApiException("The periodization already exists for the selected client");
                }
            }

            Guid? clientId = new();
            if (periodizationRequestViewModel.ClientId != null)
            {
                ClientProfessional _clientProfessional = this.clientProfessionalRepository.Find(x => x.ClientId == periodizationRequestViewModel.ClientId
                                                        && x.ProfessionalId == _professionalLogged.Id && !x.IsDeleted)
                                                        ?? throw new ApiException("The professional is not associated with the specified client", HttpStatusCode.BadRequest);
            
                clientId = periodizationRequestViewModel.ClientId.Value;
            } else clientId = null;

            Periodization _periodization = new()
            {
                Name = periodizationRequestViewModel.Name,
                Description = periodizationRequestViewModel.Description,
                DateStart = periodizationRequestViewModel.DateStart,
                DateEnd = periodizationRequestViewModel.DateEnd,
                WeeklyTrainingFrequency = periodizationRequestViewModel.WeeklyTrainingFrequency,
                ProfessionalId = loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase) ? null : validId,
                ClientId = clientId,
                DateUpdated = DateTime.UtcNow,
                IsDeleted = false
            };

            try
            {
                periodizationRepository.Create(_periodization);
                return mapper.Map<PeriodizationViewModel>(_periodization);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public PeriodizationViewModel Put(string tokenId, PeriodizationUpdateViewModel periodizationUpdateViewModel)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);
            if (periodizationUpdateViewModel == null)
                throw new ApiException("Periodization is required", HttpStatusCode.BadRequest);

            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.Forbidden);

            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            Periodization _periodization = this.periodizationRepository.Find(x => x.Id == periodizationUpdateViewModel.Id && !x.IsDeleted)
                ?? throw new ApiException("Periodization not found", HttpStatusCode.BadRequest);


            if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Admin pode modificar se professionalId for null ou pertencer ao mesmo usuário
                if (_periodization.ProfessionalId == null || _periodization.ProfessionalId == validId)
                {
                    _periodization.Name = periodizationUpdateViewModel.Name ?? _periodization.Name;
                    _periodization.Description = periodizationUpdateViewModel.Description ?? _periodization.Description;
                    _periodization.DateStart = periodizationUpdateViewModel.DateStart ?? _periodization.DateStart;
                    _periodization.DateEnd = periodizationUpdateViewModel.DateEnd ?? _periodization.DateEnd;
                    _periodization.WeeklyTrainingFrequency = periodizationUpdateViewModel.WeeklyTrainingFrequency ?? _periodization.WeeklyTrainingFrequency;
                    _periodization.DateUpdated = DateTime.UtcNow;

                    this.periodizationRepository.Update(_periodization);
                }
                else
                {
                    throw new ApiException("You are not authorized to modify this record", HttpStatusCode.Forbidden);
                }
            }
            else if (loggedInUserType.Equals("Professional", StringComparison.OrdinalIgnoreCase))
            {
                // Professional só pode modificar se for o dono do registro
                if (_periodization.ProfessionalId == validId)
                {
                    if (periodizationUpdateViewModel.ClientId != null)
                    {
                        ClientProfessional _clientProfessional = this.clientProfessionalRepository.Find(x => x.ClientId == periodizationUpdateViewModel.ClientId
                                                                && x.ProfessionalId == _professionalLogged.Id && !x.IsDeleted)
                                                                ?? throw new ApiException("The professional is not associated with the specified client", HttpStatusCode.BadRequest);

                        _periodization.ClientId = periodizationUpdateViewModel.ClientId;
                    }

                    _periodization.Name = periodizationUpdateViewModel.Name ?? _periodization.Name;
                    _periodization.Description = periodizationUpdateViewModel.Description ?? _periodization.Description;
                    _periodization.DateStart = periodizationUpdateViewModel.DateStart ?? _periodization.DateStart;
                    _periodization.DateEnd = periodizationUpdateViewModel.DateEnd ?? _periodization.DateEnd;
                    _periodization.WeeklyTrainingFrequency = periodizationUpdateViewModel.WeeklyTrainingFrequency ?? _periodization.WeeklyTrainingFrequency;
                    _periodization.DateUpdated = DateTime.UtcNow;

                    this.periodizationRepository.Update(_periodization);
                }
                else
                {
                    throw new ApiException("You are not authorized to modify this record", HttpStatusCode.Forbidden);
                }
            }

            return mapper.Map<PeriodizationViewModel>(_periodization);
        }

        public void Delete(string tokenId, Guid periodizationId)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            Periodization _periodizationToDelete = this.periodizationRepository.Find(x => x.Id == periodizationId)
                                                ?? throw new ApiException("Periodization not found", HttpStatusCode.NotFound);
            if (_periodizationToDelete.IsDeleted == true)
                throw new ApiException("Periodization already deleted", HttpStatusCode.BadRequest);


            string _loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);

            if (_loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                if (_periodizationToDelete.ProfessionalId == null || _periodizationToDelete.ProfessionalId == validId)
                    this.periodizationRepository.IsDeleted(_periodizationToDelete);
                else
                    throw new ApiException("You are not authorized to modify this record", HttpStatusCode.Forbidden);
            }
            else if (_loggedInUserType.Equals("Professional", StringComparison.OrdinalIgnoreCase))
            {
                // Professional só pode deletar se for o dono do registro
                if (_periodizationToDelete.ProfessionalId == validId)
                    this.periodizationRepository.IsDeleted(_periodizationToDelete);
                else
                    throw new ApiException("You are not authorized to modify this record", HttpStatusCode.Forbidden);
            }
        }
    }
}
