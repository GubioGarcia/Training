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
    public class WorkoutCategoryRepository : Repository<WorkoutCategory>, IWorkoutCategoryRepository
    {
        public WorkoutCategoryRepository(TrainingContext context) : base(context) { }

        public IEnumerable<WorkoutCategory> GetAll()
        {
            return Query(x => !x.IsDeleted);
        }
    }
}
