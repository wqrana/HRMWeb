using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("EmployeeAppraisalDocument")]
    public partial class EmployeeAppraisalDocument : BaseEntity
    {
        [Column("EmployeeAppraisalDocumentId")]
        public override int Id { get; set; }
        public int EmployeeAppraisalId { get; set; }
        public string AppraisalDocumentName { get; set; }
        public string DocumentFileName { get; set; }
        public string DocumentFilePath { get; set; }
        public virtual EmployeeAppraisal EmployeeAppraisal { get; set; }
     
    }
}
