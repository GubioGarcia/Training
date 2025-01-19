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
    public class ExerciseService : IExerciseService
    {

        private readonly IUserServiceBase<Professional> userServiceBase;
        private readonly IExerciseRepository exerciseRepository;
        private readonly IMapper mapper;

        public ExerciseService(IUserServiceBase<Professional> userServiceBase, IExerciseRepository exerciseRepository, IMapper mapper)
        {
            this.userServiceBase = userServiceBase;
            this.exerciseRepository = exerciseRepository;
            this.mapper = mapper;
        }

        public List<ExerciseViewModel> Get(string tokenId)
        {
            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBase.IsLoggedInUserOfValidType(tokenId, ["Admin"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            try
            {
                List<ExerciseViewModel> _exerciseViewModels = [];

                IEnumerable<Exercise> _exercises = this.exerciseRepository.GetAll();

                _exerciseViewModels = mapper.Map<List<ExerciseViewModel>>(_exercises);

                return _exerciseViewModels;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
