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
                throw new Exception("You are not authorized to perform this operation");

            List<UsersTypeViewModel> _usersTypeViewModels = new List<UsersTypeViewModel>();

            IEnumerable<UsersType> _usersTypes = this.usersTypeRepository.GetAll();

            _usersTypeViewModels = mapper.Map<List<UsersTypeViewModel>>(_usersTypes);

            return _usersTypeViewModels;
        }

        public UsersTypeViewModel GetById(string tokenId, string id)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new Exception("You are not authorized to perform this operation");

            if (!Guid.TryParse(id, out Guid userId))
                throw new Exception("Id is not valid");

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == userId && !x.IsDeleted);
            if (_usersType == null)
                throw new Exception("User type not found");

            return mapper.Map<UsersTypeViewModel>(_usersType);
        }

        public bool Post(string tokenId, UsersTypeViewModel usersTypeViewModel)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new Exception("You are not authorized to perform this operation");

            UsersType _usersType = mapper.Map<UsersType>(usersTypeViewModel);

            this.usersTypeRepository.Create(_usersType);

            return true;
        }

        public bool Put(string tokenId, UsersTypeViewModel usersTypeViewModel)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new Exception("You are not authorized to perform this operation");

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == usersTypeViewModel.Id && !x.IsDeleted);
            if (_usersType == null)
                throw new Exception("User not found");

            _usersType = mapper.Map<UsersType>(usersTypeViewModel);

            this.usersTypeRepository.Update(_usersType);

            return true;
        }

        public bool Delete(string tokenId, string id)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new Exception("You are not authorized to perform this operation");

            if (!Guid.TryParse(id, out Guid userId))
                throw new Exception("Id is not valid");

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == userId && !x.IsDeleted);
            if (_usersType == null)
                throw new Exception("User type not found");

            return this.usersTypeRepository.Delete(_usersType);
        }
    }
}
