using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.ViewModels.ClientViewModels
{
    public class ClientRequestViewModel : ClientMinimalFieldViewModel
    {
        public string Password { get; set; }
        public string Fone { get; set; }
        public DateTime DateBirth { get; set; }
        public string InitialObjective { get; set; }
        public decimal Height { get; set; }
        public decimal StartingWeight { get; set; }
        public string? UrlProfilePhoto { get; set; }
    }
}
