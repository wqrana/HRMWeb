using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeAide.Web.CustomFilters
{
  

    public class FutureDatePicker : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            return value != null && (DateTime)value < DateTime.Now && (DateTime)value < DateTime.Now.AddMonths(1).AddDays(1).AddYears(1);
        }
    }
}