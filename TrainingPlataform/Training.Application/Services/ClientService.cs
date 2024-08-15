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
    public class ClientService : IClientService
    {
        private readonly IClientRepository clientRepository;
        private readonly IMapper mapper;

        public ClientService(IClientRepository clientRepository, IMapper mapper)
        {
            this.clientRepository = clientRepository;
            this.mapper = mapper;
        }

        public List<ClientMinimalFieldViewModel> Get()
        {
            List<ClientMinimalFieldViewModel> _clientMinimalFieldViewModels = new List<ClientMinimalFieldViewModel>();

            IEnumerable<Client> _clients = this.clientRepository.GetAll();

            _clientMinimalFieldViewModels = mapper.Map<List<ClientMinimalFieldViewModel>>(_clients);

            return _clientMinimalFieldViewModels;
        } 
    }
}
