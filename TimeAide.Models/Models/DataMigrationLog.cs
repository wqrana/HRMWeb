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
    [Table("DataMigrationLog")]
    public class DataMigrationLog : BaseEntity
    {
        public DataMigrationLog()
        {
            DataMigrationLogDetail = new HashSet<DataMigrationLogDetail>();
        }
        [Key]
        [Display(Name = "Data Migration Log Id")]
        [Column("DataMigrationLogId")]
        public override int Id { get; set; }

        [StringLength(500)]
        [Display(Name = "Log Name")]
        public string LogName { get; set; }

        [StringLength(2000)]
        [Display(Name = "Log Description")]
        public string LogDescription { get; set; }

        [StringLength(2000)]
        [Display(Name = "Log Remarks")]
        public string LogRemarks { get; set; }

        public virtual ICollection<DataMigrationLogDetail> DataMigrationLogDetail { get; set; }
    }
}
