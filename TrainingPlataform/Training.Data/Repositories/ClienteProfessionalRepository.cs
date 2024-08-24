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
    public class ClienteProfessionalRepository : Repository<ClientProfessional>, IClientProfessionalRepository
    {
        public ClienteProfessionalRepository(TrainingContext context) 
            : base(context) { }

        public IEnumerable<ClientProfessional> GetAll()
        {
            return Query(x => !x.IsDeleted);
        }
    }
}
