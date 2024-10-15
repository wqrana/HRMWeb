namespace TimeAide.Web.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.Entity.Core.Objects;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using TimeAide.Common.Helpers;

    //[DbConfigurationType(typeof(CustomDbConfiguration))]
    public partial class TimeAideContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserInformation>()
            .HasMany(e => e.EmployeeDocument)
            .WithRequired(e => e.UserInformation)
            .WillCascadeOnDelete(false);

            #region ContactInformation Foreign Keys
            modelBuilder.Entity<City>()
            .HasMany(e => e.HomeCityUserContactInformation)
            .WithOptional(e => e.HomeCity)
            .HasForeignKey(e => e.HomeCityId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<City>()
            .HasMany(e => e.MailingCityUserContactInformation)
            .WithOptional(e => e.MailingCity)
            .HasForeignKey(e => e.MailingCityId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<State>()
            .HasMany(e => e.HomeStateUserContactInformation)
            .WithOptional(e => e.HomeState)
            .HasForeignKey(e => e.HomeStateId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<State>()
            .HasMany(e => e.MailingStateUserContactInformation)
            .WithOptional(e => e.MailingState)
            .HasForeignKey(e => e.MailingStateId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<Country>()
            .HasMany(e => e.HomeCountryUserContactInformation)
            .WithOptional(e => e.HomeCountry)
            .HasForeignKey(e => e.HomeCountryId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<Country>()
            .HasMany(e => e.MailingCountryUserContactInformation)
            .WithOptional(e => e.MailingCountry)
            .HasForeignKey(e => e.MailingCountryId)
            .WillCascadeOnDelete(false);
            #endregion

            #region UserInformation FK with SupervisorUserSupervisor and EmployeeUser
            modelBuilder.Entity<UserInformation>()
            .HasMany(e => e.SupervisorUserSupervisor)
            .WithRequired(e => e.SupervisorUser)
            .HasForeignKey(e => e.SupervisorUserId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserInformation>()
            .HasMany(e => e.EmployeeUserSupervisor)
            .WithRequired(e => e.EmployeeUser)
            .HasForeignKey(e => e.EmployeeUserId)
            .WillCascadeOnDelete(false);
            #endregion


            modelBuilder.Entity<Client>()
            .HasMany(e => e.Form)
            .WithOptional(e => e.Client)
            .HasForeignKey(e => e.ClientId);

            modelBuilder.Entity<Client>()
            .HasMany(e => e.Form1)
            .WithOptional(e => e.Client1)
            .HasForeignKey(e => e.ClientId);

            modelBuilder.Entity<UserInformation>()
            .HasMany(e => e.EmploymentHistory)
            .WithRequired(e => e.UserInformation)
            .HasForeignKey(e => e.UserInformationId)
            .WillCascadeOnDelete(false);

            #region NotificationMessages
            modelBuilder.Entity<NotificationMessage>()
            .HasMany(e => e.Workflow)
            .WithOptional(e => e.ClosingNotificationMessage)
            .HasForeignKey(e => e.ClosingNotificationId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<NotificationMessage>()
            .HasMany(e => e.Workflow)
            .WithOptional(e => e.ReminderNotificationMessage)
            .HasForeignKey(e => e.ReminderNotificationMessageId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<NotificationMessage>()
            .HasMany(e => e.Workflow)
            .WithOptional(e => e.CancelNotificationMessage)
            .HasForeignKey(e => e.CancelNotificationMessageId)
            .WillCascadeOnDelete(false);
            #endregion
            modelBuilder.Entity<RateFrequency>()
            .HasMany(e => e.PayInformationHistory)
            .WithOptional(e => e.CommRateFrequency)
            .HasForeignKey(e => e.CommRateFrequencyId)
            .WillCascadeOnDelete(false);

            #region Decimal Precision setting
            modelBuilder.Entity<Department>()
            .Property(e => e.CFSECompanyPercent)
            .HasPrecision(18, 5);
            modelBuilder.Entity<Client>()
            .Property(e => e.DepartamentoDelTrabajoRate)
            .HasPrecision(18, 5);
            modelBuilder.Entity<SubDepartment>()
            .Property(e => e.CFSECompanyPercent)
            .HasPrecision(18, 5);
            modelBuilder.Entity<PayInformationHistory>().Property(e => e.RateAmount)
            .HasColumnType("Decimal")
            .HasPrecision(18, 5);
            modelBuilder.Entity<PayInformationHistory>().Property(e => e.CommRateAmount)
            .HasColumnType("Decimal")
            .HasPrecision(18, 5);
            modelBuilder.Entity<PayInformationHistory>().Property(e => e.PeriodGrossPay)
            .HasColumnType("Decimal")
            .HasPrecision(18, 5);
            modelBuilder.Entity<PayInformationHistory>().Property(e => e.YearlyGrossPay)
            .HasColumnType("Decimal")
            .HasPrecision(18, 5);
            modelBuilder.Entity<PayInformationHistory>().Property(e => e.YearlyCommBasePay)
            .HasColumnType("Decimal")
            .HasPrecision(18, 5);
            modelBuilder.Entity<PayInformationHistory>().Property(e => e.YearlyBaseNCommPay)
            .HasColumnType("Decimal")
            .HasPrecision(18, 5);
            #endregion

            modelBuilder.Entity<CompanyCompensation>()
            .HasOptional(s => s.CompanyCompensationPRPayExport)
            .WithRequired(ad => ad.CompanyCompensation);

            
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public override int SaveChanges()
        {
            var manager = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager;
            var list = GetLogEntries(manager);
            if (list.Any(l => l.ReferenceId == 0))
            {
                base.SaveChanges();
                foreach (var each in list.Where(l => l.ReferenceId == 0))
                {
                    each.ReferenceId = (each.ReferenceObject as BaseEntity).Id;
                }
            }
            return base.SaveChanges();


        }

        public List<AuditLog> GetLogEntries(ObjectStateManager entities)
        {
            var list = ChangeTracker.Entries().ToList();
            List<AuditLog> listLogs = new List<AuditLog>();
            var entries = entities.GetObjectStateEntries(EntityState.Added | EntityState.Modified | EntityState.Deleted);

            foreach (var entry in entries)
            {

                var tableName = entry.EntitySet.Name;
                if (tableName == "AuditLog" || tableName == "AuditLogDetail" || tableName == "UserSessionLog" || tableName == "UserSessionLogDetail" || tableName == "NotificationServiceEvent" || tableName == "DataMigrationLog" || tableName == "DataMigrationLogDetail")
                    continue;
                if (!string.IsNullOrEmpty(tableName) && tableName.Contains("_"))
                {
                    var parts = tableName.Split('_');
                    if (parts.Count() == 2)
                    {
                        tableName = parts[0];
                    }
                }
                var log = new AuditLog()
                {
                    ActionType = entry.State.ToString(),
                    TableName = tableName,
                    ReferenceId = GetPrimaryKeys(entry),
                    ReferenceObject = entry.Entity,
                };
                if(entry.State== EntityState.Added && tableName == "EmailBlast")
                {
                    if ((log.ReferenceObject as EmailBlast).IsSavedAsDraft)
                        log.Remarks = "Saved as draft";
                }

                if (SessionHelper.LoginId != 0)
                    log.UserInformationId = SessionHelper.LoginId;
                if (SessionHelper.UserSessionLogDetailId != 0)
                    log.UserSessionLogDetailId = SessionHelper.UserSessionLogDetailId;
                this.AuditLog.Add(log);
                listLogs.Add(log);
                if (entry.State == EntityState.Deleted)
                {
                    SetReferenceUserId(entry, tableName, log);
                    LogDelete(entities, entry, tableName, log);
                }
                else
                {
                    SetReferenceUserId(entry, tableName, log);
                    if (!IsSoftDeleted(entities, entry))
                    {
                        LogAddEdit(entities, entry, tableName, log);
                    }
                    else
                    {
                        LogSoftDeleted(entities, entry, tableName, log);
                    }
                }

            }
            return listLogs;
        }
        private void SetReferenceUserId(ObjectStateEntry entry, string tableName, AuditLog log)
        {
            int? refUserId=null;
            if(entry.Entity is BaseUserObjects)
            {
                refUserId = (entry.Entity as BaseUserObjects).UserInformationId;
            }
            else if (entry.Entity is ChangeRequestBase)
            {
                refUserId = (entry.Entity as ChangeRequestBase).UserInformationId;
            }
            else
            {
                switch (tableName)
                {
                    case "UserInformation":
                        refUserId = (entry.Entity as UserInformation).Id;
                        break;
                    case "UserContactInformation":
                        refUserId = (entry.Entity as UserContactInformation).UserInformationId;
                        break;
                    case "ChangePasswordByAdminReason":
                        refUserId = (entry.Entity as ChangePasswordByAdminReason).UserInformationId;
                        break;
                    case "EmergencyContact":
                        refUserId = (entry.Entity as EmergencyContact).UserInformationId;
                        break;
                    case "EmployeeSupervisor":
                        refUserId = (entry.Entity as EmployeeSupervisor).EmployeeUserId;
                        break;
                    case "SupervisorCompany":
                        refUserId = (entry.Entity as SupervisorCompany).UserInformationId;
                        break;
                    case "SupervisorDepartment":
                        refUserId = (entry.Entity as SupervisorDepartment).UserInformationId;
                        break;
                    case "SupervisorEmployeeType":
                        refUserId = (entry.Entity as SupervisorEmployeeType).UserInformationId;
                        break;
                    case "SupervisorSubDepartment":
                        refUserId = (entry.Entity as SupervisorSubDepartment).UserInformationId;
                        break;
                    case "UserEmployeeGroup":
                        refUserId = (entry.Entity as UserEmployeeGroup).UserInformationId;
                        break;
                    case "UserInformationRole":
                        refUserId = (entry.Entity as UserInformationRole).UserInformationId;
                        break;
                    case "EmploymentHistoryAuthorizer":
                        refUserId = (entry.Entity as EmploymentHistoryAuthorizer).EmploymentHistory.UserInformationId;
                        break;
                    case "PayInformationHistoryAuthorizer":
                        refUserId = (entry.Entity as PayInformationHistoryAuthorizer).PayInformationHistory.UserInformationId;
                        break;
                    case "EmployeeVeteranStatus":
                       
                        refUserId = (entry.Entity as EmployeeVeteranStatus).UserInformationId;
                        break;
                    case "UserInformationActivation":                      
                        refUserId = (entry.Entity as UserInformationActivation).UserInformationId;
                        break;
                    case "WorkflowTriggerRequest":
                        refUserId = (entry.Entity as WorkflowTriggerRequest).ChangeRequest.UserInformationId;
                        break;
                    case "WorkflowTriggerRequestDetail":
                        refUserId = (entry.Entity as WorkflowTriggerRequestDetail).WorkflowTriggerRequest.ChangeRequest.UserInformationId;
                        break;
                    case "WorkflowTriggerRequestDetailEmail":
                        refUserId = (entry.Entity as WorkflowTriggerRequestDetailEmail).WorkflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequest.UserInformationId;
                        break;
                    case "EmployeeTimeOffRequestDocument":
                        refUserId = (entry.Entity as EmployeeTimeOffRequestDocument).EmployeeTimeOffRequest.UserInformationId;
                        break;
                        
                }             


            }
            log.RefUserInformationId = refUserId;

        }
        private bool IsSoftDeleted(ObjectStateManager entities, ObjectStateEntry entry)
        {
            var currentEntry = entities.GetObjectStateEntry(entry.EntityKey);
            var currentValues = currentEntry.CurrentValues;
            var properties = currentEntry.GetModifiedProperties();
            var recCount = properties.Where(s => s == "DataEntryStatus").Count();
            if (recCount > 0)
            {
              var recStatus= int.Parse(currentValues["DataEntryStatus"].ToString());
                if (recStatus == 0)
                {
                    return true;
                }  
            }
            return false;
        }
        private void LogSoftDeleted(ObjectStateManager entities, ObjectStateEntry entry, string tableName, AuditLog log)
        {
            var currentEntry = entities.GetObjectStateEntry(entry.EntityKey);
            var originalValues = this.Entry(entry.Entity).GetDatabaseValues();
            BaseEntity baseEntity = entry.Entity as BaseEntity;
            log.ActionType = "Deleted";
            var logDetail = new AuditLogDetail()
            {
                AuditLog = log,
                ColumnName = "Id",
                OldValue = "",
                NewValue = baseEntity.Id.ToString()
                };
                log.AuditLogDetail.Add(logDetail);
            }
        private void LogAddEdit(ObjectStateManager entities, ObjectStateEntry entry, string tableName, AuditLog log)
        {
            var currentEntry = entities.GetObjectStateEntry(entry.EntityKey);
            var currentValues = currentEntry.CurrentValues;
            //Get properties in case of added record
            IEnumerable<string> properties = currentValues.DataRecordInfo.FieldMetadata.Select(s => s.FieldType.Name);
            
            //var originalValues = currentEntry.OriginalValues;
            DbPropertyValues originalValues = null;
            if (entry.State == EntityState.Modified)
            {
                originalValues = this.Entry(entry.Entity).GetDatabaseValues();
                properties = currentEntry.GetModifiedProperties();
            }
            string[] ignoredProperties = { "Id", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "Old_Id" };
            foreach (var propName in properties)
            {
                if (ignoredProperties.Contains(propName))
                    continue;
                var oldValue = "";
                if (originalValues != null && originalValues[propName] != null)                   
                        oldValue = originalValues[propName].ToString();

                var newValue = currentValues[propName].ToString();
                if (oldValue == newValue) continue;

                var logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = propName,
                    OldValue = oldValue,
                    NewValue = newValue
                };
                log.AuditLogDetail.Add(logDetail);
            }
            if (tableName == "EmployeeDocument")
            {
                try
                {
                    EmployeeDocument baseEntity = entry.Entity as EmployeeDocument;
                    log.Remarks = baseEntity.SefServiceRemarks;
                }
                catch
                { }
            }
            else if (tableName == "EmergencyContact")
            {
                try
                {
                    EmergencyContact baseEntity = entry.Entity as EmergencyContact;
                    log.Remarks = baseEntity.SefServiceRemarks;
                }
                catch
                { }
            }
            else if (tableName == "EmployeeCredential")
            {
                try
                {
                    EmployeeCredential baseEntity = entry.Entity as EmployeeCredential;
                    log.Remarks = baseEntity.SefServiceRemarks;
                }
                catch
                { }
            }
            else if (tableName == "ChangePasswordByAdminReason")
            {
                try
                { 
                    ChangePasswordByAdminReason baseEntity = entry.Entity as ChangePasswordByAdminReason;
                    log.ReferenceId = baseEntity.UserInformationId??0;
                    var logDetail = new AuditLogDetail()
                    {
                        AuditLog = log,
                        ColumnName = "Reason",
                        OldValue = "",
                        NewValue = baseEntity.Reason
                    };
                    log.AuditLogDetail.Add(logDetail);
                }
                catch
                { }
            }
            else if (tableName == "EmployeeSupervisor")
            {
                AddSupervisorDetail(log, currentValues, "EmployeeUserId");
                AddSupervisorDetail(log, currentValues, "SupervisorUserId");
            }
            else if (tableName == "SupervisorCompany")
            {
                AddSupervisorDetail(log, currentValues, "CompanyId");
                AddSupervisorDetail(log, currentValues, "UserInformationId");
            }
            else if (tableName == "SupervisorDepartment")
            {
                AddSupervisorDetail(log, currentValues, "DepartmentId");
                AddSupervisorDetail(log, currentValues, "UserInformationId");
            }
            else if (tableName == "SupervisorEmployeeType")
            {
                AddSupervisorDetail(log, currentValues, "EmployeeTypeId");
                AddSupervisorDetail(log, currentValues, "UserInformationId");
            }
            else if (tableName == "SupervisorSubDepartment")
            {
                AddSupervisorDetail(log, currentValues, "SubDepartmentId");
                AddSupervisorDetail(log, currentValues, "UserInformationId");
            }
            else if (tableName == "UserEmployeeGroup")
            {
                AddSupervisorDetail(log, currentValues, "EmployeeGroupId");
                AddSupervisorDetail(log, currentValues, "UserInformationId");
            }
            else if (tableName == "UserInformationRole")
            {
                AddSupervisorDetail(log, currentValues, "RoleId");
                AddSupervisorDetail(log, currentValues, "UserInformationId");
            }
            else if (tableName == "EmploymentHistoryAuthorizer")
            {
                log.ReferenceId1 = long.Parse(currentValues["EmploymentHistoryId"].ToString());
                AddSupervisorDetail(log, currentValues, "AuthorizeById");               
            }

        }
        private void LogDelete(ObjectStateManager entities, ObjectStateEntry entry, string tableName, AuditLog log)
        {
            var currentEntry = entities.GetObjectStateEntry(entry.EntityKey);
            var originalValues = this.Entry(entry.Entity).GetDatabaseValues();
            BaseEntity baseEntity = entry.Entity as BaseEntity;
            if (tableName == "EmployeeSupervisor")
            {
                EmployeeSupervisor employeeSupervisor = baseEntity as EmployeeSupervisor;
                var logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "EmployeeUserId",
                    OldValue = "",
                    NewValue = employeeSupervisor.EmployeeUserId.ToString()
                };
                log.AuditLogDetail.Add(logDetail);

                logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "SupervisorUserId",
                    OldValue = "",
                    NewValue = employeeSupervisor.SupervisorUserId.ToString()
                };
                log.AuditLogDetail.Add(logDetail);
            }
            else if (tableName == "SupervisorCompany")
            {
                SupervisorCompany supervisorCompany = baseEntity as SupervisorCompany;
                var logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "UserInformationId",
                    OldValue = "",
                    NewValue = supervisorCompany.UserInformationId.ToString()
                };
                log.AuditLogDetail.Add(logDetail);

                logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "CompanyId",
                    OldValue = "",
                    NewValue = supervisorCompany.CompanyId.ToString()
                };
                log.AuditLogDetail.Add(logDetail);
            }
            else if (tableName == "SupervisorDepartment")
            {
                SupervisorDepartment supervisorDepartment = baseEntity as SupervisorDepartment;
                var logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "UserInformationId",
                    OldValue = "",
                    NewValue = supervisorDepartment.UserInformationId.ToString()
                };
                log.AuditLogDetail.Add(logDetail);

                logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "DepartmentId",
                    OldValue = "",
                    NewValue = supervisorDepartment.DepartmentId.ToString()
                };
                log.AuditLogDetail.Add(logDetail);
            }
            else if (tableName == "SupervisorEmployeeType")
            {
                SupervisorEmployeeType supervisorEmployeeType = baseEntity as SupervisorEmployeeType;
                var logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "UserInformationId",
                    OldValue = "",
                    NewValue = supervisorEmployeeType.UserInformationId.ToString()
                };
                log.AuditLogDetail.Add(logDetail);

                logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "EmployeeTypeId",
                    OldValue = "",
                    NewValue = supervisorEmployeeType.EmployeeTypeId.ToString()
                };
                log.AuditLogDetail.Add(logDetail);
            }
            else if (tableName == "SupervisorSubDepartment")
            {
                SupervisorSubDepartment supervisorSubDepartment = baseEntity as SupervisorSubDepartment;
                var logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "UserInformationId",
                    OldValue = "",
                    NewValue = supervisorSubDepartment.UserInformationId.ToString()
                };
                log.AuditLogDetail.Add(logDetail);

                logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "SubDepartmentId",
                    OldValue = "",
                    NewValue = supervisorSubDepartment.SubDepartmentId.ToString()
                };
                log.AuditLogDetail.Add(logDetail);
            }

            else if (tableName == "UserEmployeeGroup")
            {
                UserEmployeeGroup userEmployeeGroup = baseEntity as UserEmployeeGroup;
                var logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "UserInformationId",
                    OldValue = "",
                    NewValue = userEmployeeGroup.UserInformationId.ToString()
                };
                log.AuditLogDetail.Add(logDetail);

                logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "EmployeeGroupId",
                    OldValue = "",
                    NewValue = userEmployeeGroup.EmployeeGroupId.ToString()
                };
                log.AuditLogDetail.Add(logDetail);
            }
            else if (tableName == "UserInformationRole")
            {
                UserInformationRole userInformationRole = baseEntity as UserInformationRole;
                var logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "UserInformationId",
                    OldValue = "",
                    NewValue = userInformationRole.UserInformationId.ToString()
                };
                log.AuditLogDetail.Add(logDetail);

                logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "RoleId",
                    OldValue = "",
                    NewValue = userInformationRole.RoleId.ToString()
                };
                log.AuditLogDetail.Add(logDetail);
            }
            else if (tableName == "EmploymentHistoryAuthorizer")
            {
                //EmploymentHistoryAuthorizer employmentHistoryAuthorizer = baseEntity as EmploymentHistoryAuthorizer;
                log.ReferenceId1 = long.Parse(originalValues["EmploymentHistoryId"].ToString());
                var  logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "AuthorizeById",
                    OldValue = "",
                    NewValue = originalValues["AuthorizeById"].ToString()
                };
                log.AuditLogDetail.Add(logDetail);
            }
            else if (tableName == "PayInformationHistoryAuthorizer")
            {
                //EmploymentHistoryAuthorizer employmentHistoryAuthorizer = baseEntity as EmploymentHistoryAuthorizer;
                log.ReferenceId1 = long.Parse(originalValues["PayInformationHistoryId"].ToString());
                var logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "AuthorizeById",
                    OldValue = "",
                    NewValue = originalValues["AuthorizeById"].ToString()
                };
                log.AuditLogDetail.Add(logDetail);
            }
            else if (tableName == "EmployeeVeteranStatus")
            {
                //EmploymentHistoryAuthorizer employmentHistoryAuthorizer = baseEntity as EmploymentHistoryAuthorizer;
                log.ReferenceId1 = long.Parse(originalValues["Id"].ToString());
                var logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "VeteranStatusId",
                    OldValue = "",
                    NewValue = originalValues["VeteranStatusId"].ToString()
                };
                log.AuditLogDetail.Add(logDetail);
            }
        }
        private static void AddSupervisorDetail(AuditLog log, CurrentValueRecord currentValues, string propertyName)
        {
            string newValue = currentValues[propertyName].ToString();
            if (string.IsNullOrEmpty(newValue))
                return;
            var logDetail = new AuditLogDetail()
            {
                AuditLog = log,
                ColumnName = propertyName,
                OldValue = "",
                NewValue = newValue
            };
            log.AuditLogDetail.Add(logDetail);
        }
        private static int GetPrimaryKeys(ObjectStateEntry entry)
        {
            if (entry.EntityKey == null || entry.EntityKey.EntityKeyValues == null || entry.EntityKey.EntityKeyValues.Length == 0) return 0;
            System.Data.Entity.Core.EntityKeyMember objPk = entry.EntityKey.EntityKeyValues[0];
            int pk;
            if (int.TryParse(objPk.Value.ToString(), out pk))
                return pk;
            else
                return 0;
        }

        //public UserInformation DataMigrationUser { get; set; }
    }
}
