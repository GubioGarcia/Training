﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.ViewModels
{
    public class ClientMinimalFieldViewModel
    {
        public Guid Id { get; set; }
        public Guid UserTypeId { get; set; }
        public string Cpf { get; set; }
        public string Name { get; set; }
    }
}