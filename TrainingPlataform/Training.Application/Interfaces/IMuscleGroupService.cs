﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels;

namespace Training.Application.Interfaces
{
    public interface IMuscleGroupService
    {
        List<MuscleGroupViewModel> Get(string tokenId);
    }
}
