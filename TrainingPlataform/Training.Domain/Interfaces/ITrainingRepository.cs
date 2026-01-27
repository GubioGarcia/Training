using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Domain.Entities;

namespace Training.Domain.Interfaces
{
    public interface ITrainingRepository : IRepository<Entities.Training>
    {
        IEnumerable<Entities.Training> GetAll();
        bool IsDeleted(Domain.Entities.Training model);
    }
}
