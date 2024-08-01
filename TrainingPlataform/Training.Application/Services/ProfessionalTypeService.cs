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
    public class ProfessionalTypeService : IProfessionalTypeService
    {
        private readonly IProfessionalTypeRepository professionalTypeRepository;
        private readonly IMapper mapper;

        public ProfessionalTypeService(IProfessionalTypeRepository professionalTypeRepository, IMapper mapper)
        {
            this.professionalTypeRepository = professionalTypeRepository;
            this.mapper = mapper;
        }

        public List<ProfessionalTypeViewModel> Get()
        {
            List<ProfessionalTypeViewModel> _professionalTypeViewModels = new List<ProfessionalTypeViewModel>();

            IEnumerable<ProfessionalType> _professionalTypes = this.professionalTypeRepository.GetAll();

            _professionalTypeViewModels = mapper.Map<List<ProfessionalTypeViewModel>>(_professionalTypes);

            return _professionalTypeViewModels;
        }

        public ProfessionalTypeViewModel GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid professionalTypeId))
                throw new Exception("Id is not valid");

            ProfessionalType _professionalType = this.professionalTypeRepository.Find(x => x.Id == professionalTypeId && !x.IsDeleted);
            if (_professionalType == null)
                throw new Exception("Professional type not found");

            return mapper.Map<ProfessionalTypeViewModel>(_professionalType);
        }

        public bool Post(ProfessionalTypeViewModel professionalTypeViewModel)
        {
            ProfessionalType _professionalType = mapper.Map<ProfessionalType>(professionalTypeViewModel);

            this.professionalTypeRepository.Create(_professionalType);

            return true;
        }
    }
}
