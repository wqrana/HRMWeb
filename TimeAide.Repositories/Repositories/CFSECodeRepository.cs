using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.DAL.Repositories;
using TimeAide.Web.Models;

namespace TimeAide.Repositories.Repositories
{
    public class CFSECodeRepository: Repository<CFSECode>, ICFSECodeRepository
    {
        public CFSECodeRepository(TimeAideContext context) : base(context)
        {
        }
        public CFSECode GetCFSECodes(int CFSECodeId,int clientId)
        {
            return TimeAideContext.Find<CFSECode>(CFSECodeId, clientId);
                //.OrderByDescending(o => o.Id).ToList();
        }

        public TimeAideContext TimeAideContext
        {
            get { return Context as TimeAideContext; }
        }
    }
}
