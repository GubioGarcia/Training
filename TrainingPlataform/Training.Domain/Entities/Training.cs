using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Domain.Entities
{
    public class Training
    {
        public Guid Id { get; set; }
        public Guid PeriodizationId { get; set; }
        public PeriodizationTraining PeriodizationTraining { get; set; }

        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime DateUpdated { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
