using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model.Dto
{
    public class ReportDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int AssignToUserId { get; set; }
        public string CreatedAt { get; set; }
        public string ReportText { get; set; }
        public string Status { get; set; }
    }

    public class ReportCreateDto
    {
        public int PatientId { get; set; }
        public int AssignToUserId { get; set; }
        public string ReportText { get; set; }
    }

    public class ReportUpdateDto
    {
        public string ReportText { get; set; }
    }

}
