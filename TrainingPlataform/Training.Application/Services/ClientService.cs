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
using Training.Application.ViewModels.ClientViewModels;
using Training.Application.ViewModels.ProfessionalViewModels;
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

        public bool Post(ClientRequestViewModel clientRequestViewModel)
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

            return true;
        }

        public bool Put(ClientUpdateRequestViewModel clientUpdateRequestViewModel)
        {
            Client _client = this.clientRepository.Find(x => x.Id == clientUpdateRequestViewModel.Id);
            if (_client == null)
                throw new Exception("Client not found");
            
            if (clientUpdateRequestViewModel.Cpf != null)
            {
                if (!checker.isValidCpf(clientUpdateRequestViewModel.Cpf))
                    throw new Exception("CPF is not valid");

                Client _auxClient = this.clientRepository.Find(x => x.Cpf == clientUpdateRequestViewModel.Cpf);

                if (_auxClient.Id != _client.Id)
                    throw new Exception("There is already a client registered with this CPF");
            }

            manualMapper.MapClientUpdateRequestToClient(clientUpdateRequestViewModel, _client);
            _client.DateUpdated = DateTime.UtcNow;

            if (clientUpdateRequestViewModel.Password != null)
                _client.Password = this.HashPassword(clientUpdateRequestViewModel.Password);

            if (clientUpdateRequestViewModel.CurrentWeight == null || clientUpdateRequestViewModel.CurrentWeight == 0)
                _client.CurrentWeight = _client.StartingWeight;
            
            this.clientRepository.Update(_client);

            return true;
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
