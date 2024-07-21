using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Data.Context;
using Training.Domain.Entities;
using Training.Domain.Interfaces;

namespace Training.Data.Repositories
{
    public class UsersTypeRepository : Repository<UsersType>, IUsersTypeRepository
    {
        public UsersTypeRepository(TrainingContext context)
            : base(context) { }

        public IEnumerable<UsersType> GetAll()
        {
            return Query(x => !x.IsDeleted);
        }
    }
}
