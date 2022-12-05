using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MustafidAppModels.Models;

#nullable disable

namespace MustafidAppModels.Context
{
    public partial class MustafidAppContext : DbContext
    {
        public MustafidAppContext()
        {
        }
        public MustafidAppContext(DbContextOptions<MustafidAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicantType> ApplicantTypes { get; set; }
        public virtual DbSet<CheckBoxType> CheckBoxTypes { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<DelayedTransaction> DelayedTransactions { get; set; }
        public virtual DbSet<EForm> EForms { get; set; }
        public virtual DbSet<EFormsAnswer> EFormsAnswers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<IdDocument> IdDocuments { get; set; }
        public virtual DbSet<InputType> InputTypes { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobPermission> JobPermissions { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<MainService> MainServices { get; set; }
        public virtual DbSet<Paragraph> Paragraphs { get; set; }
        public virtual DbSet<PersonEform> PersonEforms { get; set; }
        public virtual DbSet<PersonelDatum> PersonelData { get; set; }
        public virtual DbSet<PhoneNumberNotification> PhoneNumberNotifications { get; set; }
        public virtual DbSet<PreviewEformApproval> PreviewEformApprovals { get; set; }
        public virtual DbSet<PreviewTableCol> PreviewTableCols { get; set; }
        public virtual DbSet<Privilage> Privilages { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<RadioType> RadioTypes { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<RequestDatum> RequestData { get; set; }
        public virtual DbSet<RequestFile> RequestFiles { get; set; }
        public virtual DbSet<RequestLog> RequestLogs { get; set; }
        public virtual DbSet<RequestState> RequestStates { get; set; }
        public virtual DbSet<RequestTransaction> RequestTransactions { get; set; }
        public virtual DbSet<RequestType> RequestTypes { get; set; }
        public virtual DbSet<RequiredDocument> RequiredDocuments { get; set; }
        public virtual DbSet<Separator> Separators { get; set; }
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }
        public virtual DbSet<SubService> SubServices { get; set; }
        public virtual DbSet<SystemLog> SystemLogs { get; set; }
        public virtual DbSet<TableColumn> TableColumns { get; set; }
        public virtual DbSet<TablesAnsware> TablesAnswares { get; set; }
        public virtual DbSet<TitleMiddleName> TitleMiddleNames { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<UnitLevel> UnitLevels { get; set; }
        public virtual DbSet<UnitMainService> UnitMainServices { get; set; }
        public virtual DbSet<UnitServiceType> UnitServiceTypes { get; set; }
        public virtual DbSet<UnitSignature> UnitSignatures { get; set; }
        public virtual DbSet<UnitsLocation> UnitsLocations { get; set; }
        public virtual DbSet<UnitsRequestType> UnitsRequestTypes { get; set; }
        public virtual DbSet<UnitsType> UnitsTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserFcmtoken> UserFcmtokens { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }
        public virtual DbSet<ValidTo> ValidTos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-CN4OLI1Q\\SQL19;Database=[FromServer]Mustafid05062022;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ApplicantType>(entity =>
            {
                entity.ToTable("Applicant_Type");

                entity.Property(e => e.ApplicantTypeId).HasColumnName("Applicant_Type_ID");

                entity.Property(e => e.ApplicantTypeNameAr)
                    .HasMaxLength(250)
                    .HasColumnName("Applicant_Type_Name_AR");

                entity.Property(e => e.ApplicantTypeNameEn)
                    .HasMaxLength(250)
                    .HasColumnName("Applicant_Type_Name_EN");

                entity.Property(e => e.Index).HasDefaultValueSql("((30))");

                entity.Property(e => e.IsAction)
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");
            });

            modelBuilder.Entity<CheckBoxType>(entity =>
            {
                entity.ToTable("CheckBox_Type");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Name_EN");

                entity.Property(e => e.QuestionId).HasColumnName("Question_ID");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.CheckBoxTypes)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_CheckBox_Type_Question");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");

                entity.Property(e => e.CityId).HasColumnName("City_ID");

                entity.Property(e => e.CityNameAr)
                    .HasMaxLength(250)
                    .HasColumnName("City_Name_AR");

                entity.Property(e => e.CityNameEn)
                    .HasMaxLength(250)
                    .HasColumnName("City_Name_EN");

                entity.Property(e => e.IsAction)
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");

                entity.Property(e => e.RegionId).HasColumnName("Region_ID");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_City_Region");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.Property(e => e.CountryId)
                    .ValueGeneratedNever()
                    .HasColumnName("Country_ID");

                entity.Property(e => e.CountryNameAr)
                    .HasMaxLength(250)
                    .HasColumnName("Country_Name_AR");

                entity.Property(e => e.CountryNameEn)
                    .HasMaxLength(250)
                    .HasColumnName("Country_Name_EN");

                entity.Property(e => e.IsAction)
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");
            });

            modelBuilder.Entity<DelayedTransaction>(entity =>
            {
                entity.ToTable("DelayedTransaction");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddedDate).HasColumnType("datetime");

                entity.Property(e => e.DelayedOnUnitId).HasColumnName("DelayedOnUnitID");

                entity.Property(e => e.RequestCode)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.RequestId).HasColumnName("RequestID");

                entity.Property(e => e.TransactionDate).HasColumnType("date");

                entity.HasOne(d => d.DelayedOnUnit)
                    .WithMany(p => p.DelayedTransactions)
                    .HasForeignKey(d => d.DelayedOnUnitId)
                    .HasConstraintName("FK_DelayedTransaction_Units");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.DelayedTransactions)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DelayedTransaction_Request_Data");

                entity.HasOne(d => d.RequestStatusNavigation)
                    .WithMany(p => p.DelayedTransactions)
                    .HasForeignKey(d => d.RequestStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DelayedTransaction_Request_State");
            });

            modelBuilder.Entity<EForm>(entity =>
            {
                entity.ToTable("E-Forms");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.IsAction)
                    .IsRequired()
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Name_EN");

                entity.Property(e => e.SubServiceId).HasColumnName("SubServiceID");

                entity.Property(e => e.UnitToApprove).HasDefaultValueSql("((19))");

                entity.HasOne(d => d.SubService)
                    .WithMany(p => p.EForms)
                    .HasForeignKey(d => d.SubServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_E-Forms_Sub_Services");

                entity.HasOne(d => d.UnitToApproveNavigation)
                    .WithMany(p => p.EForms)
                    .HasForeignKey(d => d.UnitToApprove)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_E-Forms_Units");
            });

            modelBuilder.Entity<EFormsAnswer>(entity =>
            {
                entity.ToTable("E-Forms_Answer");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EformId).HasColumnName("EForm_ID");

                entity.Property(e => e.FillDate).HasColumnType("datetime");

                entity.Property(e => e.IndexOrder).HasColumnName("Index_Order");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("Name_En");

                entity.Property(e => e.QuestionId).HasColumnName("Question_ID");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsFixedLength(true);

                entity.Property(e => e.Value).IsRequired();

                entity.Property(e => e.ValueEn)
                    .IsRequired()
                    .HasColumnName("Value_En");

                entity.HasOne(d => d.Eform)
                    .WithMany(p => p.EFormsAnswers)
                    .HasForeignKey(d => d.EformId)
                    .HasConstraintName("FK_E-Forms_Answer_Person_Eform");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.EFormsAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_E-Forms_Answer_Question");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(150)
                    .HasColumnName("Employee_Name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<IdDocument>(entity =>
            {
                entity.HasKey(e => e.IdDocument1);

                entity.ToTable("ID_Document");

                entity.Property(e => e.IdDocument1)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Document");

                entity.Property(e => e.DocumentNameAr)
                    .HasMaxLength(250)
                    .HasColumnName("Document_Name_AR");

                entity.Property(e => e.DocumentNameEn)
                    .HasMaxLength(250)
                    .HasColumnName("Document_Name_EN");

                entity.Property(e => e.IsAction)
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");
            });

            modelBuilder.Entity<InputType>(entity =>
            {
                entity.HasKey(e => e.QuestionId);

                entity.ToTable("Input_Type");

                entity.Property(e => e.QuestionId)
                    .ValueGeneratedNever()
                    .HasColumnName("Question_ID");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.IsNumber)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Placeholder)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PlaceholderEn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Placeholder_EN");

                entity.HasOne(d => d.Question)
                    .WithOne(p => p.InputType)
                    .HasForeignKey<InputType>(d => d.QuestionId)
                    .HasConstraintName("FK_Input_Type_Question");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasKey(e => e.UserPermissionsTypeId)
                    .HasName("PK_User_Permissions_Type");

                entity.ToTable("Job");

                entity.Property(e => e.UserPermissionsTypeId).HasColumnName("User_Permissions_Type_ID");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.UserPermissionsTypeNameAr)
                    .HasMaxLength(100)
                    .HasColumnName("User_Permissions_Type_Name_AR");

                entity.Property(e => e.UserPermissionsTypeNameEn)
                    .HasMaxLength(100)
                    .HasColumnName("User_Permissions_Type_Name_EN");
            });

