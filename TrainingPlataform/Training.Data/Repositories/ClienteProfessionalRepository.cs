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
    public class ClienteProfessionalRepository : Repository<ClientProfessional>, IClientProfessionalRepository
    {
        public ClienteProfessionalRepository(TrainingContext context) 
            : base(context) { }

        public IEnumerable<ClientProfessional> GetAll()
        {
            return Query(x => !x.IsDeleted);
        }

        public bool Delete(ClientProfessional model)
        {
            try
            {
                if (model is ClientProfessional)
                {
                    (model as ClientProfessional).IsDeleted = true;
                    EntityEntry<ClientProfessional> _entry = _context.Entry(model);

                    DbSet.Attach(model);

                    _entry.State = EntityState.Modified;
                }
                else
                {
                    EntityEntry<ClientProfessional> _entry = _context.Entry(model);
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
