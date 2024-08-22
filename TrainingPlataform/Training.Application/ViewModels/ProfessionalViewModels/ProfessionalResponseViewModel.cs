using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.ViewModels.ProfessionalViewModels
{
    public class ProfessionalResponseViewModel : ProfessionalMinimalFieldViewModel
    {
        public string Password { get; set; }
        public Guid UsersTypeId { get; set; }
        public Guid ProfessionalTypeId { get; set; }
        public string Fone { get; set; }
        public string CurrentNumberClients { get; set; }
        public string? ProfessionalRegistration { get; set; }
        public string? UrlProfilePhoto { get; set; }
    }
}
