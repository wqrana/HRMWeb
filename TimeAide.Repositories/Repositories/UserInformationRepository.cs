using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeAide.Web.Models;
using TimeAide.Web.DAL.RepositoryInterfaces;

using System.Data.SqlClient;
using System.Text;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Text.RegularExpressions;

namespace TimeAide.Web.DAL.Repositories
{
    public class UserInformationRepository : Repository<UserInformation>, IUserInformationRepository
    {
        public UserInformationRepository(TimeAideContext context)
            : base(context)
        {
        }

        public IEnumerable<UserInformation> GetUserInformations(int clientId)
        {
            return TimeAideContext.GetAll<UserInformation>(clientId)
                //.OrderBy(o => o.OrderDate)
                //.Skip((pageIndex - 1) * pageSize)
                //.Take(pageSize)
                .ToList();
        }
              
        public TimeAideContext TimeAideContext
        {
            get { return Context as TimeAideContext; }
        }
    }
}