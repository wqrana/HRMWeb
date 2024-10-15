using TimeAide.Web.DAL.RepositoryInterfaces;
using TimeAide.Web.DAL.Repositories;
using TimeAide.Web.DAL;
using TimeAide.Web.Models;
using TimeAide.Repositories.Repositories;

namespace TimeAide.Web.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TimeAideContext _context;

        public UnitOfWork(TimeAideContext context)
        {
            _context = context;
            UserInformations = new UserInformationRepository(_context);
            CFSECodes = new CFSECodeRepository(_context);
        }
                
        public IUserInformationRepository UserInformations { get; private set; }
        public ICFSECodeRepository CFSECodes { get; private set; }
        
        public int Save()
        {
            return _context.SaveChanges();
        }

        public TimeAideContext TimeAideContext
        {
            get { return _context as TimeAideContext; }
        }

        public System.Data.Entity.Database Database 
        {
            get { return _context.Database; }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}