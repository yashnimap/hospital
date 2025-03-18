using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model.Dto
{
    public class CreatePatientDto
    {
        public string Name { get; set; }
        public string ModalityName { get; set; }
        public string HospitalName { get; set; }
        public string AssignedToUsername { get; set; }
        public int? IsEmergency { get; set; }  
        public int? IsPriority { get; set; }   
    }

}
