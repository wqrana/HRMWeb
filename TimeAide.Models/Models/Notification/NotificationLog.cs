using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{
    [Table("NotificationLog")]
    public class NotificationLog : BaseCompanyObjects
    {
        public NotificationLog()
        {
            NotificationLogEmail = new HashSet<NotificationLogEmail>();
            NotificationLogMessageReadBy = new HashSet<NotificationLogMessageReadBy>();
        }
        [Display(Name = "Notification Log Id")]
        [Column("NotificationLogId")]
        public override int Id { get; set; }

        public int? NotificationScheduleDetailId { get; set; }
        public int DeliveryStatusId { get; set; }
        public int? EmployeeDocumentId { get; set; }
        public virtual EmployeeDocument EmployeeDocument { get; set; }
        public int? EmployeeCredentialId { get; set; }
        public virtual EmployeeCredential EmployeeCredential { get; set; }
        public virtual NotificationScheduleDetail NotificationScheduleDetail { get; set; }
        public int? EmployeeCustomFieldId { get; set; }
        public virtual EmployeeCustomField EmployeeCustomField { get; set; }
        public int? EmploymentId { get; set; }
        public virtual Employment Employment { get; set; }
        public virtual int? EmployeePerformanceId { get; set; }
        public virtual EmployeePerformance EmployeePerformance { get; set; }
        public int? EmployeeTrainingId { get; set; }
        public virtual EmployeeTraining EmployeeTraining { get; set; }
        public int? EmployeeHealthInsuranceId { get; set; }
        public virtual EmployeeHealthInsurance EmployeeHealthInsurance { get; set; }
        public int? EmployeeDentalInsuranceId { get; set; }
        public virtual EmployeeDentalInsurance EmployeeDentalInsurance { get; set; }
        public int? EmployeeActionId { get; set; }
        public virtual EmployeeAction EmployeeAction { get; set; }
        public int? EmployeeBenefitHistoryId { get; set; }
        public virtual EmployeeBenefitHistory EmployeeBenefitHistory { get; set; }
        public int? UserInformationActivationId { get; set; }
        public virtual UserInformationActivation UserInformationActivation  {get; set; }
        public int? ReferredUserInformationId { get; set; }
        public virtual UserInformation ReferredUserInformation { get; set; }

        public int? EmailBlastId { get; set; }
        public virtual EmailBlast EmailBlast { get; set; }
        
        public bool? IsNotificationAsExpired { get; set; }

        public int? ChangeRequestAddressId { get; set; }
        public virtual ChangeRequestAddress ChangeRequestAddress { get; set; }

        [NotMapped]
        public string UserInformationName
        {
            get
            {
                if (EmployeeDocument != null)
                    return EmployeeDocument.UserInformation.ShortFullName;
                else if (EmployeeCredential != null)
                    return EmployeeCredential.UserInformation.ShortFullName;
                else if (EmployeeCustomField != null)
                    return EmployeeCustomField.UserInformation.ShortFullName;
                else if (Employment != null)
                    return Employment.UserInformation.ShortFullName;
                else if (EmployeePerformance != null)
                    return EmployeePerformance.UserInformation.ShortFullName;
                else if (EmployeeTraining != null)
                    return EmployeeTraining.UserInformation.ShortFullName;
                else if (EmployeeHealthInsurance != null)
                    return EmployeeHealthInsurance.UserInformation.ShortFullName;
                else if (EmployeeDentalInsurance != null)
                    return EmployeeDentalInsurance.UserInformation.ShortFullName;
                else if (EmployeeAction != null)
                    return EmployeeAction.UserInformation.ShortFullName;
                else if (EmployeeBenefitHistory != null)
                    return EmployeeBenefitHistory.UserInformation.ShortFullName;
                else if (UserInformationActivation != null)
                    return UserInformationActivation.UserInformation.ShortFullName;

                return "";
            }

        }
        public UserInformation UserInformation
        {
            get
            {
                if (EmployeeDocument != null)
                    return EmployeeDocument.UserInformation;
                else if (EmployeeCredential != null)
                    return EmployeeCredential.UserInformation;
                else if (EmployeeCustomField != null)
                    return EmployeeCustomField.UserInformation;
                else if (Employment != null)
                    return Employment.UserInformation;
                else if (EmployeePerformance != null)
                    return EmployeePerformance.UserInformation;
                else if (EmployeeTraining != null)
                    return EmployeeTraining.UserInformation;
                else if (EmployeeHealthInsurance != null)
                    return EmployeeHealthInsurance.UserInformation;
                else if (EmployeeDentalInsurance != null)
                    return EmployeeDentalInsurance.UserInformation;
                else if (EmployeeAction != null)
                    return EmployeeAction.UserInformation;
                else if (EmployeeBenefitHistory != null)
                    return EmployeeBenefitHistory.UserInformation;
                else if (UserInformationActivation != null)
                    return UserInformationActivation.UserInformation;
                else if (ReferredUserInformation != null)
                    return ReferredUserInformation;
                return null;
            }

        }
        [NotMapped]
        public string RecordType
        {
            get
            {
                if (EmployeeDocument != null)
                    return "Document";
                else if (EmployeeCredential != null)
                    return "Credential";
                else if (EmployeeCustomField != null)
                    return "CustomField";
                else if (Employment != null)
                    return "Employment";
                else if (EmployeePerformance != null)
                    return "Performance";
                else if (EmployeeTraining != null)
                    return "Training";
                else if (EmployeeHealthInsurance != null)
                    return "Health Insurance";
                else if (EmployeeDentalInsurance != null)
                    return "Dental Insurance";
                else if (EmployeeAction != null)
                    return "Action";
                else if (EmployeeBenefitHistory != null)
                    return "Benefit History";
                else if (UserInformationActivation != null)
                    return "UserInformation Activation";
                return "";
            }

        }
        [NotMapped]
        public string RecordName
        {
            get
            {
                if (EmployeeDocument != null)
                    return EmployeeDocument.Document.DocumentName;
                else if (EmployeeCredential != null)
                    return EmployeeCredential.Credential.CredentialName;
                else if (EmployeeCustomField != null)
                    return EmployeeCustomField.CustomField.CustomFieldName;
                else if (Employment != null)
                    return Employment.UserInformation.ShortFullName;
                else if (EmployeePerformance != null)
                    return EmployeePerformance.PerformanceDescription.PerformanceDescriptionName;
                else if (EmployeeTraining != null)
                    return EmployeeTraining.Training.TrainingName;
                else if (EmployeeHealthInsurance != null)
                    return EmployeeHealthInsurance.InsuranceType.InsuranceTypeName;
                else if (EmployeeDentalInsurance != null)
                    return EmployeeDentalInsurance.InsuranceType.InsuranceTypeName;
                else if (EmployeeAction != null)
                    return EmployeeAction.ActionDescription;
                else if (EmployeeBenefitHistory != null)
                    return EmployeeBenefitHistory.Benefit.BenefitName;
                else if (UserInformationActivation != null)
                    return UserInformationActivation.UserInformation.ShortFullName;
                return "";
            }

        }
        [NotMapped]
        public DateTime? ExpirationDate
        {
            get
            {
                if (EmployeeDocument != null && EmployeeDocument.ExpirationDate.HasValue)
                    return EmployeeDocument.ExpirationDate.Value;
                if (EmployeeCredential != null && EmployeeCredential.ExpirationDate.HasValue)
                    return EmployeeCredential.ExpirationDate.Value;
                if (EmployeeCustomField != null && EmployeeCustomField.ExpirationDate.HasValue)
                    return EmployeeCustomField.ExpirationDate.Value;
                else if (Employment != null && Employment.ProbationEndDate.HasValue)
                    return Employment.ProbationEndDate.Value;
                else if (EmployeePerformance != null && EmployeePerformance.ExpiryDate.HasValue)
                    return EmployeePerformance.ExpiryDate.Value;
                else if (EmployeeTraining != null && EmployeeTraining.ExpiryDate.HasValue)
                    return EmployeeTraining.ExpiryDate.Value;
                else if (EmployeeHealthInsurance != null && EmployeeHealthInsurance.InsuranceExpiryDate.HasValue)
                    return EmployeeHealthInsurance.InsuranceExpiryDate.Value;
                else if (EmployeeDentalInsurance != null && EmployeeDentalInsurance.InsuranceExpiryDate.HasValue)
                    return EmployeeDentalInsurance.InsuranceExpiryDate.Value;
                else if (EmployeeAction != null && EmployeeAction.ActionExpiryDate.HasValue)
                    return EmployeeAction.ActionExpiryDate.Value;
                else if (EmployeeBenefitHistory != null && EmployeeBenefitHistory.ExpiryDate.HasValue)
                    return EmployeeBenefitHistory.ExpiryDate.Value;
                else if (UserInformationActivation != null)
                    return UserInformationActivation.CreatedDate;
                return null;
            }

        }

        [NotMapped]
        public int ExpiringDays
        {
            get
            {
                DateTime testdate = DateTime.Now;
                //if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["TestDate"].ToString()))
                //{
                //    Console.WriteLine("Application is executing with test date from configuration.");
                //    testdate = Convert.ToDateTime(ConfigurationManager.AppSettings["TestDate"].ToString());
                //}

                if (EmployeeDocument != null && EmployeeDocument.ExpirationDate.HasValue)
                    return (int)(Math.Ceiling((EmployeeDocument.ExpirationDate.Value - testdate).TotalDays));
                else if (EmployeeCredential != null && EmployeeCredential.ExpirationDate.HasValue)
                    return (int)(Math.Ceiling((EmployeeCredential.ExpirationDate.Value - testdate).TotalDays));
                else if (EmployeeCustomField != null && EmployeeCustomField.ExpirationDate.HasValue)
                    return (int)(Math.Ceiling((EmployeeCustomField.ExpirationDate.Value - testdate).TotalDays));

                else if (Employment != null && Employment.ProbationEndDate.HasValue)
                    return (int)(Math.Ceiling((Employment.ProbationEndDate.Value - testdate).TotalDays));
                else if (EmployeePerformance != null && EmployeePerformance.ExpiryDate.HasValue)
                    return (int)(Math.Ceiling((EmployeePerformance.ExpiryDate.Value - testdate).TotalDays));
                else if (EmployeeTraining != null && EmployeeTraining.ExpiryDate.HasValue)
                    return (int)(Math.Ceiling((EmployeeTraining.ExpiryDate.Value - testdate).TotalDays));
                else if (EmployeeHealthInsurance != null && EmployeeHealthInsurance.InsuranceExpiryDate.HasValue)
                    return (int)(Math.Ceiling((EmployeeHealthInsurance.InsuranceExpiryDate.Value - testdate).TotalDays));
                else if (EmployeeDentalInsurance != null && EmployeeDentalInsurance.InsuranceExpiryDate.HasValue)
                    return (int)(Math.Ceiling((EmployeeDentalInsurance.InsuranceExpiryDate.Value - testdate).TotalDays));
                else if (EmployeeAction != null && EmployeeAction.ActionExpiryDate.HasValue)
                    return (int)(Math.Ceiling((EmployeeAction.ActionExpiryDate.Value - testdate).TotalDays));
                else if (EmployeeBenefitHistory != null && EmployeeBenefitHistory.ExpiryDate.HasValue)
                    return (int)(Math.Ceiling((EmployeeBenefitHistory.ExpiryDate.Value - testdate).TotalDays));
                else if (UserInformationActivation != null)
                    return (int)(Math.Ceiling((UserInformationActivation.CreatedDate - testdate).TotalDays));
                return 0;
            }

        }
        public int NotificationTypeId
        {
            get;
            set;
        }
        public virtual NotificationType NotificationType
        {
            get;
            set;
        }
        public virtual ICollection<NotificationLogMessageReadBy> NotificationLogMessageReadBy { get; set; }
        public virtual ICollection<NotificationLogEmail> NotificationLogEmail { get; set; }
    }
    public enum DeliveryStatus 
    {
        PendingSend = 1,
        Sent=2
    }
}
