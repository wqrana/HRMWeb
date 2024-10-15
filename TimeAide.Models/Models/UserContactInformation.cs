namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Reflection.Emit;
    using System.Web.Razor.Parser.SyntaxTree;
    using TimeAide.Common.Helpers;

    [Table("UserContactInformation")]
    public partial class UserContactInformation : BaseEntity
    {
        public UserContactInformation()
        {
            ChangeRequestAddress = new HashSet<ChangeRequestAddress>();
        }
        [Key]
        [Column("UserContactInformationId")]
        public override int Id { get; set; }

        public int UserInformationId { get; set; }

        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Login Email Address")]
        public string LoginEmail { get; set; }

        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Notification Email Address")]
        public string NotificationEmail { get; set; }
       
        [StringLength(50)]
        public string HomeAddress1 { get; set; }

        [StringLength(50)]
        public string HomeAddress2 { get; set; }

        public int? HomeCityId { get; set; }
        public int? HomeStateId { get; set; }
        public int? HomeCountryId { get; set; }

        [StringLength(10)]
        public string HomeZipCode { get; set; }
        [NotMapped]
        public string HomeZipCodeFormatted
        {
            get
            {
                if (!string.IsNullOrEmpty(HomeZipCode) && HomeZipCode.Length == 9)
                    return HomeZipCode.Insert(5, "-");
                else
                    return HomeZipCode;
            }
        }

        [StringLength(50)]
        public string MailingAddress1 { get; set; }

        [StringLength(50)]
        public string MailingAddress2 { get; set; }

        public int? MailingCityId { get; set; }
        public int? MailingStateId { get; set; }
        public int? MailingCountryId { get; set; }

        [StringLength(10)]
        public string MailingZipCode { get; set; }
        [NotMapped]
        public string MailingZipCodeFormatted
        {
            get
            {
                if (!string.IsNullOrEmpty(MailingZipCode) && MailingZipCode.Length == 9)
                    return MailingZipCode.Insert(5, "-");
                else
                    return MailingZipCode;
            }
        }

        [StringLength(20)]
        public string HomeNumber { get; set; }


        [DisplayName("Phone")]
        [StringLength(20)]
        public string CelNumber { get; set; }

        [StringLength(20)]
        public string FaxNumber { get; set; }

        [StringLength(20)]
        public string OtherNumber { get; set; }

        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Work Email Address")]
        public string WorkEmail { get; set; }

        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Personal Email Address")]
        public string PersonalEmail { get; set; }
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Other Email Address")]
        public string OtherEmail { get; set; }
        [StringLength(20)]
        public string WorkNumber { get; set; }
        [StringLength(10)]
        public string WorkExtension { get; set; }
        public virtual City HomeCity { get; set; }
        public virtual City MailingCity { get; set; }
        public virtual State HomeState { get; set; }
        public virtual State MailingState { get; set; }
        public virtual Country HomeCountry { get; set; }
        public virtual Country MailingCountry { get; set; }
        public virtual UserInformation UserInformation { get; set; }
        public bool IsSameHomeAddress { get; set; }
        public virtual ICollection<ChangeRequestAddress> ChangeRequestAddress { get; set; }
        public virtual ICollection<ChangeRequestEmailNumbers> ChangeRequestEmailNumbers { get; set; }
        [NotMapped]
        public int? SupervisoryLevelId { get; set; }
        [NotMapped]
        public bool HasHomeAddress
        {
            get
            {
                return (!string.IsNullOrEmpty(HomeAddress1) || !string.IsNullOrEmpty(HomeAddress2) || !string.IsNullOrEmpty(HomeZipCode) || HomeCityId > 0 || HomeStateId > 0 || HomeCountryId > 0);
            }
        }

        [NotMapped]
        public string HomeAddress
        {
            get
            {
                return (GetStringWithComma(HomeAddress1) + GetStringWithComma(HomeAddress2) + GetStringWithComma(HomeCityName) + GetStringWithComma(HomeStateName) + GetStringWithComma(HomeCountryName) + GetStringWithComma(HomeZipCode)).TrimEnd(',');
            }
        }

        [NotMapped]
        public string PhoneNumbers
        {
            get
            {
                string numbers = (GetStringWithComma(WorkNumberWithExtension) + GetStringWithComma(CelNumber) + GetStringWithComma(HomeNumber));
                return numbers.TrimEnd(',');
            }
        }

        public string GetStringWithComma(string inputString, string separator = null)
        {
            if (String.IsNullOrEmpty(separator))
                separator = ", ";
            if (string.IsNullOrEmpty(inputString))
                return "";
            return inputString + separator;
        }
        [NotMapped]
        public string WorkNumberWithExtension
        {
            get
            {
                if (string.IsNullOrEmpty(WorkNumber))
                    return "";
                if (string.IsNullOrEmpty(WorkExtension))
                    return GetFormattedNumber(WorkNumber);
                return GetFormattedNumber(WorkNumber) + " Ext: " + WorkExtension;
            }
        }
        [NotMapped]
        public bool HasMailingAddress
        {
            get
            {
                return (!string.IsNullOrEmpty(MailingAddress1) || !string.IsNullOrEmpty(MailingAddress2) || !string.IsNullOrEmpty(MailingZipCode) || MailingCityId > 0 || MailingStateId > 0 || MailingCountryId > 0);
            }
        }
        [NotMapped]
        public String AddressType
        {
            get;
            set;
        }
        [NotMapped]
        public string Address1
        {
            get
            {
                if (AddressType == "Home")
                    return HomeAddress1;
                else
                    return MailingAddress1;
            }
            set
            {
                if (AddressType == "Home")
                    HomeAddress1 = value;
                else
                    MailingAddress1 = value;
            }
        }
        [NotMapped]
        public string Address2
        {
            get
            {
                if (AddressType == "Home")
                    return HomeAddress2;
                else
                    return MailingAddress2;
            }
            set
            {
                if (AddressType == "Home")
                    HomeAddress2 = value;
                else
                    MailingAddress2 = value;
            }
        }
        [NotMapped]
        public int? CityId
        {
            get
            {
                if (AddressType == "Home")
                    return HomeCityId;
                else
                    return MailingCityId;
            }
            set
            {
                if (AddressType == "Home")
                    HomeCityId = value;
                else
                    MailingCityId = value;
            }
        }
        [NotMapped]
        public int? StateId
        {
            get
            {
                if (AddressType == "Home")
                    return HomeStateId;
                else
                    return MailingStateId;
            }
            set
            {
                if (AddressType == "Home")
                    HomeStateId = value;
                else
                    MailingStateId = value;
            }
        }
        [NotMapped]
        public int? CountryId
        {
            get
            {
                if (AddressType == "Home")
                    return HomeCountryId;
                else
                    return MailingCountryId;
            }
            set
            {
                if (AddressType == "Home")
                    HomeCountryId = value;
                else
                    MailingCountryId = value;
            }
        }
        [NotMapped]
        public string ZipCode
        {
            get
            {
                if (AddressType == "Home")
                    return HomeZipCode;
                else
                    return MailingZipCode;
            }
            set
            {
                if (AddressType == "Home")
                    HomeZipCode = value;
                else
                    MailingZipCode = value;
            }
        }
        [NotMapped]
        public string ZipCodeFormatted
        {
            get
            {
                if (AddressType == "Home")
                    return HomeZipCodeFormatted;
                else
                    return MailingZipCodeFormatted;
            }
        }
        [NotMapped]
        public virtual City City
        {
            get
            {
                if (AddressType == "Home")
                    return HomeCity;
                else
                    return MailingCity;
            }
        }
        [NotMapped]
        public virtual State State
        {
            get
            {
                if (AddressType == "Home")
                    return HomeState;
                else
                    return MailingState;
            }
        }
        [NotMapped]
        public virtual Country Country
        {
            get
            {
                if (AddressType == "Home")
                    return HomeCountry;
                else
                    return MailingCountry;
            }
        }
        [NotMapped]
        public String HomeCityName
        {
            get
            {
                if (HomeCity != null)
                    return HomeCity.CityName;
                else
                    return "";
            }
        }
        [NotMapped]
        public String HomeStateName
        {
            get
            {
                if (HomeState != null)
                    return HomeState.StateName;
                else
                    return "";
            }
        }
        [NotMapped]
        public String HomeCountryName
        {
            get
            {
                if (HomeCountry != null)
                    return HomeCountry.CountryName;
                else
                    return "";
            }
        }
        public string GetFormattedNumber(String contactNumber)
        {
            if (!string.IsNullOrEmpty(contactNumber) && contactNumber.Length == 10)
                return String.Format("{0:(000) 000-0000}", Convert.ToInt64(contactNumber));
            else
                return contactNumber;
        }
    }
}
