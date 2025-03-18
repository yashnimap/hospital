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
    public class ReportService : IReportService
    {
        private readonly IConfiguration _configuration;

        public ReportService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        
        public async Task<IEnumerable<ReportDto>> GetUserReportsAsync(int userId)
        {
            using var connection = CreateConnection();
            string sql = @"
            SELECT r.Id, r.PatientId, r.AssignToUserId, r.CreatedAt, r.ReportText, rs.StatusName AS Status
            FROM Reports r
            JOIN ReportStatus rs ON r.Status = rs.Id
            WHERE r.AssignToUserId = @UserId";

            return await connection.QueryAsync<ReportDto>(sql, new { UserId = userId });
        }

        
        public async Task<int> CreateReportAsync(ReportCreateDto reportDto)
        {
            using var connection = CreateConnection();
            string sql = @"
            INSERT INTO Reports (PatientId, AssignToUserId, CreatedAt, ReportText, Status)
            VALUES (@PatientId, @AssignToUserId, GETDATE(), @ReportText, (SELECT Id FROM ReportStatus WHERE StatusName = 'Read'))
            SELECT SCOPE_IDENTITY();";

            return await connection.ExecuteScalarAsync<int>(sql, reportDto);
        }

        
        public async Task<bool> UpdateReportAsync(int reportId, ReportUpdateDto reportDto, int userId)
        {
            using var connection = CreateConnection();
            
            string checkSql = @"
            SELECT COUNT(1) FROM Reports r
            JOIN ReportStatus rs ON r.Status = rs.Id
            WHERE r.Id = @ReportId AND r.AssignToUserId = @UserId AND rs.StatusName = 'Write'";

            int count = await connection.ExecuteScalarAsync<int>(checkSql, new { ReportId = reportId, UserId = userId });

            if (count == 0) return false;

            string updateSql = "UPDATE Reports SET ReportText = @ReportText WHERE Id = @ReportId";
            int rowsAffected = await connection.ExecuteAsync(updateSql, new { ReportText = reportDto.ReportText, ReportId = reportId });

            return rowsAffected > 0;
        }
        
        public async Task<bool> DeleteReportAsync(int reportId, int userId)
        {
            using var connection = CreateConnection();
            
            string checkSql = @"
            SELECT COUNT(1) FROM Reports r
            JOIN ReportStatus rs ON r.Status = rs.Id
            WHERE r.Id = @ReportId AND r.AssignToUserId = @UserId AND rs.StatusName = 'Write'";

            int count = await connection.ExecuteScalarAsync<int>(checkSql, new { ReportId = reportId, UserId = userId });

            if (count == 0) return false;

            
            string deleteSql = "DELETE FROM Reports WHERE Id = @ReportId";
            int rowsAffected = await connection.ExecuteAsync(deleteSql, new { ReportId = reportId });

            return rowsAffected > 0;
        }

    }
}
