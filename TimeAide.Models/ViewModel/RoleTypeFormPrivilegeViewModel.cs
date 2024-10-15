using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TimeAide.Web.Models;

namespace TimeAide.Web.ViewModel
{
    public class RoleTypeFormPrivilegeViewModel
    {
        [Key]
        public long RolePrivilegeId { get; set; }

        public long? RoleId { get; set; }
        public long? RoleTypeID { get; set; }

        public int? PrivilegeId { get; set; }

        public int? FormId { get; set; }

        public virtual Form Form { get; set; }

        public virtual Privilege Privilege { get; set; }

        public virtual Role Role { get; set; }
        public virtual RoleType RoleType { get; set; }
        public virtual bool IsAssigned { get; set; }
    }
    public class RoleTypeFormPrivilegeViewModel1
    {
        [Key]
        public long RolePrivilegeId { get; set; }

        public long? RoleTypeID { get; set; }

        public int? PrivilegeId { get; set; }

        public int? FormId { get; set; }

        public virtual Form Form { get; set; }

        public bool AllowAdd { get; set; }
        public int AllowAddInt
        {
            set
            {
                if (value == 1)
                    AllowAdd = true;
                else
                    AllowAdd = false;
            }
            get
            {
                if (AllowAdd)
                    return 1;
                else
                    return 0;
            }
        }
        public bool AllowEdit { get; set; }
        public int AllowEditInt
        {
            set
            {
                if (value == 1)
                    AllowEdit = true;
                else
                    AllowEdit = false;
            }
            get
            {
                if (AllowEdit)
                    return 1;
                else
                    return 0;
            }
        }
        public bool AllowDelete { get; set; }
        public int AllowDeleteInt
        {
            set
            {
                if (value == 1)
                    AllowDelete = true;
                else
                    AllowDelete = false;
            }
            get
            {
                if (AllowDelete)
                    return 1;
                else
                    return 0;
            }
        }
        public bool AllowView { get; set; }
        public int AllowViewInt
        {
            set
            {
                if (value == 1)
                    AllowView = true;
                else
                    AllowView = false;
            }
            get
            {
                if (AllowView)
                    return 1;
                else
                    return 0;
            }
        }
        public bool IsRestricted { get; set; }
        public int IsRestrictedInt
        {
            set
            {
                if (value == 1)
                    IsRestricted = true;
                else
                    IsRestricted = false;
            }
            get
            {
                if (IsRestricted)
                    return 1;
                else
                    return 0;
            }
        }

        public virtual Role Role { get; set; }
        public virtual bool IsAssigned { get; set; }
    }
    public class RoleTypeFormPrivilegeMatrixViewModel1
    {
        public int? RoleTypeID { get; set; }
        public virtual RoleType RoleType { get; set; }
        public int? RoleId { get; set; }
        public virtual Role Role { get; set; }
        public List<RoleTypeFormPrivilegeViewModel1> RoleTypeFormPrivileges
        {
            get;
            set;
        }
    }
}