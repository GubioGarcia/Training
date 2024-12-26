using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Template.CrossCutting.ExceptionHandler.Extensions;
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
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            try
            {
                List<ProfessionalTypeViewModel> _professionalTypeViewModels = new List<ProfessionalTypeViewModel>();

                IEnumerable<ProfessionalType> _professionalTypes = this.professionalTypeRepository.GetAll();

                _professionalTypeViewModels = mapper.Map<List<ProfessionalTypeViewModel>>(_professionalTypes);

                return _professionalTypeViewModels;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public ProfessionalTypeViewModel GetById(string tokenId, string id)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            if (!Guid.TryParse(id, out Guid professionalTypeId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            ProfessionalType _professionalType = this.professionalTypeRepository.Find(x => x.Id == professionalTypeId && !x.IsDeleted);
            if (_professionalType == null)
                throw new ApiException("Professional type not found", HttpStatusCode.NotFound);

            return mapper.Map<ProfessionalTypeViewModel>(_professionalType);
        }

        public bool Post(string tokenId, ProfessionalTypeViewModel professionalTypeViewModel)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            try
            {
                ProfessionalType _professionalType = mapper.Map<ProfessionalType>(professionalTypeViewModel);

                this.professionalTypeRepository.Create(_professionalType);

                return true;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public bool Put(string tokenId, ProfessionalTypeViewModel professionalTypeViewModel)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            ProfessionalType _professionalType = this.professionalTypeRepository.Find(x => x.Id == professionalTypeViewModel.Id && !x.IsDeleted);
            if (_professionalType == null)
                throw new ApiException("Professional type not found", HttpStatusCode.NotFound);

            try
            {
                _professionalType = mapper.Map<ProfessionalType>(professionalTypeViewModel);

                this.professionalTypeRepository.Update(_professionalType);

                return true;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public bool Delete(string tokenId, string id)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            if (!Guid.TryParse(id, out Guid professionalTypeId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            ProfessionalType _professionalType = this.professionalTypeRepository.Find(x => x.Id == professionalTypeId && !x.IsDeleted);
            if (_professionalType == null)
                throw new ApiException("Professional type not found", HttpStatusCode.NotFound);

            this.professionalTypeRepository.Delete(_professionalType);

            return true;
        }
    }
}
