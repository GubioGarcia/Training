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
    public class UsersTypeService : IUsersTypeService
    {
        private readonly IUsersTypeRepository usersTypeRepository;
        private readonly IProfessionalService professionalService;
        private readonly IChecker checker;
        private readonly IMapper mapper;

        public UsersTypeService(IUsersTypeRepository usersTypeRepository, IProfessionalService professionalService,
                                IChecker checker, IMapper mapper)
        {
            this.usersTypeRepository = usersTypeRepository;
            this.professionalService = professionalService;
            this.checker = checker;
            this.mapper = mapper;
        }

        public List<UsersTypeViewModel> Get(string tokenId)
        {
            this.checker.IsValidUserType(this.professionalService.PullUsersTypeId(tokenId), "Admin");

            List<UsersTypeViewModel> _usersTypeViewModels = new List<UsersTypeViewModel>();

            IEnumerable<UsersType> _usersTypes = this.usersTypeRepository.GetAll();

            _usersTypeViewModels = mapper.Map<List<UsersTypeViewModel>>(_usersTypes);

            return _usersTypeViewModels;
        }

        public UsersTypeViewModel GetById(string tokenId, string id)
        {
            this.checker.IsValidUserType(this.professionalService.PullUsersTypeId(tokenId), "Admin");

            if (!Guid.TryParse(id, out Guid userId))
                throw new Exception("Id is not valid");

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == userId && !x.IsDeleted);
            if (_usersType == null)
                throw new Exception("User type not found");

            return mapper.Map<UsersTypeViewModel>(_usersType);
        }

        public bool Post(string tokenId, UsersTypeViewModel usersTypeViewModel)
        {
            this.checker.IsValidUserType(this.professionalService.PullUsersTypeId(tokenId), "Admin");

            UsersType _usersType = mapper.Map<UsersType>(usersTypeViewModel);

            this.usersTypeRepository.Create(_usersType);

            return true;
        }

        public bool Put(string tokenId, UsersTypeViewModel usersTypeViewModel)
        {
            this.checker.IsValidUserType(this.professionalService.PullUsersTypeId(tokenId), "Admin");

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == usersTypeViewModel.Id && !x.IsDeleted);
            if (_usersType == null)
                throw new Exception("User not found");

            _usersType = mapper.Map<UsersType>(usersTypeViewModel);

            this.usersTypeRepository.Update(_usersType);

            return true;
        }

        public bool Delete(string tokenId, string id)
        {
            this.checker.IsValidUserType(this.professionalService.PullUsersTypeId(tokenId), "Admin");

            if (!Guid.TryParse(id, out Guid userId))
                throw new Exception("Id is not valid");

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == userId && !x.IsDeleted);
            if (_usersType == null)
                throw new Exception("User type not found");

            return this.usersTypeRepository.Delete(_usersType);
        }

        public bool IsValidUserType(Guid usersTypeId, string type)
        {
            UsersType _usersType = this.usersTypeRepository.Find(x => x.Name == type && !x.IsDeleted);
            if (usersTypeId != _usersType.Id)
                throw new Exception("You are not authorized to perform this operation");

            return true;
        }
    }
}
