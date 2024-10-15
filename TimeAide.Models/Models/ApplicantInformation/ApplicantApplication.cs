namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    [Table("ApplicantApplication")]
    public partial class ApplicantApplication : BaseApplicantObjects
    {
       [Display(Name = "Applicant Application Id")]
        [Column("ApplicantApplicationId")]
        public override int Id { get; set; }
        public int PositionId { get; set; }
        public int? ApplicantReferenceTypeId { get; set; }
        public int? ApplicantReferenceSourceId { get; set; }
        public int? JobLocationId { get; set; } 
        public DateTime DateApplied { get; set; }
        public DateTime? DateAvailable { get; set; }
        public decimal? Rate { get; set; }
        public int? RateFrequencyId { get; set; }

        public bool? IsMondayShift { get; set; }
        public DateTime? MondayStartShift { get; set; }
        public DateTime? MondayEndShift { get; set; }

        public bool? IsTuesdayShift { get; set; }
        public DateTime? TuesdayStartShift { get; set; }
        public DateTime? TuesdayEndShift { get; set; }

        public bool? IsWednesdayShift { get; set; }
        public DateTime? WednesdayStartShift { get; set; }
        public DateTime? WednesdayEndShift { get; set; }

        public bool? IsThursdayShift { get; set; }
        public DateTime? ThursdayStartShift { get; set; }
        public DateTime? ThursdayEndShift { get; set; }

        public bool? IsFridayShift { get; set; }
        public DateTime? FridayStartShift { get; set; }
        public DateTime? FridayEndShift { get; set; }

        public bool? IsSaturdayShift { get; set; }
        public DateTime? SaturdayStartShift { get; set; }
        public DateTime? SaturdayEndShift { get; set; }

        public bool? IsSundayShift { get; set; }
        public DateTime? SundayStartShift { get; set; }
        public DateTime? SundayEndShift { get; set; }

        public bool? IsOvertime { get; set; }
        public bool? IsWorkedBefore { get; set; }
        public DateTime? WorkedBeforeDate { get; set; }
        public bool? IsRelativeInCompany { get; set; }
        public string RelativeName { get; set; }
        [NotMapped]
        public string SelectedEmploymentTypeId { get; set; }
        [NotMapped]
        public string SelectedJobLocationId { get; set; }
        [NotMapped]
        public string SelectedLocationNames
        {
            get
            {
                if (ApplicantApplicationLocations != null)
                {
                    return String.Join(",", ApplicantApplicationLocations
                         .Select(s => s.Location.LocationName)
                         .ToArray());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public virtual Position Position { get; set; }

        public virtual ApplicantReferenceType ApplicantReferenceType { get; set; }

        public virtual ApplicantReferenceSource ApplicantReferenceSource { get; set; }

        public virtual RateFrequency RateFrequency { get; set; }
        public virtual  ICollection<ApplicantApplicationLocation> ApplicantApplicationLocations { get; set; }

        [NotMapped]
        public  List<ApplicantEmploymentType> ApplicantEmploymentTypes { get; set; }


    }

    [Table("ApplicantApplicationLocation")]
    public partial class ApplicantApplicationLocation : BaseEntity
    {

        [Column("ApplicantApplicationLocationId")]
        public override int Id { get; set; }

        public int ApplicantApplicationId { get; set; }

        public int LocationId { get; set; }

        public virtual ApplicantApplication ApplicantApplication { get; set; }
        public virtual Location Location { get; set; }


    }
}
