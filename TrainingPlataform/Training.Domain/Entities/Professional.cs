using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Domain.Interfaces;
using Training.Domain.Models;

namespace Training.Domain.Entities
{
    public sealed class Professional : EntityUsers
    {
        public Guid ProfessionalTypesId { get; set; }
        public ProfessionalType ProfessionalType { get; set; }
        public string? ProfessionalRegistration { get; set; }
        public int CurrentNumberClients { get; set; }

        public ICollection<WorkoutCategory> WorkoutCategorys { get; set; }
        public ICollection<MuscleGroup> MuscleGroups { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
        public ICollection<Training> Trainings { get; set; } = [];
    }
}
