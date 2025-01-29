using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Template.CrossCutting.ExceptionHandler.Extensions;
using Training.Application.Interfaces;
using Training.Application.ViewModels.WorkoutCategoryViewModels;
using Training.Domain.Entities;
using Training.Domain.Interfaces;

namespace Training.Application.Services
{
    public class WorkoutCategoryService : IWorkoutCategoryService
    {
        private readonly IUserServiceBase<Professional> userServiceBase;
        private readonly IWorkoutCategoryRepository workoutCategoryRepository;
        private readonly IMapper mapper;

        public WorkoutCategoryService(IUserServiceBase<Professional> userServiceBase, IWorkoutCategoryRepository workoutCategoryRepository, IMapper mapper)
        {
            this.userServiceBase = userServiceBase;
            this.workoutCategoryRepository = workoutCategoryRepository;
            this.mapper = mapper;
        }

        public List<WorkoutCategoryViewModel> Get(string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            try
            {
                List<WorkoutCategoryViewModel> _workoutCategoryViewModels = [];

                IEnumerable<WorkoutCategory> _workoutCategorys = this.workoutCategoryRepository.GetAll();

                _workoutCategoryViewModels = mapper.Map<List<WorkoutCategoryViewModel>>(_workoutCategorys);

                return _workoutCategoryViewModels;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        //public WorkoutCategoryViewModel Post(string tokenId, WorkoutCategoryRequestViewModel workoutCategoryRequestViewModel)
        //{
        //    // Valida tipo de usuário com acesso ao método
        //    if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
        //        throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);
        //}
    }
}
