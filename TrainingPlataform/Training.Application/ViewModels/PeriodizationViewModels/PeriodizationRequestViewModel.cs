using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.ViewModels.PeriodizationViewModels
{
    public class PeriodizationRequestViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int WeeklyTrainingFrequency { get; set; }
        public Guid? ClientId { get; set; }
    }
}
