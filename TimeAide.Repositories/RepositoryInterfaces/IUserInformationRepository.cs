using System.Collections.Generic;
using TimeAide.Web.Models;


namespace TimeAide.Web.DAL.RepositoryInterfaces
{
    public interface IUserInformationRepository : IRepository<UserInformation>
    {
        IEnumerable<UserInformation> GetUserInformations(int clientId);
        
    }
}