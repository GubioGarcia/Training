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
        private readonly IMapper mapper;

        public UsersTypeService(IUsersTypeRepository usersTypeRepository, IMapper mapper)
        {
            this.usersTypeRepository = usersTypeRepository;
            this.mapper = mapper;
        }

        public List<UsersTypeViewModel> Get()
        {
            List<UsersTypeViewModel> _usersTypeViewModels = new List<UsersTypeViewModel>();

            IEnumerable<UsersType> _usersTypes = this.usersTypeRepository.GetAll();

            _usersTypeViewModels = mapper.Map<List<UsersTypeViewModel>>(_usersTypes);

            return _usersTypeViewModels;
        }

        public UsersTypeViewModel GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
                throw new Exception("Id is not valid");

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == userId && !x.IsDeleted);
            if (_usersType == null)
                throw new Exception("User type not found");

            return mapper.Map<UsersTypeViewModel>(_usersType);
        }

        public bool Post(UsersTypeViewModel usersTypeViewModel)
        {
            UsersType _usersType = mapper.Map<UsersType>(usersTypeViewModel);

            this.usersTypeRepository.Create(_usersType);

            return true;
        }

        public bool Put(UsersTypeViewModel usersTypeViewModel)
        {
            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == usersTypeViewModel.Id && !x.IsDeleted);
            if (_usersType == null)
                throw new Exception("User not found");

            _usersType = mapper.Map<UsersType>(usersTypeViewModel);

            this.usersTypeRepository.Update(_usersType);

            return true;
        }

        public bool Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
                throw new Exception("Id is not valid");

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == userId && !x.IsDeleted);
            if (_usersType == null)
                throw new Exception("User type not found");

            return this.usersTypeRepository.Delete(_usersType);
        }
    }
}
