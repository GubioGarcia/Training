using AutoMapper;
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

namespace Training.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository clientRepository;
        private readonly IUsersTypeRepository usersTypeRepository;
        private readonly IChecker checker;
        private readonly IMapper mapper;
        private readonly ManualMapperSetup manualMapper;

        public ClientService(IClientRepository clientRepository, IUsersTypeRepository usersTypeRepository,
                             IMapper mapper, IChecker checker, ManualMapperSetup manualMapper)
        {
            this.clientRepository = clientRepository;
            this.usersTypeRepository = usersTypeRepository;
            this.checker = checker;
            this.mapper = mapper;
            this.manualMapper = manualMapper;
        }

        public List<ClientMinimalFieldViewModel> Get()
        {
            List<ClientMinimalFieldViewModel> _clientMinimalFieldViewModels = new List<ClientMinimalFieldViewModel>();

            IEnumerable<Client> _clients = this.clientRepository.GetAll();

            _clientMinimalFieldViewModels = mapper.Map<List<ClientMinimalFieldViewModel>>(_clients);

            return _clientMinimalFieldViewModels;
        }

        public ClientResponseViewModel GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid clientId))
                throw new Exception("Id is not valid");

            Client _client = this.clientRepository.Find(x => x.Id == clientId && !x.IsDeleted);
            if (_client == null)
                throw new Exception("Client not found");

            return mapper.Map<ClientResponseViewModel>(_client);
        }

        public ClientMinimalFieldViewModel Post(ClientRequestViewModel clientRequestViewModel)
        {
            if (!checker.isValidCpf(clientRequestViewModel.Cpf))
                throw new Exception("CPF is not valid");

            Client _client = this.clientRepository.Find(x => x.Cpf == clientRequestViewModel.Cpf);
            if (_client != null)
                throw new Exception("There is already a professional registered with this CPF");

            if (!checker.isValidFone(clientRequestViewModel.Fone))
                throw new Exception("Phone is not valid");

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == clientRequestViewModel.UsersTypeId && !x.IsDeleted);
            if (_usersType == null)
                throw new Exception("Id type user not found");

            _client = mapper.Map<Client>(clientRequestViewModel);
            _client.Password = this.HashPassword(clientRequestViewModel.Password);
            _client.DateRegistration = DateTime.Now;

            if (clientRequestViewModel.UrlProfilePhoto == "")
                _client.UrlProfilePhoto = null;

            this.clientRepository.Create(_client);

            return mapper.Map<ClientMinimalFieldViewModel>(_client);
        }

        public ClientResponseViewModel Put(ClientRequestUpdateViewModel clientRequestUpdateViewModel)
        {
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
            _client.DateUpdated = DateTime.UtcNow;

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
