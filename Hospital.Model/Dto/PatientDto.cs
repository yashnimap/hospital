using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model.Dto
{    
    public class PatientDto
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public string Modality { get; set; }
        public string HospitalName { get; set; }
        public string AssignedTo { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEmergency { get; set; }
        public bool IsPriority { get; set; }
    }

}
