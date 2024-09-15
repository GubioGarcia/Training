using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.Interfaces;
using Training.Application.ViewModels.ClientProfessionalViewModels;
using Training.Domain.Entities;
using Training.Domain.Interfaces;

namespace Training.Application.Services
{
    public class ClientProfessinalService : IClientProfessionalService
    {
        private readonly IClientProfessionalRepository clientProfessionalRepository;
        private readonly IProfessionalRepository professionalRepository;
        private readonly IProfessionalService professionalService;
        private readonly IClientRepository clientRepository;
        private readonly IUserServiceBase<Professional> userServiceBase;
        private readonly IChecker checker;
        private readonly IMapper mapper;

        public ClientProfessinalService(IClientProfessionalRepository clientProfessionalRepository, IMapper mapper, IProfessionalRepository professionalRepository,
                                        IClientRepository clientRepository, IChecker checker, IProfessionalService professionalService, 
                                        IUserServiceBase<Professional> userServiceBase)
        {
            this.clientProfessionalRepository = clientProfessionalRepository;
            this.professionalRepository = professionalRepository;
            this.professionalService = professionalService;
            this.clientRepository = clientRepository;
            this.userServiceBase = userServiceBase;
            this.checker = checker;
            this.mapper = mapper;
        }

        public List<ClientProfessionalViewModel> Get(string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new Exception("You are not authorized to perform this operation");

            List<ClientProfessionalViewModel> _clientProfessionalViewModel = new List<ClientProfessionalViewModel>();

            IEnumerable<ClientProfessional> _clientProfessionals = this.clientProfessionalRepository.GetAll();
            if (_clientProfessionals == null)
                throw new Exception("Client-Professional Relationship not found");

            _clientProfessionalViewModel = mapper.Map<List<ClientProfessionalViewModel>>(_clientProfessionals);

            foreach (ClientProfessionalViewModel item in _clientProfessionalViewModel)
            {
                Professional _auxProfessional = this.professionalRepository.Find(x => x.Id == item.ProfessionalId && !x.IsDeleted);
                Client _auxClient = this.clientRepository.Find(x => x.Id == item.ClientId && !x.IsDeleted);

                if (_auxProfessional != null)
                {
                    item.ProfessionalCpf = _auxProfessional.Cpf;
                    item.ProfessionalName = _auxProfessional.Name;
                }

                if (_auxClient != null)
                {
                    item.ClientCpf = _auxClient.Cpf;
                    item.ClientName = _auxClient.Name;
                }
            }

            return _clientProfessionalViewModel;
        }

        public ClientProfessionalViewModel GetById(string tokenId, string id)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new Exception("You are not authorized to perform this operation");

            if (!Guid.TryParse(id, out Guid clientProfessionalId))
                throw new Exception("Id is not valid");

            ClientProfessional _clientProfessional = this.clientProfessionalRepository.Find(x => x.Id.Equals(clientProfessionalId) && !x.IsDeleted);
            if (_clientProfessional == null)
                throw new Exception("Client-Professional Relationship not found");

            ClientProfessionalViewModel _clientProfessionalViewModel = mapper.Map<ClientProfessionalViewModel>(_clientProfessional);

            Professional _auxProfessional = this.professionalRepository.Find(x => x.Id == _clientProfessional.ProfessionalId && !x.IsDeleted);
            Client _auxClient = this.clientRepository.Find(x => x.Id == _clientProfessional.ClientId && !x.IsDeleted);

            if (_auxProfessional != null)
            {
                _clientProfessionalViewModel.ProfessionalCpf = _auxProfessional.Cpf;
                _clientProfessionalViewModel.ProfessionalName = _auxProfessional.Name;
            }

            if (_auxClient != null)
            {
                _clientProfessionalViewModel.ClientCpf = _auxClient.Cpf;
                _clientProfessionalViewModel.ClientName = _auxClient.Name;
            }

            return _clientProfessionalViewModel;
        }

        public List<ClientProfessionalViewModel> GetClientsByProfessionalId(string tokenId, string id)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new Exception("You are not authorized to perform this operation");

            if (!Guid.TryParse(id, out Guid professionalId))
                throw new Exception("Id is not valid");

            List<ClientProfessionalViewModel> _clientProfessionalViewModel = new List<ClientProfessionalViewModel>();

            IEnumerable<ClientProfessional> _clientProfessionals = this.clientProfessionalRepository.Query(x => x.ProfessionalId == professionalId
                                                                                                          && !x.IsDeleted);

            _clientProfessionalViewModel = mapper.Map<List<ClientProfessionalViewModel>>(_clientProfessionals);
            if (_clientProfessionals.Count() == 0)
                throw new Exception("Client-Professional Relationship not found");

