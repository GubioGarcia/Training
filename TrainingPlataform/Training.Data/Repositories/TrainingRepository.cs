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
    public class TrainingRepository : Repository<Domain.Entities.Training>, ITrainingRepository
    {
        public TrainingRepository(TrainingContext context)
            : base(context) { }

        public IEnumerable<Domain.Entities.Training> GetAll()
        {
            return Query(x => !x.IsDeleted);
        }
    }
}
