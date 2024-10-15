namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("GLAccountType")]
    public partial class GLAccountType : BaseGlobalEntity
    {
        public GLAccountType()
        {
        }

        [Display(Name = "Id")]
        [Column("GLAccountTypeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Name")]
        public string GLAccountTypeName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string GLAccountTypeDescription { get; set; }
    }
}
