using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.ViewModels.ProfessionalViewModels
{
    public class ProfessionalRequestUpdateViewModel
    {
        public Guid Id { get; set; }
        public Guid? ProfessionalTypeId { get; set; }
        public string? Cpf { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Fone { get; set; }
        public int? CurrentNumberClients { get; set; }
        public string? ProfessionalRegistration { get; set; }
        public string? UrlProfilePhoto { get; set; }
        public bool? IsActive { get; set; }
    }
}
