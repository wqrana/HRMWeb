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
    [Table("DataMigrationLogDetail")]
    public class DataMigrationLogDetail : BaseEntity
    {
        [Key]
        [Display(Name = "Data Migration Log Detail Id")]
        [Column("DataMigrationLogDetailId")]
        public override int Id { get; set; }

        [StringLength(500)]
        [Display(Name = "Log Command Name")]
        public string LogCommandName { get; set; }
        [StringLength(500)]
        [Display(Name = "Log Name")]
        public string LogDetailName { get; set; }
        [StringLength(2000)]
        [Display(Name = "Log Description")]
        public string LogDescription { get; set; }
        [StringLength(2000)]
        [Display(Name = "Log Remarks")]
        public string LogRemarks { get; set; }
        public int RowCount { get; set; }
        public int DataMigrationLogId { get; set; }
        public virtual DataMigrationLog DataMigrationLog { get; set; }
    }
}
