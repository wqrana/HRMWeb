using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeAide.Web.ViewModel
{
    public class CompanyPortalViewModel
    {
        public int? UserId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyProfilePicture { get; set; }
        public bool IsDefaultCompanyProfilePicture { get; set; }
        public int? DefaultProfilePictureCompanyId { get; set; }
        public string CompanyMediaType { get; set; }
        public string CompanyDefaultMediaPath { get; set; }
        public string CompanyDefaultMediaPosterPath { get; set; }
        public string CompanyDefaultMediaTitle { get; set; }
        public bool IsPortalAdmin { get; set; }
        [AllowHtml]
        public string PortalWelcomeStatement { get; set; }
        public bool IsDefaultCompanyPortalStatement { get; set; }
        public int? DefaultPortalStatementCompanyId { get; set; }
    }
    public class CompanyStats
    {
        public int FemaleEmployees
        {
            get;
            set;
        }
        public int MaleEmployees
        {
            get;
            set;
        }
        public Decimal GenderRatio
        {
            get
            {
                if (MaleEmployees > 0 && FemaleEmployees > 0)
                    return (FemaleEmployees / (Decimal)TotalEmpoyees) * 100;
                return 0;
            }
        }
        public int TotalEmpoyees
        {
            get;
            set;
        }
        public int NewHiring
        {
            get;
            set;
        }
        public int NewTerminations
        {
            get;
            set;
        }
        public int TurnOverRatio
        {
            get
            {
                if (TotalEmpoyees > 0 && NewTerminations > 0)
                    return (NewTerminations / TotalEmpoyees) * 100;
                return 0;
            }
        }
    }

}