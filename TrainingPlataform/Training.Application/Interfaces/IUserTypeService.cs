using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels;

namespace Training.Application.Interfaces
{
    public interface IUsersTypeService
    {
        List<UsersTypeViewModel> Get(string tokenId);
        UsersTypeViewModel GetById(string tokenId, string id);
        bool Post(string tokenId, UsersTypeViewModel usersTypeViewModel);
        bool Put(string tokenId, UsersTypeViewModel usersTypeViewModel);
        bool Delete(string tokenId, string id);
    }
}
