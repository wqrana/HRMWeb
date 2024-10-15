namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("UserInformation")]
    public partial class UserInformation : BaseCompanyObjects
    {
        public UserInformation()
        {
            CreatedBy = Common.Helpers.SessionHelper.LoginId;
            UserContactInformations = new HashSet<UserContactInformation>();
            EmployeeUserSupervisor = new HashSet<EmployeeSupervisor>();
            SupervisorUserSupervisor = new HashSet<EmployeeSupervisor>();
            UserInformationActivation = new HashSet<UserInformationActivation>();
            UserInformationRole = new HashSet<UserInformationRole>();
            EmergencyContact = new HashSet<EmergencyContact>();
            EmploymentHistory = new HashSet<EmploymentHistory>();
            Employment = new HashSet<Employment>();
            EmploymentHistory1 = new HashSet<EmploymentHistory>();
            EmployeeVeteranStatus = new HashSet<EmployeeVeteranStatus>();
            //ApprovedEmployment = new HashSet<EmploymentHistory>();
            //EmployeeRequiredCredential = new HashSet<EmployeeRequiredCredential>();
            EmployeeCredential = new HashSet<EmployeeCredential>();
            EmployeeDocument = new HashSet<EmployeeDocument>();
            SupervisorSubDepartment = new HashSet<SupervisorSubDepartment>();
            SupervisorDepartment = new HashSet<SupervisorDepartment>();
            SupervisorCompany = new HashSet<SupervisorCompany>();
            UserEmployeeGroup = new HashSet<UserEmployeeGroup>();
            SupervisorEmployeeType = new HashSet<SupervisorEmployeeType>();
            PayInformationHistory =new HashSet<PayInformationHistory>();
        }

        [Display(Name = "User Id")]
        [Column("UserInformationId")]
        public override int Id { get; set; }

        [Display(Name = "Employee Id")]
        public int? EmployeeId { get; set; }
        // [Required]
        [StringLength(20)]
        [Display(Name = "Id Number")]
        public string IdNumber { get; set; }

        [StringLength(20)]
        [Display(Name = "System Id")]
        public string SystemId { get; set; }

        [StringLength(30)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(2)]
        [Display(Name = "Middle Initial")]
        public string MiddleInitial { get; set; }

        [StringLength(30)]
        [Display(Name = "First Last Name")]
        public string FirstLastName { get; set; }

        [StringLength(30)]
        [Display(Name = "Second Last Name")]
        public string SecondLastName { get; set; }

        [StringLength(50)]
        [Display(Name = "Short Full Name")]
        public string ShortFullName { get; set; }

        //[Display(Name = "Company Id")]
        //public int CompanyID { get; set; }

        [Display(Name = "Department Id")]
        public int? DepartmentId { get; set; }

        [Display(Name = "Sub Department Id")]
        public int? SubDepartmentId { get; set; }

        public int? PositionId { get; set; }

        [Display(Name = "Employee Type Id")]
        public int? EmployeeTypeID { get; set; }

        [Display(Name = "Employment Status Id")]
        public int? EmploymentStatusId { get; set; }

        [Display(Name = "Default Job Code")]
        public int? DefaultJobCodeId { get; set; }

        public int? EthnicityId { get; set; }

        public int? DisabilityId { get; set; }

        [Display(Name = "Employee Note")]
        [StringLength(50)]
        public string EmployeeNote { get; set; }

        [Display(Name = "Gender")]
        public int? GenderId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "DOB")]
        public DateTime? BirthDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Birth Place")]
        public string BirthPlace { get; set; }


        [Display(Name = "Marital Status")]
        public int? MaritalStatusId { get; set; }

        public MaritalStatus MaritalStatus { get; set; }
        public bool? HasAllCompany { get; set; }

        [NotMapped]
        public string SSN
        {
            get
            {
                if (!string.IsNullOrEmpty(SSNEncrypted))
                    return Common.Helpers.Encryption.Decrypt(SSNEncrypted);
                else
                    return SSNEncrypted;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    SSNEncrypted = Common.Helpers.Encryption.Encrypt(value);
                else

                    SSNEncrypted = value;
            }
        }

        [StringLength(512)]
        [Display(Name = "SSN Encrypted")]
        public string SSNEncrypted { get; set; }
        //[StringLength(9)]
        //[Display(Name = "SSN End")]
        public string SSNEnd { get; set; }
        [NotMapped]
        public string SSNEndComputed
        {
            get
            {
                if (SSN != null && SSN.Length == 9)
                    return SSN.Substring(SSN.Length - 4);
                else return null;
            }
        }
        [NotMapped]
        public string SSNComputed
        {
            get
            {
                if (SSN != null && SSN.Length == 9)
                {

                    return string.Format("{0}-{1}-{2}", SSN.Substring(0, 3), SSN.Substring(3, 2), SSN.Substring(5, 4));
                }
                else return null;
            }
        }
        //[Column(TypeName = "image")]
        //public byte[] imgPhoto { get; set; }
        [StringLength(512)]
        public string PictureFilePath { get; set; }

        [StringLength(512)]
        public string ResumeFilePath { get; set; }

        [StringLength(512)]
        public string PasswordHash { get; set; }

        [NotMapped]
        public string SelectedVeteranStatus { get; set; }

        public new int? CreatedBy { get; set; }
        public int? EmployeeStatusId { get; set; }
        public DateTime? EmployeeStatusDate { get; set; }
        public virtual EmployeeStatus EmployeeStatus { get; set; }
        public virtual Company Company { get; set; }

        public virtual Department Department { get; set; }

        public virtual EmploymentStatus EmploymentStatu { get; set; }

        public virtual JobCode DefaultJobCode { get; set; }
        public virtual EmployeeType EmployeeType { get; set; }
        //public virtual SupervisoryLevel SupervisoryLevel { get; set; }

        public virtual Client Client { get; set; }

        public virtual SubDepartment SubDepartment { get; set; }

        public virtual Position Position { get; set; }
        public virtual Ethnicity Ethnicity { get; set; }
        public virtual Disability Disability { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual BaseSchedule BaseSchedule { get; set; }

        public virtual ICollection<UserContactInformation> UserContactInformations { get; set; }
        public UserContactInformation ActiveUserContactInformation
        {
            get
            {
                return UserContactInformations.FirstOrDefault(u => u.DataEntryStatus == 1);
            }
        }

        //[NotMapped]
        //[Display(Name = "Login Email")]
        //[EmailAddress(ErrorMessage = "Invalid login email address")]
        //public string LoginEmail { get; set; }

        [StringLength(128)]

        public string AspNetUserId { get; set; }

        public int? RefUserInformationId { get; set; }
        public bool? IsRotatingSchedule { get; set; }
        public int? BaseScheduleId { get; set; }
        //[ForeignKey("AspNetUserId")]
        //public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual ICollection<UserInformationActivation> UserInformationActivation { get; set; }
        public virtual ICollection<PassswordResetCode> PassswordResetCode { get; set; }

        public virtual ICollection<SupervisorCompany> SupervisorCompany { get; set; }

        public virtual ICollection<UserEmployeeGroup> UserEmployeeGroup { get; set; }

        public virtual ICollection<SupervisorEmployeeType> SupervisorEmployeeType { get; set; }

        public virtual ICollection<SupervisorDepartment> SupervisorDepartment { get; set; }

        public virtual ICollection<SupervisorSubDepartment> SupervisorSubDepartment { get; set; }

        
        public virtual ICollection<EmployeeSupervisor> EmployeeUserSupervisor { get; set; }

        
        public virtual ICollection<EmployeeSupervisor> SupervisorUserSupervisor { get; set; }



        
        public virtual ICollection<ChangeRequestAddress> ChangeRequestAddress { get; set; }
        public virtual ICollection<ChangeRequestEmailNumbers> ChangeRequestEmailNumbers { get; set; }
        public virtual ICollection<ChangeRequestEmergencyContact> ChangeRequestEmergencyContact { get; set; }
        public virtual ICollection<ChangeRequestEmployeeDependent> ChangeRequestEmployeeDependent { get; set; }

        public virtual ICollection<SelfServiceEmployeeCredential> SelfServiceEmployeeCredential { get; set; }

        public virtual ICollection<SelfServiceEmployeeDocument> SelfServiceEmployeeDocument { get; set; }


        public virtual ICollection<Employment> Employment { get; set; }
        public virtual ICollection<EmploymentHistory> EmploymentHistory { get; set; }

        public virtual ICollection<PayInformationHistory> PayInformationHistory { get; set; }


        public virtual ICollection<EmploymentHistory> EmploymentHistory1 { get; set; }

        
        public virtual ICollection<EmployeeVeteranStatus> EmployeeVeteranStatus { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(ShortFullName))
                {
                    return ShortFullName;
                }
                else
                {                    
                   string name = (FirstName ?? "") + " " + (FirstLastName ?? "") + " " + (SecondLastName ?? "");

                    return (name.Length>50?  name.Substring(0,49): name);
                }
            }
        }

        [NotMapped]
        public string FullNameInitials
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FullName))
                {
                    return string.Empty;
                }
                var splitted = FullName?.Split(' ');
                try
                {
                    var initials = $"{splitted[0][0]}{(splitted.Length > 1 ? splitted[splitted.Length - 1][0] : (char?)null)}";
                    return initials;
                }
                catch (Exception ex)
                {
                    return "";
                }
                
            }
        }

        [NotMapped]
        public string UserInformationName
        {
            get
            {
                if (!string.IsNullOrEmpty(ShortFullName))
                {
                    return ShortFullName;
                }
                else
                {
                    return (FirstName ?? "") + " " + (FirstLastName ?? "") + " " + (SecondLastName ?? "");
                }
            }
        }

        
        public virtual ICollection<UserInformationRole> UserInformationRole { get; set; }


        //
        //public virtual ICollection<UserInformationContact> UserInformationContact { get; set; }

        //
        //public virtual ICollection<UserInformationAddress> UserInformationAddress { get; set; }

        
        public virtual ICollection<EmergencyContact> EmergencyContact { get; set; }
        public virtual ICollection<EmployeeDependent> EmployeeDependent { get; set; }
        public virtual ICollection<EmployeeEducation> EmployeeEducation { get; set; }
        //
        //public virtual ICollection<EmployeeRequiredCredential> EmployeeRequiredCredential { get; set; }

        public virtual ICollection<EmployeeCredential> EmployeeCredential { get; set; }
        
        public virtual ICollection<EmployeeDocument> EmployeeDocument { get; set; }
        public virtual ICollection<EmployeeTraining> EmployeeTraining { get; set; }
        [NotMapped]
        public bool SendRegistrationEmail { get; set; }
        [NotMapped]
        public bool IsRegistered 
        {
            get 
            {
                if (ActiveUserContactInformation != null && !String.IsNullOrEmpty(ActiveUserContactInformation.LoginEmail))
                    return true;
                return false;
            }
        }
    }
    
    //public partial class AspNetUsers
    //{
    //    public AspNetUsers()
    //    {
    //        UserInformation = new HashSet<UserInformation>();
    //    }

    //    public string Id { get; set; }

    //    [StringLength(256)]
    //    public string Email { get; set; }

    //    public bool EmailConfirmed { get; set; }

    //    public string PasswordHash { get; set; }

    //    public string SecurityStamp { get; set; }

    //    public string PhoneNumber { get; set; }

    //    public bool PhoneNumberConfirmed { get; set; }

    //    public bool TwoFactorEnabled { get; set; }

    //    public DateTime? LockoutEndDateUtc { get; set; }

    //    public bool LockoutEnabled { get; set; }

    //    public int AccessFailedCount { get; set; }

    //    [Required]
    //    [StringLength(256)]
    //    public string UserName { get; set; }

    //    public virtual ICollection<UserInformation> UserInformation { get; set; }
    //}

    public class UserInformationActivation : BaseEntity
    {
        [Key]
        [Column("UserInformationActivationId")]
        public override int Id { get; set; }

        [Display(Name = "User Id")]
        public int UserInformationId { get; set; }

        public Guid ActivationCode { get; set; }

        //[ForeignKey("Id")]
        public virtual UserInformation UserInformation { get; set; }
        
    }

    public class PassswordResetCode : BaseEntity
    {
        [Key]
        [Column("PassswordResetCodeId")]
        public override int Id { get; set; }

        [Display(Name = "User Id")]
        public int UserInformationId { get; set; }

        public string ResetCode { get; set; }

        public Guid ActivationCode { get; set; }

        //[ForeignKey("Id")]
        public virtual UserInformation UserInformation { get; set; }
        


    }
}
