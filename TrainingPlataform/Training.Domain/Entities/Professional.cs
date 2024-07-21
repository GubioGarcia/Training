using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Domain.Models;

namespace Training.Domain.Entities
{
    public class Professional: EntityUsers
    {
        public Guid UsersType_Id { get; set; }
        public UsersType UsersType { get; set; }
        public Guid ProfessionalTypes_Id { get; set; }
        public ProfessionalType ProfessionalType { get; set; }

        public string ProfessionalRegistration { get; set; }
        public int CurrentNumberClients { get; set; }
    }
}
