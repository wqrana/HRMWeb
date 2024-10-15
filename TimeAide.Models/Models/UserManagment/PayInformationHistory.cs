namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PayInformationHistory")]
    public partial class PayInformationHistory : BaseUserObjects
    {

        public PayInformationHistory()
        {
            StartDate = DateTime.Now;
            PayInformationHistoryAuthorizer = new HashSet<PayInformationHistoryAuthorizer>();
        }
        [Display(Name = "Pay Information History Id")]
        [Column("PayInformationHistoryId")]
        public override int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? EEOCategoryId { get; set; }

        public int PayTypeId { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}", ApplyFormatInEditMode = true)]
        public decimal RateAmount { get; set; }
        public int RateFrequencyId { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}", ApplyFormatInEditMode = true)]
        public decimal? CommRateAmount { get; set; }
        public int? CommRateFrequencyId { get; set; }
        public int PayFrequencyId { get; set; }
        public decimal? PeriodHours { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}", ApplyFormatInEditMode = true)]
        public decimal PeriodGrossPay { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}", ApplyFormatInEditMode = true)]
        public decimal YearlyGrossPay { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}", ApplyFormatInEditMode = true)]
        public decimal? YearlyCommBasePay { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}", ApplyFormatInEditMode = true)]
        public decimal? YearlyBaseNCommPay { get; set; }
        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:0.00000}", ApplyFormatInEditMode = true)]
        public decimal HourlyRate
        {
            get
            {
                if (RateFrequency != null)
                {
                    var rateHours = RateFrequency.HourlyMultiplier;
                    if (rateHours.HasValue && rateHours.Value > 0)
                        return Math.Round(RateAmount / rateHours.Value,5);
                }
                return 0;
            }
        }
        [NotMapped]
        public decimal HourlyCommRate
        {
            get
            {
                if (CommRateFrequency != null)
                {
                    var rateHours = CommRateFrequency.HourlyMultiplier;
                    if (rateHours.HasValue && rateHours.Value > 0)
                        return Math.Round((CommRateAmount ?? 0) / (rateHours.Value),5);
                }
                return 0;
            }
        }


        [StringLength(200)]
        public string ChangeReason { get; set; }

        public virtual ICollection<PayInformationHistoryAuthorizer> PayInformationHistoryAuthorizer { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [StringLength(50)]
        public string docName { get; set; }

        [StringLength(10)]
        public string docExtension { get; set; }

        [Column(TypeName = "image")]
        public byte[] docFile { get; set; }

        public int? WCClassCodeId { get; set; }

        public int? PayScaleId { get; set; }

        public int EmploymentId { get; set; }
        public virtual Employment Employment { get; set; }

        public virtual EEOCategory EEOCategory { get; set; }

        public virtual PayFrequency PayFrequency { get; set; }

        public virtual PayType PayType { get; set; }

        public virtual PayScale PayScale { get; set; }

        public virtual RateFrequency RateFrequency { get; set; }

        public virtual RateFrequency CommRateFrequency { get; set; }

        public virtual WCClassCode WCClassCode { get; set; }

        public virtual UserInformation UserInformation { get; set; }

        [NotMapped]
        public string SelectedAuthorizById { get; set; }

        [NotMapped]
        public string SelectedAuthorizByName
        {
            get
            {
                if (PayInformationHistoryAuthorizer != null && PayInformationHistoryAuthorizer.Count > 0)
                    // return String.Join(",", PayInformationHistoryAuthorizer.Where(w => w.DataEntryStatus == 1).Select(c => c.PayInformationHistory.UserInformation.ShortFullName.ToString()));
                    return String.Join(",", PayInformationHistoryAuthorizer.Where(w => w.DataEntryStatus == 1).Select(c => c.AuthorizeBy.ShortFullName.ToString()));
                return "";
            }

        }
        [NotMapped]
        public string SelectedAuthorizByIds
        {
            get
            {
                if (PayInformationHistoryAuthorizer != null && PayInformationHistoryAuthorizer.Count > 0)
                    // return String.Join(",", PayInformationHistoryAuthorizer.Where(w => w.DataEntryStatus == 1).Select(c => c.PayInformationHistory.UserInformation.ShortFullName.ToString()));
                    return String.Join(",", PayInformationHistoryAuthorizer.Where(w => w.DataEntryStatus == 1).Select(c => c.AuthorizeBy.Id.ToString()));
                return "";
            }

        }
        [NotMapped]
        public bool CanAddNew
        {
            get
            {
                if (Id == 0)
                    return true;
                if (UserInformation == null)
                    return false;
                var model = UserInformation.PayInformationHistory.Where(W=>W.DataEntryStatus==1).OrderByDescending(e => e.StartDate).FirstOrDefault();
                //if (model == null || model.Id == 0 || model.PayInformationHistoryAuthorizer.Count() > 0)
                if (model == null || model.Id == 0 || model.EndDate != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [NotMapped]
        public bool CanEdit
        {
            get
            {
                //if (Id != 0 && !(this.PayInformationHistoryAuthorizer.Count() > 0))
                //if (Id != 0 && EndDate==null)
                if (Id != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        [NotMapped]
        public bool CanEditClosing
        {
            get
            {
                //if (Id!=0 && !(this.EmploymentHistoryAuthorizer.Count() > 0))
                if (Id != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
