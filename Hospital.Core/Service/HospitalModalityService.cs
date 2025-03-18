using Dapper;
using Hospital.Model.Dto;
using Hospital.Model.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Service
{
    public class HospitalModalityService : IHospitalModalityService 
    {
        private readonly IConfiguration _configuration;

        public HospitalModalityService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        
        public async Task<IEnumerable<HospitalDto>> GetHospitalsAsync()
        {
            using var connection = CreateConnection();
            string sql = "SELECT Id, Name FROM Hospitals WHERE IsDeleted = 1";
            return await connection.QueryAsync<HospitalDto>(sql);
        }

        
        public async Task AddHospitalAsync(HospitalDto hospitalDto)
        {
            using var connection = CreateConnection();
            string sql = "INSERT INTO Hospitals (Name, IsDeleted) VALUES (@Name, 1)";
            await connection.ExecuteAsync(sql, hospitalDto);
        }

        
        public async Task SoftDeleteHospitalAsync(int hospitalId)
        {
            using var connection = CreateConnection();
            string sql = @"
            UPDATE Hospitals SET IsDeleted = 1 WHERE Id = @Id;
            UPDATE Patients SET HospitalId = NULL WHERE HospitalId = @Id;";
            await connection.ExecuteAsync(sql, new { Id = hospitalId });
        }

        
        public async Task<IEnumerable<ModalityDto>> GetModalitiesAsync()
        {
            using var connection = CreateConnection();
            string sql = "SELECT ModalityID, ModalityName FROM Modality WHERE IsDeleted = 1";
            return await connection.QueryAsync<ModalityDto>(sql);
        }

        
        public async Task AddModalityAsync(ModalityDto modalityDto)
        {
            using var connection = CreateConnection();
            string sql = "INSERT INTO Modality (ModalityName, IsDeleted) VALUES (@ModalityName, 1)";
            await connection.ExecuteAsync(sql, modalityDto);
        }

        
        public async Task SoftDeleteModalityAsync(int modalityId)
        {
            using var connection = CreateConnection();
            string sql = @"
            UPDATE Modality SET IsDeleted = 1 WHERE ModalityID = @Id;
            UPDATE Patients SET ModalityId = NULL WHERE ModalityId = @Id;";
            await connection.ExecuteAsync(sql, new { Id = modalityId });
        }
    }

}
