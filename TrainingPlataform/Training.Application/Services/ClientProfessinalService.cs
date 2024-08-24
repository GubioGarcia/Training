using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.Interfaces;
using Training.Application.ViewModels;
using Training.Domain.Entities;
using Training.Domain.Interfaces;

namespace Training.Application.Services
{
    public class ClientProfessinalService : IClientProfessionalService
    {
        private readonly IClientProfessionalRepository clientProfessionalRepository;
        private readonly IProfessionalRepository professionalRepository;
        private readonly IClientRepository clientRepository;
        private readonly IMapper mapper;

        public ClientProfessinalService(IClientProfessionalRepository clientProfessionalRepository, IMapper mapper,
                                        IProfessionalRepository professionalRepository, IClientRepository clientRepository)
        {
            this.clientProfessionalRepository = clientProfessionalRepository;
            this.mapper = mapper;
            this.professionalRepository = professionalRepository;
            this.clientRepository = clientRepository;
        }

        public List<ClientProfessionalViewModel> Get()
        {
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

        public ClientProfessionalViewModel GetById(string id)
        {
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

        public List<ClientProfessionalViewModel> GetClientsByProfessionalId(string id)
        {
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
    }
}
