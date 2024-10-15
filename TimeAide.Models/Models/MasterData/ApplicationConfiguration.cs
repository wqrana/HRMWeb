using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("ApplicationConfiguration")]
    public partial class ApplicationConfiguration : BaseCompanyObjects
    {
        [Column("ApplicationConfigurationId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Configuration Name")]
        [StringLength(150)]
        public string ApplicationConfigurationName { get; set; }
        [Display(Name = "Configuration Value")]
        [StringLength(150)]
        public string ApplicationConfigurationValue { get; set; }
        public string ModuleFormName { get; set; }
        public string ValueType { get; set; }
        [NotMapped]
        public bool ValueAsBoolean
        {
            get
            {
                return (ApplicationConfigurationValue == "1");
            }
            set
            {
                if (value)
                {
                    ApplicationConfigurationValue = "1";
                }
                else
                {
                    ApplicationConfigurationValue = "0";
                }
            }
        }
        [NotMapped]
        public int? ValueAsInt
        {
            get
            {
                int configurationValue;
                if (int.TryParse(ApplicationConfigurationValue, out configurationValue))
                {
                    return configurationValue;
                }
                return null;
            }
        }
    }
}
