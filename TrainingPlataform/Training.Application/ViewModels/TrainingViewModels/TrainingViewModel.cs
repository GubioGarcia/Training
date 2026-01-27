using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.ViewModels.TrainingViewModels
{
    public class TrainingViewModel
    {
        public Guid Id { get; set; }
        public Guid? ProfessionalId { get; set; } = Guid.Empty;
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
