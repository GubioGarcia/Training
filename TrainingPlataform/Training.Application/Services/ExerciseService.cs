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
using Training.Application.ViewModels.MuscleGroupViewModels;
using Training.Domain.Entities;
using Training.Domain.Interfaces;

namespace Training.Application.Services
{
    public class ExerciseService : IExerciseService
    {

        private readonly IUserServiceBase<Professional> userServiceBaseProfessional;
        private readonly IProfessionalRepository professionalRepository;
        private readonly IExerciseRepository exerciseRepository;
        private readonly IMapper mapper;

        public ExerciseService(IUserServiceBase<Professional> userServiceBaseProfessional, IExerciseRepository exerciseRepository, IMapper mapper,
                               IProfessionalRepository professionalRepository)
        {
            this.userServiceBaseProfessional = userServiceBaseProfessional;
            this.professionalRepository = professionalRepository;
            this.exerciseRepository = exerciseRepository;
            this.mapper = mapper;
        }

        public List<ExerciseViewModel> Get(string tokenId)
        {
            if (!Guid.TryParse(tokenId, out Guid validId))
                throw new ApiException("Id is not valid", HttpStatusCode.BadRequest);

            // Valida tipo de usuário com acesso ao método
            if (!this.userServiceBaseProfessional.IsLoggedInUserOfValidType(tokenId, ["Admin", "Professional"]))
                throw new ApiException("You are not authorized to perform this operation", HttpStatusCode.BadRequest);

            // Recebe o tipo do usuário logado e instância profissional logado
            string loggedInUserType = this.userServiceBaseProfessional.LoggedInUserType(tokenId);
            Professional _professionalLogged = this.professionalRepository.Find(x => x.Id == validId && !x.IsDeleted)
                                                ?? throw new ApiException("Professional not found", HttpStatusCode.NotFound);
            //try
            //{
            //    List<ExerciseViewModel> _exerciseViewModels = [];

            //    IEnumerable<Exercise> _exercises = this.exerciseRepository.GetAll();

            //    _exerciseViewModels = mapper.Map<List<ExerciseViewModel>>(_exercises);

            //    return _exerciseViewModels;
            //}

            try
            {
                List<Exercise> _exercises;

                if (loggedInUserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _exercises = [.. this.exerciseRepository.Query(x => !x.IsDeleted)];

                    if (_exercises.Count == 0)
                        throw new ApiException("Exercises not found", HttpStatusCode.NotFound);
                }
                else
                {
                    _exercises = [.. this.exerciseRepository.Query(x => x.ProfessionalId == _professionalLogged.Id && !x.IsDeleted)];

                    if (_exercises.Count == 0)
                        throw new ApiException("No exercises found for this professional", HttpStatusCode.NotFound);
                }

                return mapper.Map<List<ExerciseViewModel>>(_exercises);
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected error occurred: {ex.Message}", HttpStatusCode.InternalServerError);
            }

        }
    }
}
