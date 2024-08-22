using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Application.ViewModels.ClientViewModels
{
    public class ClientRequestUpdateViewModel
    {
        public Guid Id { get; set; }
        public string? Cpf { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Fone { get; set; }
        public DateTime? DateBirth { get; set; }
        public string? InitialObjective { get; set; }
        public decimal? Height { get; set; }
        public decimal? StartingWeight { get; set; }
        public decimal? CurrentWeight { get; set; }
        public string? UrlProfilePhoto { get; set; }
        public bool? IsActive { get; set; }
    }
}
