using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Domain.Models;

namespace Training.Application.Interfaces
{
    public interface IUserServiceBase<TEntity> where TEntity : class, IIdentifiable
    {
        public bool IsLoggedInUserOfValidType(string id, string[] validUserTypes);
        public string LoggedInUserType(string id);
    }
}
