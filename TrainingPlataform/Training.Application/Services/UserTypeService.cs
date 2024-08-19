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
    public class UserTypeService : IUserTypeService
    {
        private readonly IUserTypeRepository userTypeRepository;
        private readonly IMapper mapper;

        public UserTypeService(IUserTypeRepository userTypeRepository, IMapper mapper)
        {
            this.userTypeRepository = userTypeRepository;
            this.mapper = mapper;
        }

        public List<UserTypeViewModel> Get()
        {
            List<UserTypeViewModel> _userTypeViewModels = new List<UserTypeViewModel>();

            IEnumerable<UserType> _userTypes = this.userTypeRepository.GetAll();

            _userTypeViewModels = mapper.Map<List<UserTypeViewModel>>(_userTypes);

            return _userTypeViewModels;
        }

        public UserTypeViewModel GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
                throw new Exception("Id is not valid");

            UserType _userType = this.userTypeRepository.Find(x => x.Id == userId && !x.IsDeleted);
            if (_userType == null)
                throw new Exception("User type not found");

            return mapper.Map<UserTypeViewModel>(_userType);
        }

        public bool Post(UserTypeViewModel userTypeViewModel)
        {
            UserType _userType = mapper.Map<UserType>(userTypeViewModel);

            this.userTypeRepository.Create(_userType);

            return true;
        }

        public bool Put(UserTypeViewModel userTypeViewModel)
        {
            UserType _userType = this.userTypeRepository.Find(x => x.Id == userTypeViewModel.Id && !x.IsDeleted);
            if (_userType == null)
                throw new Exception("User not found");

            _userType = mapper.Map<UserType>(userTypeViewModel);

            this.userTypeRepository.Update(_userType);

            return true;
        }

        public bool Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
                throw new Exception("Id is not valid");

            UserType _userType = this.userTypeRepository.Find(x => x.Id == userId && !x.IsDeleted);
            if (_userType == null)
                throw new Exception("User type not found");

            return this.userTypeRepository.Delete(_userType);
        }
    }
}
