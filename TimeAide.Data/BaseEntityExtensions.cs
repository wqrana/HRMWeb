using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{
    public static class BaseEntityExtensions
    {
        public static void SetUpdated<T>(this BaseEntity entity) where T : BaseEntity
        {
            TimeAideContext db = new TimeAideContext();
            var dbEntity = db.Find<T>(entity.Id, entity.ClientId??0);

            //MethodInfo method = typeof(TimeAideContext).GetMethod("Find");
            //MethodInfo generic = method.MakeGenericMethod(entity.GetType());
            //generic.Invoke(db, new object[] { entity.Id,SessionHelper.ClientId });

            
            entity.ModifiedBy = SessionHelper.LoginId;
            entity.ModifiedDate = DateTime.Now;

            entity.CreatedDate = dbEntity.CreatedDate;
            entity.DataEntryStatus = dbEntity.DataEntryStatus;
            entity.CreatedBy = dbEntity.CreatedBy;
            entity.Old_Id = dbEntity.Old_Id;

        }
    }
}
