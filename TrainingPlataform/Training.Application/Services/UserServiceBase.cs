﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Template.CrossCutting.ExceptionHandler.Extensions;
using Training.Application.Interfaces;
using Training.Domain.Entities;
using Training.Domain.Interfaces;
using Training.Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Training.Application.Services
{
    public class UserServiceBase<TEntity> : IUserServiceBase<TEntity> where TEntity : class, IIdentifiable
    {
        protected readonly IRepository<TEntity> repository;
        protected readonly IUsersTypeRepository usersTypeRepository;

        public UserServiceBase(IRepository<TEntity> repository, IUsersTypeRepository usersTypeRepository)
        {
            this.repository = repository;
            this.usersTypeRepository = usersTypeRepository;
        }

        // verifica se o usuário logado é do tipo permitido de usuário com acesso ao método
        public bool IsLoggedInUserOfValidType(string id, string[] validUserTypes)
        {
            if (!Guid.TryParse(id, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            TEntity _user = this.repository.Find(x => x.Id == validId && !x.IsDeleted);
            if (_user == null)
                return false;

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == _user.UsersTypeId && !x.IsDeleted);
            if (_usersType == null)
                throw new ApiException("User type not found", HttpStatusCode.BadRequest);

            if (!validUserTypes.Contains(_usersType.Name, StringComparer.OrdinalIgnoreCase))
                return false;

            return true;
        }

        // retorna o UserType do id
        public string LoggedInUserType(string id)
        {
            if (!Guid.TryParse(id, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            TEntity _user = this.repository.Find(x => x.Id == validId && !x.IsDeleted);
            if (_user == null)
                return null;

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == _user.UsersTypeId && !x.IsDeleted);
            if (_usersType == null)
                return null;

            return _usersType.Name;
        }
    }
}
