using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.ViewModels.AuthenticateViewModels
{
    public class UserAuthenticateRequestViewModel
    {
        public string Cpf { get; set; }
        public string Password { get; set; }
    }
}