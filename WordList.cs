using System.Collections.Generic;

namespace DataArmor
{
    
    internal class WordList
    {
        public List<string> EID = new List<string>();
        public List<string> QID = new List<string>();
        public List<string> SA = new List<string>();

        List<string> Name = new List<string> { "Name","FullName", "EmployeeName", "FirstName", "first_name", "First_Name", "LastName", "EmployeeFullName", "CustomerFullName", "full_name", "GivenName", "FamilyName", "NameConcatenated",
            "PersonFullName", "UserFullName", "ClientName", "PatientName", "AuthorName"
            ,"ContactName", "LegalEntityName", "middle_name", "mid_nm", "All Employees",
            "User", "TeamMember", "team_member", "initial" };

        List<string> Id = new List<string> { "ID", "Identifier", "UniqueID", "RecordID", "EntityID", "KeyID", "ReferenceID", "ObjectID", "PrimaryID", "SecondaryID", "UserID", "ItemID", "DocumentID", "TransactionID" };
        List<string> SSN = new List<string> { "SSN", "SocialSecurityNumber", "SSNID", "SSNNumber" };
        //QID
        List<string> Phone_number = new List<string>{"PhoneNumber", "Phone", "MobileNumber", "CellNumber", "LandlineNumber", "CountryCode", "AreaCode", "Extension", "ContactNumber", "PrimaryPhone", "SecondaryPhone", "HomePhone", "WorkPhone", "FaxNumber"};
        List<string> Age = new List<string>{"Age", "AgeYears", "AgeMonths", "AgeDays", "AgeHours", "AgeMinutes", "AgeSeconds", "AgeGroup", "AgeRange", "YearsOld", "MonthsOld", "DaysOld", "HoursOld", "MinutesOld", "SecondsOld", "DateOfBirth", "BirthYear", "BirthMonth", "BirthDay", "BirthDate", "DateOfDeath", "DeathYear", "DeathMonth", "DeathDay", "DeathDate", "PersonAge", "UserAge", "EmployeeAge", "CustomerAge", "PatientAge", "MemberAge", "ApplicantAge", "VisitorAge"};
        List<string> DOB = new List<string>{"DateOfBirth", "BirthDate", "DOB", "BirthDay", "BirthMonth", "BirthYear", "PersonDOB", "UserDOB", "EmployeeDOB", "CustomerDOB", "PatientDOB", "MemberDOB", "ApplicantDOB", "VisitorDOB"};
        List<string> job = new List<string> { "JobTitle", "Occupation", "Position", "Title", "Department", "Division", "JobDescription", "EmploymentStatus", "HireDate", "StartDate", "EndDate", "Salary", "PayGrade", "EmploymentType", "ContractType", "ContractStartDate", "ContractEndDate", "SupervisorName", "ManagerName", "TeamLeader", "TeamManager" };
        List<string> country= new List<string>{"Country", "CountryName", "CountryCode", "CountryISOCode", "CountryRegion", "CountryCapital", "CountryPopulation", "CountryLanguage", "CountryCurrency", "CountryContinent", "CountryFlag", "CountryCallingCode", "CountryTimeZone", "CountryArea", "CountryGovernment", "CountryPresident", "CountryPrimeMinister", "CapitalCity"};
        List<string> zip = new List<string>{"ZipCode", "PostalCode", "PostCode", "ZIP", "PostalArea", "PostalRegion", "PostalZone", "ZipArea", "ZipRegion", "ZipZone", "ZipPostalCode", "ZipPostalArea", "ZipPostalRegion", "ZipPostalZone"};
        List<string> Address = new List<string>{"Address", "StreetAddress", "Street", "HouseNumber", "ApartmentNumber", "BuildingName", "City", "Town", "State", "Province", "Region", "PostalCode", "ZipCode", "Country", "Latitude", "Longitude", "Neighborhood", "Landmark", "District", "Block", "Floor", "Unit"};
        List<string> gender = new List<string>{"Gender", "Sex", "Sexuality", "GenderIdentity", "BiologicalSex", "GenderExpression", "Pronouns", "TransgenderStatus", "IntersexStatus", "GenderRole", "GenderPreference", "GenderPresentation", "GenderStereotype"};
        List<string> Departments = new List<string>{"Department", "DepartmentName", "DepartmentCode", "DepartmentHead", "DepartmentManager", "DepartmentDescription", "DepartmentLocation", "DepartmentBudget", "DepartmentEmployees", "DepartmentProjects", "DepartmentSupervisor"};
        List<string> Contact = new List<string>{"Contact", "ContactName", "ContactDetails", "ContactInfo", "ContactMethod", "ContactType", "ContactAddress", "ContactEmail", "ContactPhoneNumber", "ContactRole", "ContactRelation", "ContactNotes", "ContactPreferences"};
        //SA

        List<string> Health_state = new List<string> { "HealthStatus", "MedicalCondition", "Illness", "Disease", "Disability", "Allergies", "Medications", "Treatments", "Diagnosis", "Symptoms", "VitalSigns", "HealthHistory", "DoctorName", "HospitalName", "HealthInsurance", "BloodType", "OrganDonor", "EmergencyContact", "LastCheckupDate", "NextCheckupDate", "Prescription", "MedicalRecords" };
        List<string> Salary = new List<string> {"Salary", "Wage", "Pay", "Compensation", "PayGrade", "PayScale", "HourlyRate", "AnnualSalary", "MonthlySalary", "WeeklySalary", "Bonus", "Benefits", "OvertimeRate", "GrossPay", "NetPay", "Deductions", "Allowances", "Commission", "Incentives"};
        List<string> Condition = new List<string> { "Condition", "MedicalCondition", "HealthCondition", "ConditionName", "ConditionCode", "ConditionDescription", "ConditionSeverity", "ConditionStatus", "ConditionOnsetDate", "ConditionEndDate", "ConditionTreatment", "ConditionSymptoms", "ConditionDiagnosis", "ConditionNotes" };
        List<string> Diagnosis = new List<string> {"Diagnosis", "MedicalDiagnosis", "DiagnosisCode", "DiagnosisName", "DiagnosisDescription", "DiagnosisDate", "DiagnosingDoctor", "DiagnosingHospital", "DiagnosisStatus", "DiagnosisNotes", "DiagnosisTreatment", "DiagnosisOutcome", "DiagnosisSeverity"};
        List<string> Treatment = new List<string> {"Treatment", "MedicalTreatment", "TreatmentName", "TreatmentDescription", "TreatmentType", "TreatmentDate", "TreatingDoctor", "TreatingHospital", "TreatmentDuration", "TreatmentOutcome", "TreatmentNotes", "TreatmentCost"};

        public WordList()
        {
            EID.AddRange(Name);
            EID.AddRange(SSN);
            EID.AddRange(Id);

            QID.AddRange(Phone_number);
            QID.AddRange(Age);
            QID.AddRange(DOB);
            QID.AddRange(job);
            QID.AddRange(country);
            QID.AddRange(zip);
            QID.AddRange(Address);
            QID.AddRange(gender);
            QID.AddRange(Departments);
            QID.AddRange(Contact);

            SA.AddRange(Salary);
            SA.AddRange(Health_state);
            SA.AddRange(Condition);
            SA.AddRange(Diagnosis);
            SA.AddRange(Treatment);
        }



    }
}
