using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Domain.Entities;

namespace Training.Application.ViewModels
{
    public class PeriodizationTrainingViewModel
    {
        public Guid Id { get; set; }
        public Guid PeriodizationId { get; set; }
        public Guid TrainingId { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
