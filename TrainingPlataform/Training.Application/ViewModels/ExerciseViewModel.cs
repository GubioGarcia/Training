﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.ViewModels
{
    public class ExerciseViewModel
    {
        public Guid Id { get; set; }
        public Guid WorkoutCategoryId { get; set; }
        public Guid MuscleGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UrlMedia { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
