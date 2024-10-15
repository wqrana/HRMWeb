
namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;
    using TimeAide.Web.Models;


    [Table("CompanyDocument")]
    public partial class CompanyDocument : BaseCompanyObjects
    {
       public CompanyDocument()
        {
            DocumentUserId = CreatedBy;
        }
        
        [Column("CompanyDocumentId")]
        public override int Id { get; set; }
        public string DocumentName { get; set; }
        
        public string DocumentFilePath { get; set; }

        public int DocumentUserId { get; set; }
       
        public virtual UserInformation DocumentUser { get; set; }
    }
}