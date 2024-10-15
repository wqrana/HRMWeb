namespace TimeAide.Web.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.Entity.SqlServer;
    using System.Data.Entity.Migrations.Model;

    public partial class TimeAideContext : DbContext
    {
        public virtual DbSet<UserContactInformation> UserContactInformation { get; set; }
        public virtual DbSet<UserInformation> UserInformation { get; set; }
        public virtual DbSet<UserInformationActivation> UserInformationActivation { get; set; }
        public virtual DbSet<PassswordResetCode> PassswordResetCode { get; set; }
        public virtual DbSet<RoleType> RoleType { get; set; }
        public virtual DbSet<EmailType> EmailType { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Form> Form { get; set; }
        //public virtual DbSet<InterfaceControl> InterfaceControl { get; set; }
        //public virtual DbSet<InterfaceControlForm> InterfaceControlForm { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<Privilege> Privilege { get; set; }
        public virtual DbSet<RoleFormPrivilege> RoleFormPrivilege { get; set; }
        public virtual DbSet<RoleTypeFormPrivilege> RoleTypeFormPrivilege { get; set; }
        public virtual DbSet<RoleInterfaceControlPrivilege> RoleInterfaceControlPrivilege { get; set; }
        public virtual DbSet<UserInformationRole> UserInformationRole { get; set; }
        public virtual DbSet<UserMenu> UserMenus { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }
        public virtual DbSet<NotificationMessage> NotificationMessage { get; set; }
        public virtual DbSet<NotificationSchedule> NotificationSchedule { get; set; }
        public virtual DbSet<NotificationScheduleDetail> NotificationScheduleDetail { get; set; }
        public virtual DbSet<NotificationLog> NotificationLog { get; set; }
        //public virtual DbSet<NotificationMessage> NotificationMessage { get; set; }
        public virtual DbSet<NotificationServiceEvent> NotificationServiceEvent { get; set; }
        public virtual DbSet<NotificationScheduleEmployeeGroup> NotificationScheduleEmployeeGroup { get; set; }
        public virtual DbSet<NotificationLogMessageReadBy> NotificationLogMessageReadBy { get; set; }
        public virtual DbSet<NotificationLogEmail> NotificationLogEmail { get; set; }
        public virtual DbSet<NotificationType> NotificationType { get; set; }
        public virtual DbSet<EmploymentHistoryAuthorizer> EmploymentHistoryAuthorizer { get; set; }
        public virtual DbSet<PayInformationHistoryAuthorizer> PayInformationHistoryAuthorizer { get; set; }

        public virtual DbSet<Workflow> Workflow { get; set; }

        public virtual DbSet<WorkflowLevel> WorkflowLevel { get; set; }
        public virtual DbSet<WorkflowLevelType> WorkflowLevelType { get; set; }
        public virtual DbSet<WorkflowLevelGroup> WorkflowLevelGroup { get; set; }
        public virtual DbSet<ClosingNotificationType> ClosingNotificationType { get; set; }
        public virtual DbSet<WorkflowTriggerType> WorkflowTriggerType { get; set; }
        public virtual DbSet<WorkflowTrigger> WorkflowTrigger { get; set; }

        public virtual DbSet<WorkflowTriggerRequest> WorkflowTriggerRequest { get; set; }
        public virtual DbSet<WorkflowTriggerRequestDetail> WorkflowTriggerRequestDetail { get; set; }
        public virtual DbSet<WorkflowTriggerRequestDetailEmail> WorkflowTriggerRequestDetailEmail { get; set; }
        public virtual DbSet<WorkflowActionType> WorkflowActionType { get; set; }

    }

    public class CustomSqlGenerator : SqlServerMigrationSqlGenerator
    {
        protected override void Generate(AddForeignKeyOperation addForeignKeyOperation)
        {
            addForeignKeyOperation.Name = getFkName(addForeignKeyOperation.PrincipalTable,
                addForeignKeyOperation.DependentTable, addForeignKeyOperation.DependentColumns.ToArray());
            base.Generate(addForeignKeyOperation);
        }

        protected override void Generate(DropForeignKeyOperation dropForeignKeyOperation)
        {
            dropForeignKeyOperation.Name = getFkName(dropForeignKeyOperation.PrincipalTable,
                dropForeignKeyOperation.DependentTable, dropForeignKeyOperation.DependentColumns.ToArray());
            base.Generate(dropForeignKeyOperation);
        }

        protected override void Generate(CreateTableOperation createTableOperation)
        {
            createTableOperation.PrimaryKey.Name = getPkName(createTableOperation.Name);
            base.Generate(createTableOperation);
        }

        protected override void Generate(AddPrimaryKeyOperation addPrimaryKeyOperation)
        {
            addPrimaryKeyOperation.Name = getPkName(addPrimaryKeyOperation.Table);
            base.Generate(addPrimaryKeyOperation);
        }

        protected override void Generate(DropPrimaryKeyOperation dropPrimaryKeyOperation)
        {
            dropPrimaryKeyOperation.Name = getPkName(dropPrimaryKeyOperation.Table);
            base.Generate(dropPrimaryKeyOperation);
        }

        private static string getFkName(string primaryKeyTable, string foreignKeyTable, params string[] foreignTableFields)
        {
            return "FK_" + primaryKeyTable.Replace("dbo.", "") + "_" + foreignKeyTable.Replace(".dbo", "") + "_" + foreignTableFields[0];
        }
        private static string getPkName(string primaryKeyTable)
        {
            return "PK_" + primaryKeyTable.Replace("dbo.", "");
        }
    }

    //public class CustomDbConfiguration : DbConfiguration
    //{
    //    public CustomDbConfiguration()
    //    {
    //        SetMigrationSqlGenerator(SqlProviderServices.ProviderInvariantName,
    //            () => new CustomSqlGenerator());
    //    }
    //}
}
