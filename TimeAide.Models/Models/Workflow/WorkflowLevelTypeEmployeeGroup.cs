using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{
    [Table("WorkflowLevelGroup")]
    public partial class WorkflowLevelGroup : BaseEntity
    {
        [Key]
        [Column("WorkflowLevelGroupId")]
        public override int Id { get; set; }
        public virtual int EmployeeGroupId { get; set; }
        public virtual EmployeeGroup EmployeeGroup { get; set; }
        public int WorkflowLevelId { get; set; }
        public int? CompanyId { get; set; }
        public virtual WorkflowLevel WorkflowLevel { get; set; }
    }
}
