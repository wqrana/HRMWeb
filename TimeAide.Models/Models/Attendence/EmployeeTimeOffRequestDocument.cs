using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("EmployeeTimeOffRequestDocument")]
    public partial class EmployeeTimeOffRequestDocument : BaseEntity
    {
        public EmployeeTimeOffRequestDocument()
        {
          
        }

        [Column("EmployeeTimeOffRequestDocumentId")]
        public override int Id { get; set; }       
        public int EmployeeTimeOffRequestId { get; set; }
        [Display(Name = "Document Name")]
        [StringLength(50)]
        public string DocumentName { get; set; }
        [Display(Name = "Document Type")]
        [StringLength(25)]
        public string DocumentType { get; set; }
        [Display(Name = "Submission Type")]
        [StringLength(25)]
        public string SubmissionType { get; set; }
        public int? TimeoffDays { get; set; }
        [StringLength(10)]
        public string Status { get; set; }

        public byte[] DocumentFile1 { get; set; }
        public string DocumentFile1Name { get; set; }
        public string DocumentFile1Ext { get; set; }

        public byte[] DocumentFile2 { get; set; }
        public string DocumentFile2Name { get; set; }
        public string DocumentFile2Ext { get; set; }

        public byte[] DocumentFile3 { get; set; }
        public string DocumentFile3Name { get; set; }
        public string DocumentFile3Ext { get; set; }
        public virtual EmployeeTimeOffRequest EmployeeTimeOffRequest { get; set; }
    }
}
