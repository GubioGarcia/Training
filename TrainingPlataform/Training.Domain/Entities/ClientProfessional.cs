using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Domain.Entities
{
    public class ClientProfessional
    {
        public Guid Id { get; set; }
        public Guid ProfessionalId { get; set; }
        public Professional Professional { get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; }

        public string DescriptionProfessional { get; set; }
        public DateTime? DateUpdated { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
