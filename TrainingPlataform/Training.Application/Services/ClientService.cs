using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Template.CrossCutting.ExceptionHandler.Extensions;
using Training.Application.Interfaces;
using Training.Application.Mapper;
using Training.Application.ViewModels;
using Training.Application.ViewModels.AuthenticateViewModels;
using Training.Application.ViewModels.ClientProfessionalViewModels;
using Training.Application.ViewModels.ClientViewModels;
using Training.Application.ViewModels.ProfessionalViewModels;
using Training.Auth.Services;
using Training.Domain.Entities;
using Training.Domain.Interfaces;
using Training.Domain.Models;

namespace Training.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository clientRepository;
        private readonly IProfessionalRepository professionalRepository;
        private readonly IClientProfessionalService clientProfessionalService;
        private readonly IClientProfessionalRepository clientProfessionalRepository;
        private readonly IUsersTypeRepository usersTypeRepository;
        private readonly ManualMapperSetup manualMapper;
        private readonly IUserServiceBase<Professional> userServiceBaseProfessional;
        private readonly IUserServiceBase<Client> userServiceBaseClient;
        private readonly IChecker checker;
        private readonly IMapper mapper;

        public ClientService(IClientRepository clientRepository, IProfessionalRepository professionalRepository, IUsersTypeRepository usersTypeRepository, 
                             IClientProfessionalRepository clientProfessionalRepository, IClientProfessionalService clientProfessionalService, ManualMapperSetup manualMapper,
                             IMapper mapper, IChecker checker, IUserServiceBase<Professional> userServiceBaseProfessional, IUserServiceBase<Client> userServiceBaseClient)
        {
            this.clientRepository = clientRepository;
            this.professionalRepository = professionalRepository;
            this.clientProfessionalService = clientProfessionalService;
            this.clientProfessionalRepository = clientProfessionalRepository;
            this.usersTypeRepository = usersTypeRepository;
            this.userServiceBaseProfessional = userServiceBaseProfessional;
            this.userServiceBaseClient = userServiceBaseClient;
            this.manualMapper = manualMapper;
            this.checker = checker;
            this.mapper = mapper;
        }

        // Acessado apenas pelo professional logado e visualiza apenas os clientes vinculados a ele
        public List<ClientMinimalFieldViewModel> Get(string tokenId)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            Professional _professional = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                         ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            // objeto que recebe os clientes relacionados ao professional
            IEnumerable<ClientProfessional> _clientProfessionalRelations = this.clientProfessionalRepository
                                            .Query(x => x.ProfessionalId == _professional.Id && !x.IsDeleted).ToList();
            if (!_clientProfessionalRelations.Any())
                throw new ApiException("No clients found for this professional", HttpStatusCode.NotFound);

            try
            {
                // Obtem os ClientId dos registros encontrados, busca os clientes correspondentes e mapeia para a ViewModel
                List<Guid> _clientIds = _clientProfessionalRelations.Select(x => x.ClientId).ToList();
                IEnumerable<Client> _clients = this.clientRepository.Query(x => _clientIds.Contains(x.Id) && !x.IsDeleted).ToList();
                List<ClientMinimalFieldViewModel> _clientMinimalFieldViewModels = this.mapper.Map<List<ClientMinimalFieldViewModel>>(_clients);

                return _clientMinimalFieldViewModels;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        // Acessado apenas se o cliente pesquisado estiver vinculado com o professional
        public ClientResponseViewModel GetById(Guid clientId, string tokenId)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Recebe tipo do usuário logado
            string _userTypeLogged = userServiceBaseClient.LoggedInUserType(tokenId);
            if (_userTypeLogged == null || _userTypeLogged == "")
                _userTypeLogged = userServiceBaseProfessional.LoggedInUserType(tokenId);
            else if (_userTypeLogged == null || _userTypeLogged == "") 
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            // Valida se usuário logado possui acesso liberado ao método
            if (_userTypeLogged == "Client")
            {
                Client _clientLogged = this.clientRepository.Find(x => x.Id == validId && !x.IsDeleted);
                if (_clientLogged.Id != clientId)
                    throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);
            }
            else if (_userTypeLogged == "Professional")
            {
                Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                             ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

                ClientProfessional _clientProfessional = this.clientProfessionalRepository.Find(x => x.Professional.Id == _professionalLogged.Id
                                                        && x.ClientId == clientId && !x.IsDeleted) ?? throw new ApiException("Client not found", HttpStatusCode.NotFound);
            }

            Client _client = this.clientRepository.Find(x => x.Id == clientId && !x.IsDeleted);
            if (_client == null)
                throw new ApiException("Client not found", HttpStatusCode.NotFound);

            return mapper.Map<ClientResponseViewModel>(_client);
        }

        // Acessado apenas se o cliente pesquisado estiver vinculado com o professional
        public ClientResponseViewModel GetByCpf(string cpf, string tokenId)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            if (!checker.isValidCpf(cpf))
                throw new ApiException("CPF is not valid", HttpStatusCode.BadRequest);

            // recebe o professional logado
            Professional _professional = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                         ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            // recebe o cliente a ser retornado
            Client _client = this.clientRepository.Find(x => x.Cpf == cpf && !x.IsDeleted);
            if (_client == null)
                throw new ApiException("Client not found", HttpStatusCode.NotFound);

            // valida se o professional logado tem relacionamento com o cliente a ser retornado
            ClientProfessional _clientProfessional = this.clientProfessionalRepository.Find(x => x.Professional.Id == _professional.Id
                                                    && x.ClientId == _client.Id && !x.IsDeleted) ?? throw new ApiException("Client not found", HttpStatusCode.NotFound);

            return mapper.Map<ClientResponseViewModel>(_client);
        }

        // Retorna apenas os clientes vinculados com o professional logado
        public List<ClientMinimalFieldViewModel> GetByName(string name, string tokenId)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            Professional _professional = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                         ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            // objeto que recebe os clientes relacionados ao professional
            IEnumerable<ClientProfessional> _clientProfessionalRelations = this.clientProfessionalRepository
                                            .Query(x => x.ProfessionalId == _professional.Id && !x.IsDeleted).ToList();
            if (!_clientProfessionalRelations.Any())
                throw new ApiException("No clients found for this professional", HttpStatusCode.NotFound);

            try
            {
                // Obtem os ClientId dos registros encontrados, busca os clientes correspondentes e mapeia para a ViewModel
                List<Guid> _clientIds = _clientProfessionalRelations.Select(x => x.ClientId).ToList();
                IEnumerable<Client> _clients = this.clientRepository.Query(x => _clientIds.Contains(x.Id)
                                                && EF.Functions.Like(x.Name, $"%{name}%")&& !x.IsDeleted).ToList();
                List<ClientMinimalFieldViewModel> _clientMinimalFieldViewModels = this.mapper.Map<List<ClientMinimalFieldViewModel>>(_clients);

                return _clientMinimalFieldViewModels;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        // Acessado apenas por usuário do tipo 'Professional', cria relação automática cliente/professional
        public ClientMinimalFieldViewModel Post(ClientRequestViewModel clientRequestViewModel, string tokenId)
        {
            if (!Guid.TryParse(tokenId, out Guid professionalId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            if (!checker.isValidCpf(clientRequestViewModel.Cpf))
                throw new ApiException("CPF is not valid", HttpStatusCode.BadRequest);

            // Verifica se já existe cliente cadastrado com o CPF, entre os clientes relacionados ao profissional
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == professionalId && !x.IsDeleted);
            List<Guid> _clientProfessionalRelations = this.clientProfessionalRepository
                                                      .Query(x => x.ProfessionalId == _professionalLogged.Id).Select(x => x.ClientId).ToList();
            Client _client = this.clientRepository.Find(x => x.Cpf == clientRequestViewModel.Cpf && _clientProfessionalRelations.Contains(x.Id));
            if (_client != null)
                throw new ApiException("There is already a client registered with this CPF", HttpStatusCode.BadRequest);

            if (!checker.isValidFone(clientRequestViewModel.Fone))
                throw new ApiException("Phone is not valid", HttpStatusCode.BadRequest);

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Name == "Client" && !x.IsDeleted);
            if (_usersType == null)
                throw new ApiException("Id type user not found", HttpStatusCode.NotFound);

            try
            {
                _client = mapper.Map<Client>(clientRequestViewModel);
                _client.Password = this.HashPassword(clientRequestViewModel.Password);
                _client.DateRegistration = DateTime.Now;
                _client.UsersTypeId = _usersType.Id;

                if (clientRequestViewModel.UrlProfilePhoto == "")
                    _client.UrlProfilePhoto = null;

                this.clientRepository.Create(_client);

                // Gera relacionamento do client com professional logado
                ClientProfessionalRequestViewModel _clientProfessionalRequestViewModel = new ClientProfessionalRequestViewModel();
                _clientProfessionalRequestViewModel.ProfessionalId = professionalId;
                _clientProfessionalRequestViewModel.ClientId = _client.Id;
                _clientProfessionalRequestViewModel.DescriptionProfessional = "Novo cliente";
                this.clientProfessionalService.Post(tokenId, _clientProfessionalRequestViewModel);

                return mapper.Map<ClientMinimalFieldViewModel>(_client);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        // Acessado apenas pelo próprio usuário cliente logado ou pelo usuário professional se possuir relação com o cliente a ser alterado
        public ClientResponseViewModel Put(ClientRequestUpdateViewModel clientRequestUpdateViewModel, string tokenId)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Recebe tipo do usuário logado
            string _userTypeLogged = userServiceBaseClient.LoggedInUserType(tokenId);
            if (_userTypeLogged == null || _userTypeLogged == "")
                _userTypeLogged = userServiceBaseProfessional.LoggedInUserType(tokenId);
            else if (_userTypeLogged == null || _userTypeLogged == "")
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            // Valida se usuário logado possui acesso liberado ao método
            if (_userTypeLogged == "Client")
            {
                Client _clientLogged = this.clientRepository.Find(x => x.Id == validId && !x.IsDeleted);
                if (_clientLogged.Id != clientRequestUpdateViewModel.Id)
                    throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);
            } else if (_userTypeLogged == "Professional")
            {
                Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted);
                ClientProfessional _clientProfessional = this.clientProfessionalRepository.Find(x => x.ProfessionalId == _professionalLogged.Id && x.ClientId == clientRequestUpdateViewModel.Id);
                if (_clientProfessional == null)
                    throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);
            }

            Client _client = this.clientRepository.Find(x => x.Id == clientRequestUpdateViewModel.Id);
            if (_client == null)
                throw new ApiException("Client not found", HttpStatusCode.NotFound);
     
            if (clientRequestUpdateViewModel.Cpf != null)
            {
                if (!checker.isValidCpf(clientRequestUpdateViewModel.Cpf))
                    throw new ApiException("CPF is not valid", HttpStatusCode.BadRequest);

                Client _auxClient = this.clientRepository.Find(x => x.Cpf == clientRequestUpdateViewModel.Cpf);
                if (_auxClient != null && _auxClient.Id != _client.Id)
                    throw new ApiException("There is already a client registered with this CPF", HttpStatusCode.BadRequest);
            }

            try
            {
                manualMapper.MapClientRequestUpdateToClient(clientRequestUpdateViewModel, _client);
                _client.DateUpdated = DateTime.Now;

                if (clientRequestUpdateViewModel.Password != null)
                    _client.Password = this.HashPassword(clientRequestUpdateViewModel.Password);

                if (clientRequestUpdateViewModel.CurrentWeight == null || clientRequestUpdateViewModel.CurrentWeight == 0)
                    _client.CurrentWeight = _client.StartingWeight;
            
                this.clientRepository.Update(_client);

                return mapper.Map<ClientResponseViewModel>(_client);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public bool Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid clientId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            Client _client = this.clientRepository.Find(x => x.Id == clientId && !x.IsDeleted);
            if (_client == null)
                throw new ApiException("Professional not found", HttpStatusCode.NotFound);

            return this.clientRepository.Delete(_client);
        }

        public UserAuthenticateResponseViewModel Authenticate(UserAuthenticateRequestViewModel client)
        {
            if (string.IsNullOrEmpty(client.Cpf) || string.IsNullOrEmpty(client.Password))
                throw new ApiException("CPF/Password is required", HttpStatusCode.BadRequest);

            if (!checker.isValidCpf(client.Cpf))
                throw new ApiException("CPF is not valid", HttpStatusCode.BadRequest);

            Client _client = this.clientRepository.Find(x => x.Cpf == client.Cpf && !x.IsDeleted);

            if (_client == null || !this.VerifyPassword(client.Password, _client.Password))
                throw new ApiException("Invalid credentials", HttpStatusCode.BadRequest);

            try
            {
                UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == _client.UsersTypeId && !x.IsDeleted);

                var (_token, _expiry) = TokenService.GenerateToken(_client);

                return new UserAuthenticateResponseViewModel(_token, _expiry, _client.Id, _client.Cpf, _client.Name,
                                                             _usersType.Name);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
