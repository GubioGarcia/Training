using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public class UserTypeRepository : Repository<UserType>, IUserTypeRepository
    {
        public UserTypeRepository(TrainingContext context)
            : base(context) { }

        public IEnumerable<UserType> GetAll()
        {
            return Query(x => !x.IsDeleted);
        }

        public bool Delete(UserType model)
        {
            try
            {
                if (model is UserType)
                {
                    (model as UserType).IsDeleted = true;
                    EntityEntry<UserType> _entry = _context.Entry(model);

                    DbSet.Attach(model);

                    _entry.State = EntityState.Modified;
                }
                else
                {
                    EntityEntry<UserType> _entry = _context.Entry(model);
                    DbSet.Attach(model);
                    _entry.State = EntityState.Deleted;
                }

                return Save() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
