
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
    public class MuscleGroupRepository : Repository<MuscleGroup>, IMuscleGroupRepository
    {
        public MuscleGroupRepository(TrainingContext context)
            : base(context) { }

        public IEnumerable<MuscleGroup> GetAll()
        {
            return Query(x => !x.IsDeleted);
        }

        public bool IsDeleted(MuscleGroup model)
        {
            try
            {
                model.IsDeleted = true;
                EntityEntry<MuscleGroup> entry = _context.Entry(model);

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
