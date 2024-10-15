namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;
    using TimeAide.Web.Models;


    [Table("CompanyConfigurableLink")]
    public partial class CompanyConfigurableLink : BaseCompanyObjects
    {
       
        [Column("CompanyConfigurableLinkId")]
        public override int Id { get; set; }
        public string LinkName { get; set; }
        public string LinkURL { get; set; }

    }
}