namespace TimeAide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeAideContextV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CFSECode",
                c => new
                    {
                        CFSECodeId = c.Int(nullable: false, identity: true),
                        CFSECodeName = c.String(nullable: false, maxLength: 150, unicode: false),
                        CFSECodeDescription = c.String(maxLength: 150, unicode: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CFSECodeId);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(nullable: false, maxLength: 50, unicode: false),
                        DepartmentDescription = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Boolean(nullable: false),
                        USECFSEAssignment = c.Boolean(nullable: false),
                        CFSECodeId = c.Int(nullable: false),
                        CFSECompanyPercent = c.Decimal(nullable: false, precision: 18, scale: 5),
                        PayrollCompanyId = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DepartmentId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId, cascadeDelete: true)
                .ForeignKey("dbo.CFSECode", t => t.CFSECodeId)
                .Index(t => t.CFSECodeId)
                .Index(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.PayrollCompany",
                c => new
                    {
                        PayrollCompanyId = c.Int(nullable: false, identity: true),
                        PayrollCompanyName = c.String(nullable: false, maxLength: 50),
                        WebSite = c.String(maxLength: 50),
                        ContactName = c.String(maxLength: 50),
                        PayrollName = c.String(maxLength: 50),
                        PayrollZipCode = c.String(maxLength: 50),
                        EIN = c.String(nullable: false, maxLength: 50),
                        PayrollContactName = c.String(maxLength: 50),
                        PayrollContactTitle = c.String(maxLength: 50),
                        PayrollContactPhone = c.String(maxLength: 50),
                        CompanyStartDate = c.DateTime(storeType: "date"),
                        SICCode = c.String(nullable: false, maxLength: 50),
                        NAICSCode = c.String(nullable: false, maxLength: 50),
                        SeguroChoferilAccount = c.String(nullable: false, maxLength: 50),
                        DepartamentoDelTrabajoAccount = c.String(nullable: false, maxLength: 50),
                        DepartamentoDelTrabajoRate = c.Decimal(nullable: false, precision: 18, scale: 5),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 50, unicode: false),
                        CompanyDescription = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Boolean(nullable: false),
                        PayrollCompanyId = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId, cascadeDelete: true)
                .Index(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.UserInformation",
                c => new
                    {
                        UserInformationId = c.Int(nullable: false),
                        IdNumber = c.String(maxLength: 20),
                        SystemId = c.String(maxLength: 20),
                        FirstName = c.String(maxLength: 30),
                        MiddleInitial = c.String(maxLength: 2),
                        FirstLastName = c.String(maxLength: 30, unicode: false),
                        SecondLastName = c.String(maxLength: 30, unicode: false),
                        ShortFullName = c.String(maxLength: 50),
                        PayrollCompanyId = c.Int(),
                        CompanyID = c.Int(nullable: false),
                        DepartmentId = c.Int(),
                        SubDepartmentId = c.Int(),
                        EmployeeTypeID = c.Int(),
                        EmploymentStatusId = c.Int(),
                        DefaultJobCodeId = c.Int(),
                        SupervisoryLevelId = c.Int(),
                        AccessRightsLevelsId = c.Int(),
                        EthnicityId = c.Int(),
                        DisabilityId = c.Int(),
                        EmployeeNote = c.String(maxLength: 50),
                        GenderId = c.Int(),
                        BirthDate = c.DateTime(),
                        BirthPlace = c.String(maxLength: 50),
                        MaritalStatusId = c.Int(),
                        SSNEnd = c.String(maxLength: 9),
                        SSNEncrypted = c.String(maxLength: 512),
                        imgPhoto = c.Binary(storeType: "image"),
                        PasswordHash = c.String(maxLength: 512, unicode: false),
                        CreatedBy = c.Int(),
                        EmployeeStatusId = c.Int(),
                        AspNetUserId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserInformationId)
                .ForeignKey("dbo.JobCode", t => t.DefaultJobCodeId)
                .ForeignKey("dbo.Disability", t => t.DisabilityId)
                .ForeignKey("dbo.EmployeeStatus", t => t.EmployeeStatusId)
                .ForeignKey("dbo.SubDepartment", t => t.SubDepartmentId)
                .ForeignKey("dbo.EmployeeType", t => t.EmployeeTypeID)
                .ForeignKey("dbo.EmploymentStatus", t => t.EmploymentStatusId)
                .ForeignKey("dbo.Ethnicity", t => t.EthnicityId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId)
                .ForeignKey("dbo.SupervisoryLevel", t => t.SupervisoryLevelId)
                .ForeignKey("dbo.Company", t => t.CompanyID)
                .ForeignKey("dbo.Department", t => t.DepartmentId)
                .Index(t => t.PayrollCompanyId)
                .Index(t => t.CompanyID)
                .Index(t => t.DepartmentId)
                .Index(t => t.SubDepartmentId)
                .Index(t => t.EmployeeTypeID)
                .Index(t => t.EmploymentStatusId)
                .Index(t => t.DefaultJobCodeId)
                .Index(t => t.SupervisoryLevelId)
                .Index(t => t.EthnicityId)
                .Index(t => t.DisabilityId)
                .Index(t => t.EmployeeStatusId);
            
            CreateTable(
                "dbo.ContactPerson",
                c => new
                    {
                        ContactPersonId = c.Int(nullable: false, identity: true),
                        UserInformationId = c.Int(),
                        PayrollCompanyId = c.Int(),
                        ContactPersonTypeId = c.Int(),
                        DesignationId = c.Int(),
                        RelationshipId = c.Int(),
                        ContactPersonName = c.String(maxLength: 20),
                        IsDefault = c.Boolean(),
                        MainNumber = c.String(maxLength: 50),
                        AlternateNumber = c.String(maxLength: 50),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ContactPersonId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId)
                .ForeignKey("dbo.Relationship", t => t.RelationshipId)
                .ForeignKey("dbo.UserInformation", t => t.UserInformationId)
                .Index(t => t.UserInformationId)
                .Index(t => t.PayrollCompanyId)
                .Index(t => t.RelationshipId);
            
            CreateTable(
                "dbo.Relationship",
                c => new
                    {
                        RelationshipId = c.Int(nullable: false, identity: true),
                        RelationshipName = c.String(maxLength: 100),
                        RelationshipDescription = c.String(maxLength: 100),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RelationshipId);
            
            CreateTable(
                "dbo.JobCode",
                c => new
                    {
                        JobCodeId = c.Int(nullable: false, identity: true),
                        JobCodeName = c.String(nullable: false, maxLength: 50, unicode: false),
                        JobCodeDescription = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Boolean(nullable: false),
                        ProjectId = c.Int(),
                        PayrollCompanyId = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.JobCodeId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId, cascadeDelete: true)
                .Index(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.Disability",
                c => new
                    {
                        DisabilityId = c.Int(nullable: false, identity: true),
                        DisabilityName = c.String(nullable: false, maxLength: 500),
                        DisabilityDescription = c.String(maxLength: 1000),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DisabilityId);
            
            CreateTable(
                "dbo.EmployeeStatus",
                c => new
                    {
                        EmployeeStatusId = c.Int(nullable: false, identity: true),
                        EmployeeStatusName = c.String(nullable: false, maxLength: 500),
                        EmployeeStatusDescription = c.String(maxLength: 1000),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmployeeStatusId);
            
            CreateTable(
                "dbo.EmployeeSupervisor",
                c => new
                    {
                        EmployeeSupervisorId = c.Int(nullable: false, identity: true),
                        EmployeeUserId = c.Int(nullable: false),
                        SupervisorUserId = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmployeeSupervisorId)
                .ForeignKey("dbo.UserInformation", t => t.EmployeeUserId)
                .ForeignKey("dbo.UserInformation", t => t.SupervisorUserId)
                .Index(t => t.EmployeeUserId)
                .Index(t => t.SupervisorUserId);
            
            CreateTable(
                "dbo.EmployeeType",
                c => new
                    {
                        EmployeeTypeId = c.Int(nullable: false, identity: true),
                        EmployeeTypeName = c.String(nullable: false, maxLength: 150),
                        EmployeeTypeDescription = c.String(maxLength: 150),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmployeeTypeId);
            
            CreateTable(
                "dbo.EmploymentHistory",
                c => new
                    {
                        EmploymentHistoryId = c.Int(nullable: false, identity: true),
                        UserInformationId = c.Int(nullable: false),
                        CurrentRecord = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        PositionId = c.Int(),
                        EmployeeTypeId = c.Int(nullable: false),
                        ChangeReason = c.String(maxLength: 200, unicode: false),
                        Location = c.Int(),
                        DepartmentId = c.Int(),
                        SubDepartmentId = c.Int(),
                        CompanyId = c.Int(nullable: false),
                        SupervisorId = c.Int(),
                        AuthorizeByID = c.String(maxLength: 20, unicode: false),
                        ApprovedDate = c.DateTime(),
                        Document = c.Binary(storeType: "image"),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmploymentHistoryId)
                .ForeignKey("dbo.Company", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Department", t => t.DepartmentId)
                .ForeignKey("dbo.Position", t => t.PositionId)
                .ForeignKey("dbo.SubDepartment", t => t.SubDepartmentId)
                .ForeignKey("dbo.EmployeeType", t => t.EmployeeTypeId)
                .ForeignKey("dbo.UserInformation", t => t.UserInformationId)
                .ForeignKey("dbo.UserInformation", t => t.SupervisorId)
                .Index(t => t.UserInformationId)
                .Index(t => t.PositionId)
                .Index(t => t.EmployeeTypeId)
                .Index(t => t.DepartmentId)
                .Index(t => t.SubDepartmentId)
                .Index(t => t.CompanyId)
                .Index(t => t.SupervisorId);
            
            CreateTable(
                "dbo.Position",
                c => new
                    {
                        PositionId = c.Int(nullable: false, identity: true),
                        PositionName = c.String(nullable: false, maxLength: 50, unicode: false),
                        PositionDescription = c.String(maxLength: 200, unicode: false),
                        PositionCode = c.String(maxLength: 50, unicode: false),
                        DefaultPayScaleId = c.Int(),
                        DefaultEEOCategoryId = c.Int(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PositionId);
            
            CreateTable(
                "dbo.SubDepartment",
                c => new
                    {
                        SubDepartmentId = c.Int(nullable: false, identity: true),
                        SubDepartmentName = c.String(nullable: false, maxLength: 50, unicode: false),
                        SubDepartmentDescription = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Boolean(nullable: false),
                        USECFSEAssignment = c.Boolean(nullable: false),
                        CFSECodeId = c.Int(nullable: false),
                        CFSECompanyPercent = c.Decimal(nullable: false, precision: 18, scale: 5),
                        PayrollCompanyId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SubDepartmentId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Department", t => t.DepartmentId)
                .ForeignKey("dbo.CFSECode", t => t.CFSECodeId)
                .Index(t => t.CFSECodeId)
                .Index(t => t.PayrollCompanyId)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.EmployeeVeteranStatus",
                c => new
                    {
                        EmployeeVeteranStatusId = c.Int(nullable: false, identity: true),
                        UserInformationId = c.Int(nullable: false),
                        VeteranStatusId = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmployeeVeteranStatusId)
                .ForeignKey("dbo.VeteranStatus", t => t.VeteranStatusId)
                .ForeignKey("dbo.UserInformation", t => t.UserInformationId)
                .Index(t => t.UserInformationId)
                .Index(t => t.VeteranStatusId);
            
            CreateTable(
                "dbo.VeteranStatus",
                c => new
                    {
                        VeteranStatusId = c.Int(nullable: false, identity: true),
                        VeteranStatusName = c.String(nullable: false, maxLength: 500),
                        VeteranStatusDescription = c.String(maxLength: 1000),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VeteranStatusId);
            
            CreateTable(
                "dbo.EmploymentStatus",
                c => new
                    {
                        EmploymentStatusId = c.Int(nullable: false, identity: true),
                        EmploymentStatusName = c.String(nullable: false, maxLength: 50, unicode: false),
                        EmploymentStatusDescription = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Boolean(nullable: false),
                        PayrollCompanyId = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmploymentStatusId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId, cascadeDelete: true)
                .Index(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.Ethnicity",
                c => new
                    {
                        EthnicityId = c.Int(nullable: false, identity: true),
                        EthnicityName = c.String(nullable: false, maxLength: 500),
                        EthnicityDescription = c.String(maxLength: 1000),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EthnicityId);
            
            CreateTable(
                "dbo.SupervisoryLevel",
                c => new
                    {
                        SupervisoryLevelId = c.Int(nullable: false, identity: true),
                        SupervisoryLevelName = c.String(nullable: false, maxLength: 150),
                        SupervisoryLevelDescription = c.String(maxLength: 150),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SupervisoryLevelId);
            
            CreateTable(
                "dbo.UserContactInformation",
                c => new
                    {
                        UserContactInformationId = c.Int(nullable: false, identity: true),
                        UserInformationId = c.Int(nullable: false),
                        LoginEmail = c.String(maxLength: 50),
                        HomeAddress1 = c.String(maxLength: 50),
                        HomeAddress2 = c.String(maxLength: 50),
                        HomeCityId = c.Int(),
                        HomeStateId = c.Int(),
                        HomeZipCode = c.String(maxLength: 9),
                        MailingAddress1 = c.String(maxLength: 50),
                        MailingAddress2 = c.String(maxLength: 50),
                        MailingCityId = c.Int(),
                        MailingStateId = c.Int(),
                        MailingZipCode = c.String(maxLength: 9),
                        HomeNumber = c.String(maxLength: 10),
                        CelNumber = c.String(maxLength: 10),
                        FaxNumber = c.String(maxLength: 10),
                        OtherNumber = c.String(maxLength: 10),
                        WorkEmail = c.String(maxLength: 50),
                        PersonalEmail = c.String(maxLength: 50),
                        OtherEmail = c.String(maxLength: 50),
                        WorkNumber = c.String(maxLength: 10),
                        WorkExtension = c.String(maxLength: 50),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        City_Id = c.Int(),
                        City1_Id = c.Int(),
                    })
                .PrimaryKey(t => t.UserContactInformationId)
                .ForeignKey("dbo.City", t => t.City_Id)
                .ForeignKey("dbo.City", t => t.City1_Id)
                .ForeignKey("dbo.UserInformation", t => t.UserInformationId, cascadeDelete: true)
                .Index(t => t.UserInformationId)
                .Index(t => t.City_Id)
                .Index(t => t.City1_Id);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        CityCode = c.String(maxLength: 20),
                        CityName = c.String(nullable: false, maxLength: 200),
                        CityDescription = c.String(maxLength: 200, unicode: false),
                        StateId = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CityId)
                .ForeignKey("dbo.State", t => t.StateId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        StateCode = c.String(maxLength: 20, unicode: false),
                        StateName = c.String(nullable: false, maxLength: 200, unicode: false),
                        StateDescription = c.String(maxLength: 200, unicode: false),
                        CountryId = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.StateId)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        CountryCode = c.String(nullable: false, maxLength: 150),
                        CountryName = c.String(nullable: false, maxLength: 150),
                        CountryDescription = c.String(maxLength: 150, unicode: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.UserInformationActivation",
                c => new
                    {
                        UserInformationActivationId = c.Int(nullable: false, identity: true),
                        UserInformationId = c.Int(nullable: false),
                        ActivationCode = c.Guid(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserInformationActivationId)
                .ForeignKey("dbo.UserInformation", t => t.UserInformationId, cascadeDelete: true)
                .Index(t => t.UserInformationId);
            
            CreateTable(
                "dbo.UserInformationRole",
                c => new
                    {
                        UserRoleID = c.Int(nullable: false, identity: true),
                        RoleID = c.Int(nullable: false),
                        UserInformationId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CompanyId = c.Int(),
                        PayrollCompanyId = c.Int(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserRoleID)
                .ForeignKey("dbo.Role", t => t.RoleID)
                .ForeignKey("dbo.UserInformation", t => t.UserInformationId, cascadeDelete: true)
                .Index(t => t.RoleID)
                .Index(t => t.UserInformationId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 200),
                        RoleTypeId = c.Int(),
                        CompanyId = c.Int(),
                        PayrollCompanyId = c.Int(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RoleId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.RoleType", t => t.RoleTypeId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId)
                .Index(t => t.RoleTypeId)
                .Index(t => t.CompanyId)
                .Index(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.RoleFormPrivilege",
                c => new
                    {
                        RolePrivilegeId = c.Int(nullable: false, identity: true),
                        RoleID = c.Int(),
                        PrivilegeId = c.Int(),
                        FormId = c.Int(),
                        CompanyId = c.Int(),
                        PayrollCompanyId = c.Int(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RolePrivilegeId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.Privilege", t => t.PrivilegeId)
                .ForeignKey("dbo.Form", t => t.FormId)
                .ForeignKey("dbo.Role", t => t.RoleID)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId)
                .Index(t => t.RoleID)
                .Index(t => t.PrivilegeId)
                .Index(t => t.FormId)
                .Index(t => t.CompanyId)
                .Index(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.Form",
                c => new
                    {
                        FormId = c.Int(nullable: false, identity: true),
                        ModuleId = c.Int(),
                        FormName = c.String(maxLength: 100),
                        Url = c.String(maxLength: 200),
                        Label = c.String(maxLength: 100),
                        CompanyId = c.Int(),
                        PayrollCompanyId = c.Int(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FormId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.Module", t => t.ModuleId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId)
                .Index(t => t.ModuleId)
                .Index(t => t.CompanyId)
                .Index(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.InterfaceControlForm",
                c => new
                    {
                        InterfaceControlFormId = c.Int(nullable: false, identity: true),
                        InterfaceControlId = c.Int(),
                        ModuleId = c.Int(),
                        FormId = c.Int(),
                        PrivilegeId = c.Int(),
                        CompanyId = c.Int(),
                        PayrollCompanyId = c.Int(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.InterfaceControlFormId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.Form", t => t.FormId)
                .ForeignKey("dbo.InterfaceControl", t => t.InterfaceControlId)
                .ForeignKey("dbo.Module", t => t.ModuleId)
                .ForeignKey("dbo.Privilege", t => t.PrivilegeId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId)
                .Index(t => t.InterfaceControlId)
                .Index(t => t.ModuleId)
                .Index(t => t.FormId)
                .Index(t => t.PrivilegeId)
                .Index(t => t.CompanyId)
                .Index(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.InterfaceControl",
                c => new
                    {
                        InterfaceControlId = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 100),
                        Name = c.String(maxLength: 100),
                        CompanyId = c.Int(),
                        PayrollCompanyId = c.Int(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.InterfaceControlId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId)
                .Index(t => t.CompanyId)
                .Index(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.UserMenu",
                c => new
                    {
                        LinkId = c.Int(nullable: false, identity: true),
                        ParentLinkId = c.Int(),
                        FormId = c.Int(),
                        UserInterfaceId = c.Int(),
                        Weight = c.Int(),
                        Title = c.String(maxLength: 100),
                        Alt = c.String(maxLength: 100),
                        Anchor = c.String(maxLength: 100),
                        AnchorClass = c.String(maxLength: 100),
                        Description = c.String(maxLength: 100),
                        CompanyId = c.Int(),
                        PayrollCompanyId = c.Int(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.LinkId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.Form", t => t.FormId)
                .ForeignKey("dbo.InterfaceControl", t => t.UserInterfaceId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId)
                .Index(t => t.FormId)
                .Index(t => t.UserInterfaceId)
                .Index(t => t.CompanyId)
                .Index(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.Module",
                c => new
                    {
                        ModuleId = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 20),
                        Name = c.String(maxLength: 100),
                        CompanyId = c.Int(),
                        PayrollCompanyId = c.Int(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ModuleId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId)
                .Index(t => t.CompanyId)
                .Index(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.Privilege",
                c => new
                    {
                        PrivilegeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(maxLength: 20),
                        CompanyId = c.Int(),
                        PayrollCompanyId = c.Int(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PrivilegeId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId)
                .Index(t => t.CompanyId)
                .Index(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.RoleInterfaceControlPrivilege",
                c => new
                    {
                        RoleInterfacePrivilegeId = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(),
                        PrivilegeId = c.Int(),
                        InterfaceControlFormId = c.Int(),
                        CompanyId = c.Int(),
                        PayrollCompanyId = c.Int(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RoleInterfacePrivilegeId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.InterfaceControlForm", t => t.InterfaceControlFormId)
                .ForeignKey("dbo.Privilege", t => t.PrivilegeId)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId)
                .Index(t => t.RoleId)
                .Index(t => t.PrivilegeId)
                .Index(t => t.InterfaceControlFormId)
                .Index(t => t.CompanyId)
                .Index(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.RoleTypeFormPrivilege",
                c => new
                    {
                        RoleTypeFormPrivilegeId = c.Int(nullable: false, identity: true),
                        RoleTypeId = c.Int(),
                        PrivilegeId = c.Int(),
                        FormId = c.Int(),
                        CompanyId = c.Int(),
                        PayrollCompanyId = c.Int(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RoleTypeFormPrivilegeId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.Form", t => t.FormId)
                .ForeignKey("dbo.Privilege", t => t.PrivilegeId)
                .ForeignKey("dbo.RoleType", t => t.RoleTypeId)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId)
                .Index(t => t.RoleTypeId)
                .Index(t => t.PrivilegeId)
                .Index(t => t.FormId)
                .Index(t => t.CompanyId)
                .Index(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.RoleType",
                c => new
                    {
                        RoleTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        CompanyId = c.Int(nullable: false),
                        PayrollCompanyId = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RoleTypeId)
                .ForeignKey("dbo.Company", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.PayrollCompany", t => t.PayrollCompanyId)
                .Index(t => t.CompanyId)
                .Index(t => t.PayrollCompanyId);
            
            CreateTable(
                "dbo.Employment",
                c => new
                    {
                        EmploymentId = c.Int(nullable: false, identity: true),
                        UserInformationId = c.Int(),
                        CurrentRecord = c.Int(),
                        OriginalHireDate = c.DateTime(),
                        EffectiveHireDate = c.DateTime(),
                        ProbationStartDate = c.DateTime(),
                        ProbationEndDate = c.DateTime(),
                        EmploymentStatusId = c.Int(),
                        TerminationDate = c.DateTime(),
                        TerminationTypeId = c.Int(),
                        TerminationReasonId = c.Int(),
                        DocumentName = c.String(maxLength: 50),
                        DocumentExtension = c.String(maxLength: 500),
                        DocumentFile = c.String(maxLength: 500),
                        TerminationEligibilityId = c.Int(),
                        TerminationNotes = c.String(maxLength: 500),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmploymentId)
                .ForeignKey("dbo.EmploymentStatus", t => t.EmploymentStatusId)
                .ForeignKey("dbo.TerminationEligibility", t => t.TerminationEligibilityId)
                .ForeignKey("dbo.TerminationReason", t => t.TerminationReasonId)
                .ForeignKey("dbo.TerminationType", t => t.TerminationTypeId)
                .ForeignKey("dbo.UserInformation", t => t.UserInformationId)
                .Index(t => t.UserInformationId)
                .Index(t => t.EmploymentStatusId)
                .Index(t => t.TerminationTypeId)
                .Index(t => t.TerminationReasonId)
                .Index(t => t.TerminationEligibilityId);
            
            CreateTable(
                "dbo.TerminationEligibility",
                c => new
                    {
                        TerminationEligibilityId = c.Int(nullable: false, identity: true),
                        TerminationEligibilityName = c.String(nullable: false, maxLength: 20),
                        TerminationEligibilityDescription = c.String(maxLength: 50),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TerminationEligibilityId);
            
            CreateTable(
                "dbo.TerminationReason",
                c => new
                    {
                        TerminationReasonId = c.Int(nullable: false, identity: true),
                        TerminationReasonName = c.String(nullable: false, maxLength: 20),
                        TerminationReasonDescription = c.String(maxLength: 50),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TerminationReasonId);
            
            CreateTable(
                "dbo.TerminationType",
                c => new
                    {
                        TerminationTypeId = c.Int(nullable: false, identity: true),
                        TerminationTypeName = c.String(nullable: false, maxLength: 20),
                        TerminationTypeDescription = c.String(maxLength: 50),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TerminationTypeId);
            
            CreateTable(
                "dbo.Gender",
                c => new
                    {
                        GenderId = c.Int(nullable: false, identity: true),
                        GenderName = c.String(nullable: false, maxLength: 20),
                        GenderDescription = c.String(maxLength: 50),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.GenderId);
            
            CreateTable(
                "dbo.MaritalStatus",
                c => new
                    {
                        MaritalStatusId = c.Int(nullable: false, identity: true),
                        MaritalStatusName = c.String(nullable: false, maxLength: 20),
                        MaritalStatusDescription = c.String(maxLength: 50),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DataEntryStatus = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MaritalStatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employment", "UserInformationId", "dbo.UserInformation");
            DropForeignKey("dbo.Employment", "TerminationTypeId", "dbo.TerminationType");
            DropForeignKey("dbo.Employment", "TerminationReasonId", "dbo.TerminationReason");
            DropForeignKey("dbo.Employment", "TerminationEligibilityId", "dbo.TerminationEligibility");
            DropForeignKey("dbo.Employment", "EmploymentStatusId", "dbo.EmploymentStatus");
            DropForeignKey("dbo.SubDepartment", "CFSECodeId", "dbo.CFSECode");
            DropForeignKey("dbo.Department", "CFSECodeId", "dbo.CFSECode");
            DropForeignKey("dbo.UserInformation", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.SubDepartment", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.Department", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.UserMenu", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.RoleTypeFormPrivilege", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.RoleType", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.RoleInterfaceControlPrivilege", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.RoleFormPrivilege", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.Role", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.Privilege", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.Module", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.InterfaceControlForm", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.InterfaceControl", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.Form", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.UserInformation", "CompanyID", "dbo.Company");
            DropForeignKey("dbo.UserInformationRole", "UserInformationId", "dbo.UserInformation");
            DropForeignKey("dbo.UserInformationRole", "RoleID", "dbo.Role");
            DropForeignKey("dbo.RoleTypeFormPrivilege", "RoleTypeId", "dbo.RoleType");
            DropForeignKey("dbo.Role", "RoleTypeId", "dbo.RoleType");
            DropForeignKey("dbo.RoleType", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.RoleFormPrivilege", "RoleID", "dbo.Role");
            DropForeignKey("dbo.RoleFormPrivilege", "FormId", "dbo.Form");
            DropForeignKey("dbo.RoleTypeFormPrivilege", "PrivilegeId", "dbo.Privilege");
            DropForeignKey("dbo.RoleTypeFormPrivilege", "FormId", "dbo.Form");
            DropForeignKey("dbo.RoleTypeFormPrivilege", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.RoleInterfaceControlPrivilege", "RoleId", "dbo.Role");
            DropForeignKey("dbo.RoleInterfaceControlPrivilege", "PrivilegeId", "dbo.Privilege");
            DropForeignKey("dbo.RoleInterfaceControlPrivilege", "InterfaceControlFormId", "dbo.InterfaceControlForm");
            DropForeignKey("dbo.RoleInterfaceControlPrivilege", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.RoleFormPrivilege", "PrivilegeId", "dbo.Privilege");
            DropForeignKey("dbo.InterfaceControlForm", "PrivilegeId", "dbo.Privilege");
            DropForeignKey("dbo.Privilege", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.InterfaceControlForm", "ModuleId", "dbo.Module");
            DropForeignKey("dbo.Form", "ModuleId", "dbo.Module");
            DropForeignKey("dbo.Module", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.UserMenu", "UserInterfaceId", "dbo.InterfaceControl");
            DropForeignKey("dbo.UserMenu", "FormId", "dbo.Form");
            DropForeignKey("dbo.UserMenu", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.InterfaceControlForm", "InterfaceControlId", "dbo.InterfaceControl");
            DropForeignKey("dbo.InterfaceControl", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.InterfaceControlForm", "FormId", "dbo.Form");
            DropForeignKey("dbo.InterfaceControlForm", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Form", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.RoleFormPrivilege", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Role", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.UserInformationActivation", "UserInformationId", "dbo.UserInformation");
            DropForeignKey("dbo.UserContactInformation", "UserInformationId", "dbo.UserInformation");
            DropForeignKey("dbo.UserContactInformation", "City1_Id", "dbo.City");
            DropForeignKey("dbo.UserContactInformation", "City_Id", "dbo.City");
            DropForeignKey("dbo.State", "CountryId", "dbo.Country");
            DropForeignKey("dbo.City", "StateId", "dbo.State");
            DropForeignKey("dbo.UserInformation", "SupervisoryLevelId", "dbo.SupervisoryLevel");
            DropForeignKey("dbo.UserInformation", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.UserInformation", "EthnicityId", "dbo.Ethnicity");
            DropForeignKey("dbo.UserInformation", "EmploymentStatusId", "dbo.EmploymentStatus");
            DropForeignKey("dbo.EmploymentStatus", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.EmploymentHistory", "SupervisorId", "dbo.UserInformation");
            DropForeignKey("dbo.EmploymentHistory", "UserInformationId", "dbo.UserInformation");
            DropForeignKey("dbo.EmployeeVeteranStatus", "UserInformationId", "dbo.UserInformation");
            DropForeignKey("dbo.EmployeeVeteranStatus", "VeteranStatusId", "dbo.VeteranStatus");
            DropForeignKey("dbo.UserInformation", "EmployeeTypeID", "dbo.EmployeeType");
            DropForeignKey("dbo.EmploymentHistory", "EmployeeTypeId", "dbo.EmployeeType");
            DropForeignKey("dbo.EmploymentHistory", "SubDepartmentId", "dbo.SubDepartment");
            DropForeignKey("dbo.UserInformation", "SubDepartmentId", "dbo.SubDepartment");
            DropForeignKey("dbo.SubDepartment", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.EmploymentHistory", "PositionId", "dbo.Position");
            DropForeignKey("dbo.EmploymentHistory", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.EmploymentHistory", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.EmployeeSupervisor", "SupervisorUserId", "dbo.UserInformation");
            DropForeignKey("dbo.EmployeeSupervisor", "EmployeeUserId", "dbo.UserInformation");
            DropForeignKey("dbo.UserInformation", "EmployeeStatusId", "dbo.EmployeeStatus");
            DropForeignKey("dbo.UserInformation", "DisabilityId", "dbo.Disability");
            DropForeignKey("dbo.UserInformation", "DefaultJobCodeId", "dbo.JobCode");
            DropForeignKey("dbo.JobCode", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.ContactPerson", "UserInformationId", "dbo.UserInformation");
            DropForeignKey("dbo.ContactPerson", "RelationshipId", "dbo.Relationship");
            DropForeignKey("dbo.ContactPerson", "PayrollCompanyId", "dbo.PayrollCompany");
            DropForeignKey("dbo.Company", "PayrollCompanyId", "dbo.PayrollCompany");
            DropIndex("dbo.Employment", new[] { "TerminationEligibilityId" });
            DropIndex("dbo.Employment", new[] { "TerminationReasonId" });
            DropIndex("dbo.Employment", new[] { "TerminationTypeId" });
            DropIndex("dbo.Employment", new[] { "EmploymentStatusId" });
            DropIndex("dbo.Employment", new[] { "UserInformationId" });
            DropIndex("dbo.RoleType", new[] { "PayrollCompanyId" });
            DropIndex("dbo.RoleType", new[] { "CompanyId" });
            DropIndex("dbo.RoleTypeFormPrivilege", new[] { "PayrollCompanyId" });
            DropIndex("dbo.RoleTypeFormPrivilege", new[] { "CompanyId" });
            DropIndex("dbo.RoleTypeFormPrivilege", new[] { "FormId" });
            DropIndex("dbo.RoleTypeFormPrivilege", new[] { "PrivilegeId" });
            DropIndex("dbo.RoleTypeFormPrivilege", new[] { "RoleTypeId" });
            DropIndex("dbo.RoleInterfaceControlPrivilege", new[] { "PayrollCompanyId" });
            DropIndex("dbo.RoleInterfaceControlPrivilege", new[] { "CompanyId" });
            DropIndex("dbo.RoleInterfaceControlPrivilege", new[] { "InterfaceControlFormId" });
            DropIndex("dbo.RoleInterfaceControlPrivilege", new[] { "PrivilegeId" });
            DropIndex("dbo.RoleInterfaceControlPrivilege", new[] { "RoleId" });
            DropIndex("dbo.Privilege", new[] { "PayrollCompanyId" });
            DropIndex("dbo.Privilege", new[] { "CompanyId" });
            DropIndex("dbo.Module", new[] { "PayrollCompanyId" });
            DropIndex("dbo.Module", new[] { "CompanyId" });
            DropIndex("dbo.UserMenu", new[] { "PayrollCompanyId" });
            DropIndex("dbo.UserMenu", new[] { "CompanyId" });
            DropIndex("dbo.UserMenu", new[] { "UserInterfaceId" });
            DropIndex("dbo.UserMenu", new[] { "FormId" });
            DropIndex("dbo.InterfaceControl", new[] { "PayrollCompanyId" });
            DropIndex("dbo.InterfaceControl", new[] { "CompanyId" });
            DropIndex("dbo.InterfaceControlForm", new[] { "PayrollCompanyId" });
            DropIndex("dbo.InterfaceControlForm", new[] { "CompanyId" });
            DropIndex("dbo.InterfaceControlForm", new[] { "PrivilegeId" });
            DropIndex("dbo.InterfaceControlForm", new[] { "FormId" });
            DropIndex("dbo.InterfaceControlForm", new[] { "ModuleId" });
            DropIndex("dbo.InterfaceControlForm", new[] { "InterfaceControlId" });
            DropIndex("dbo.Form", new[] { "PayrollCompanyId" });
            DropIndex("dbo.Form", new[] { "CompanyId" });
            DropIndex("dbo.Form", new[] { "ModuleId" });
            DropIndex("dbo.RoleFormPrivilege", new[] { "PayrollCompanyId" });
            DropIndex("dbo.RoleFormPrivilege", new[] { "CompanyId" });
            DropIndex("dbo.RoleFormPrivilege", new[] { "FormId" });
            DropIndex("dbo.RoleFormPrivilege", new[] { "PrivilegeId" });
            DropIndex("dbo.RoleFormPrivilege", new[] { "RoleID" });
            DropIndex("dbo.Role", new[] { "PayrollCompanyId" });
            DropIndex("dbo.Role", new[] { "CompanyId" });
            DropIndex("dbo.Role", new[] { "RoleTypeId" });
            DropIndex("dbo.UserInformationRole", new[] { "UserInformationId" });
            DropIndex("dbo.UserInformationRole", new[] { "RoleID" });
            DropIndex("dbo.UserInformationActivation", new[] { "UserInformationId" });
            DropIndex("dbo.State", new[] { "CountryId" });
            DropIndex("dbo.City", new[] { "StateId" });
            DropIndex("dbo.UserContactInformation", new[] { "City1_Id" });
            DropIndex("dbo.UserContactInformation", new[] { "City_Id" });
            DropIndex("dbo.UserContactInformation", new[] { "UserInformationId" });
            DropIndex("dbo.EmploymentStatus", new[] { "PayrollCompanyId" });
            DropIndex("dbo.EmployeeVeteranStatus", new[] { "VeteranStatusId" });
            DropIndex("dbo.EmployeeVeteranStatus", new[] { "UserInformationId" });
            DropIndex("dbo.SubDepartment", new[] { "DepartmentId" });
            DropIndex("dbo.SubDepartment", new[] { "PayrollCompanyId" });
            DropIndex("dbo.SubDepartment", new[] { "CFSECodeId" });
            DropIndex("dbo.EmploymentHistory", new[] { "SupervisorId" });
            DropIndex("dbo.EmploymentHistory", new[] { "CompanyId" });
            DropIndex("dbo.EmploymentHistory", new[] { "SubDepartmentId" });
            DropIndex("dbo.EmploymentHistory", new[] { "DepartmentId" });
            DropIndex("dbo.EmploymentHistory", new[] { "EmployeeTypeId" });
            DropIndex("dbo.EmploymentHistory", new[] { "PositionId" });
            DropIndex("dbo.EmploymentHistory", new[] { "UserInformationId" });
            DropIndex("dbo.EmployeeSupervisor", new[] { "SupervisorUserId" });
            DropIndex("dbo.EmployeeSupervisor", new[] { "EmployeeUserId" });
            DropIndex("dbo.JobCode", new[] { "PayrollCompanyId" });
            DropIndex("dbo.ContactPerson", new[] { "RelationshipId" });
            DropIndex("dbo.ContactPerson", new[] { "PayrollCompanyId" });
            DropIndex("dbo.ContactPerson", new[] { "UserInformationId" });
            DropIndex("dbo.UserInformation", new[] { "EmployeeStatusId" });
            DropIndex("dbo.UserInformation", new[] { "DisabilityId" });
            DropIndex("dbo.UserInformation", new[] { "EthnicityId" });
            DropIndex("dbo.UserInformation", new[] { "SupervisoryLevelId" });
            DropIndex("dbo.UserInformation", new[] { "DefaultJobCodeId" });
            DropIndex("dbo.UserInformation", new[] { "EmploymentStatusId" });
            DropIndex("dbo.UserInformation", new[] { "EmployeeTypeID" });
            DropIndex("dbo.UserInformation", new[] { "SubDepartmentId" });
            DropIndex("dbo.UserInformation", new[] { "DepartmentId" });
            DropIndex("dbo.UserInformation", new[] { "CompanyID" });
            DropIndex("dbo.UserInformation", new[] { "PayrollCompanyId" });
            DropIndex("dbo.Company", new[] { "PayrollCompanyId" });
            DropIndex("dbo.Department", new[] { "PayrollCompanyId" });
            DropIndex("dbo.Department", new[] { "CFSECodeId" });
            DropTable("dbo.MaritalStatus");
            DropTable("dbo.Gender");
            DropTable("dbo.TerminationType");
            DropTable("dbo.TerminationReason");
            DropTable("dbo.TerminationEligibility");
            DropTable("dbo.Employment");
            DropTable("dbo.RoleType");
            DropTable("dbo.RoleTypeFormPrivilege");
            DropTable("dbo.RoleInterfaceControlPrivilege");
            DropTable("dbo.Privilege");
            DropTable("dbo.Module");
            DropTable("dbo.UserMenu");
            DropTable("dbo.InterfaceControl");
            DropTable("dbo.InterfaceControlForm");
            DropTable("dbo.Form");
            DropTable("dbo.RoleFormPrivilege");
            DropTable("dbo.Role");
            DropTable("dbo.UserInformationRole");
            DropTable("dbo.UserInformationActivation");
            DropTable("dbo.Country");
            DropTable("dbo.State");
            DropTable("dbo.City");
            DropTable("dbo.UserContactInformation");
            DropTable("dbo.SupervisoryLevel");
            DropTable("dbo.Ethnicity");
            DropTable("dbo.EmploymentStatus");
            DropTable("dbo.VeteranStatus");
            DropTable("dbo.EmployeeVeteranStatus");
            DropTable("dbo.SubDepartment");
            DropTable("dbo.Position");
            DropTable("dbo.EmploymentHistory");
            DropTable("dbo.EmployeeType");
            DropTable("dbo.EmployeeSupervisor");
            DropTable("dbo.EmployeeStatus");
            DropTable("dbo.Disability");
            DropTable("dbo.JobCode");
            DropTable("dbo.Relationship");
            DropTable("dbo.ContactPerson");
            DropTable("dbo.UserInformation");
            DropTable("dbo.Company");
            DropTable("dbo.PayrollCompany");
            DropTable("dbo.Department");
            DropTable("dbo.CFSECode");
        }
    }
}
