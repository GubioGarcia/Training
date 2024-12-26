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
using Training.Domain.Models;

namespace Training.Application.Services
{
    public class UsersTypeService : IUsersTypeService
    {
        private readonly IUsersTypeRepository usersTypeRepository;
        private readonly IProfessionalService professionalService;
        private readonly UserServiceBase<Professional> userServiceBase;
        private readonly IChecker checker;
        private readonly IMapper mapper;

        public UsersTypeService(IUsersTypeRepository usersTypeRepository, IProfessionalService professionalService,
                                IChecker checker, IMapper mapper, UserServiceBase<Professional> userServiceBase)
        {
            this.usersTypeRepository = usersTypeRepository;
            this.professionalService = professionalService;
            this.userServiceBase = userServiceBase;
            this.checker = checker;
            this.mapper = mapper;
        }

        public List<UsersTypeViewModel> Get(string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            try
            {
                List<UsersTypeViewModel> _usersTypeViewModels = new List<UsersTypeViewModel>();

                IEnumerable<UsersType> _usersTypes = this.usersTypeRepository.GetAll();

                _usersTypeViewModels = mapper.Map<List<UsersTypeViewModel>>(_usersTypes);

                return _usersTypeViewModels;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public UsersTypeViewModel GetById(string tokenId, string id)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            if (!Guid.TryParse(id, out Guid userId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == userId && !x.IsDeleted);
            if (_usersType == null)
                throw new ApiException("User type not found", HttpStatusCode.NotFound);

            return mapper.Map<UsersTypeViewModel>(_usersType);
        }

        public bool Post(string tokenId, UsersTypeViewModel usersTypeViewModel)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            try
            {
                UsersType _usersType = mapper.Map<UsersType>(usersTypeViewModel);

                this.usersTypeRepository.Create(_usersType);

                return true;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public bool Put(string tokenId, UsersTypeViewModel usersTypeViewModel)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == usersTypeViewModel.Id && !x.IsDeleted);
            if (_usersType == null)
                throw new ApiException("User not found", HttpStatusCode.NotFound);

            try
            {
                _usersType = mapper.Map<UsersType>(usersTypeViewModel);

                this.usersTypeRepository.Update(_usersType);

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

            if (!Guid.TryParse(id, out Guid userId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == userId && !x.IsDeleted);
            if (_usersType == null)
                throw new ApiException("User type not found", HttpStatusCode.NotFound);

            return this.usersTypeRepository.Delete(_usersType);
        }
    }
}
