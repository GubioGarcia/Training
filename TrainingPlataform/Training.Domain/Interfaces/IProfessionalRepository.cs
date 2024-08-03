﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Domain.Entities;

namespace Training.Domain.Interfaces
{
    public interface IProfessionalRepository : IRepository<Professional>
    {
        IEnumerable<Professional> GetAll();
    }
}
