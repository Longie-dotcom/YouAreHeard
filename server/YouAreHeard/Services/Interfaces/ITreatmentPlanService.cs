﻿using YouAreHeard.Models;

namespace YouAreHeard.Services.Interfaces
{
    public interface ITreatmentPlanService
    {
        List<ARVRegimenDTO> GetARVRegimens();
        List<MedicationDTO> GetMedications();
        void CreateTreatmentPlan(RequestTreatmentPlanDTO requestTreatmentPlan);
        TreatmentPlanDetailsDTO? GetLatestTreatmentPlanByPatientID(int patientId);
    }
}