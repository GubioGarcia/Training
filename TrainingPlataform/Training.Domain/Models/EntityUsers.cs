using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Domain.Entities;

namespace Training.Domain.Models
{
    public class EntityUsers
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string Cpf { get; set; }
        public string Name { get; set; }
        public DateTime DateRegistration { get; set; }
        public string Fone { get; set; }
        public string? UrlProfilePhoto { get; set; }
    }
}
