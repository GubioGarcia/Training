using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public class ProfessionalTypeRepository : Repository<ProfessionalType>, IProfessionalTypeRepository
    {
        public ProfessionalTypeRepository(TrainingContext context)
            : base(context) { }

        public IEnumerable<ProfessionalType> GetAll()
        {
            return Query(x => !x.IsDeleted);
        }

        public bool Delete(ProfessionalType model)
        {
            try
            {
                if (model is ProfessionalType)
                {
                    (model as ProfessionalType).IsDeleted = true;
                    EntityEntry<ProfessionalType> _entry = _context.Entry(model);

                    DbSet.Attach(model);

                    _entry.State = EntityState.Modified;
                }
                else
                {
                    EntityEntry<ProfessionalType> _entry = _context.Entry(model);
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
