using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.ViewModels.ClientViewModels
{
    public class ClientResponseViewModel : ClientMinimalFieldViewModel
    {
        public bool IsActive { get; set; }
        public DateTime DateRegistration { get; set; }
        public string Fone { get; set; }
        public string InitialObjective { get; set; }
    }
}
