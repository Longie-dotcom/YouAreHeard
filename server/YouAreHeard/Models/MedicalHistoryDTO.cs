namespace YouAreHeard.Models
{
    public class MedicalHistoryDTO
    {
        public int MedicalHistoryID { get; set; }
        public DateTime DateTime { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
    }
}
