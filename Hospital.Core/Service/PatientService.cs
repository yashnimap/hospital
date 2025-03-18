using Dapper;
using Hospital.Model.Dto;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Service
{

    public class PatientService : IPatientService
    {
        private readonly IConfiguration _configuration;

        public PatientService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        //public async Task<IEnumerable<PatientDto>> GetPatientsAsync(int userId)
        //{
        //    using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        //    string sql = @"
        //    SELECT p.Id, p.Name, p.Modality, h.Name AS HospitalName
        //    FROM Patients p
        //    INNER JOIN Hospitals h ON p.HospitalId = h.Id
        //    INNER JOIN PatientAssignments pa ON p.Id = pa.PatientId
        //    WHERE pa.AssignedToUserId = @UserId";

        //    return await connection.QueryAsync<PatientDto>(sql, new { UserId = userId });
        //}

        public async Task<IEnumerable<PatientDto>> GetPatientsAsync(int userId)
        {
            using var connection = CreateConnection();
            string sql = @"
            SELECT p.PatientId, p.Name, m.ModalityName AS Modality, h.Name AS HospitalName, 
                   u.Username AS AssignedTo, p.CreatedDate, p.IsEmergency, p.IsPriority
            FROM Patients p
            INNER JOIN Modality m ON p.ModalityId = m.ModalityID
            INNER JOIN Hospitals h ON p.HospitalId = h.Id
            INNER JOIN Users u ON p.AssignedToUserId = u.Id
            WHERE p.AssignedToUserId = @UserId
            ORDER BY p.IsEmergency DESC, p.IsPriority DESC, p.CreatedDate DESC";

            return await connection.QueryAsync<PatientDto>(sql, new { UserId = userId });
        }


        public async Task<int> CreatePatientAsync(CreatePatientDto patientDto)
        {
            using var connection = CreateConnection();
            
            var sqlModality = "SELECT ModalityID FROM Modality WHERE ModalityName = @ModalityName";
            var sqlHospital = "SELECT Id FROM Hospitals WHERE Name = @HospitalName";
            var sqlUser = "SELECT Id FROM Users WHERE Username = @Username";

            var modalityId = await connection.ExecuteScalarAsync<int?>(sqlModality, new { patientDto.ModalityName });
            var hospitalId = await connection.ExecuteScalarAsync<int?>(sqlHospital, new { patientDto.HospitalName });
            var assignedUserId = await connection.ExecuteScalarAsync<int?>(sqlUser, new { Username = patientDto.AssignedToUsername });

            if (modalityId == null || hospitalId == null || assignedUserId == null)
                throw new ArgumentException("Invalid Modality, Hospital, or Assigned User");

            
            string sqlInsert = @"
            INSERT INTO Patients (Name, ModalityId, HospitalId, AssignedToUserId, CreatedDate, IsEmergency, IsPriority) 
            VALUES (@Name, @ModalityId, @HospitalId, @AssignedToUserId, GETDATE(), @IsEmergency, @IsPriority);
            SELECT SCOPE_IDENTITY();";

            return await connection.ExecuteScalarAsync<int>(sqlInsert, new
            {
                patientDto.Name,
                ModalityId = modalityId,
                HospitalId = hospitalId,
                AssignedToUserId = assignedUserId,
                IsEmergency = patientDto.IsEmergency ?? 0,  
                IsPriority = patientDto.IsPriority ?? 0
            });
        }

        public async Task<bool> UpdatePatientAsync(int patientId, CreatePatientDto patientDto)
        {
            using var connection = CreateConnection();
            
            var sqlModality = "SELECT ModalityID FROM Modality WHERE ModalityName = @ModalityName";
            var sqlHospital = "SELECT Id FROM Hospitals WHERE Name = @HospitalName";
            var sqlUser = "SELECT Id FROM Users WHERE Username = @Username";

            var modalityId = await connection.ExecuteScalarAsync<int?>(sqlModality, new { patientDto.ModalityName });
            var hospitalId = await connection.ExecuteScalarAsync<int?>(sqlHospital, new { patientDto.HospitalName });
            var assignedUserId = await connection.ExecuteScalarAsync<int?>(sqlUser, new { Username = patientDto.AssignedToUsername });

            if (modalityId == null || hospitalId == null || assignedUserId == null)
                throw new ArgumentException("Invalid Modality, Hospital, or Assigned User");

            string sqlUpdate = @"
            UPDATE Patients 
            SET Name = @Name, 
                ModalityId = @ModalityId, 
                HospitalId = @HospitalId, 
                AssignedToUserId = @AssignedToUserId, 
                ModifiedDate = GETDATE(),
                IsEmergency = @IsEmergency,  
                IsPriority = @IsPriority
            WHERE PatientId = @PatientId";

            var affectedRows = await connection.ExecuteAsync(sqlUpdate, new
            {
                patientDto.Name,
                ModalityId = modalityId,
                HospitalId = hospitalId,
                AssignedToUserId = assignedUserId,
                PatientId = patientId,
                IsEmergency = patientDto.IsEmergency ?? 0,
                IsPriority = patientDto.IsPriority ?? 0
            });

            return affectedRows > 0;
        }

        public async Task<bool> DeletePatientAsync(int patientId)
        {
            using var connection = CreateConnection();
            string sql = "DELETE FROM Patients WHERE PatientId = @PatientId";

            var affectedRows = await connection.ExecuteAsync(sql, new { PatientId = patientId });
            return affectedRows > 0;
        }
    }
}
