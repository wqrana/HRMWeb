namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Module")]
    public partial class Module : BaseGlobalEntity
    {
        
        public Module()
        {
            Forms = new HashSet<Form>();
            InterfaceControlForms = new HashSet<InterfaceControlForm>();
            Module1 = new HashSet<Module>();
        }
        [Column("ModuleId")]
        public override int Id { get; set; }

        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(100)]
        [Required]
        public string ModuleName { get; set; }

        [StringLength(100)]
        [Required]
        public string ModuleLabel { get; set; }
        
        public int? CompanyId { get; set; }
        public int? ParentModuleId { get; set; }

        public virtual Module ParentModule { get; set; }

        public virtual Company Company { get; set; }

        
        public virtual ICollection<Form> Forms { get; set; }

        
        public virtual ICollection<InterfaceControlForm> InterfaceControlForms { get; set; }

        public virtual Client Client { get; set; }

        
        public virtual ICollection<Module> Module1 { get; set; }

    }
}
