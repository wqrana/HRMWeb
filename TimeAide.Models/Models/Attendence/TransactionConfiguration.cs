namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    [Table("TransactionConfiguration")]
    public partial class TransactionConfiguration : BaseEntity
    {
        public TransactionConfiguration()
        {
            CompensationTransaction = new HashSet<CompensationTransaction>();
        }

        [Display(Name = "Transaction Configuration Id")]
        [Column("TransactionConfigurationId")]
        public override int Id { get; set; }

        public string ConfigurationName { get; set; }
        public int ConfigurationCode { get; set; }
        public string ConfigurationDescription { get; set; }
        public int ProcessCodeId { get; set; }
        public virtual ProcessCode ProcessCode { get; set; }
        public bool IsAbsent { get; set; }
        public int AttendanceCategoryId { get; set; }
        public int PrimaryTransactionId { get; set; }
        public virtual PrimaryTransaction PrimaryTransaction { get; set; }
        public int AccrualTypeId { get; set; }
        public virtual AccrualType AccrualType { get; set; }
        public string AccrualImportName { get; set; }
        public bool AttendanceRevision { get; set; }
        public bool AttendanceRevisionLetter { get; set; }
        public bool TardinessRevision { get; set; }
        public bool TardinessRevisionLetter { get; set; }
        //public bool CountForSickAccrual { get; set; }
        //public bool CountForVacationAccrual { get; set; }
        //public bool CountForCompensationAccruals { get; set; }

        
        public int PayRateMultiplierId { get; set; }
        public virtual PayRateMultiplier PayRateMultiplier { get; set; }

        public int VacationAccrualTypeId { get; set; }
        public virtual VacationAccrualType VacationAccrualType { get; set; }

        public int? CompensationAccrualTypeId { get; set; }
        public virtual CompensationAccrualType CompensationAccrualType { get; set; }

        public int? SickAccrualTypeId { get; set; }
        public virtual SickAccrualType SickAccrualType { get; set; }
        

        public string AdditionalPayAmount { get; set; }
        public bool IsMoneyTrans { get; set; }
        public bool IsMoneyAmountFixed { get; set; }
        public decimal MoneyAmount { get; set; }
        
        
        public string CompforCompensationAccrual { get; set; }
        public decimal PayRateOffset { get; set; }
        public string PayRateTransaction { get; set; }
        [NotMapped]
        public bool PayTransaction { get; set; }
        public virtual ICollection<CompensationTransaction> CompensationTransaction { get; set; }
    }
}
