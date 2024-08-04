using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels;

namespace Template.Application.ViewModels
{
    public class UserAuthenticateResponseViewModel
    {
        public UserAuthenticateResponseViewModel(ProfessionalViewModel user, string token)
        {
            this.User = user;
            this.Token = token;
        }

        // informações armazenadas pelo FrontEnd, seja em Local Storage ou em Cookies
        public ProfessionalViewModel User { get; set; }
        public string Token { get; set; }
    }
}
