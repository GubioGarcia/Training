using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Domain.Models
{
    public interface IIdentifiable
    {
        Guid Id { get; }
        Guid UsersTypeId { get; }
        bool IsDeleted { get; }
    }
}
