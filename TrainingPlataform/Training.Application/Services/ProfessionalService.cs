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

            _professional.DateRegistration = DateTime.Now;

            this.professionalRepository.Create(_professional);

            return true;
        }
    }
}
