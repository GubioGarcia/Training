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
    }
}
