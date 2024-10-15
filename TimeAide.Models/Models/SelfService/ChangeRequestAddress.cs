using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{
    public class ChangeRequestAddress: ChangeRequestBase
    {
        public int UserContactInformationId { get; set; }
        public virtual UserContactInformation UserContactInformation { get; set; }

        [Key]
        [Column("ChangeRequestAddressId")]
        public override int Id { get; set; }
        [StringLength(50)]
        public string Address1 { get; set; }
        [StringLength(50)]
        public string Address2 { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        [StringLength(10)]
        public string ZipCode { get; set; }
        [NotMapped]
        public string ZipCodeFormatted
        {
            get
            {
                if (!string.IsNullOrEmpty(ZipCode) && ZipCode.Length == 9)
                    return ZipCode.Insert(5, "-");
                else
                    return ZipCode;
            }
        }
        [StringLength(50)]
        public string NewAddress1 { get; set; }
        [StringLength(50)]
        public string NewAddress2 { get; set; }
        public int? NewCityId { get; set; }
        public int? NewStateId { get; set; }
        public int? NewCountryId { get; set; }
        [StringLength(10)]
        public string NewZipCode { get; set; }

        [NotMapped]
        public string NewZipCodeFormatted
        {
            get
            {
                if (!string.IsNullOrEmpty(NewZipCode) && NewZipCode.Length == 9)
                    return NewZipCode.Insert(5, "-");
                else
                    return NewZipCode;
            }
        }

        [StringLength(20)]
        public virtual City City { get; set; }
        public virtual State State { get; set; }
        public virtual Country Country { get; set; }
        public virtual City NewCity { get; set; }
        public virtual State NewState { get; set; }
        public virtual Country NewCountry { get; set; }
        public String AddressType
        {
            get;
            set;
        }

        [NotMapped]
        public bool IsModified { get; set; }
    }
}
