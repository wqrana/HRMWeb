namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;



    [Table("Client")]
    public partial class Client : BaseEntity
    {
        public Client()
        {
            UserInformation = new HashSet<UserInformation>();
            Company = new HashSet<Company>();

            Form = new HashSet<Form>();
            Form1 = new HashSet<Form>();
            InterfaceControl = new HashSet<InterfaceControl>();
            InterfaceControlForm = new HashSet<InterfaceControlForm>();
            Module = new HashSet<Module>();
            Privilege = new HashSet<Privilege>();
            Role = new HashSet<Role>();
            RoleFormPrivilege = new HashSet<RoleFormPrivilege>();
            RoleTypeFormPrivilege = new HashSet<RoleTypeFormPrivilege>();
            RoleInterfaceControlPrivilege = new HashSet<RoleInterfaceControlPrivilege>();
            RoleType = new HashSet<RoleType>();
            UserMenu = new HashSet<UserMenu>();
            //Addresses = new HashSet<Address>();
            //Contacts = new HashSet<ContactInformation>();
            EmployeeGroup = new HashSet<EmployeeGroup>();
            
        }

        [Key]
        [Display(Name = "Client Id")]
        [Column("ClientId")]
        public override int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [StringLength(50)]
        public string Address1 { get; set; }

        [StringLength(50)]
        public string Address2 { get; set; }

        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }
        public int? StateId { get; set; }
        public virtual State State { get; set; }
        public int? CityId { get; set; }
        public virtual City City { get; set; }

        [StringLength(50)]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }       

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [StringLength(50)]
        public string WebSite { get; set; }

        [StringLength(50)]
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }

        [StringLength(50)]
        [Display(Name = "Payroll Name")]
        public string PayrollName { get; set; }

        [StringLength(50)]
        [Display(Name = "Payroll Address1")]
        public string PayrollAddress1 { get; set; }

        [StringLength(50)]
        [Display(Name = "Payroll Address2")]
        public string PayrollAddress2 { get; set; }

        public int? PayrollCountryId { get; set; }
        public virtual Country PayrollCountry { get; set; }
        public int? PayrollStateId { get; set; }
        public virtual State PayrollState { get; set; }
        public int? PayrollCityId { get; set; }
        public virtual City PayrollCity { get; set; }

        [StringLength(50)]
        [Display(Name = "Payroll Zip Code")]
        public string PayrollZipCode { get; set; }

        [StringLength(50)]
        public string EIN { get; set; }

        [StringLength(50)]
        [Display(Name = "Payroll Fax")]
        public string PayrollFax { get; set; }

        [StringLength(50)]
        [Display(Name = "Payroll Contact Name")]
        public string PayrollContactName { get; set; }

        [StringLength(50)]
        [Display(Name = "Payroll Contact Title")]
        public string PayrollContactTitle { get; set; }

        [StringLength(50)]
        [Display(Name = "Payroll Contact Phone")]
        public string PayrollContactPhone { get; set; }

        [StringLength(50)]
        [Display(Name = "Payroll Email")]
        [EmailAddress(ErrorMessage = "Invalid Payroll Email Address")]
        public string PayrollEmail { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Company Start Date")]
        public DateTime? CompanyStartDate { get; set; }

        
        [StringLength(50)]
        [Display(Name = "SIC Code")]
        public string SICCode { get; set; }

        
        [StringLength(50)]
        [Display(Name = "NAICS Code")]
        public string NAICSCode { get; set; }

       
        [StringLength(50)]
        [Display(Name = "Seguro Choferil Account")]
        public string SeguroChoferilAccount { get; set; }

        
        [StringLength(50)]
        [Display(Name = "Departamento Del Trabajo Account")]
        public string DepartamentoDelTrabajoAccount { get; set; }

        [Display(Name = "Departamento Del Trabajo Rate")]
        public decimal? DepartamentoDelTrabajoRate { get; set; }
        [Display(Name = "TimeAide-Window")]
        public bool IsTimeAideWindow { get; set; }
        [Display(Name = "DB Server")]
        public string DBServerName { get; set; }
        [Display(Name = "DB Name")]
        public string DBName { get; set; }
        [Display(Name = "User ID")]
        public string DBUser { get; set; }
        [Display(Name = "Password")]
        public string DBPassword { get; set; }
        new public int? CreatedBy { get; set; }
        [NotMapped]
        new public int ClientId { get; set; }
        public virtual ICollection<UserInformation> UserInformation { get; set; }
        public virtual ICollection<Company> Company { get; set; }
        public virtual ICollection<Form> Form { get; set; }
        public virtual ICollection<Form> Form1 { get; set; }
        public virtual ICollection<InterfaceControl> InterfaceControl { get; set; }
        public virtual ICollection<InterfaceControlForm> InterfaceControlForm { get; set; }
        public virtual ICollection<Module> Module { get; set; }
        public virtual ICollection<Privilege> Privilege { get; set; }
        public virtual ICollection<Role> Role { get; set; }
        public virtual ICollection<RoleFormPrivilege> RoleFormPrivilege { get; set; }
        public virtual ICollection<RoleTypeFormPrivilege> RoleTypeFormPrivilege { get; set; }
        public virtual ICollection<RoleInterfaceControlPrivilege> RoleInterfaceControlPrivilege { get; set; }
        public virtual ICollection<RoleType> RoleType { get; set; }
        public virtual ICollection<UserMenu> UserMenu { get; set; }
        public virtual ICollection<EmployeeGroup> EmployeeGroup { get; set; }
    }
}
