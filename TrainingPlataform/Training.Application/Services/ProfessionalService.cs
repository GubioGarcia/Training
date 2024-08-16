using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.Interfaces;
using Training.Domain.Entities;
using Training.Domain.Interfaces;
using Training.Auth.Services;
using Training.Application.ViewModels.ProfessionalViewModels;
using Training.Application.ViewModels.AuthenticateViewModels;

namespace Training.Application.Services
{
    public class ProfessionalService : IProfessionalService
    {
        private readonly IProfessionalRepository professionalRepository;
        private readonly IUsersTypeRepository usersTypeRepository;
        private readonly IProfessionalTypeRepository professionalTypeRepository;
        private readonly IMapper mapper;
        private readonly IChecker checker;

        public ProfessionalService(IProfessionalRepository professionalRepository, IUsersTypeRepository usersTypeRepository,
                                   IProfessionalTypeRepository professionalTypeRepository, IMapper mapper, IChecker checker) 
        {
            this.professionalRepository = professionalRepository;
            this.usersTypeRepository = usersTypeRepository;
            this.professionalTypeRepository = professionalTypeRepository;
            this.mapper = mapper;
            this.checker = checker;
        }

        public List<ProfessionalViewModel> Get()
        {
            List<ProfessionalViewModel> _professionalViewModels = new List<ProfessionalViewModel>();

            IEnumerable<Professional> _professionals = this.professionalRepository.GetAll();

            _professionalViewModels = mapper.Map<List<ProfessionalViewModel>>(_professionals);
            
            return _professionalViewModels;
        }

        public ProfessionalViewModel GetByid(string id)
        {
            if (!Guid.TryParse(id, out Guid professionalId))
                throw new Exception("Id is not valid");

            Professional _professional = this.professionalRepository.Find(x => x.Id == professionalId && !x.IsDeleted);
            if (_professional == null)
                throw new Exception("Professional not found");

            return mapper.Map<ProfessionalViewModel>(_professional);
        }

        public bool Post(ProfessionalViewModel professionalViewModel)
        {
            if (!checker.isValidCpf(professionalViewModel.Cpf))
                throw new Exception("CPF is not valid");

            Professional _professional = this.professionalRepository.Find(x => x.Cpf == professionalViewModel.Cpf && !x.IsDeleted);
            if (_professional != null)
                throw new Exception("There is already a professional registered with this CPF");

            if (!checker.isValidFone(professionalViewModel.Fone))
                throw new Exception("Phone is not valid");
            
            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == professionalViewModel.UsersTypeId && !x.IsDeleted);
            if (_usersType == null)
                throw new Exception("Id type users not found");

            ProfessionalType _professionalType = this.professionalTypeRepository.Find(x => x.Id == professionalViewModel.ProfessionalTypesId 
                                                                                     && !x.IsDeleted);
            if (_professionalType == null)
                throw new Exception("Id professional type not found");

            _professional = mapper.Map<Professional>(professionalViewModel);
            _professional.Password = this.HashPassword(_professional.Password);

            if (professionalViewModel.UrlProfilePhoto == "")
                _professional.UrlProfilePhoto = null;
            
            _professional.DateRegistration = DateTime.Now;

            this.professionalRepository.Create(_professional);

            return true;
        }

        public bool Put(ProfessionalViewModel professionalViewModel)
        {
            if (!checker.isValidCpf(professionalViewModel.Cpf))
                throw new Exception("CPF is not valid");

            Professional _professional = this.professionalRepository.Find(x => x.Id == professionalViewModel.Id && !x.IsDeleted);
            if (_professional == null)
                throw new Exception("Professional not found");

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == professionalViewModel.UsersTypeId && !x.IsDeleted);
            if (_usersType == null)
                throw new Exception("Id type users not found");

            ProfessionalType professionalType = this.professionalTypeRepository.Find(x => x.Id == professionalViewModel.ProfessionalTypesId
                                                                                     && !x.IsDeleted);
            if (professionalType == null)
                throw new Exception("Id professional type not found");

            if (!checker.isValidFone(professionalViewModel.Fone))
                throw new Exception("Phone is not valid");

            _professional = mapper.Map<Professional>(professionalViewModel);
            _professional.Password = this.HashPassword(_professional.Password);
            _professional.DateUpdated = DateTime.UtcNow;

            if (professionalViewModel.UrlProfilePhoto == "")
                _professional.UrlProfilePhoto = null;

            this.professionalRepository.Update(_professional);

            return true;
        }

        public bool Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid professionalId))
                throw new Exception("Id is not valid");

            Professional _professional = this.professionalRepository.Find(x => x.Id == professionalId && !x.IsDeleted);
            if (_professional == null)
                throw new Exception("Professional not found");

            return this.professionalRepository.Delete(_professional);
        }

        public UserAuthenticateResponseViewModel Authenticate(UserAuthenticateRequestViewModel professional)
        {
            if(string.IsNullOrEmpty(professional.Cpf) || string.IsNullOrEmpty(professional.Password))
                throw new Exception("CPF/Password is required");

            if (!checker.isValidCpf(professional.Cpf))
                throw new Exception("CPF is not valid");

            Professional _professional = this.professionalRepository.Find(x => !x.IsDeleted);

            if (_professional == null || !this.VerifyPassword(professional.Password, _professional.Password))
                throw new Exception("Invalid credentials");

            UsersType _usersType = this.usersTypeRepository.Find(x => x.Id == _professional.UsersTypeId && !x.IsDeleted);
            ProfessionalType _professionalType = this.professionalTypeRepository.Find(x => x.Id == _professional.ProfessionalTypesId && !x.IsDeleted );

            var (_token, _expiry) = TokenService.GenerateToken(_professional);

            return new UserAuthenticateResponseViewModel(_token, _expiry, _professional.Id, _professional.Cpf, _professional.Name,
                                                         _usersType.Name, _professionalType.Name);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
