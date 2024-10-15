using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Models.Models.UserManagment
{
    public class Address : BaseEntityWithId
    {
        [Column("AddressId")]
        public override int Id { get; set; }
    }
}
