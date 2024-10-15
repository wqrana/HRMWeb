namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("GLAccount")]
    public partial class GLAccount : BaseCompanyObjects
    {
        
        public GLAccount()
        {
        }

        [Display(Name = "GLAccount Id")]
        [Column("GLAccountId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "GL Account")]
        public string GLAccountName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string GLAccountDescription { get; set; }
        public int GLAccountTypeId { get; set; }
        public virtual GLAccountType GLAccountType { get; set; }
    }
}
