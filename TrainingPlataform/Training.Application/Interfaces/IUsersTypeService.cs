﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels;

namespace Training.Application.Interfaces
{
    public interface IUsersTypeService
    {
        List<UsersTypeViewModel> Get();
        UsersTypeViewModel GetById(string id);
        bool Post(UsersTypeViewModel usersTypeViewModel);
        bool Put(UsersTypeViewModel usersTypeViewModel);
        bool Delete(string id);
    }
}
