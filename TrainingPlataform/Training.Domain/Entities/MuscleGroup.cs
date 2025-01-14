using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Domain.Entities
{
    public class MuscleGroup
    {
        public Guid Id { get; set; }
        public Guid? ProfessionalId { get; set; }
        public Professional Professional { get; set; }

        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
