﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.ViewModels.WorkoutCategoryViewModels
{
    public class WorkoutCategoryViewModel
    {
        public Guid Id { get; set; }
        public Guid? ProfessionalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
