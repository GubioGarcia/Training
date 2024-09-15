using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.Interfaces;
using Training.Application.Mapper;
using Training.Application.ViewModels;
using Training.Application.ViewModels.AuthenticateViewModels;
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
        private readonly IUsersTypeRepository usersTypeRepository;
        private readonly IUserServiceBase<Professional> userServiceBaseProfessional;
        private readonly IUserServiceBase<Client> userServiceBaseClient;
        private readonly ManualMapperSetup manualMapper;
        private readonly IChecker checker;
        private readonly IMapper mapper;

        public ClientService(IClientRepository clientRepository, IUsersTypeRepository usersTypeRepository, ManualMapperSetup manualMapper,
                             IMapper mapper, IChecker checker, IUserServiceBase<Professional> userServiceBaseProfessional,
                             IUserServiceBase<Client> userServiceBaseClient)
        {
            this.clientRepository = clientRepository;
            this.usersTypeRepository = usersTypeRepository;
            this.checker = checker;
            this.mapper = mapper;
            this.manualMapper = manualMapper;
            this.userServiceBaseProfessional = userServiceBaseProfessional;
            this.userServiceBaseClient = userServiceBaseClient;
        }

        public List<ClientMinimalFieldViewModel> Get(string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new Exception("You are not authorized to perform this operation");

            List<ClientMinimalFieldViewModel> _clientMinimalFieldViewModels = new List<ClientMinimalFieldViewModel>();

            IEnumerable<Client> _clients = this.clientRepository.GetAll();

            _clientMinimalFieldViewModels = mapper.Map<List<ClientMinimalFieldViewModel>>(_clients);

            return _clientMinimalFieldViewModels;
        }

        public ClientResponseViewModel GetById(string id, string tokenId)
        {/*
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"])
                && !this.userServiceBaseClient.IsLoggedInUserOfValidType(tokenId, ["Client"]))
                throw new Exception("You are not authorized to perform this operation");

            #region 'Valid if logged in user is the same user tho be changed'

            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new Exception("Id is not valid");

            Client _clientLogged = this.clientRepository.Find(x => x.Id == validId && !x.IsDeleted);
            if (_clientLogged.Id != clientRequestUpdateViewModel.Id)
                throw new Exception("You are not authorized to perform this operation");

            #endregion
            */
            if (!Guid.TryParse(id, out Guid clientId))
                throw new Exception("Id is not valid");

            Client _client = this.clientRepository.Find(x => x.Id == clientId && !x.IsDeleted);
            if (_client == null)
                throw new Exception("Client not found");

            return mapper.Map<ClientResponseViewModel>(_client);
        }

        public ClientResponseViewModel GetByCpf(string cpf, string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new Exception("You are not authorized to perform this operation");

            if (!checker.isValidCpf(cpf))
                throw new Exception("CPF is not valid");

            Client _client = this.clientRepository.Find(x => x.Cpf == cpf && !x.IsDeleted);
            if (_client == null)
                throw new Exception("Client not found");

            return mapper.Map<ClientResponseViewModel>(_client);
        }

        public List<ClientMinimalFieldViewModel> GetByName(string name, string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new Exception("You are not authorized to perform this operation");

            if (string.IsNullOrEmpty(name))
                throw new Exception("Name is required");

            List<ClientMinimalFieldViewModel> _clientMinimalFieldViewModels = new List<ClientMinimalFieldViewModel>();

            IEnumerable<Client> _clients = this.clientRepository.Query(p => EF.Functions.Like(p.Name, $"%{name}%") && !p.IsDeleted);

            _clientMinimalFieldViewModels = mapper.Map<List<ClientMinimalFieldViewModel>>(_clients);

            return _clientMinimalFieldViewModels;
        }

        public ClientMinimalFieldViewModel Post(ClientRequestViewModel clientRequestViewModel, string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new Exception("You are not authorized to perform this operation");

            if (!checker.isValidCpf(clientRequestViewModel.Cpf))
                throw new Exception("CPF is not valid");

            Client _client = this.clientRepository.Find(x => x.Cpf == clientRequestViewModel.Cpf);
            if (_client != null)
                throw new Exception("There is already a professional registered with this CPF");

            if (!checker.isValidFone(clientRequestViewModel.Fone))
                throw new Exception("Phone is not valid");

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Name == "Client" && !x.IsDeleted);
            if (_usersType == null)
                throw new Exception("Id type user not found");

            _client = mapper.Map<Client>(clientRequestViewModel);
            _client.Password = this.HashPassword(clientRequestViewModel.Password);
            _client.DateRegistration = DateTime.Now;
            _client.UsersTypeId = _usersType.Id;

            if (clientRequestViewModel.UrlProfilePhoto == "")
                _client.UrlProfilePhoto = null;

            this.clientRepository.Create(_client);

            return mapper.Map<ClientMinimalFieldViewModel>(_client);
        }

        public ClientResponseViewModel Put(ClientRequestUpdateViewModel clientRequestUpdateViewModel, string tokenId)
        {/*
            // Valida tipo de usuário com acesso ao método
            if (this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
            {
                Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted);
                if (_professionalLogged.Id != professionalRequestUpdateViewModel.Id)
                    throw new Exception("You are not authorized to perform this operation");
            }

            bool _isClient = this.userServiceBaseClient.IsLoggedInUserOfValidType(tokenId, ["Client"]);
            if (!_isProfessional && !_isClient)
                throw new Exception("You are not authorized to perform this operation");

            #region 'Valid if logged in user is the same user tho be changed'

            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new Exception("Id is not valid");

            Client _clientLogged = this.clientRepository.Find(x => x.Id == validId && !x.IsDeleted);
            if (_clientLogged.Id != clientRequestUpdateViewModel.Id)
                throw new Exception("You are not authorized to perform this operation");

            #endregion
            */
            Client _client = this.clientRepository.Find(x => x.Id == clientRequestUpdateViewModel.Id);
            if (_client == null)
                throw new Exception("Client not found");
            
            if (clientRequestUpdateViewModel.Cpf != null)
            {
                if (!checker.isValidCpf(clientRequestUpdateViewModel.Cpf))
                    throw new Exception("CPF is not valid");

                Client _auxClient = this.clientRepository.Find(x => x.Cpf == clientRequestUpdateViewModel.Cpf);
                if (_auxClient != null && _auxClient.Id != _client.Id)
                    throw new Exception("There is already a client registered with this CPF");
            }

            manualMapper.MapClientRequestUpdateToClient(clientRequestUpdateViewModel, _client);
            _client.DateUpdated = DateTime.Now;

            if (clientRequestUpdateViewModel.Password != null)
                _client.Password = this.HashPassword(clientRequestUpdateViewModel.Password);

            if (clientRequestUpdateViewModel.CurrentWeight == null || clientRequestUpdateViewModel.CurrentWeight == 0)
                _client.CurrentWeight = _client.StartingWeight;
            
            this.clientRepository.Update(_client);

            return mapper.Map<ClientResponseViewModel>(_client);
        }

        public bool Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid clientId))
                throw new Exception("Id is not valid");

            Client _client = this.clientRepository.Find(x => x.Id == clientId && !x.IsDeleted);
            if (_client == null)
                throw new Exception("Professional not found");

            return this.clientRepository.Delete(_client);
        }

        public UserAuthenticateResponseViewModel Authenticate(UserAuthenticateRequestViewModel client)
        {
            if (string.IsNullOrEmpty(client.Cpf) || string.IsNullOrEmpty(client.Password))
                throw new Exception("CPF/Password is required");

            if (!checker.isValidCpf(client.Cpf))
                throw new Exception("CPF is not valid");

            Client _client = this.clientRepository.Find(x => x.Cpf == client.Cpf && !x.IsDeleted);

            if (_client == null || !this.VerifyPassword(client.Password, _client.Password))
                throw new Exception("Invalid credentials");

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == _client.UsersTypeId && !x.IsDeleted);

            var (_token, _expiry) = TokenService.GenerateToken(_client);

            return new UserAuthenticateResponseViewModel(_token, _expiry, _client.Id, _client.Cpf, _client.Name,
                                                         _usersType.Name);
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
