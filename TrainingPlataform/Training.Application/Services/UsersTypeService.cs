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

        public bool Post(UsersTypeViewModel usersTypeViewModel)
        {
            UsersType _usersType = mapper.Map<UsersType>(usersTypeViewModel);

            this.usersTypeRepository.Create(_usersType);

            return true;
        }
    }
}
