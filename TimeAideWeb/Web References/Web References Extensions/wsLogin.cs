using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeAide.Web.WebPunchLoginService
{
    public partial class wsLogin
    {
        public wsLogin(string url)
        {
            this.Url = url;
            if ((this.IsLocalFileSystemWebService(this.Url) == true))
            {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else
            {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
    }
}