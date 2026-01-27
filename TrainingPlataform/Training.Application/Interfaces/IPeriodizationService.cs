using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels.PeriodizationViewModels;
using Training.Application.ViewModels.TrainingViewModels;

namespace Training.Application.Interfaces
{
    public interface IPeriodizationService
    {
        List<PeriodizationViewModel> Get(string tokenId);
        PeriodizationViewModel GetById(string tokenId, Guid id);
        List<PeriodizationViewModel> GetByProfessional(string tokenId, Guid id);
        List<PeriodizationViewModel> GetByClient(string tokenId, Guid id);
        List<PeriodizationViewModel> GetByName(string name, string tokenId);
        List<PeriodizationViewModel> GetByDateRange(string tokenId, DateTime dateStart, DateTime dateEnd);
        List<PeriodizationViewModel> GetByWeeklyFrequency(string tokenId, int id);
        PeriodizationViewModel Post(string tokenId, PeriodizationRequestViewModel periodizationRequestViewModel);
        PeriodizationViewModel Put(string tokenId, PeriodizationUpdateViewModel periodizationUpdateViewModel);
        void Delete(string tokenId, Guid periodizationId);
    }
}
