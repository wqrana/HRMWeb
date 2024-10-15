
namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;
    using TimeAide.Web.Models;


    [Table("CompanyMedia")]
    public partial class CompanyMedia : BaseCompanyObjects
    {
        public CompanyMedia()
        {

        }

        [Column("CompanyMediaId")]
        public override int Id { get; set; }
        public string Name { get; set; }
        public string MediaType { get; set; }
        public string MediaFilePath { get; set; }
        public string FileName { get; set; }
        public string PosterFilePath {get;set;}
        public string PosterFileName { get; set; }
        public bool IsDefaultMedia { get; set; }
        [NotMapped]
        public new bool IsAllCompanies { get; set; }
        
        [NotMapped]
        public string MediaLink { get; set; }
    }
}