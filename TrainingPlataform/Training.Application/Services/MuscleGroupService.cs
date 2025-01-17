using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Template.CrossCutting.ExceptionHandler.Extensions;
using Training.Application.Interfaces;
using Training.Application.ViewModels;
using Training.Domain.Entities;
using Training.Domain.Interfaces;

namespace Training.Application.Services
{
    public class MuscleGroupService : IMuscleGroupService
    {
        private readonly IUserServiceBase<Professional> userServiceBase;
        private readonly IMuscleGroupRepository muscleGroupRepository;
        private readonly IMapper mapper;

        public MuscleGroupService(IUserServiceBase<Professional> userServiceBase, IMuscleGroupRepository muscleGroupRepository,
                                  IMapper mapper)
        {
            this.userServiceBase = userServiceBase;
            this.muscleGroupRepository = muscleGroupRepository;
            this.mapper = mapper;
        }

        public List<MuscleGroupViewModel> Get(string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            try
            {
                List<MuscleGroupViewModel> _muscleGroupViewModels = [];

                IEnumerable<MuscleGroup> _muscleGroups = this.muscleGroupRepository.GetAll();

                _muscleGroupViewModels = mapper.Map<List<MuscleGroupViewModel>>(_muscleGroups);

                return _muscleGroupViewModels;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
