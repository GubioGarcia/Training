using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Domain.Entities
{
    public sealed class Training
    {
        public Guid Id { get; set; }
        public Guid PeriodizationTrainingId { get; set; }
        public PeriodizationTraining PeriodizationTraining { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
