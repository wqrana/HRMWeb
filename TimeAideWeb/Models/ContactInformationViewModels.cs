using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeAide.Web.Models
{
    public class ContactInformationViewModel
    {
        public int ContactId { get; set; }

        public int UserInformationId { get; set; }

        public int? ContactTypeId { get; set; }

        public string ContactTypeName { get; set; }

        public int? ContactMediumId { get; set; }
        public string ContactMediumName { get; set; }
        public string Contact { get; set; }

    }
}