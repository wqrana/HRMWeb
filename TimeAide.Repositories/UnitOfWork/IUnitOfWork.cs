using System;
using TimeAide.Web.DAL.RepositoryInterfaces;

namespace TimeAide.Web.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserInformationRepository UserInformations { get; }
        int Save();
    }
}