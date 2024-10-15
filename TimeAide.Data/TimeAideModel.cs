namespace TimeAide.Web.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Collections.Generic;
    using TimeAide.Common.Helpers;
    using System.Data.SqlClient;
    using TimeAide.Models.ViewModel;
    using System.Threading.Tasks;
    using TimeAide.Models.Models;
  
    public partial class TimeAideContext : DbContext
    {
        public TimeAideContext() : base("name=TimeAideContext")
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<TimeAideContext, TimeAide.Data.Migrations.Configuration>());
            //Database.SetInitializer<TimeAideContext>(null);
            this.Database.CommandTimeout = 60;
        }
        public TimeAideContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<TimeAideContext, TimeAide.Data.Migrations.Configuration>());
            this.Database.CommandTimeout = 60;
        }

        public virtual DbSet<CFSECode> CFSECode { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<CompanyEmployeeTab> CompanyEmployeeTab { get; set; }

        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<MaritalStatus> MaritalStatus { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<EmployeeType> EmployeeType { get; set; }
        public virtual DbSet<EmployeeGroupType> EmployeeGroupType { get; set; }
        public virtual DbSet<EmployeeGroup> EmployeeGroup { get; set; }
        public virtual DbSet<EmployeeNotification> EmployeeNotification { get; set; }
        public virtual DbSet<EmployeeNotificationType> EmployeeNotificationType { get; set; }
        public virtual DbSet<UserEmployeeGroup> UserEmployeeGroup { get; set; }
        public virtual DbSet<EmploymentType> EmploymentType { get; set; }
        public virtual DbSet<EmploymentStatus> EmploymentStatus { get; set; }
        public virtual DbSet<EmployeeSupervisor> EmployeeSupervisor { get; set; }
        public virtual DbSet<JobCode> JobCode { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<SubDepartment> SubDepartment { get; set; }
        //public virtual DbSet<SupervisoryLevel> SupervisoryLevel { get; set; }
        public virtual DbSet<EmergencyContact> EmergencyContact { get; set; }
        public virtual DbSet<Relationship> Relationship { get; set; }
        public virtual DbSet<PayInformationHistory> PayInformationHistory { get; set; }
        //public virtual DbSet<UserInformationAddress> UserInformationAddress { get; set; }
        //public virtual DbSet<UserInformationContact> UserInformationContact { get; set; }
        //public virtual DbSet<ContactMedium> ContactMedium { get; set; }
        public virtual DbSet<Employment> Employment { get; set; }
        //public virtual DbSet<EmploymentTerminate> EmploymentTerminate { get; set; }
        public virtual DbSet<TerminationEligibility> TerminationEligibility { get; set; }
        public virtual DbSet<TerminationReason> TerminationReason { get; set; }
        public virtual DbSet<TerminationType> TerminationType { get; set; }
        public virtual DbSet<EmploymentHistory> EmploymentHistory { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<PositionTraining> PositionTraining { get; set; }
        public virtual DbSet<PositionCredential> PositionCredential { get; set; }
        public virtual DbSet<PositionAppraisalTemplate> PositionAppraisalTemplate { get; set; }
        public virtual DbSet<VeteranStatus> VeteranStatus { get; set; }
        public virtual DbSet<EmployeeVeteranStatus> EmployeeVeteranStatus { get; set; }
        public virtual DbSet<Disability> Disability { get; set; }
        public virtual DbSet<Ethnicity> Ethnicity { get; set; }
        public virtual DbSet<EmployeeStatus> EmployeeStatus { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<EEOCategory> EEOCategory { get; set; }
        public virtual DbSet<PayFrequency> PayFrequency { get; set; }
        public virtual DbSet<PayScale> PayScale { get; set; }
        public virtual DbSet<PayScaleLevel> PayScaleLevel { get; set; }
        public virtual DbSet<PayType> PayType { get; set; }
        public virtual DbSet<RateFrequency> RateFrequency { get; set; }
        public virtual DbSet<WCClassCode> WCClassCode { get; set; }
        public virtual DbSet<Degree> Degree { get; set; }
        public virtual DbSet<EmployeeEducation> EmployeeEducation { get; set; }
        public virtual DbSet<Training> Training { get; set; }
        public virtual DbSet<TrainingType> TrainingType { get; set; }
        
        public virtual DbSet<EmployeeTraining> EmployeeTraining { get; set; }
        public virtual DbSet<ActionTaken> ActionTaken { get; set; }
        public virtual DbSet<ApplicationConfiguration> ApplicationConfiguration { get; set; }

        public virtual DbSet<PerformanceDescription> PerformanceDescription { get; set; }
        public virtual DbSet<PerformanceResult> PerformanceResult { get; set; }
        public virtual DbSet<EmployeePerformance> EmployeePerformance { get; set; }
        public virtual DbSet<CredentialType> CredentialType { get; set; }
        //public virtual DbSet<CredentialType> CredentialType { get; set; }
        public virtual DbSet<Credential> Credential { get; set; }
        public virtual DbSet<EmployeeCredential> EmployeeCredential { get; set; }
        //public virtual DbSet<EmployeeRequiredCredential> EmployeeRequiredCredential { get; set; }
        public virtual DbSet<DependentStatus> DependentStatus { get; set; }
        public virtual DbSet<EmployeeDependent> EmployeeDependent { get; set; }
        public virtual DbSet<EmployeeTimeAndAttendanceSetting> EmployeeTimeAndAttendanceSetting { get; set; }
        
        public virtual DbSet<InsuranceType> InsuranceType { get; set; }
        public virtual DbSet<InsuranceStatus> InsuranceStatus { get; set; }
        public virtual DbSet<InsuranceCoverage> InsuranceCoverage { get; set; }
        public virtual DbSet<CobraPaymentStatus> CobraPaymentStatus { get; set; }
        public virtual DbSet<CobraStatus> CobraStatus { get; set; }
        public virtual DbSet<EmployeeHealthInsurance> EmployeeHealthInsurance { get; set; }
        public virtual DbSet<HealthInsuranceCobraHistory> HealthInsuranceCobraHistory { get; set; }
        public virtual DbSet<EmployeeDentalInsurance> EmployeeDentalInsurance { get; set; }
        public virtual DbSet<DentalInsuranceCobraHistory> DentalInsuranceCobraHistory { get; set; }
        public virtual DbSet<SupervisorDepartment> SupervisorDepartment { get; set; }
        public virtual DbSet<SupervisorSubDepartment> SupervisorSubDepartment { get; set; }
        public virtual DbSet<SupervisorCompany> SupervisorCompany { get; set; }
        public virtual DbSet<SupervisorEmployeeType> SupervisorEmployeeType { get; set; }
        public virtual DbSet<EmployeeCompanyTransfer> EmployeeCompanyTransfer { get; set; }


        public virtual DbSet<Benefit> Benefit { get; set; }
        public virtual DbSet<EmployeeBenefitHistory> EmployeeBenefitHistory { get; set; }
        public virtual DbSet<EmployeeBenefitEnlisted> EmployeeBenefitEnlisted { get; set; }

        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<EmployeeDocument> EmployeeDocument { get; set; }

        public virtual DbSet<CustomField> CustomField { get; set; }
        public virtual DbSet<CustomFieldType> CustomFieldType { get; set; }
        public virtual DbSet<EmployeeCustomField> EmployeeCustomField { get; set; }

        public virtual DbSet<ActionType> ActionType { get; set; }
        public virtual DbSet<EmployeeAction> EmployeeAction { get; set; }

        public virtual DbSet<AppraisalRatingScale> AppraisalRatingScale { get; set; }
        public virtual DbSet<AppraisalRatingScaleDetail> AppraisalRatingScaleDetail { get; set; }
        public virtual DbSet<RatingLevel> RatingLevel { get; set; }
        public virtual DbSet<AppraisalGoal> AppraisalGoal { get; set; }
        public virtual DbSet<AppraisalSkill> AppraisalSkill { get; set; }
        public virtual DbSet<AppraisalResult> AppraisalResult { get; set; }
        public virtual DbSet<AppraisalTemplate> AppraisalTemplate { get; set; }
        public virtual DbSet<AppraisalTemplateGoal> AppraisalTemplateGoal { get; set; }
        public virtual DbSet<AppraisalTemplateSkill> AppraisalTemplateSkill { get; set; }

        public virtual DbSet<EmployeeAppraisal> EmployeeAppraisal { get; set; }
        public virtual DbSet<EmployeeAppraisalGoal> EmployeeAppraisalGoal { get; set; }
        public virtual DbSet<EmployeeAppraisalSkill> EmployeeAppraisalSkill { get; set; }
        public virtual DbSet<EmployeeAppraisalDocument> EmployeeAppraisalDocument { get; set; }
        public virtual DbSet<IncidentType> IncidentType { get; set; }
        public virtual DbSet<IncidentArea> IncidentArea { get; set; }
        public virtual DbSet<IncidentBodyPart> IncidentBodyPart { get; set; }
        public virtual DbSet<IncidentInjuryDescription> IncidentInjuryDescription { get; set; }
        public virtual DbSet<IncidentInjurySource> IncidentInjurySource { get; set; }
        public virtual DbSet<IncidentTreatmentFacility> IncidentTreatmentFacility { get; set; }
        public virtual DbSet<OSHACaseClassification> OSHACaseClassification { get; set; }
        public virtual DbSet<OSHAInjuryClassification> OSHAInjuryClassification { get; set; }
        public virtual DbSet<EmployeeIncident> EmployeeIncident { get; set; }
        public virtual DbSet<ReportCriteriaTemplate> ReportCriteriaTemplate { get; set; }
        public virtual DbSet<DataMigrationLog> DataMigrationLog { get; set; }
        public virtual DbSet<DataMigrationLogDetail> DataMigrationLogDetail { get; set; }
        public virtual DbSet<CompanyAnnouncement> CompanyAnnouncement { get; set; }
        public virtual DbSet<CompanyDocument> CompanyDocument { get; set; }
        public virtual DbSet<CompanyConfigurableLink> CompanyConfigurableLink { get; set; }
        //Applicate EF Model
        public virtual DbSet<ApplicantStatus> ApplicantStatus { get; set; }
        public virtual DbSet<ApplicantInformation> ApplicantInformation { get; set; }
        public virtual DbSet<ApplicantContactInformation> ApplicantContactInformation { get; set; }
        public virtual DbSet<ApplicantEducation> ApplicantEducation { get; set; }
        public virtual DbSet<ApplicantCustomField> ApplicantCustomField { get; set; }
        public virtual DbSet<ApplicantDocument> ApplicantDocument { get; set; }
        public virtual DbSet<ApplicantApplication> ApplicantApplication { get; set; }
        public virtual DbSet<ApplicantReferenceType> ApplicantReferenceType { get; set; }
        public virtual DbSet<ApplicantReferenceSource> ApplicantReferenceSource { get; set; }
        public virtual DbSet<ApplicantEmploymentType> ApplicantEmploymentType { get; set; }
        public virtual DbSet<ApplicantApplicationLocation> ApplicantApplicationLocation { get; set; }

        public virtual DbSet<ApplicantCompany> ApplicantCompany { get; set; }
        public virtual DbSet<ApplicantPosition> ApplicantPosition { get; set; }
        public virtual DbSet<ApplicantExitType> ApplicantExitType { get; set; }
        public virtual DbSet<ApplicantEmployment> ApplicantEmployment { get; set; }

        public virtual DbSet<ApplicantInterviewQuestion> ApplicantInterviewQuestion { get; set; }
        public virtual DbSet<ApplicantInterviewAnswer> ApplicantInterviewAnswer { get; set; }
        public virtual DbSet<ApplicantInterview> ApplicantInterview { get; set; }
        public virtual DbSet<ApplicantAction> ApplicantAction { get; set; }
        //ApplicantCustomField
        //END
        public virtual DbSet<ChangeRequestAddress> ChangeRequestAddress { get; set; }
        public virtual DbSet<ChangeRequestEmailNumbers> ChangeRequestEmailNumbers { get; set; }
        public virtual DbSet<ChangeRequestEmergencyContact> ChangeRequestEmergencyContact { get; set; }
        public virtual DbSet<ChangeRequestEmployeeDependent> ChangeRequestEmployeeDependent { get; set; }
        public virtual DbSet<ChatConversation> ChatConversation { get; set; }
        public virtual DbSet<ChatConversationParticipant> ChatConversationParticipant { get; set; }
        public virtual DbSet<SelfServiceEmployeeDocument> SelfServiceEmployeeDocument { get; set; }
        public virtual DbSet<SelfServiceEmployeeCredential> SelfServiceEmployeeCredential { get; set; }
        public virtual DbSet<ChangePasswordByAdminReason> ChangePasswordByAdminReason { get; set; }


        public virtual DbSet<ChatUsers> ChatUsers { get; set; }

        public virtual DbSet<ChatMessage> ChatMessage { get; set; }
        public virtual DbSet<EmployeeTimeOffRequest> EmployeeTimeOffRequest { get; set; }
        public virtual DbSet<TimeOffRequestStatus> TimeOffRequestStatus { get; set; }
        public virtual DbSet<ChangeRequestStatus> ChangeRequestStatus { get; set; }
        public virtual DbSet<JobCertificationSignee> JobCertificationSignee { get; set; }
        public virtual DbSet<JobCertificationTemplate> JobCertificationTemplate { get; set; }
        public virtual DbSet<UserSessionLog> UserSessionLog { get; set; }
        public virtual DbSet<UserSessionLogDetail> UserSessionLogDetail { get; set; }
        public virtual DbSet<AuditLog> AuditLog { get; set; }
        public virtual DbSet<AuditLogDetail> AuditLogDetail { get; set; }
        public virtual DbSet<UserSessionLogEvent> UserSessionLogEvent { get; set; }
        public virtual DbSet<CountryIPAddress> CountryIPAddress { get; set; }
        public virtual DbSet<JobPostingDetail> JobPostingDetail { get; set; }
        public virtual DbSet<JobPostingLocation> JobPostingLocation { get; set; }
        public virtual DbSet<JobPostingStatus> JobPostingStatus { get; set; }
        public virtual DbSet<PositionQuestion> PositionQuestion { get; set; }
        public virtual DbSet<PayRateMultiplier> PayRateMultiplier { get; set; }
        public virtual DbSet<AccrualType> AccrualType { get; set; }
        public virtual DbSet<GLAccountType> GLAccountType { get; set; }
        public virtual DbSet<GLAccount> GLAccount { get; set; }
        public virtual DbSet<CompensationAccrualType> CompensationAccrualType { get; set; }
        public virtual DbSet<CompensationComputationType> CompensationComputationType { get; set; }
        public virtual DbSet<CompensationImportType> CompensationImportType { get; set; }
        //public virtual DbSet<CompensationItem> CompensationItem { get; set; }
        public virtual DbSet<CompanyCompensation> CompanyCompensation { get; set; }
        public virtual DbSet<CompensationPeriodEntry> CompensationPeriodEntry { get; set; }
        public virtual DbSet<CompensationTransaction> CompensationTransaction { get; set; }
        public virtual DbSet<CompensationType> CompensationType { get; set; }
        public virtual DbSet<PrimaryTransaction> PrimaryTransaction { get; set; }
        public virtual DbSet<ProcessCode> ProcessCode { get; set; }
        public virtual DbSet<SickAccrualType> SickAccrualType { get; set; }
        public virtual DbSet<TransactionConfiguration> TransactionConfiguration { get; set; }
        public virtual DbSet<VacationAccrualType> VacationAccrualType { get; set; }
        public virtual DbSet<CompanyCompensationPRPayExport> CompanyCompensationPRPayExport { get; set; }

        public virtual DbSet<CompanyWithholding> CompanyWithholding { get; set; }
        public virtual DbSet<CompanyWithholdingCompensationExclusion> CompanyWithholdingCompensationExclusion { get; set; }
        public virtual DbSet<CompanyWithholdingLoan> CompanyWithholdingLoan { get; set; }
        public virtual DbSet<CompanyWithholdingPRPayExport> CompanyWithholdingPRPayExport { get; set; }
        public virtual DbSet<CompanyWithholding401K> CompanyWithholding401K { get; set; }
        public virtual DbSet<WithholdingTaxType> WithholdingTaxType { get; set; }
        public virtual DbSet<WitholdingComputationType> WitholdingComputationType { get; set; }
        public virtual DbSet<WitholdingPrePostType> WitholdingPrePostType { get; set; }
        public virtual DbSet<CompanyContribution> CompanyContribution { get; set; }
        public virtual DbSet<CompanyContributionPRPayExport> CompanyContributionPRPayExport { get; set; }
        public virtual DbSet<CompanyContribution401K> CompanyContribution401K { get; set; }
        public virtual DbSet<CompanyContributionCompensationExclusion> CompanyContributionCompensationExclusion { get; set; }


        public virtual DbSet<AccrualRule> AccrualRule { get; set; }
        public virtual DbSet<AccrualRuleTier> AccrualRuleTier { get; set; }
        public virtual DbSet<AccrualRuleWorkedHoursTier> AccrualRuleWorkedHoursTier { get; set; }
        public virtual DbSet<ApplicantQAnswerOption> ApplicantQAnswerOption { get; set; }
        public virtual DbSet<ApplicantInterviewQAnswer> ApplicantInterviewQAnswer { get; set; }
        public virtual DbSet<Withholding401KType> Withholding401KType { get; set; }
        public virtual DbSet<DocumentRequiredBy> DocumentRequiredBy { get; set; }
        public virtual DbSet<SenderEmailConfiguration> SenderEmailConfiguration { get; set; }
        public virtual DbSet<EmployeeAccrualRule> EmployeeAccrualRule { get; set; }
        public virtual DbSet<EmployeeAccrualBalance> EmployeeAccrualBalance { get; set; }
        public virtual DbSet<CompanyMedia> CompanyMedia { get; set; }
        public virtual DbSet<EmployeeCompensation> EmployeeCompensation { get; set; }
        public virtual DbSet<EmployeeContribution> EmployeeContribution { get; set; }
        
        public virtual DbSet<EmployeeWithholding> EmployeeWithholding { get; set; }
        
        public virtual DbSet<EmployeeCompensationPreviousHistory> EmployeeCompensationPreviousHistory { get; set; }

        public virtual DbSet<EmployeeTimeOffRequestDocument> EmployeeTimeOffRequestDocument { get; set; }
        public virtual DbSet<BaseSchedule> BaseSchedule { get; set; }
        public virtual DbSet<BaseScheduleDayInfo> BaseScheduleDayInfo { get; set; }
        public virtual DbSet<WebPunchConfiguration> WebPunchConfiguration { get; set; }
        

        public virtual DbSet<EmployeeRotatingSchedule> EmployeeRotatingSchedule { get; set; }
        public virtual DbSet<EmployeeFutureSchedule> EmployeeFutureSchedule { get; set; }

        public virtual DbSet<EmployeeWebScheduledPeriod> EmployeeWebScheduledPeriod { get; set; }
        public virtual DbSet<EmployeeWebScheduledPeriodDetail> EmployeeWebScheduledPeriodDetail { get; set; }
        public virtual DbSet<EmployeeWebTimeSheet> EmployeeWebTimeSheet { get; set; }
        public virtual DbSet<EmailBlast> EmailBlast { get; set; }
        public virtual DbSet<EmailBlastDetail> EmailBlastDetail { get; set; }

        public virtual DbSet<DeviceMetaData> DeviceMetaData { get; set; }
        public virtual DbSet<SecurityCode> SecurityCode { get; set; }

        public List<T> GetAll<T>() where T : BaseEntity
        {
            return Set<T>().Where(t => t.DataEntryStatus == 1).ToList();
        }
        public List<T> GetAll<T>(int clientId) where T : BaseEntity
        {
            if (typeof(T) == typeof(Client))
                return Set<T>().Where(t => t.DataEntryStatus == 1).ToList();
            else
                return Set<T>().Where(t => t.DataEntryStatus == 1 && t.ClientId == clientId).ToList();
        }
        public List<T> GetAllByUser<T>(int? userInformationId, int clientId) where T : BaseEntity
        {
            return GetAll<T>(clientId).OfType<BaseUserObjects>().Where(t => t.UserInformationId == userInformationId).OfType<T>().ToList();
        }

        public List<T> GetAllByCompany<T>(int? companyId, int clientId) where T : BaseEntity
        {
            return GetAll<T>(clientId).OfType<BaseCompanyObjects>().Where(t => t.CompanyId == companyId || !t.CompanyId.HasValue).OfType<T>().ToList();
        }
        public T Find<T>(int id, int clientId) where T : BaseEntity
        {
            return Set<T>().FirstOrDefault(entity => entity.Id == id && entity.ClientId == clientId);
        }
        public void Add<T>(T newItem) where T : BaseEntity
        {
            Set<T>().Add(newItem);
        }
        public List<T> SP_UserInformation<T>(int employeeId, string employeeName, int positionId, int employeeStatusId, int superviserId, int clientId, int companyId) where T : UserInformationViewModel
        {
            var employeeIdParameter = new SqlParameter("@employeeId", employeeId);
            var employeeNameParameter = new SqlParameter("@employeeName", employeeName);
            var employeePostionIdParameter = new SqlParameter("@positionId", positionId);
            var employeeStatusIdParameter = new SqlParameter("@employeeStatusId", employeeStatusId);
            var superviserIdParameter = new SqlParameter("@superviserId", superviserId);
            var clientIdParameter = new SqlParameter("@clientId", clientId);
            var companyIdParameter = new SqlParameter("@companyId", companyId);
            try
            {
                var userInformationList = this.Database
                   .SqlQuery<T>("sp_UserInformation @employeeId, @employeeName,@positionId ,@employeeStatusId, @superviserId, @clientId, @companyId ",
                                                    employeeIdParameter, employeeNameParameter, employeePostionIdParameter, employeeStatusIdParameter
                                                    , superviserIdParameter, clientIdParameter, companyIdParameter)
                   .ToList<T>();
                return userInformationList;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<T> SP_EmployeeUserInformationByFilter<T>( int superviserId, int clientId, int EmployeeStatusId,int CompanyId) where T : EmployeeUserInformationView
        {
           
            var superviserIdParameter = new SqlParameter("@superviserId", superviserId);
            var clientIdParameter = new SqlParameter("@clientId", clientId);
            var employeeStatusIdParameter = new SqlParameter("@EmployeeStatusId", EmployeeStatusId);
            var companyIdParameter = new SqlParameter("@CompanyId", CompanyId);

            try
            {
                var userInformationList = this.Database
                   .SqlQuery<T>("sp_EmployeeUserInformationByFilter @superviserId, @clientId,@EmployeeStatusId,@CompanyId", superviserIdParameter, clientIdParameter, employeeStatusIdParameter, companyIdParameter)
                   .ToList<T>();
                return userInformationList;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }





        public List<T> SP_UserInformationByFilter<T>(int employeeId, string employeeName,int departmentId,int subDepartmentId, int positionId,int employmentTypeId,int employeeTypeId, int employeeStatusId, int superviserId, int clientId, int companyId) where T : UserInformationViewModel
        {
            var employeeIdParameter = new SqlParameter("@employeeId", employeeId);
            var employeeNameParameter = new SqlParameter("@employeeName", employeeName);
            var employeeDepartmentIdParameter = new SqlParameter("@departmentId", departmentId);
            var employeeSubDepartmentIdParameter = new SqlParameter("@subDepartmentId", subDepartmentId);
            var employeePostionIdParameter = new SqlParameter("@positionId", positionId);
            var employeeEmploymentTypeIdParameter = new SqlParameter("@employmentTypeId", employmentTypeId);
            var employeeEmployeeTypeIdParameter = new SqlParameter("@employeeTypeId", employeeTypeId);
            var employeeStatusIdParameter = new SqlParameter("@employeeStatusId", employeeStatusId);
            var superviserIdParameter = new SqlParameter("@superviserId", superviserId);
            var clientIdParameter = new SqlParameter("@clientId", clientId);
            var companyIdParameter = new SqlParameter("@companyId", companyId);
            try
            {
                var userInformationList = this.Database
                   .SqlQuery<T>("sp_UserInformationByFilter @employeeId, @employeeName,@departmentId,@subDepartmentId,@positionId,@employmentTypeId,@employeeTypeId,@employeeStatusId, @superviserId, @clientId, @companyId ",
                                                    employeeIdParameter, employeeNameParameter, employeeDepartmentIdParameter, employeeSubDepartmentIdParameter, employeePostionIdParameter, employeeEmploymentTypeIdParameter,
                                                    employeeEmployeeTypeIdParameter,employeeStatusIdParameter, superviserIdParameter, clientIdParameter, companyIdParameter)
                   .ToList<T>();
                return userInformationList;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string sp_GetSupervisedEmployeeIds(int clientId, int supervisorUserId, Data.tReportWeek model, string spName)
        {
            var clientIdParameter = new SqlParameter("@ClientId", clientId);
            var supervisorUserIdParameter = new SqlParameter("@SupervisorUserId", supervisorUserId);
            var employeeIdParameter = new SqlParameter("@EmployeeId", (model.e_id.HasValue ? (object)model.e_id.Value : DBNull.Value));
            var departmentIdParameter = new SqlParameter("@DepartmentId", (model.nDept.HasValue ? (object)model.nDept.Value : DBNull.Value));
            var subDepartmentIdParameter = new SqlParameter("@SubDepartmentId", (model.nJobTitleID.HasValue ? (object)model.nJobTitleID.Value : DBNull.Value));
            var employeeTypeIdParameter = new SqlParameter("@EmployeeTypeId", (model.nEmployeeType.HasValue ? (object)model.nEmployeeType.Value : DBNull.Value));
            try
            {
                var userInformationList = this.Database
                   .SqlQuery<string>(spName + " @ClientId,@SupervisorUserId,@EmployeeId, @DepartmentId, @SubDepartmentId,@EmployeeTypeId",
                                                    clientIdParameter, employeeIdParameter, supervisorUserIdParameter, departmentIdParameter, subDepartmentIdParameter, employeeTypeIdParameter)
                   .ToList<string>();
                return userInformationList.FirstOrDefault();
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<T> SP_GetEmployeeSupervisors<T>(int userInformationId, int clientId) where T : UserInformationViewModel
        {
            var userInformationIdParameter = new SqlParameter("@UserInformationId", userInformationId);
            var clientIdParameter = new SqlParameter("@clientId", clientId);
            try
            {
                var userInformationList = this.Database
                   .SqlQuery<T>("sp_GetEmployeeSupervisors @UserInformationId, @clientId",
                                                    userInformationIdParameter, clientIdParameter)
                   .ToList<T>();
                return userInformationList;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }


        public  Task<List<T>> SP_GetClosedNotifications<T>(string userLoginEmail, int clientId, bool Unread, int UserLoginId, NotificationFilterViewModel notificationFilterViewModel) where T : BellIconNotificationViewModel
        {
            var LoginEmailIdParameter = new SqlParameter("@LoginEmail", userLoginEmail);
            var clientIdParameter = new SqlParameter("@clientId", clientId);
            var loginUserIdParameter = new SqlParameter("@LoginUserId", UserLoginId);
            var UnreadParameter = new SqlParameter("@Unread", Unread);
            var ChangeRequestStatusId = new SqlParameter("@ChangeRequestStatusId", 2);


            var fromDate = new SqlParameter("@FromDate", notificationFilterViewModel.StartDate);
            fromDate.Value = (object)notificationFilterViewModel.StartDate ?? DBNull.Value;

            var toDate = new SqlParameter("@ToDate", notificationFilterViewModel.EndDate);
            toDate.Value = (object)notificationFilterViewModel.EndDate ?? DBNull.Value;

            var employeeid = new SqlParameter("@EmployeeId", notificationFilterViewModel.EmployeeId);
            employeeid.Value = (object)notificationFilterViewModel.EmployeeId ?? DBNull.Value;

            var shortfullname = new SqlParameter("@ShortFullName", notificationFilterViewModel.EmployeeName);
            shortfullname.Value = (object)notificationFilterViewModel.EmployeeName ?? DBNull.Value;


            var workflowtriggertypeid = new SqlParameter("@WorkFlowTriggerTypeId", notificationFilterViewModel.WorkflowTriggerTypeId);
            workflowtriggertypeid.Value = (object)notificationFilterViewModel.WorkflowTriggerTypeId ?? DBNull.Value;

            var selectedworkflowstatusid = new SqlParameter("@WorkFlowActionTypeIds", notificationFilterViewModel.SelectedWorkflowStatusId);
            selectedworkflowstatusid.Value = (object)notificationFilterViewModel.SelectedWorkflowStatusId ?? DBNull.Value;


            try
            {
         
      
                return Task.Factory.StartNew<List<T>>(() =>
                {
                    //return this.Database
                    //   .SqlQuery<T>("sp_GetClosedWorkFlowNotifications @LoginEmail, @clientId , @LoginUserId, @Unread, @ChangeRequestStatusId, @FromDate, @ToDate, @EmployeeId, @ShortFullName, @WorkFlowTriggerTypeId, @WorkFlowActionTypeIds ",
                    //                                    LoginEmailIdParameter, clientIdParameter, loginUserIdParameter, UnreadParameter, ChangeRequestStatusId, fromDate, toDate,employeeid,shortfullname,workflowtriggertypeid,selectedworkflowstatusid)
                    //   .ToList<T>();
                    var res = this.Database
                       .SqlQuery<T>("sp_GetClosedWorkFlowNotifications @clientId , @LoginUserId, @Unread, @ChangeRequestStatusId, @FromDate, @ToDate, @EmployeeId, @ShortFullName, @WorkFlowTriggerTypeId, @WorkFlowActionTypeIds,@LoginEmail",
                                                         clientIdParameter, loginUserIdParameter, UnreadParameter, ChangeRequestStatusId, fromDate, toDate, employeeid, shortfullname, workflowtriggertypeid, selectedworkflowstatusid, LoginEmailIdParameter)
                      ;
                    return res.ToList<T>();
                }
                );
             
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }


        public Task<List<T>> SP_GetPendingNotifications<T>(int userLoginId, int clientId, int ChangeRequestStatusId = 1) where T : BellIconNotificationViewModel
        {
            var LoginUserIdParameter = new SqlParameter("@LoginUserId", userLoginId);
            var clientIdParameter = new SqlParameter("@clientId", clientId);
            var changeRequestIdParameter = new SqlParameter("@ChangeRequestStatusId", ChangeRequestStatusId);

            try
            {
                return Task.Factory.StartNew<List<T>>(() =>
                {
                    return this.Database
                   .SqlQuery<T>("sp_GetWorkFlowNotifications  @ClientId, @LoginUserId, @ChangeRequestStatusId",
                                                   clientIdParameter, LoginUserIdParameter, changeRequestIdParameter)
                   .ToList<T>();

                });
                //return pendingnotificationlist;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        

        public Task<List<T>> SP_GetNotificationsHistory<T>(string userLoginEmail, int clientId,NotificationFilterViewModel notificationFilterViewModel) where T : BellIconNotificationViewModel
        {
            var LoginEmailIdParameter = new SqlParameter("@LoginEmail", userLoginEmail);
            var clientIdParameter = new SqlParameter("@clientId", clientId);

            var fromDate = new SqlParameter("@FromDate", notificationFilterViewModel.StartDate);
            fromDate.Value = (object)notificationFilterViewModel.StartDate ?? DBNull.Value;

            var toDate = new SqlParameter("@ToDate", notificationFilterViewModel.EndDate);
            toDate.Value = (object)notificationFilterViewModel.EndDate ?? DBNull.Value;

            var employeeid = new SqlParameter("@EmployeeId", notificationFilterViewModel.EmployeeId);
            employeeid.Value = (object)notificationFilterViewModel.EmployeeId ?? DBNull.Value;

            var shortfullname = new SqlParameter("@ShortFullName", notificationFilterViewModel.EmployeeName);
            shortfullname.Value = (object)notificationFilterViewModel.EmployeeName ?? DBNull.Value;


            var workflowtriggertypeid = new SqlParameter("@WorkFlowTriggerTypeId", notificationFilterViewModel.WorkflowTriggerTypeId);
            workflowtriggertypeid.Value = (object)notificationFilterViewModel.WorkflowTriggerTypeId ?? DBNull.Value;

            var selectedworkflowstatusid = new SqlParameter("@WorkFlowActionTypeIds", notificationFilterViewModel.SelectedWorkflowStatusId);
            selectedworkflowstatusid.Value = (object)notificationFilterViewModel.SelectedWorkflowStatusId ?? DBNull.Value;

            try
            {
                return Task.Factory.StartNew<List<T>>(() =>
                {
                    return this.Database
                   .SqlQuery<T>("sp_GetWorkflowNotificationHistory  @ClientId, @LoginEmail,@FromDate, @ToDate, @EmployeeId, @ShortFullName, @WorkFlowTriggerTypeId, @WorkFlowActionTypeIds ",
                                                   clientIdParameter, LoginEmailIdParameter,fromDate,toDate, employeeid, shortfullname, workflowtriggertypeid, selectedworkflowstatusid)
                   .ToList<T>();

                });
                //return pendingnotificationlist;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }


        public List<T> SP_GetUserInformationByWorkflowLevelGroupId<T>(int workflowLevelId, int clientId) where T : UserInformationViewModel
        {
            var workflowLevelIdParameter = new SqlParameter("@WorkflowLevelId", workflowLevelId);
            var clientIdParameter = new SqlParameter("@clientId", clientId);
            try
            {
                var userInformationList = this.Database
                   .SqlQuery<T>("sp_GetUserInformationByWorkflowLevelGroupId @WorkflowLevelId, @clientId",
                                                    workflowLevelIdParameter, clientIdParameter)
                   .ToList<T>();
                return userInformationList;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        public T SP_UserInformationById<T>(int id) where T : UserInformation
        {
            var userIdParameter = new SqlParameter("@userId", id);
            try
            {
                var userInformation = this.Database
                    .SqlQuery<T>("sp_UserInformationById @userId", userIdParameter)
                    .FirstOrDefault<T>();
                return userInformation;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public T SP_UserInformationWithEmploymentById<T>(int id) where T : UserInformationViewModel
        {
            var userIdParameter = new SqlParameter("@userId", id);
            try
            {
                var userInformation = this.Database
                    .SqlQuery<T>("sp_UserInformationById @userId", userIdParameter)
                    .FirstOrDefault<T>();
                return userInformation;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<T> SP_ApplicantInformation<T>(string applicantName, int positionId, int applicantStatusId,int locationId ,string applicantQAQuery, int qACriteriaCount, int clientId, int companyId) where T : ApplicantInformationViewModel
        {

            var applicantNameParameter = new SqlParameter("@applicantName", applicantName);
            var applicantPostionIdParameter = new SqlParameter("@positionId", positionId);
            var applicantStatusIdParameter = new SqlParameter("@applicantStatusId", applicantStatusId);
            var applicantLocationIdParameter = new SqlParameter("@locationId", locationId);
            var applicantInterviewQAQueryParameter = new SqlParameter("@applicantQAQuery", applicantQAQuery);
            var applicantQACriteriaCountParameter = new SqlParameter("@applicantQACriteriaCount", qACriteriaCount);
            var clientIdParameter = new SqlParameter("@clientId", clientId);
            var companyIdParameter = new SqlParameter("@companyId", companyId);
            try
            {
                var appInformationList = this.Database
                   .SqlQuery<T>("sp_ApplicantInformation  @applicantName,@positionId ,@applicantStatusId,@locationId ,@applicantQAQuery,@applicantQACriteriaCount, @clientId, @companyId ",
                                                     applicantNameParameter, applicantPostionIdParameter, applicantStatusIdParameter,applicantLocationIdParameter ,applicantInterviewQAQueryParameter, applicantQACriteriaCountParameter
                                                    , clientIdParameter, companyIdParameter)
                   .ToList<T>();
                return appInformationList;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public T SP_ApplicantInformationById<T>(int id) where T : ApplicantInformationViewModel
        {
            var applicantIdParameter = new SqlParameter("@applicantId", id);
            try
            {
                var applicantInformation = this.Database
                    .SqlQuery<T>("sp_ApplicantInformationById @applicantId", applicantIdParameter)
                    .FirstOrDefault<T>();
                return applicantInformation;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<T> SP_TimeOffRequestAutoCancellation<T>(int clientId, DateTime executionDate) where T : EmployeeTimeOffRequestViewModel
        {
            var clientIdParameter = new SqlParameter("@clientId", clientId);
            var executionDateParameter = new SqlParameter("@executionDate", executionDate);
            try
            {
                var timeOffRequests = this.Database
                    .SqlQuery<T>("sp_TimeOffRequestAutoCancellation @clientId, @executionDate", clientIdParameter, executionDateParameter)
                    .ToList<T>();

                return timeOffRequests;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<T> SP_CRAutoCancellation<T>(int clientId, DateTime executionDate) where T : ChangeRequestViewModel
        {
            var clientIdParameter = new SqlParameter("@clientId", clientId);
            var executionDateParameter = new SqlParameter("@executionDate", executionDate);
            try
            {
                var changeRequests = this.Database
                    .SqlQuery<T>("sp_CRAutoCancellation @clientId, @executionDate", clientIdParameter, executionDateParameter)
                    .ToList<T>();

                return changeRequests;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<T> SP_GetEmployeeAccrualRules<T>(int id) where T : EmployeeAccrualRuleViewModel
        {
            var userIdParameter = new SqlParameter("@userId", id);
            try
            {
                var employeeAccrualRules = this.Database
                    .SqlQuery<T>("sp_GetEmployeeAccrualRules @userId", userIdParameter)
                    .ToList<T>();
                return employeeAccrualRules;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<T> SP_GetEmployeeAccrualBalances<T>(int id) where T : EmployeeAccrualBalanceViewModel
        {
            var userIdParameter = new SqlParameter("@userId", id);
            try
            {
                var employeeAccrualBalances = this.Database
                    .SqlQuery<T>("sp_GetEmployeeAccrualBalances @userId", userIdParameter)
                    .ToList<T>();
                return employeeAccrualBalances;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void SP_MarkInvalidateSecurityCode(int userInformationId, int clientId) 
        {

           
            var userInfoIdParameter = new SqlParameter("@UserInformationId", userInformationId);
            var cltIdParameter = new SqlParameter("@clientId", clientId);
            try
            {
                var userInformationList = this.Database
                   .SqlQuery<int>("sp_MarkInvalideSecurityCode @UserInformationId, @clientId",
                                                    userInfoIdParameter, cltIdParameter).FirstOrDefault();
               
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<T> SP_AuditLogInfo<T>(DateTime? fromDate,DateTime? toDate,int employeeId, string employeeName,string actionType,string recordType ,int superviserId, int clientId, int companyId) where T : AuditLogListViewModel
        {
            var fromDateParameter = new SqlParameter("@fromDate", fromDate);
            var toDateParameter = new SqlParameter("@toDate", toDate);
            var employeeIdParameter = new SqlParameter("@employeeId", employeeId);
            var employeeNameParameter = new SqlParameter("@employeeName", employeeName??"");
            var actionTypeParameter = new SqlParameter("@actionType", actionType??"");
            var recordTypeParameter = new SqlParameter("@recordType", recordType??"");
            var superviserIdParameter = new SqlParameter("@superviserId", superviserId);
            var clientIdParameter = new SqlParameter("@clientId", clientId);
            var companyIdParameter = new SqlParameter("@companyId", companyId);
            
            try
            {
                var auditLogList = this.Database
                   .SqlQuery<T>("sp_AuditLogInfo @fromDate,@toDate,@actionType,@recordType,@employeeId,@employeeName,@superviserId, @clientId,@companyId",
                                                    fromDateParameter, toDateParameter, actionTypeParameter, recordTypeParameter, employeeIdParameter, employeeNameParameter, superviserIdParameter, clientIdParameter, companyIdParameter)
                   .ToList<T>();
                return auditLogList;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
