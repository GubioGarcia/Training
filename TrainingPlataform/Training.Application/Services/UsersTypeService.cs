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

        public UsersTypeService(IUsersTypeRepository usersTypeRepository)
        {
            this.usersTypeRepository = usersTypeRepository;
        }

        public List<UsersTypeViewModel> Get()
        {
            List<UsersTypeViewModel> _usersTypeViewModels = new List<UsersTypeViewModel>();

            IEnumerable<UsersType> _usersTypes = this.usersTypeRepository.GetAll();
            foreach (var item in _usersTypes)
                _usersTypeViewModels.Add(new UsersTypeViewModel { Id = item.Id, Name = item.Name, Description = item.Description});

            return _usersTypeViewModels;
        }
    }
}
