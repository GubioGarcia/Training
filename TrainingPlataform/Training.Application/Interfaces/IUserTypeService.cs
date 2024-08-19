using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels;

namespace Training.Application.Interfaces
{
    public interface IUserTypeService
    {
        List<UserTypeViewModel> Get();
        UserTypeViewModel GetById(string id);
        bool Post(UserTypeViewModel userTypeViewModel);
        bool Put(UserTypeViewModel userTypeViewModel);
        bool Delete(string id);
    }
}