            foreach (ClientProfessionalViewModel item in _clientProfessionalViewModel)
            {
                Professional _auxProfessional = this.professionalRepository.Find(x => x.Id == item.ProfessionalId && !x.IsDeleted);
                Client _auxClient = this.clientRepository.Find(x => x.Id == item.ClientId && !x.IsDeleted);

                if (_auxProfessional != null)
                {
                    item.ProfessionalCpf = _auxProfessional.Cpf;
                    item.ProfessionalName = _auxProfessional.Name;
                }

                if (_auxClient != null)
                {
                    item.ClientCpf = _auxClient.Cpf;
                    item.ClientName = _auxClient.Name;
                }
            }

            return _clientProfessionalViewModel;
        }

        public ClientProfessionalViewModel Post(string tokenId, ClientProfessionalRequestViewModel clientProfessionalRequestViewModels)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new Exception("You are not authorized to perform this operation");

            Professional _professinal = this.professionalRepository.Find(x => x.Id == clientProfessionalRequestViewModels.ProfessionalId);
            if (_professinal == null)
                throw new Exception("Professional not found");

            Client _client = this.clientRepository.Find(x => x.Id == clientProfessionalRequestViewModels.ClientId);
            if (_client == null)
                throw new Exception("Client not found");

            ClientProfessional _clientProfessional = this.clientProfessionalRepository.Find(x => x.ProfessionalId == clientProfessionalRequestViewModels.ProfessionalId
                                                                                            && x.ClientId == clientProfessionalRequestViewModels.ClientId);
            if (_clientProfessional != null)
                throw new Exception("There is already an existing relationship between this professional and the client");

            _clientProfessional = mapper.Map<ClientProfessional>(clientProfessionalRequestViewModels);
            _clientProfessional.DateUpdated = DateTime.Now;

            this.clientProfessionalRepository.Create(_clientProfessional);

            ClientProfessionalViewModel _clientProfessionalViewModel = mapper.Map<ClientProfessionalViewModel>(_clientProfessional);
            _clientProfessionalViewModel.ProfessionalCpf = _professinal.Cpf;
            _clientProfessionalViewModel.ProfessionalName = _professinal.Name;
            _clientProfessionalViewModel.ClientCpf = _client.Cpf;
            _clientProfessionalViewModel.ClientName = _client.Name;

            return _clientProfessionalViewModel;
        }

        public ClientProfessionalViewModel Put(string tokenId, ClientProfessionalRequestUpdateViewModel clientProfessionalRequestUpdateViewModels)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new Exception("You are not authorized to perform this operation");

            ClientProfessional _clientProfessional = this.clientProfessionalRepository.Find(x => x.Id == clientProfessionalRequestUpdateViewModels.Id);
            if (_clientProfessional == null)
                throw new Exception("Client-Professional Relationship not found");

            if (clientProfessionalRequestUpdateViewModels.DescriptionProfessional == "")
                throw new Exception("DescriptionProfessional is required");

            _clientProfessional.DescriptionProfessional = clientProfessionalRequestUpdateViewModels.DescriptionProfessional;
            _clientProfessional.DateUpdated = DateTime.Now;

            this.clientProfessionalRepository.Update(_clientProfessional);

            ClientProfessionalViewModel _clientProfessionalViewModel = mapper.Map<ClientProfessionalViewModel>(_clientProfessional);

            Professional _auxProfessional = this.professionalRepository.Find(x => x.Id == _clientProfessional.ProfessionalId);
            Client _auxClient = this.clientRepository.Find(x => x.Id == _clientProfessional.ClientId && !x.IsDeleted);

            _clientProfessionalViewModel.ProfessionalCpf = _auxProfessional.Cpf;
            _clientProfessionalViewModel.ProfessionalName = _auxProfessional.Name;
            _clientProfessionalViewModel.ClientCpf = _auxClient.Cpf;
            _clientProfessionalViewModel.ClientName = _auxClient.Name;

            return _clientProfessionalViewModel;
        }

        public bool Delete(string tokenId, string professionalId, string clientId)
        {
            if (!Guid.TryParse(tokenId, out Guid validTokeId) || !Guid.TryParse(clientId, out Guid validClientId)
                || !Guid.TryParse(professionalId, out Guid validProfessionalId))
                throw new Exception("Id is not valid");

            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new Exception("You are not authorized to perform this operation");

            ClientProfessional _clientProfessional = this.clientProfessionalRepository.Find(x => x.ProfessionalId == validProfessionalId
                                                                                            && x.ClientId == validClientId && !x.IsDeleted);
            if (_clientProfessional == null)
                throw new Exception("Client-Professional Relationship not found");

            return this.clientProfessionalRepository.Delete(_clientProfessional);
        }
    }
}
