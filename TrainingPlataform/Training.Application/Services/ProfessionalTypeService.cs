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
        private readonly IProfessionalService professionalService;
        private readonly IChecker checker;
        private readonly IMapper mapper;

        public ProfessionalTypeService(IProfessionalTypeRepository professionalTypeRepository, IMapper mapper,
                                       IProfessionalService professionalService, IChecker checker)
        {
            this.professionalTypeRepository = professionalTypeRepository;
            this.mapper = mapper;
            this.professionalService = professionalService;
            this.checker = checker;
        }

        public List<ProfessionalTypeViewModel> Get(string tokenId)
        {
            this.checker.IsValidUserType(this.professionalService.PullUsersTypeId(tokenId), "Admin");

            List<ProfessionalTypeViewModel> _professionalTypeViewModels = new List<ProfessionalTypeViewModel>();

            IEnumerable<ProfessionalType> _professionalTypes = this.professionalTypeRepository.GetAll();

            _professionalTypeViewModels = mapper.Map<List<ProfessionalTypeViewModel>>(_professionalTypes);

            return _professionalTypeViewModels;
        }

        public ProfessionalTypeViewModel GetById(string tokenId, string id)
        {
            this.checker.IsValidUserType(this.professionalService.PullUsersTypeId(tokenId), "Admin");

            if (!Guid.TryParse(id, out Guid professionalTypeId))
                throw new Exception("Id is not valid");

            ProfessionalType _professionalType = this.professionalTypeRepository.Find(x => x.Id == professionalTypeId && !x.IsDeleted);
            if (_professionalType == null)
                throw new Exception("Professional type not found");

            return mapper.Map<ProfessionalTypeViewModel>(_professionalType);
        }

        public bool Post(string tokenId, ProfessionalTypeViewModel professionalTypeViewModel)
        {
            this.checker.IsValidUserType(this.professionalService.PullUsersTypeId(tokenId), "Admin");

            ProfessionalType _professionalType = mapper.Map<ProfessionalType>(professionalTypeViewModel);

            this.professionalTypeRepository.Create(_professionalType);

            return true;
        }

        public bool Put(string tokenId, ProfessionalTypeViewModel professionalTypeViewModel)
        {
            this.checker.IsValidUserType(this.professionalService.PullUsersTypeId(tokenId), "Admin");

            ProfessionalType _professionalType = this.professionalTypeRepository.Find(x => x.Id == professionalTypeViewModel.Id && !x.IsDeleted);
            if (_professionalType == null)
                throw new Exception("Professional type not found");

            _professionalType = mapper.Map<ProfessionalType>(professionalTypeViewModel);

            this.professionalTypeRepository.Update(_professionalType);

            return true;
        }

        public bool Delete(string tokenId, string id)
        {
            this.checker.IsValidUserType(this.professionalService.PullUsersTypeId(tokenId), "Admin");

            if (!Guid.TryParse(id, out Guid professionalTypeId))
                throw new Exception("Id is not valid");

            ProfessionalType _professionalType = this.professionalTypeRepository.Find(x => x.Id == professionalTypeId && !x.IsDeleted);
            if (_professionalType == null)
                throw new Exception("Professional type not found");

            this.professionalTypeRepository.Delete(_professionalType);

            return true;
        }
    }
}