            modelBuilder.Entity<JobPermission>(entity =>
            {
                entity.ToTable("Job_Permissions");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.JobId).HasColumnName("Job_ID");

                entity.Property(e => e.PrivilageId).HasColumnName("PrivilageID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobPermissions)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_User_Permissions_Job");

                entity.HasOne(d => d.Privilage)
                    .WithMany(p => p.JobPermissions)
                    .HasForeignKey(d => d.PrivilageId)
                    .HasConstraintName("FK_User_Permissions_Privilage");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.LocationId).HasColumnName("Location_ID");

                entity.Property(e => e.IsAction)
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");

                entity.Property(e => e.LocationNameAr)
                    .HasMaxLength(250)
                    .HasColumnName("Location_Name_AR");

                entity.Property(e => e.LocationNameEn)
                    .HasMaxLength(250)
                    .HasColumnName("Location_Name_EN");
            });

            modelBuilder.Entity<MainService>(entity =>
            {
                entity.HasKey(e => e.MainServicesId);

                entity.ToTable("Main_Services");

                entity.Property(e => e.MainServicesId).HasColumnName("Main_Services_ID");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.IsAction)
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");

                entity.Property(e => e.MainServicesNameAr).HasColumnName("Main_Services_Name_AR");

                entity.Property(e => e.MainServicesNameEn).HasColumnName("Main_Services_Name_EN");

                entity.Property(e => e.ServiceTypeId).HasColumnName("ServiceTypeID");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.MainServices)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .HasConstraintName("FK_Main_Services_Service_Type");
            });

            modelBuilder.Entity<Paragraph>(entity =>
            {
                entity.HasKey(e => e.QuestionId)
                    .HasName("PK_Paragraph_1");

                entity.ToTable("Paragraph");

                entity.Property(e => e.QuestionId)
                    .ValueGeneratedNever()
                    .HasColumnName("QuestionID");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasColumnName("Name_En");

                entity.HasOne(d => d.Question)
                    .WithOne(p => p.Paragraph)
                    .HasForeignKey<Paragraph>(d => d.QuestionId)
                    .HasConstraintName("FK_Paragraph_Question");
            });

            modelBuilder.Entity<PersonEform>(entity =>
            {
                entity.ToTable("Person_Eform");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(2)
                    .IsFixedLength(true);

                entity.Property(e => e.FillDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("Name_EN");

                entity.Property(e => e.PersonId).HasColumnName("Person_ID");

                entity.Property(e => e.RequestId).HasColumnName("RequestID");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PersonEforms)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_Person_Eform_Personel_Data");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.PersonEforms)
                    .HasForeignKey(d => d.RequestId)
                    .HasConstraintName("FK_Person_Eform_Request_Data");
            });

            modelBuilder.Entity<PersonelDatum>(entity =>
            {
                entity.HasKey(e => e.PersonelDataId);

                entity.ToTable("Personel_Data");

                entity.Property(e => e.PersonelDataId).HasColumnName("Personel_Data_ID");

                entity.Property(e => e.AddressCity)
                    .HasMaxLength(200)
                    .HasColumnName("Address_City");

                entity.Property(e => e.AddressCityId).HasColumnName("Address_CityID");

                entity.Property(e => e.AddressCountryId).HasColumnName("Address_CountryID");

                entity.Property(e => e.AdressRegion)
                    .HasMaxLength(200)
                    .HasColumnName("Adress_Region");

                entity.Property(e => e.AdressRegionId).HasColumnName("Adress_RegionID");

                entity.Property(e => e.ApplicantTypeId).HasColumnName("Applicant_Type_ID");

                entity.Property(e => e.CountryId).HasColumnName("Country_ID");

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .HasColumnName("First_Name");

                entity.Property(e => e.IauIdNumber)
                    .HasMaxLength(100)
                    .HasColumnName("IAU_ID_Number");

                entity.Property(e => e.IdDocument).HasColumnName("ID_Document");

                entity.Property(e => e.IdNumber)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("ID_Number");

                entity.Property(e => e.IsAction)
                    .HasMaxLength(20)
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .HasColumnName("Last_Name");

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(100)
                    .HasColumnName("Middle_Name");

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.NationalityId).HasColumnName("Nationality_ID");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(100)
                    .HasColumnName("Postal_Code");

                entity.Property(e => e.TitleMiddleNamesId).HasColumnName("Title_Middle_Names_ID");

                entity.HasOne(d => d.AddressCityNavigation)
                    .WithMany(p => p.PersonelData)
                    .HasForeignKey(d => d.AddressCityId)
                    .HasConstraintName("FK_Personel_Data_City");

                entity.HasOne(d => d.AddressCountry)
                    .WithMany(p => p.PersonelDatumAddressCountries)
                    .HasForeignKey(d => d.AddressCountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Personel_Data_Country2");

                entity.HasOne(d => d.AdressRegionNavigation)
                    .WithMany(p => p.PersonelData)
                    .HasForeignKey(d => d.AdressRegionId)
                    .HasConstraintName("FK_Personel_Data_Region");

                entity.HasOne(d => d.ApplicantType)
                    .WithMany(p => p.PersonelData)
                    .HasForeignKey(d => d.ApplicantTypeId)
                    .HasConstraintName("FK_Personel_Data_Applicant_Type");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.PersonelDatumCountries)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_Personel_Data_Country1");

                entity.HasOne(d => d.IdDocumentNavigation)
                    .WithMany(p => p.PersonelData)
                    .HasForeignKey(d => d.IdDocument)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Personel_Data_ID_Document");

                entity.HasOne(d => d.Nationality)
                    .WithMany(p => p.PersonelDatumNationalities)
                    .HasForeignKey(d => d.NationalityId)
                    .HasConstraintName("FK_Personel_Data_Country");

                entity.HasOne(d => d.TitleMiddleNames)
                    .WithMany(p => p.PersonelData)
                    .HasForeignKey(d => d.TitleMiddleNamesId)
                    .HasConstraintName("FK_Personel_Data_Title_Middle_Names");
            });

            modelBuilder.Entity<PhoneNumberNotification>(entity =>
            {
                entity.ToTable("PhoneNumberNotification");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.MessageEn)
                    .IsRequired()
                    .HasColumnName("Message_EN");

                entity.Property(e => e.NotiDate).HasColumnType("datetime");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RequestId).HasColumnName("RequestID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.PhoneNumberNotifications)
                    .HasForeignKey(d => d.RequestId)
                    .HasConstraintName("FK_PhoneNumberNotification_Request_Data");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PhoneNumberNotifications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_PhoneNumberNotification_Users");
            });

            modelBuilder.Entity<PreviewEformApproval>(entity =>
            {
                entity.HasKey(e => new { e.UnitId, e.PersonEform })
                    .HasName("PK_Preview_EformApproval_1");

                entity.ToTable("Preview_EformApproval");

                entity.Property(e => e.UnitId).HasColumnName("UnitID");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("Name_En");

                entity.Property(e => e.SignDate).HasColumnType("datetime");

                entity.HasOne(d => d.PersonEformNavigation)
                    .WithMany(p => p.PreviewEformApprovals)
                    .HasForeignKey(d => d.PersonEform)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Preview_EformApproval_Person_Eform");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.PreviewEformApprovals)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Preview_EformApproval_Units");
            });

            modelBuilder.Entity<PreviewTableCol>(entity =>
            {
                entity.ToTable("Preview_TableCols");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EformAnswareId).HasColumnName("EFormAnswareID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Name_En");

                entity.HasOne(d => d.EformAnsware)
                    .WithMany(p => p.PreviewTableCols)
                    .HasForeignKey(d => d.EformAnswareId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Preview_TableCols_E-Forms_Answer");
            });

            modelBuilder.Entity<Privilage>(entity =>
            {
                entity.ToTable("Privilage");

                entity.HasIndex(e => e.NameEn, "IX_Privilage")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Name_EN");

                entity.Property(e => e.SubOf).HasColumnName("SubOF");

                entity.HasOne(d => d.DetailedFromNavigation)
                    .WithMany(p => p.InverseDetailedFromNavigation)
                    .HasForeignKey(d => d.DetailedFrom)
                    .HasConstraintName("FK_Privilage_Privilage1");

                entity.HasOne(d => d.SubOfNavigation)
                    .WithMany(p => p.InverseSubOfNavigation)
                    .HasForeignKey(d => d.SubOf)
                    .HasConstraintName("FK_Privilage_Privilage");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EformId).HasColumnName("EForm_ID");

                entity.Property(e => e.IndexOrder).HasColumnName("Index_Order");

                entity.Property(e => e.LableName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.LableNameEn)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("LableName_EN");

                entity.Property(e => e.RefTo).HasMaxLength(50);

                entity.Property(e => e.Requird)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Eform)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.EformId)
                    .HasConstraintName("FK_Question_E-Forms");
            });

            modelBuilder.Entity<RadioType>(entity =>
            {
                entity.ToTable("Radio_Type");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Name_EN");

                entity.Property(e => e.QuestionId).HasColumnName("Question_ID");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.RadioTypes)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_Radio_Type_Question");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("Region");

                entity.HasIndex(e => e.CountryId, "IX_Region");

                entity.Property(e => e.RegionId)
                    .ValueGeneratedNever()
                    .HasColumnName("Region_ID");

                entity.Property(e => e.CountryId).HasColumnName("Country_ID");

                entity.Property(e => e.IsAction)
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");

                entity.Property(e => e.RegionNameAr)
                    .HasMaxLength(250)
                    .HasColumnName("Region_Name_AR");

                entity.Property(e => e.RegionNameEn)
                    .HasMaxLength(250)
                    .HasColumnName("Region_Name_EN");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Regions)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Region_Country");
            });

            modelBuilder.Entity<RequestDatum>(entity =>
            {
                entity.HasKey(e => e.RequestDataId);

                entity.ToTable("Request_Data");

                entity.Property(e => e.RequestDataId).HasColumnName("Request_Data_ID");

                entity.Property(e => e.CodeGenerate)
                    .HasMaxLength(13)
                    .HasColumnName("Code_Generate");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.GenratedDate).HasColumnType("datetime");

                entity.Property(e => e.IsArchived).HasColumnName("Is_Archived");

                entity.Property(e => e.IsTwasulOc).HasColumnName("IsTwasul_OC");

                entity.Property(e => e.PersonelDataId).HasColumnName("Personel_Data_ID");

                entity.Property(e => e.ReadedDate).HasColumnType("datetime");

                entity.Property(e => e.RequestStateId).HasColumnName("Request_State_ID");

                entity.Property(e => e.RequestTypeId).HasColumnName("Request_Type_ID");

                entity.Property(e => e.RequiredFieldsNotes).HasColumnName("Required_Fields_Notes");

                entity.Property(e => e.ServiceTypeId).HasColumnName("Service_Type_ID");

                entity.Property(e => e.SubServicesId).HasColumnName("Sub_Services_ID");

                entity.Property(e => e.TempCode).HasMaxLength(13);

                entity.Property(e => e.UnitId).HasColumnName("Unit_ID");

                entity.HasOne(d => d.PersonelData)
                    .WithMany(p => p.RequestData)
                    .HasForeignKey(d => d.PersonelDataId)
                    .HasConstraintName("FK_Request_Data_Personel_Data");

                entity.HasOne(d => d.RequestState)
                    .WithMany(p => p.RequestData)
                    .HasForeignKey(d => d.RequestStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Request_Data_Request_State");

                entity.HasOne(d => d.RequestType)
                    .WithMany(p => p.RequestData)
                    .HasForeignKey(d => d.RequestTypeId)
                    .HasConstraintName("FK_Request_Data_Request_Type");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.RequestData)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .HasConstraintName("FK_Request_Data_Service_Type");

                entity.HasOne(d => d.SubServices)
                    .WithMany(p => p.RequestData)
                    .HasForeignKey(d => d.SubServicesId)
                    .HasConstraintName("FK_Request_Data_Sub_Services");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.RequestData)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_Request_Data_Units");
            });

            modelBuilder.Entity<RequestFile>(entity =>
            {
                entity.ToTable("Request_File");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("File_Name");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasColumnName("File_Path");

                entity.Property(e => e.RequestId).HasColumnName("Request_ID");

                entity.Property(e => e.RequiredDocId).HasColumnName("RequiredDoc_ID");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.RequestFiles)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Request_File_Request_Data");

                entity.HasOne(d => d.RequiredDoc)
                    .WithMany(p => p.RequestFiles)
                    .HasForeignKey(d => d.RequiredDocId)
                    .HasConstraintName("FK_Request_File_Required_Documents");
            });

            modelBuilder.Entity<RequestLog>(entity =>
            {
                entity.ToTable("Request_Log");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("Employee_ID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RequestId).HasColumnName("Request_ID");

                entity.Property(e => e.RequestStateId).HasColumnName("Request_State_ID");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.RequestLogs)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Request_Log_Employee");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.RequestLogs)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Request_State_Request_Data");

                entity.HasOne(d => d.RequestState)
                    .WithMany(p => p.RequestLogs)
                    .HasForeignKey(d => d.RequestStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Request_Log_Request_State");
            });

            modelBuilder.Entity<RequestState>(entity =>
            {
                entity.HasKey(e => e.StateId)
                    .HasName("PK_Request_State1");

                entity.ToTable("Request_State");

                entity.Property(e => e.StateId).HasColumnName("State_ID");

                entity.Property(e => e.StateNameAr)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("StateName_AR");

                entity.Property(e => e.StateNameEn)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("StateName_EN");
            });

            modelBuilder.Entity<RequestTransaction>(entity =>
            {
                entity.ToTable("RequestTransaction");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code).HasMaxLength(13);

                entity.Property(e => e.CommentDate).HasColumnType("datetime");

                entity.Property(e => e.ExpireDays).HasColumnType("datetime");

                entity.Property(e => e.ForwardDate).HasColumnType("datetime");

                entity.Property(e => e.FromUnitId).HasColumnName("FromUnitID");

                entity.Property(e => e.IsReminder).HasColumnName("Is_Reminder");

                entity.Property(e => e.ReadedDate).HasColumnType("datetime");

                entity.Property(e => e.RequestId).HasColumnName("Request_ID");

                entity.Property(e => e.ToUnitId).HasColumnName("ToUnitID");

                entity.HasOne(d => d.FromUnit)
                    .WithMany(p => p.RequestTransactionFromUnits)
                    .HasForeignKey(d => d.FromUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestTransaction_Units");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.RequestTransactions)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestTransaction_Request_Data");

                entity.HasOne(d => d.ToUnit)
                    .WithMany(p => p.RequestTransactionToUnits)
                    .HasForeignKey(d => d.ToUnitId)
                    .HasConstraintName("FK_RequestTransaction_Units1");
            });

            modelBuilder.Entity<RequestType>(entity =>
            {
                entity.ToTable("Request_Type");

                entity.Property(e => e.RequestTypeId).HasColumnName("Request_Type_ID");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.DescAr).HasColumnName("Desc_AR");

                entity.Property(e => e.DescEn).HasColumnName("Desc_EN");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(250)
                    .HasColumnName("Image_Path");

                entity.Property(e => e.IsAction)
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");

                entity.Property(e => e.RequestTypeNameAr)
                    .HasMaxLength(250)
                    .HasColumnName("Request_Type_Name_AR");

                entity.Property(e => e.RequestTypeNameEn)
                    .HasMaxLength(250)
                    .HasColumnName("Request_Type_Name_EN");
            });

            modelBuilder.Entity<RequiredDocument>(entity =>
            {
                entity.ToTable("Required_Documents");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeletetAt).HasColumnType("datetime");

                entity.Property(e => e.IsAction)
                    .IsRequired()
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");

                entity.Property(e => e.NameAr).HasColumnName("Name_AR");

                entity.Property(e => e.NameEn).HasColumnName("Name_EN");

                entity.Property(e => e.SubServiceId).HasColumnName("SubServiceID");

                entity.HasOne(d => d.SubService)
                    .WithMany(p => p.RequiredDocuments)
                    .HasForeignKey(d => d.SubServiceId)
                    .HasConstraintName("FK_Required_Documents_Sub_Services");
            });

            modelBuilder.Entity<Separator>(entity =>
            {
                entity.HasKey(e => e.QuestionId)
                    .HasName("PK_Separator_1");

                entity.ToTable("Separator");

                entity.Property(e => e.QuestionId)
                    .ValueGeneratedNever()
                    .HasColumnName("QuestionID");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.HasOne(d => d.Question)
                    .WithOne(p => p.Separator)
                    .HasForeignKey<Separator>(d => d.QuestionId)
                    .HasConstraintName("FK_Separator_Question");
            });

            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.ToTable("Service_Type");

                entity.Property(e => e.ServiceTypeId).HasColumnName("Service_Type_ID");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.DescAr).HasColumnName("Desc_AR");

                entity.Property(e => e.DescEn).HasColumnName("Desc_EN");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(250)
                    .HasColumnName("Image_Path");

                entity.Property(e => e.IsAction)
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");

                entity.Property(e => e.ServiceTypeNameAr)
                    .HasMaxLength(250)
                    .HasColumnName("Service_Type_Name_AR");

                entity.Property(e => e.ServiceTypeNameEn)
                    .HasMaxLength(250)
                    .HasColumnName("Service_Type_Name_EN");
            });

            modelBuilder.Entity<SubService>(entity =>
            {
                entity.HasKey(e => e.SubServicesId);

                entity.ToTable("Sub_Services");

                entity.Property(e => e.SubServicesId).HasColumnName("Sub_Services_ID");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.IsAction)
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");

                entity.Property(e => e.MainServicesId).HasColumnName("Main_Services_ID");

                entity.Property(e => e.SubServicesNameAr).HasColumnName("Sub_Services_Name_AR");

                entity.Property(e => e.SubServicesNameEn).HasColumnName("Sub_Services_Name_EN");

                entity.HasOne(d => d.MainServices)
                    .WithMany(p => p.SubServices)
                    .HasForeignKey(d => d.MainServicesId)
                    .HasConstraintName("FK_Sub_Services_Main_Services");
            });

            modelBuilder.Entity<SystemLog>(entity =>
            {
                entity.ToTable("SystemLog");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CallPath).IsRequired();

                entity.Property(e => e.Method)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ReferId).HasColumnName("ReferID");

                entity.Property(e => e.TransDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SystemLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SystemLog_Users");
            });

            modelBuilder.Entity<TableColumn>(entity =>
            {
                entity.ToTable("Table_Columns");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Name_En");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.TableColumns)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_Table_Columns_Question");
            });

            modelBuilder.Entity<TablesAnsware>(entity =>
            {
                entity.HasKey(e => new { e.Row, e.Column });

                entity.ToTable("Tables_Answare");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Value).IsRequired();

                entity.HasOne(d => d.ColumnNavigation)
                    .WithMany(p => p.TablesAnswares)
                    .HasForeignKey(d => d.Column)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tables_Answare_Preview_TableCols");
            });

            modelBuilder.Entity<TitleMiddleName>(entity =>
            {
                entity.HasKey(e => e.TitleMiddleNamesId);

                entity.ToTable("Title_Middle_Names");

                entity.Property(e => e.TitleMiddleNamesId)
                    .ValueGeneratedNever()
                    .HasColumnName("Title_Middle_Names_ID");

                entity.Property(e => e.IsAction)
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");

                entity.Property(e => e.TitleMiddleNamesNameAr)
                    .HasMaxLength(250)
                    .HasColumnName("Title_Middle_Names_Name_AR");

                entity.Property(e => e.TitleMiddleNamesNameEn)
                    .HasMaxLength(250)
                    .HasColumnName("Title_Middle_Names_Name_EN");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(e => e.UnitsId);

                entity.Property(e => e.UnitsId).HasColumnName("Units_ID");

                entity.Property(e => e.BuildingNumber)
                    .HasMaxLength(3)
                    .HasColumnName("Building_Number");

                entity.Property(e => e.Code).HasMaxLength(2);

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.IsAction)
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");

                entity.Property(e => e.IsMostafid)
                    .IsRequired()
                    .HasColumnName("IS_Mostafid")
                    .HasDefaultValueSql("(N'False')");

                entity.Property(e => e.LevelId).HasColumnName("LevelID");

                entity.Property(e => e.RefNumber)
                    .HasMaxLength(100)
                    .HasColumnName("Ref_Number");

                entity.Property(e => e.ServiceTypeId).HasColumnName("ServiceTypeID");

                entity.Property(e => e.SubId).HasColumnName("SubID");

                entity.Property(e => e.UnitsLocationId)
                    .HasColumnName("Units_Location_ID")
                    .HasComment("كود موقع الوحدة");

                entity.Property(e => e.UnitsNameAr)
                    .HasMaxLength(250)
                    .HasColumnName("Units_Name_AR")
                    .HasComment("أسم الوحدة");

                entity.Property(e => e.UnitsNameEn)
                    .HasMaxLength(250)
                    .HasColumnName("Units_Name_EN")
                    .HasComment("أسم الوحدة");

                entity.Property(e => e.UnitsTypeId)
                    .HasColumnName("Units_Type_ID")
                    .HasComment("كود نوع الوحدة");

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.Units)
                    .HasForeignKey(d => d.LevelId)
                    .HasConstraintName("FK_Units_UnitLevel");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.Units)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .HasConstraintName("FK_Units_Service_Type");

                entity.HasOne(d => d.Sub)
                    .WithMany(p => p.InverseSub)
                    .HasForeignKey(d => d.SubId)
                    .HasConstraintName("FK_Units_Units");

                entity.HasOne(d => d.UnitsLocation)
                    .WithMany(p => p.Units)
                    .HasForeignKey(d => d.UnitsLocationId)
                    .HasConstraintName("FK_Units_Units_Location");

                entity.HasOne(d => d.UnitsType)
                    .WithMany(p => p.Units)
                    .HasForeignKey(d => d.UnitsTypeId)
                    .HasConstraintName("FK_Units_Units_Type");
            });

            modelBuilder.Entity<UnitLevel>(entity =>
            {
                entity.ToTable("UnitLevel");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.NameAr)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Name_AR");

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Name_EN");
            });

            modelBuilder.Entity<UnitMainService>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MainServiceId).HasColumnName("MainServiceID");

                entity.Property(e => e.UnitId).HasColumnName("UnitID");

                entity.HasOne(d => d.MainService)
                    .WithMany(p => p.UnitMainServices)
                    .HasForeignKey(d => d.MainServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnitMainServices_Main_Services");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.UnitMainServices)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnitMainServices_Units");
            });

            modelBuilder.Entity<UnitServiceType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.ServiceTypeId).HasColumnName("ServiceTypeID");

                entity.Property(e => e.UnitId).HasColumnName("UnitID");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.UnitServiceTypes)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnitServiceTypes_Service_Type");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.UnitServiceTypes)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnitServiceTypes_Units");
            });

            modelBuilder.Entity<UnitSignature>(entity =>
            {
                entity.HasKey(e => e.UnitId)
                    .HasName("PK_Unit_Signature_1");

                entity.ToTable("Unit_Signature");

                entity.Property(e => e.UnitId)
                    .ValueGeneratedNever()
                    .HasColumnName("UnitID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Unit)
                    .WithOne(p => p.UnitSignature)
                    .HasForeignKey<UnitSignature>(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Unit_Signature_Units");
            });

            modelBuilder.Entity<UnitsLocation>(entity =>
            {
                entity.ToTable("Units_Location");

                entity.Property(e => e.UnitsLocationId).HasColumnName("Units_Location_ID");

                entity.Property(e => e.Code).HasMaxLength(2);

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.IsAction)
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");

                entity.Property(e => e.LocationId).HasColumnName("Location_ID");

                entity.Property(e => e.UnitsLocationNameAr)
                    .HasMaxLength(250)
                    .HasColumnName("Units_Location_Name_AR");

                entity.Property(e => e.UnitsLocationNameEn)
                    .HasMaxLength(250)
                    .HasColumnName("Units_Location_Name_EN");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.UnitsLocations)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_Units_Location_Location");
            });

            modelBuilder.Entity<UnitsRequestType>(entity =>
            {
                entity.ToTable("Units_Request_Type");

                entity.Property(e => e.UnitsRequestTypeId).HasColumnName("Units_Request_Type_ID");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.RequestTypeId).HasColumnName("Request_Type_ID");

                entity.Property(e => e.UnitsId).HasColumnName("Units_ID");

                entity.HasOne(d => d.RequestType)
                    .WithMany(p => p.UnitsRequestTypes)
                    .HasForeignKey(d => d.RequestTypeId)
                    .HasConstraintName("FK_Units_Request_Type_Request_Type");

                entity.HasOne(d => d.Units)
                    .WithMany(p => p.UnitsRequestTypes)
                    .HasForeignKey(d => d.UnitsId)
                    .HasConstraintName("FK_Units_Request_Type_Units");
            });

            modelBuilder.Entity<UnitsType>(entity =>
            {
                entity.ToTable("Units_Type");

                entity.Property(e => e.UnitsTypeId).HasColumnName("Units_Type_ID");

                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.IsAction)
                    .HasColumnName("IS_Action")
                    .HasDefaultValueSql("(N'True')");

                entity.Property(e => e.LevelId).HasColumnName("LevelID");

                entity.Property(e => e.UnitsTypeNameAr)
                    .HasMaxLength(250)
                    .HasColumnName("Units_Type_Name_AR");

                entity.Property(e => e.UnitsTypeNameEn)
                    .HasMaxLength(250)
                    .HasColumnName("Units_Type_Name_EN");

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.UnitsTypes)
                    .HasForeignKey(d => d.LevelId)
                    .HasConstraintName("FK_Units_Type_UnitLevel");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .HasMaxLength(1)
                    .HasColumnName("IS_Active")
                    .HasDefaultValueSql("(N'1')")
                    .IsFixedLength(true);

                entity.Property(e => e.JobId).HasColumnName("Job_ID");

                entity.Property(e => e.LoginDate).HasColumnType("datetime");

                entity.Property(e => e.TempLogin)
                    .HasMaxLength(300)
                    .HasColumnName("TEMP_Login");

                entity.Property(e => e.UnitId).HasColumnName("UnitID");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(100)
                    .HasColumnName("User_Email");

                entity.Property(e => e.UserMobile)
                    .HasMaxLength(20)
                    .HasColumnName("User_Mobile");

                entity.Property(e => e.UserName)
                    .HasMaxLength(150)
                    .HasColumnName("User_Name");

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(20)
                    .HasColumnName("User_Password");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_Users_User_Permissions_Type");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_Users_Units");
            });

            modelBuilder.Entity<UserFcmtoken>(entity =>
            {
                entity.ToTable("UserFCMToken");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Fcmtoken)
                    .IsRequired()
                    .HasColumnName("FCMToken");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.ToTable("UserToken");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddedDate).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RefToken)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Token).IsRequired();
            });

            modelBuilder.Entity<ValidTo>(entity =>
            {
                entity.ToTable("ValidTo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApplicantTypeId).HasColumnName("ApplicantTypeID");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.MainServiceId).HasColumnName("MainServiceID");

                entity.HasOne(d => d.ApplicantType)
                    .WithMany(p => p.ValidTos)
                    .HasForeignKey(d => d.ApplicantTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ValidTo_Applicant_Type");

                entity.HasOne(d => d.MainService)
                    .WithMany(p => p.ValidTos)
                    .HasForeignKey(d => d.MainServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ValidTo_Main_Services");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
