using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.ViewModels
{
    public class ProfessionalViewModel : UsersDefaultViewModel
    {
        public Guid UsersTypeId { get; set; }
        public Guid ProfessionalTypesId { get; set; }
        public string Cpf { get; set; }
        public string Password { get; set; }
        public string Fone { get; set; }
        public int CurrentNumberClients { get; set; }
        public string? UrlProfilePhoto { get; set; }
    }
}
