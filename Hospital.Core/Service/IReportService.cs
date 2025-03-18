using Hospital.Model.Dto;
using Hospital.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Service
{    
    public interface IReportService
    {
        Task<IEnumerable<ReportDto>> GetUserReportsAsync(int userId);
        Task<int> CreateReportAsync(ReportCreateDto reportDto);
        Task<bool> UpdateReportAsync(int reportId, ReportUpdateDto reportDto, int userId);
        Task<bool> DeleteReportAsync(int reportId, int userId);
    }
}
