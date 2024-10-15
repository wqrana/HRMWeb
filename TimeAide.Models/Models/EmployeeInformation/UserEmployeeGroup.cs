namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserEmployeeGroup")]
    public partial class UserEmployeeGroup : BaseCompanyObjects
    {
        [Key]
        [Column("UserEmployeeGroupId")]
        public override int Id { get; set; }
        public int EmployeeGroupId { get; set; }
        public virtual EmployeeGroup EmployeeGroup { get; set; }
        public int UserInformationId { get; set; }
        public virtual UserInformation UserInformation { get; set; }
    }
}
