using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels.ClientViewModels;
using Training.Domain.Entities;

namespace Training.Application.Mapper
{
    public class ManualMapperSetup
    {
        #region ViewModelToDomain

        public void MapClientUpdateRequestToClient(ClientUpdateRequestViewModel source, Client destination)
        {
            if (source.Cpf != null && source.Cpf != "")
                destination.Cpf = source.Cpf;

            if (source.Password != null && source.Cpf != "")
                destination.Password = source.Password;

            if (source.Name != null && source.Name != "")
                destination.Name = source.Name;

            if (source.Fone != null && source.Fone != "")
                destination.Fone = source.Fone;

            if (source.DateBirth != null)
                destination.DateBirth = (DateTime)source.DateBirth;

            if (source.InitialObjective != null && source.InitialObjective != "")
                destination.InitialObjective = source.InitialObjective;

            if (source.Height != null && source.Height != 0)
                destination.Height = source.Height.Value;

            if (source.StartingWeight != null && source.StartingWeight != 0)
                destination.StartingWeight = source.StartingWeight.Value;

            if (source.CurrentWeight != null && source.CurrentWeight != 0)
                destination.CurrentWeight = source.CurrentWeight.Value;

            if (source.UrlProfilePhoto != null && source.UrlProfilePhoto != "")
                destination.UrlProfilePhoto = source.UrlProfilePhoto;

            if (source.IsActive != null)
                destination.IsActive = source.IsActive.Value;
        }

        #endregion
    }
}
