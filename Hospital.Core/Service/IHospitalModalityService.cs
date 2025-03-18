using Hospital.Model.Dto;
using Hospital.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Service
{    
    public interface IHospitalModalityService
    {
        Task<IEnumerable<HospitalDto>> GetHospitalsAsync();
        Task AddHospitalAsync(HospitalDto hospitalDto);
        Task SoftDeleteHospitalAsync(int hospitalId);

        Task<IEnumerable<ModalityDto>> GetModalitiesAsync();
        Task AddModalityAsync(ModalityDto modalityDto);
        Task SoftDeleteModalityAsync(int modalityId);
    }
}
