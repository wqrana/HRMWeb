using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeAide.Web.WebPunchEmployeeLoginService;

namespace TimeAide.Web.WebPunchComputeService2
{
    public partial class wsComputeTimeSheet
    {

        public wsComputeTimeSheet(string url)
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