﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels;

namespace Training.Application.Interfaces
{
    public interface ITrainingService
    {
        List<TrainingViewModel> Get(string tokenId);
    }
}
