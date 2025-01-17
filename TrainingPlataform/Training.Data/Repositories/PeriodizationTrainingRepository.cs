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
    public class PeriodizationTrainingRepository : Repository<PeriodizationTraining>, IPeriodizationTrainingRepository
    {
        public PeriodizationTrainingRepository(TrainingContext context) : base(context) { }

        public IEnumerable<PeriodizationTraining> GetAll()
        {
            return Query(x => !x.IsDeleted);
        }
    }
}
