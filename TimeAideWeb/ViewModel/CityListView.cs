using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeAide.Web.ViewModel
{
    public class CityListView
    {
        public List<TimeAide.Web.Models.City> Cities
        {
            get;
            set;
        }

        public int? IndexCountryId
        {
            get;
            set;
        }

        public int? IndexStateId
        {
            get;
            set;
        }
    }

    public class StateListView
    {
        public List<TimeAide.Web.Models.State> States
        {
            get;
            set;
        }

        public int? IndexCountryId
        {
            get;
            set;
        }
    }
}