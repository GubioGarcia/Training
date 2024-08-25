using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.ViewModels.ClientProfessionalViewModels
{
    public class ClientProfessionalRequestViewModel
    {
        public Guid ProfessionalId { get; set; }
        public Guid ClientId { get; set; }
        public string? DescriptionProfessional { get; set; }
    }
}
