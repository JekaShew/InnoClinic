using FluentMigrator;

namespace ProfilesAPI.Persistance.Migrations;

[Migration(00001,"Initial DB Migration")]
public class InitialMigration : Migration
{
    public override void Down()
    {
        Delete.Index("IX_WorkStatus_Title").OnTable("WorkStatuses");
        Delete.Index("IX_Specialization_Title").OnTable("Specializations");
        Delete.Index("IX_DoctorSpecializations_DoctorId").OnTable("DoctorSpecializations");

        Delete.Table("DoctorSpecializations");
        Delete.Table("Patients");
        Delete.Table("Receptionists");
        Delete.Table("Doctors");
        Delete.Table("Administrators");
        Delete.Table("WorkStatuses");
        Delete.Table("Specializations");
    }

    public override void Up()
    {
        Create.Table("WorkStatuses")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Description").AsString().Nullable();

        Create.Table("Specializations")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Description").AsString().Nullable();

        Create.Table("Patients")
            .WithColumn("UserId").AsGuid().PrimaryKey()
            .WithColumn("FirstName").AsString().NotNullable()
            .WithColumn("LastName").AsString().NotNullable()
            .WithColumn("SecondName").AsString().Nullable()
            .WithColumn("Address").AsString().Nullable()
            .WithColumn("Phone").AsString().NotNullable()
            .WithColumn("BirthDate").AsDateTime().NotNullable()
            .WithColumn("Photo").AsString().NotNullable();

        Create.Table("Receptionists")
            .WithColumn("UserId").AsGuid().PrimaryKey()
            .WithColumn("FirstName").AsString().NotNullable()
            .WithColumn("LastName").AsString().NotNullable()
            .WithColumn("SecondName").AsString().Nullable()
            .WithColumn("Address").AsString().Nullable()
            .WithColumn("WorkEmail").AsString().NotNullable()
            .WithColumn("Phone").AsString().NotNullable()
            .WithColumn("BirthDate").AsDateTime().NotNullable()
            .WithColumn("CareerStartDate").AsDateTime().NotNullable()
            .WithColumn("Photo").AsString().NotNullable()
            .WithColumn("OfficeId").AsGuid().NotNullable()
            .WithColumn("WorkStatusId").AsGuid().NotNullable().ForeignKey("WorkStatuses", "Id");

        Create.Table("Doctors")
            .WithColumn("UserId").AsGuid().PrimaryKey()
            .WithColumn("FirstName").AsString().NotNullable()
            .WithColumn("LastName").AsString().NotNullable()
            .WithColumn("SecondName").AsString().Nullable()
            .WithColumn("Address").AsString().Nullable()
            .WithColumn("WorkEmail").AsString().NotNullable()
            .WithColumn("Phone").AsString().NotNullable()
            .WithColumn("BirthDate").AsDateTime().NotNullable()
            .WithColumn("CareerStartDate").AsDateTime().NotNullable()
            .WithColumn("Photo").AsString().NotNullable()
            .WithColumn("OfficeId").AsGuid().NotNullable()
            .WithColumn("WorkStatusId").AsGuid().NotNullable().ForeignKey("WorkStatuses", "Id");

        Create.Table("Administrators")
            .WithColumn("UserId").AsGuid().PrimaryKey()
            .WithColumn("FirstName").AsString().NotNullable()
            .WithColumn("LastName").AsString().NotNullable()
            .WithColumn("SecondName").AsString().Nullable()
            .WithColumn("Address").AsString().Nullable()
            .WithColumn("WorkEmail").AsString().NotNullable()
            .WithColumn("Phone").AsString().NotNullable()
            .WithColumn("BirthDate").AsDateTime().NotNullable()
            .WithColumn("CareerStartDate").AsDateTime().NotNullable()
            .WithColumn("Photo").AsString().NotNullable()
            .WithColumn("OfficeId").AsGuid().NotNullable()
            .WithColumn("WorkStatusId").AsGuid().NotNullable().ForeignKey("WorkStatuses", "Id");

        Create.Table("DoctorSpecializations")
           .WithColumn("Id").AsGuid().PrimaryKey()
           .WithColumn("DoctorId").AsGuid().NotNullable().ForeignKey("Doctors", "UserId")
           .WithColumn("SpecializationId").AsGuid().NotNullable().ForeignKey("Specializations", "Id")
           .WithColumn("SpecialzationAchievementDate").AsDateTime().NotNullable()
           .WithColumn("WorkExperience").AsString().Nullable()
           .WithColumn("Description").AsString().Nullable();

        Create.Index("IX_Specialization_Title")
            .OnTable("Specializations")
            .OnColumn("Title")
            .Ascending();

        Create.Index("IX_WorkStatus_Title")
            .OnTable("WorkStatuses")
            .OnColumn("Title")
            .Ascending();

        Create.Index("IX_DoctorSpecializations_DoctorId")
            .OnTable("DoctorSpecializations")
            .OnColumn("DoctorId")
            .Ascending();

        
    }
}
