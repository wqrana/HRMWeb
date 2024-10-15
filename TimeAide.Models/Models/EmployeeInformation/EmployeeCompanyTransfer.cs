 using System;
 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("EmployeeCompanyTransfer")]
    public partial class EmployeeCompanyTransfer : BaseUserObjects
    {
      
        [Column("EmployeeCompanyTransferId")]
        public override int Id { get; set; }
        public int FromUserInformationId { get; set; }
        public int FromCompanyId { get; set; }
        public int ToUserInformationId { get; set; }
        public int ToCompanyId { get; set; }
        public DateTime TransferDate { get; set; }      
       public virtual UserInformation FromUserInformation { get; set; }
        public virtual Company FromCompany { get; set; }
        public virtual UserInformation ToUserInformation { get; set; }
        public virtual Company ToCompany { get; set; }
        

    }
}
