using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Domain.Entities;
using Training.Domain.Interfaces;

namespace Training.Domain.Models
{
    public class EntityUsers : IIdentifiable
    {
        public Guid Id { get; set; }
        public Guid UsersTypeId { get; set; }
        public UsersType UsersType { get; set; }
        public bool IsActive { get; set; } = true;
        public string Cpf { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime DateRegistration { get; set; } = DateTime.Now;
        public string Fone { get; set; }
        public string? UrlProfilePhoto { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsDeleted { get; set; } = false;

        public ICollection<ClientProfessional> ClientProfessionals { get; set; } = [];
        public ICollection<PeriodizationTraining> PeriodizationTrainings { get; set; } = [];
    }
}
