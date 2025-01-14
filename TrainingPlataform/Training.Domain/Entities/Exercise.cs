using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Domain.Entities
{
    public class Exercise
    {
        public Guid Id { get; set; }
        public Guid? ProfessionalId { get; set; }
        public Professional Professional { get; set; }
        public Guid WorkoutCategoryId { get; set; }
        public WorkoutCategory WorkoutCategory { get; set; }
        public Guid MuscleGroupId { get; set; }
        public MuscleGroup MuscleGroup { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string? UrlMedia { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
