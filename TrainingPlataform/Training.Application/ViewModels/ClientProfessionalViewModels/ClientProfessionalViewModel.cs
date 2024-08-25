using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.ViewModels.ClientProfessionalViewModels
{
    public class ClientProfessionalViewModel
    {
        public Guid Id { get; set; }
        public Guid ProfessionalId { get; set; }
        public string ProfessionalName { get; set; }
        public string ProfessionalCpf { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCpf { get; set; }
        public string DescriptionProfessional { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
