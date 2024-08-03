using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.ViewModels
{
    public class ProfessionalViewModel : UsersDefaultViewModel
    {
        public string Fone { get; set; }
        public int CurrentNumberClients { get; set; }
        public string UrlProfilePhoto { get; set; }
    }
}
