using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.DAL.RepositoryInterfaces;
using TimeAide.Web.Models;

namespace TimeAide.Repositories.Repositories
{
    public interface ICFSECodeRepository: IRepository<CFSECode>
    {
        CFSECode GetCFSECodes(int cFSECodeId, int clientId);
    }
}
