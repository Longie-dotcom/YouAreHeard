﻿using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class TreatmentPlanRepository : ITreatmentPlanRepository
    {
        public int insertTreatmentPlan(TreatmentPlanDTO treatmentPlan)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            INSERT INTO TreatmentPlan
            (regimenID, doctorID, patientID, date, notes, isCustomized)
            OUTPUT INSERTED.treatmentPlanID
            VALUES
            (@RegimenID, @DoctorID, @PatientID, @Date, @Notes, @IsCustomized)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@RegimenID", treatmentPlan.RegimenID);
            cmd.Parameters.AddWithValue("@DoctorID", treatmentPlan.DoctorID);
            cmd.Parameters.AddWithValue("@PatientID", treatmentPlan.PatientID);
            cmd.Parameters.AddWithValue("@Date", treatmentPlan.Date);
            cmd.Parameters.AddWithValue("@Notes", (object?)treatmentPlan.Notes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsCustomized", treatmentPlan.IsCustomized);

            return (int)cmd.ExecuteScalar();
        }

        public TreatmentPlanDetailsDTO? GetLatestTreatmentPlanByPatientID(int patientID)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                SELECT 
                    tp.treatmentPlanID,         -- 0
                    tp.regimenID,               -- 1
                    tp.doctorID,                -- 2
                    tp.patientID,               -- 3
                    tp.date,                    -- 4
                    tp.notes,                   -- 5
                    tp.isCustomized,            -- 6

                    ar.name AS regimenName,     -- 7
                    ar.type,                    -- 8
                    ar.duration,                -- 9
                    ar.regimenSideEffects,     -- 10
                    ar.regimenIndications,     -- 11
                    ar.regimenContraindications, -- 12

                    prt.time,                   -- 13
                    prt.drinkDosage,            -- 14
                    prt.medicationID,           -- 15
                    prt.notes,                  -- 16

                    m.medicationName,           -- 17
                    m.dosageMetric              -- 18
                FROM TreatmentPlan tp
                JOIN ARVRegimen ar ON tp.regimenID = ar.regimenID
                LEFT JOIN PillRemindTimes prt ON tp.treatmentPlanID = prt.treatmentPlanID
                LEFT JOIN Medication m ON prt.medicationID = m.medicationID
                WHERE tp.patientID = @PatientID 
                  AND tp.date = (
                      SELECT MAX(date) 
                      FROM TreatmentPlan 
                      WHERE patientID = @PatientID
                  )
                ORDER BY prt.time";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PatientID", patientID);

            using var reader = cmd.ExecuteReader();

            TreatmentPlanDetailsDTO? treatmentPlan = null;

            while (reader.Read())
            {
                if (treatmentPlan == null)
                {
                    treatmentPlan = new TreatmentPlanDetailsDTO
                    {
                        TreatmentPlanID = reader.GetInt32(0),
                        RegimenID = reader.GetInt32(1),
                        DoctorID = reader.GetInt32(2),
                        PatientID = reader.GetInt32(3),
                        Date = reader.GetDateTime(4),
                        Notes = reader.IsDBNull(5) ? null : reader.GetString(5),
                        IsCustomized = reader.GetBoolean(6),

                        RegimenName = reader.GetString(7),
                        RegimenType = reader.GetString(8),
                        RegimenDuration = reader.GetString(9),
                        RegimenSideEffects = reader.GetString(10),
                        RegimenIndications = reader.GetString(11),
                        RegimenContraindications = reader.GetString(12),

                        PillRemindTimes = new List<PillRemindTimesDTO>()
                    };
                }

                if (!reader.IsDBNull(13))
                {
                    var timeSpan = reader.GetTimeSpan(13);

                    var pill = new PillRemindTimesDTO
                    {
                        TreatmentPlanID = treatmentPlan.TreatmentPlanID,
                        Time = timeSpan.ToString(@"hh\:mm"),
                        DrinkDosage = reader.IsDBNull(14) ? 0 : reader.GetInt32(14),
                        MedicationID = reader.IsDBNull(15) ? 0 : reader.GetInt32(15),
                        Notes = reader.IsDBNull(16) ? null : reader.GetString(16),
                        MedicationName = reader.IsDBNull(17) ? null : reader.GetString(17),
                        DosageMetric = reader.IsDBNull(18) ? null : reader.GetString(18)
                    };

                    treatmentPlan.PillRemindTimes.Add(pill);
                }
            }

            return treatmentPlan;
        }

        public List<TreatmentPlanDetailsDTO> GetAllTreatmentPlansByPatientID(int patientID)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
        SELECT 
            tp.treatmentPlanID,
            tp.regimenID,
            tp.doctorID,
            tp.patientID,
            tp.date,
            tp.notes,
            tp.isCustomized,

            ar.name AS regimenName,
            ar.type,
            ar.duration,
            ar.regimenSideEffects,
            ar.regimenIndications,
            ar.regimenContraindications,

            prt.time,
            prt.drinkDosage,
            prt.medicationID,
            prt.notes AS pillNotes,

            m.medicationName,
            m.dosageMetric
        FROM TreatmentPlan tp
        JOIN ARVRegimen ar ON tp.regimenID = ar.regimenID
        LEFT JOIN PillRemindTimes prt ON tp.treatmentPlanID = prt.treatmentPlanID
        LEFT JOIN Medication m ON prt.medicationID = m.medicationID
        WHERE tp.patientID = @PatientID 
        ORDER BY tp.date DESC, prt.time";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PatientID", patientID);
            using var reader = cmd.ExecuteReader();

            var treatmentPlans = new List<TreatmentPlanDetailsDTO>();
            TreatmentPlanDetailsDTO currentPlan = null;
            int lastPlanId = -1;

            while (reader.Read())
            {
                int planId = Convert.ToInt32(reader["treatmentPlanID"]);

                // New treatment plan
                if (planId != lastPlanId)
                {
                    currentPlan = new TreatmentPlanDetailsDTO
                    {
                        TreatmentPlanID = planId,
                        RegimenID = Convert.ToInt32(reader["regimenID"]),
                        DoctorID = Convert.ToInt32(reader["doctorID"]),
                        PatientID = Convert.ToInt32(reader["patientID"]),
                        Date = Convert.ToDateTime(reader["date"]),
                        Notes = reader["notes"] as string,
                        IsCustomized = Convert.ToBoolean(reader["isCustomized"]),

                        RegimenName = reader["regimenName"] as string,
                        RegimenType = reader["type"] as string,
                        RegimenDuration = reader["duration"] as string,
                        RegimenSideEffects = reader["regimenSideEffects"] as string,
                        RegimenIndications = reader["regimenIndications"] as string,
                        RegimenContraindications = reader["regimenContraindications"] as string,

                        PillRemindTimes = new List<PillRemindTimesDTO>()
                    };
                    treatmentPlans.Add(currentPlan);
                    lastPlanId = planId;
                }

                // Add pill remind time if available
                if (!reader.IsDBNull(reader.GetOrdinal("time")))
                {
                    var pill = new PillRemindTimesDTO
                    {
                        TreatmentPlanID = planId,
                        Time = ((TimeSpan)reader["time"]).ToString(@"hh\:mm"),
                        DrinkDosage = reader["drinkDosage"] is DBNull ? 0 : (int)reader["drinkDosage"],
                        MedicationID = reader["medicationID"] is DBNull ? 0 : (int)reader["medicationID"],
                        Notes = reader["notes"] is DBNull ? null : (string)reader["notes"],
                        MedicationName = reader["medicationName"] is DBNull ? null : (string)reader["medicationName"],
                        DosageMetric = reader["dosageMetric"] is DBNull ? null : (string)reader["dosageMetric"]
                    };

                    currentPlan.PillRemindTimes.Add(pill);
                }
            }

            return treatmentPlans;
        }
    }
}