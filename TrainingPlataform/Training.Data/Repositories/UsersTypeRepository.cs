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
    public class UsersTypeRepository : Repository<UsersType>, IUsersTypeRepository
    {
        public UsersTypeRepository(TrainingContext context)
            : base(context) { }

        public IEnumerable<UsersType> GetAll()
        {
            return Query(x => !x.IsDeleted);
        }

        public bool Delete(UsersType model)
        {
            try
            {
                if (model is UsersType)
                {
                    (model as UsersType).IsDeleted = true;
                    EntityEntry<UsersType> _entry = _context.Entry(model);

                    DbSet.Attach(model);

                    _entry.State = EntityState.Modified;
                }
                else
                {
                    EntityEntry<UsersType> _entry = _context.Entry(model);
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
