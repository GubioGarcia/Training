using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Domain.Entities
{
    public class PeriodizationTraining
    {
        public Guid Id { get; set; }
        public Guid ProfessionalId { get; set; }
        public Professional Professional { get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; }

        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int WeeklyTrainingFrequency { get; set; }
        public DateTime DateUpdated { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;

        public ICollection<Training> Trainings { get; set; } = [];
    }
}