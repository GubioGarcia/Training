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
    }
}
