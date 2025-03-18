using Hospital.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Service
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetPatientsAsync(int userId);
        Task<int> CreatePatientAsync(CreatePatientDto patientDto);
        Task<bool> UpdatePatientAsync(int patientId, CreatePatientDto patientDto);
        Task<bool> DeletePatientAsync(int patientId);
    }

}
