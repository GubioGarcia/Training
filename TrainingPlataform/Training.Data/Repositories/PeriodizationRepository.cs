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
    public class PeriodizationRepository : Repository<Periodization>, IPeriodizationRepository
    {
        public PeriodizationRepository(TrainingContext context) : base(context) { }

        public IEnumerable<Periodization> GetAll()
        {
            return Query(x => !x.IsDeleted);
        }

        public bool IsDeleted(Periodization model)
        {
            try
            {
                model.IsDeleted = true;
                EntityEntry<Periodization> entry = _context.Entry(model);

                DbSet.Attach(model);
                entry.State = EntityState.Modified; // Marca como modificado para salvar

                return Save() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
