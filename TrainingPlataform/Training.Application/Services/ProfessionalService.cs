using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.ViewModels;
using Training.Application.Interfaces;
using Training.Application.ViewModels;
using Training.Domain.Entities;
using Training.Domain.Interfaces;
using Training.Auth.Services;

namespace Training.Application.Services
{
    public class ProfessionalService : IProfessionalService
    {
        private readonly IProfessionalRepository professionalRepository;
        private readonly IMapper mapper;

        public ProfessionalService(IProfessionalRepository professionalRepository, IMapper mapper) 
        {
            this.professionalRepository = professionalRepository;
            this.mapper = mapper;
        }

        public List<ProfessionalViewModel> Get()
        {
            List<ProfessionalViewModel> _professionalViewModels = new List<ProfessionalViewModel>();

            IEnumerable<Professional> _professionals = this.professionalRepository.GetAll();

            _professionalViewModels = mapper.Map<List<ProfessionalViewModel>>(_professionals);
            
            return _professionalViewModels;
        }

        public ProfessionalViewModel GetByid(string id)
        {
            if (!Guid.TryParse(id, out Guid professionalId))
                throw new Exception("Id is not valid");

            Professional _professional = this.professionalRepository.Find(x => x.Id == professionalId && !x.IsDeleted);
            if (_professional == null)
                throw new Exception("Professional not found");

            return mapper.Map<ProfessionalViewModel>(_professional);
        }

        public bool Post(ProfessionalViewModel professionalViewModel)
        {
            Professional _professional = mapper.Map<Professional>(professionalViewModel);
            // criptografar password aqui

            _professional.DateRegistration = DateTime.Now;

            this.professionalRepository.Create(_professional);

            return true;
        }

        public bool Put(ProfessionalViewModel professionalViewModel)
        {
            Professional _professional = this.professionalRepository.Find(x => x.Id == professionalViewModel.Id && !x.IsDeleted);
            if (_professional == null)
                throw new Exception("Professional not found");

            _professional = mapper.Map<Professional>(professionalViewModel);
            // criptografar password aqui

            this.professionalRepository.Update(_professional);

            return true;
        }

        public bool Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid professionalId))
                throw new Exception("Id is not valid");

            Professional _professional = this.professionalRepository.Find(x => x.Id == professionalId && !x.IsDeleted);
            if (_professional == null)
                throw new Exception("Professional not found");

            return this.professionalRepository.Delete(_professional);
        }

        public UserAuthenticateResponseViewModel Authenticate(UserAuthenticateRequestViewModel professional)
        {
            if(string.IsNullOrEmpty(professional.Cpf) || string.IsNullOrEmpty(professional.Password))
                throw new Exception("CPF/Password is required");

            // criptografar password aqui

            Professional _professional = this.professionalRepository.Find(x => !x.IsDeleted 
                                                                          && x.Cpf.ToLower() == professional.Cpf.ToLower()
                                                                          && x.Password.ToLower() == professional.Password.ToLower());
            if (_professional == null)
                throw new Exception("Professional not found");

            return new UserAuthenticateResponseViewModel(mapper.Map<ProfessionalViewModel>(_professional), TokenService.GenerateToken(_professional));
        }
    }
}
