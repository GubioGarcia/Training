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
        private readonly UserServiceBase<Professional> userServiceBase;
        private readonly IChecker checker;
        private readonly IMapper mapper;

        public ProfessionalTypeService(IProfessionalTypeRepository professionalTypeRepository, IMapper mapper,
                                       IProfessionalService professionalService, IChecker checker, UserServiceBase<Professional> userServiceBase)
        {
            this.professionalTypeRepository = professionalTypeRepository;
            this.professionalService = professionalService;
            this.userServiceBase = userServiceBase;
            this.checker = checker;
            this.mapper = mapper;
        }

        public List<ProfessionalTypeViewModel> Get(string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new Exception("You are not authorized to perform this operation");

            List<ProfessionalTypeViewModel> _professionalTypeViewModels = new List<ProfessionalTypeViewModel>();

            IEnumerable<ProfessionalType> _professionalTypes = this.professionalTypeRepository.GetAll();

            _professionalTypeViewModels = mapper.Map<List<ProfessionalTypeViewModel>>(_professionalTypes);

            return _professionalTypeViewModels;
        }

        public ProfessionalTypeViewModel GetById(string tokenId, string id)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new Exception("You are not authorized to perform this operation");

            if (!Guid.TryParse(id, out Guid professionalTypeId))
                throw new Exception("Id is not valid");

            ProfessionalType _professionalType = this.professionalTypeRepository.Find(x => x.Id == professionalTypeId && !x.IsDeleted);
            if (_professionalType == null)
                throw new Exception("Professional type not found");

            return mapper.Map<ProfessionalTypeViewModel>(_professionalType);
        }

        public bool Post(string tokenId, ProfessionalTypeViewModel professionalTypeViewModel)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new Exception("You are not authorized to perform this operation");

            ProfessionalType _professionalType = mapper.Map<ProfessionalType>(professionalTypeViewModel);

            this.professionalTypeRepository.Create(_professionalType);

            return true;
        }

        public bool Put(string tokenId, ProfessionalTypeViewModel professionalTypeViewModel)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new Exception("You are not authorized to perform this operation");

            ProfessionalType _professionalType = this.professionalTypeRepository.Find(x => x.Id == professionalTypeViewModel.Id && !x.IsDeleted);
            if (_professionalType == null)
                throw new Exception("Professional type not found");

            _professionalType = mapper.Map<ProfessionalType>(professionalTypeViewModel);

            this.professionalTypeRepository.Update(_professionalType);

            return true;
        }

        public bool Delete(string tokenId, string id)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new Exception("You are not authorized to perform this operation");

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
