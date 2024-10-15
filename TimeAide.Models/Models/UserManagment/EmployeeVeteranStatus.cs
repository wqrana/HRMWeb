namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class EmployeeVeteranStatus : BaseEntity
    {
        [Column("EmployeeVeteranStatusId")]
        public override int Id { get; set; }

        public int UserInformationId { get; set; }

        public int VeteranStatusId { get; set; }

        public virtual UserInformation UserInformation { get; set; }

        public virtual VeteranStatus VeteranStatus { get; set; }
    }
}
