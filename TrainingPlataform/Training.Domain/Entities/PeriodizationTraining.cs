using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Domain.Entities
{
    public sealed class PeriodizationTraining
    {
        public Guid Id { get; set; }
        public Guid PeriodizationId { get; set; }
        public Guid TrainingId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsDeleted { get; set; } = false;

        public Periodization Periodization { get; set; }
        public Training Training { get; set; }
    }
}