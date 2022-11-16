using GoStay.DataAccess.Base;
using GoStay.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoStay.DataAccess.DBContext
{
    public partial class CommonDBContext : PDataContext
	{
		public CommonDBContext(DbContextOptions options) : base(options)
		{
		}

        public virtual DbSet<Acctype> Acctypes { get; set; } = null!;
        public virtual DbSet<Album> Albums { get; set; } = null!;
        public virtual DbSet<AspModule> AspModules { get; set; } = null!;
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetRoleModule> AspNetRoleModules { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<DmKenhTin> DmKenhTins { get; set; } = null!;
        public virtual DbSet<DmMenuChinh> DmMenuChinhs { get; set; } = null!;
        public virtual DbSet<DmTinTuc> DmTinTucs { get; set; } = null!;
        public virtual DbSet<GroupPicture> GroupPictures { get; set; } = null!;
        public virtual DbSet<Hotel> Hotels { get; set; } = null!;
        public virtual DbSet<HotelMameniti> HotelMamenitis { get; set; } = null!;
        public virtual DbSet<HotelOrder> HotelOrders { get; set; } = null!;
        public virtual DbSet<HotelPromotion> HotelPromotions { get; set; } = null!;
        public virtual DbSet<HotelRating> HotelRatings { get; set; } = null!;
        public virtual DbSet<HotelReview> HotelReviews { get; set; } = null!;
        public virtual DbSet<HotelRoom> HotelRooms { get; set; } = null!;
        public virtual DbSet<HotelRoomComment> HotelRoomComments { get; set; } = null!;
        public virtual DbSet<MulltiKeyValue> MulltiKeyValues { get; set; } = null!;
        public virtual DbSet<Picture> Pictures { get; set; } = null!;
        public virtual DbSet<PriceRange> PriceRanges { get; set; } = null!;
        public virtual DbSet<Quiz> Quizzes { get; set; } = null!;
        public virtual DbSet<RoomMameniti> RoomMamenitis { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<TbKv> TbKvs { get; set; } = null!;
        public virtual DbSet<TbQuan> TbQuans { get; set; } = null!;
        public virtual DbSet<TbTintuc> TbTintucs { get; set; } = null!;
        public virtual DbSet<TblPage> TblPages { get; set; } = null!;
        public virtual DbSet<TblTab> TblTabs { get; set; } = null!;
        public virtual DbSet<Tblbuaan> Tblbuaans { get; set; } = null!;
        public virtual DbSet<Tblcaption> Tblcaptions { get; set; } = null!;
        public virtual DbSet<Tblfaq> Tblfaqs { get; set; } = null!;
        public virtual DbSet<Tblparentmenu> Tblparentmenus { get; set; } = null!;
        public virtual DbSet<Tblservicesforcontent> Tblservicesforcontents { get; set; } = null!;
        public virtual DbSet<Tblservicesforsearch> Tblservicesforsearches { get; set; } = null!;
        public virtual DbSet<Tblstyle> Tblstyles { get; set; } = null!;
        public virtual DbSet<Tbltigium> Tbltigia { get; set; } = null!;
        public virtual DbSet<Tbluser> Tblusers { get; set; } = null!;
        public virtual DbSet<TypeHotel> TypeHotels { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<VGetAllHotel> VGetAllHotels { get; set; } = null!;
        public virtual DbSet<ViewDirection> ViewDirections { get; set; } = null!;
        public virtual DbSet<Viewhotelservice> Viewhotelservices { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acctype>(entity =>
            {
                entity.ToTable("Acctype");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Acctype1)
                    .HasMaxLength(50)
                    .HasColumnName("ACCTYPE");

                entity.Property(e => e.Stt)
                    .HasColumnName("STT")
                    .HasDefaultValueSql("((5))");
            });

            modelBuilder.Entity<Album>(entity =>
            {
                entity.ToTable("Album");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<AspModule>(entity =>
            {
                entity.ToTable("AspModule");

                entity.Property(e => e.IsEf).HasColumnName("isEF");

                entity.Property(e => e.Link).HasMaxLength(250);

                entity.Property(e => e.Logo).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.MaQuyen)
                    .HasMaxLength(50)
                    .HasColumnName("ma_quyen");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);

                entity.Property(e => e.Stt).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoleModule>(entity =>
            {
                entity.Property(e => e.MenuId)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__AspNetUs__3214EC0629572725");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.AnhCmt)
                    .HasMaxLength(50)
                    .HasColumnName("anh_cmt");

                entity.Property(e => e.AnhDaiDien)
                    .HasMaxLength(250)
                    .HasColumnName("anh_dai_dien");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.HoDem)
                    .HasMaxLength(50)
                    .HasColumnName("ho_dem");

                entity.Property(e => e.HoKhauTt)
                    .HasMaxLength(50)
                    .HasColumnName("ho_khau_tt");

                entity.Property(e => e.KhoaTaiKhoan).HasColumnName("khoa_tai_khoan");

                entity.Property(e => e.MatKhau)
                    .HasMaxLength(250)
                    .HasColumnName("mat_khau");

                entity.Property(e => e.NgaySinh)
                    .HasColumnType("datetime")
                    .HasColumnName("ngay_sinh");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.Stt).HasColumnName("stt");

                entity.Property(e => e.Ten)
                    .HasMaxLength(50)
                    .HasColumnName("ten");

                entity.Property(e => e.TinhTrangHonNhan)
                    .HasMaxLength(50)
                    .HasColumnName("tinh_trang_hon_nhan");

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).HasMaxLength(450);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(d => d.Role)
                    .WithMany()
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("courses");

                entity.Property(e => e.CourseCategoryId).HasColumnName("course_category_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Guid).HasColumnName("guid");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .HasMaxLength(300)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<DmKenhTin>(entity =>
            {
                entity.HasKey(e => e.MaKenhTin)
                    .HasName("PK_dm_kenh_tien");

                entity.ToTable("dm_kenh_tin");

                entity.Property(e => e.MaKenhTin)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ma_kenh_tin");

                entity.Property(e => e.CreatedDateUtc).HasColumnType("datetime");

                entity.Property(e => e.IsHienThi).HasColumnName("is_hien_thi");

                entity.Property(e => e.TenDanhMuc)
                    .HasMaxLength(500)
                    .HasColumnName("ten_danh_muc");

                entity.Property(e => e.UidName).HasMaxLength(50);

                entity.Property(e => e.UpdatedDateUtc).HasColumnType("datetime");
            });

            modelBuilder.Entity<DmMenuChinh>(entity =>
            {
                entity.HasKey(e => e.MaTrang);

                entity.ToTable("dm_menu_chinh");

                entity.Property(e => e.MaTrang)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ma_trang");

                entity.Property(e => e.CreatedDateUtc).HasColumnType("datetime");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.MaTrangCha)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ma_trang_cha");

                entity.Property(e => e.MoTa)
                    .HasMaxLength(200)
                    .HasColumnName("mo_ta");

                entity.Property(e => e.Stt).HasColumnName("stt");

                entity.Property(e => e.TenTrang)
                    .HasMaxLength(100)
                    .HasColumnName("ten_trang");

                entity.Property(e => e.UidName).HasMaxLength(50);

                entity.Property(e => e.UpdatedDateUtc).HasColumnType("datetime");

                entity.Property(e => e.Url)
                    .HasMaxLength(50)
                    .HasColumnName("url");
            });

            modelBuilder.Entity<DmTinTuc>(entity =>
            {
                entity.ToTable("dm_tin_tuc");

                entity.Property(e => e.AnhDaiDien)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("anh_dai_dien");

                entity.Property(e => e.CreatedDateUtc).HasColumnType("datetime");

                entity.Property(e => e.IsHienThi).HasColumnName("is_hien_thi");

                entity.Property(e => e.MaDmKenhTin)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ma_dm_kenh_tin");

                entity.Property(e => e.NoiDung)
                    .HasMaxLength(500)
                    .HasColumnName("noi_dung");

                entity.Property(e => e.NoiDungChiTiet).HasColumnName("noi_dung_chi_tiet");

                entity.Property(e => e.TieuDe)
                    .HasMaxLength(500)
                    .HasColumnName("tieu_de");

                entity.Property(e => e.TieuDeSeo)
                    .HasMaxLength(50)
                    .HasColumnName("tieu_de_seo");

                entity.Property(e => e.UidName).HasMaxLength(50);

                entity.Property(e => e.UpdatedDateUtc).HasColumnType("datetime");
            });

            modelBuilder.Entity<GroupPicture>(entity =>
            {
                entity.ToTable("GroupPicture");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("Hotel");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.AvgNight).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CleanlinessScore)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("Cleanliness_score");

                entity.Property(e => e.CodeCountry)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("Code_country");

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.Description).HasMaxLength(3000);

                entity.Property(e => e.LatMap).HasColumnName("Lat_map");

                entity.Property(e => e.LocationScore)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("Location_score");

                entity.Property(e => e.LonMap).HasColumnName("Lon_map");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NameSeo)
                    .HasMaxLength(50)
                    .HasColumnName("Name_seo");

                entity.Property(e => e.Rating).HasDefaultValueSql("((1))");

                entity.Property(e => e.ReviewScore).HasColumnName("Review_score");

                entity.Property(e => e.RoomsScore)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("Rooms_score");

                entity.Property(e => e.ServiceScore)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("Service_score");

                entity.Property(e => e.SleepQualityScore)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("SleepQuality_score");

                entity.Property(e => e.Type).HasDefaultValueSql("((1))");

                entity.Property(e => e.ValueScore)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("Value_score");

                entity.HasOne(d => d.IdPriceRangeNavigation)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.IdPriceRange)
                    .HasConstraintName("FK_Hotel_PRICE_RANGE");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("FK_Hotel_TypeHotel");
            });

            modelBuilder.Entity<HotelMameniti>(entity =>
            {
                entity.ToTable("HotelMameniti");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idhotel).HasColumnName("IDHOTEL");

                entity.Property(e => e.Idservices).HasColumnName("IDSERVICES");

                entity.HasOne(d => d.IdhotelNavigation)
                    .WithMany(p => p.HotelMamenitis)
                    .HasForeignKey(d => d.Idhotel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TBLHOTELMAMENITI_TBLHOTEL");

                entity.HasOne(d => d.IdservicesNavigation)
                    .WithMany(p => p.HotelMamenitis)
                    .HasForeignKey(d => d.Idservices)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelMameniti_Services");
            });

            modelBuilder.Entity<HotelOrder>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("HotelOrder");

                entity.Property(e => e.CreatedDateUtc).HasColumnType("datetime");

                entity.Property(e => e.DateOrder).HasColumnType("datetime");

                entity.Property(e => e.NumberRoomNd).HasColumnName("Number_Room_ND");

                entity.Property(e => e.TotalMoney)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Total_Money");

                entity.Property(e => e.UpdatedDateUtc).HasColumnType("datetime");
            });

            modelBuilder.Entity<HotelPromotion>(entity =>
            {
                entity.ToTable("HotelPromotion");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDateUtc).HasColumnType("datetime");

                entity.Property(e => e.NumberRoomNd).HasColumnName("Number_Room_ND");

                entity.Property(e => e.UpdatedDateUtc).HasColumnType("datetime");
            });

            modelBuilder.Entity<HotelRating>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.IdHotel })
                    .HasName("PK_TBLRATING");

                entity.ToTable("HotelRating");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedDateUtc).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Point).HasColumnType("decimal(3, 1)");

                entity.Property(e => e.UpdatedDateUtc).HasColumnType("datetime");
            });

            modelBuilder.Entity<HotelReview>(entity =>
            {
                entity.ToTable("HotelReview");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cleanliness).HasColumnType("decimal(2, 1)");

                entity.Property(e => e.Datego)
                    .HasColumnType("datetime")
                    .HasColumnName("DATEGO");

                entity.Property(e => e.Datepost)
                    .HasColumnType("datetime")
                    .HasColumnName("DATEPOST");

                entity.Property(e => e.Fitnessfacility)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("fitnessfacility");

                entity.Property(e => e.Idhotel).HasColumnName("IDHOTEL");

                entity.Property(e => e.Idtrip).HasColumnName("IDTRIP");

                entity.Property(e => e.Iduser).HasColumnName("IDUSER");

                entity.Property(e => e.Intmode)
                    .HasColumnName("INTMODE")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Location)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("location");

                entity.Property(e => e.Review)
                    .HasMaxLength(200)
                    .HasColumnName("REVIEW");

                entity.Property(e => e.Rooms)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("rooms");

                entity.Property(e => e.Service)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("SERVICE");

                entity.Property(e => e.SleepQuality).HasColumnType("decimal(2, 1)");

                entity.Property(e => e.Swimmingpool)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("swimmingpool");

                entity.Property(e => e.Tipgood)
                    .HasMaxLength(200)
                    .HasColumnName("TIPGOOD");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("TITLE");

                entity.Property(e => e.Value).HasColumnType("decimal(2, 1)");

                entity.HasOne(d => d.IdhotelNavigation)
                    .WithMany(p => p.HotelReviews)
                    .HasForeignKey(d => d.Idhotel)
                    .HasConstraintName("FK_TBLREVIEW_TBLHOTEL");
            });

            modelBuilder.Entity<HotelRoom>(entity =>
            {
                entity.ToTable("HotelRoom");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CheckInDate).HasColumnType("datetime");

                entity.Property(e => e.CheckOutDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Idhotel).HasColumnName("IDHOTEL");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PriceValue)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Price_Value");

                entity.Property(e => e.RoomSize).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.IdhotelNavigation)
                    .WithMany(p => p.HotelRooms)
                    .HasForeignKey(d => d.Idhotel)
                    .HasConstraintName("FK_TBLROOM_TBLHOTEL");

                entity.HasOne(d => d.ViewDirectionNavigation)
                    .WithMany(p => p.HotelRooms)
                    .HasForeignKey(d => d.ViewDirection)
                    .HasConstraintName("FK_HotelRoom_HotelRoom");
            });

            modelBuilder.Entity<HotelRoomComment>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.IdRoom, e.IdHotel, e.IdUser });

                entity.ToTable("HotelRoomComment");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDateUtc).HasColumnType("datetime");

                entity.Property(e => e.NoiDungCmt)
                    .HasMaxLength(500)
                    .HasColumnName("noi_dung_cmt");

                entity.Property(e => e.ParentId).HasColumnName("parentId");

                entity.Property(e => e.TieuDe)
                    .HasMaxLength(10)
                    .HasColumnName("tieu_de")
                    .IsFixedLength();

                entity.Property(e => e.UpdatedDateUtc).HasColumnType("datetime");
            });

            modelBuilder.Entity<MulltiKeyValue>(entity =>
            {
                entity.ToTable("MulltiKeyValue");

                entity.Property(e => e.Icon)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TextValue).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<Picture>(entity =>
            {
                entity.Property(e => e.Datein)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Url)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PriceRange>(entity =>
            {
                entity.ToTable("PRICE_RANGE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Max)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("MAX");

                entity.Property(e => e.Min)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("MIN");

                entity.Property(e => e.Stt).HasColumnName("STT");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.Property(e => e.TitleVnd)
                    .HasMaxLength(50)
                    .HasColumnName("TITLE_VND");
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("quiz");

                entity.Property(e => e.AllowedTimeMinutes).HasColumnName("allowed_time_minutes");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date");

                entity.Property(e => e.Guid).HasColumnName("guid");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.PassingMarks).HasColumnName("passing_marks");

                entity.Property(e => e.QuizId).HasColumnName("quiz_id");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .HasMaxLength(300)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<RoomMameniti>(entity =>
            {
                entity.ToTable("RoomMameniti");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idroom).HasColumnName("IDROOM");

                entity.Property(e => e.Idservices).HasColumnName("IDSERVICES");

                entity.HasOne(d => d.IdroomNavigation)
                    .WithMany(p => p.RoomMamenitis)
                    .HasForeignKey(d => d.Idroom)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoomMameniti_HotelRoom");

                entity.HasOne(d => d.IdservicesNavigation)
                    .WithMany(p => p.RoomMamenitis)
                    .HasForeignKey(d => d.Idservices)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoomMameniti_Services");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.Property(e => e.Icon)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("status");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Guid).HasColumnName("guid");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("students");

                entity.Property(e => e.Address)
                    .HasMaxLength(600)
                    .HasColumnName("address");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(70)
                    .HasColumnName("first_name");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.LastName)
                    .HasMaxLength(70)
                    .HasColumnName("last_name");

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(150)
                    .HasColumnName("mobile_no");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.Nationality)
                    .HasMaxLength(150)
                    .HasColumnName("nationality");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .HasColumnName("password");

                entity.Property(e => e.ResidenceNo)
                    .HasMaxLength(150)
                    .HasColumnName("residence_no");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.UserName)
                    .HasMaxLength(150)
                    .HasColumnName("user_name");
            });

            modelBuilder.Entity<TbKv>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TB_KV");

                entity.Property(e => e.Diengiai)
                    .HasMaxLength(500)
                    .HasColumnName("DIENGIAI");

                entity.Property(e => e.IdKv).HasColumnName("ID_KV");

                entity.Property(e => e.IdQ).HasColumnName("ID_Q");

                entity.Property(e => e.Stt).HasColumnName("STT");

                entity.Property(e => e.Tenkv)
                    .HasMaxLength(50)
                    .HasColumnName("TENKV");
            });

            modelBuilder.Entity<TbQuan>(entity =>
            {
                entity.HasKey(e => e.IdQ);

                entity.ToTable("TB_QUAN");

                entity.Property(e => e.IdQ)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Q");

                entity.Property(e => e.Diengiai)
                    .HasMaxLength(50)
                    .HasColumnName("DIENGIAI");

                entity.Property(e => e.IdTt).HasColumnName("ID_TT");

                entity.Property(e => e.Stt).HasColumnName("stt");

                entity.Property(e => e.Tenquan)
                    .HasMaxLength(50)
                    .HasColumnName("TENQUAN");
            });

            modelBuilder.Entity<TbTintuc>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TB_TINTUC");

                entity.Property(e => e.Content)
                    .HasColumnType("ntext")
                    .HasColumnName("content");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("DATE");

                entity.Property(e => e.IdNt).HasColumnName("ID_NT");

                entity.Property(e => e.IdTin).HasColumnName("ID_TIN");

                entity.Property(e => e.IdU).HasColumnName("ID_U");

                entity.Property(e => e.Numclick).HasColumnName("numclick");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("TITLE");

                entity.Property(e => e.Tomtat)
                    .HasColumnType("ntext")
                    .HasColumnName("TOMTAT");
            });

            modelBuilder.Entity<TblPage>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblPage");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Idtab).HasColumnName("IDTAB");

                entity.Property(e => e.PageName).HasMaxLength(50);

                entity.Property(e => e.Stt).HasColumnName("STT");
            });

            modelBuilder.Entity<TblTab>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblTab");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TenTab).HasMaxLength(50);
            });

            modelBuilder.Entity<Tblbuaan>(entity =>
            {
                entity.ToTable("TBLBUAAN");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Buaan)
                    .HasMaxLength(200)
                    .HasColumnName("BUAAN");
            });

            modelBuilder.Entity<Tblcaption>(entity =>
            {
                entity.ToTable("TBLCAPTION");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cap)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CAP");

                entity.Property(e => e.Idcap).HasColumnName("IDCAP");

                entity.Property(e => e.Idpage).HasColumnName("IDPAGE");

                entity.Property(e => e.Lang)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LANG");
            });

            modelBuilder.Entity<Tblfaq>(entity =>
            {
                entity.ToTable("TBLFAQ");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Content)
                    .HasColumnType("ntext")
                    .HasColumnName("CONTENT");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("DATE");

                entity.Property(e => e.Stt).HasColumnName("STT");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("TITLE");
            });

            modelBuilder.Entity<Tblparentmenu>(entity =>
            {
                entity.ToTable("TBLPARENTMENU");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Text)
                    .HasMaxLength(50)
                    .HasColumnName("TEXT");
            });

            modelBuilder.Entity<Tblservicesforcontent>(entity =>
            {
                entity.HasKey(e => new { e.Idhotel, e.Idservices })
                    .HasName("PK_TBLSERVICESFORCONTENT_1");

                entity.ToTable("TBLSERVICESFORCONTENT");

                entity.Property(e => e.Idhotel).HasColumnName("IDHOTEL");

                entity.Property(e => e.Idservices).HasColumnName("IDSERVICES");

                entity.Property(e => e.Content)
                    .HasMaxLength(500)
                    .HasColumnName("CONTENT");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Intlang)
                    .HasColumnName("INTLANG")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Tblservicesforsearch>(entity =>
            {
                entity.ToTable("TBLSERVICESFORSEARCH");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Css)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CSS");

                entity.Property(e => e.Intstyle).HasColumnName("INTSTYLE");

                entity.Property(e => e.Services)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SERVICES");

                entity.Property(e => e.ServicesVn)
                    .HasMaxLength(100)
                    .HasColumnName("SERVICES_VN");
            });

            modelBuilder.Entity<Tblstyle>(entity =>
            {
                entity.HasKey(e => e.Intstyle);

                entity.ToTable("TBLSTYLE");

                entity.Property(e => e.Intstyle).HasColumnName("INTSTYLE");

                entity.Property(e => e.Style)
                    .HasMaxLength(50)
                    .HasColumnName("STYLE");
            });

            modelBuilder.Entity<Tbltigium>(entity =>
            {
                entity.ToTable("TBLTIGIA");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("PRICE");

                entity.Property(e => e.Tigia)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIGIA");
            });

            modelBuilder.Entity<Tbluser>(entity =>
            {
                entity.ToTable("TBLUSER");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Diachi)
                    .HasMaxLength(200)
                    .HasColumnName("DIACHI");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MOBILE");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Ten)
                    .HasMaxLength(200)
                    .HasColumnName("TEN");

                entity.Property(e => e.UserType)
                    .HasColumnName("user_type")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TypeHotel>(entity =>
            {
                entity.ToTable("TypeHotel");

                entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(600)
                    .HasColumnName("address");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(70)
                    .HasColumnName("first_name");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.LastName)
                    .HasMaxLength(70)
                    .HasColumnName("last_name");

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(150)
                    .HasColumnName("mobile_no");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.Nationality)
                    .HasMaxLength(150)
                    .HasColumnName("nationality");

                entity.Property(e => e.Password)
                    .HasMaxLength(150)
                    .HasColumnName("password");

                entity.Property(e => e.Picture)
                    .HasMaxLength(600)
                    .IsUnicode(false)
                    .HasColumnName("picture");

                entity.Property(e => e.ResidenceNo)
                    .HasMaxLength(150)
                    .HasColumnName("residence_no");

                entity.Property(e => e.UserName)
                    .HasMaxLength(150)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserType).HasColumnName("user_type");
            });

            modelBuilder.Entity<VGetAllHotel>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_getAllHotels");

                entity.Property(e => e.Acctype)
                    .HasMaxLength(50)
                    .HasColumnName("ACCTYPE");

                entity.Property(e => e.Acctypeid).HasColumnName("ACCTYPEID");

                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.Avgnight).HasColumnName("AVGNIGHT");

                entity.Property(e => e.Beach).HasColumnName("BEACH");

                entity.Property(e => e.Calender).HasColumnName("CALENDER");

                entity.Property(e => e.Coffee).HasColumnName("COFFEE");

                entity.Property(e => e.Conferenceroom).HasColumnName("CONFERENCEROOM");

                entity.Property(e => e.Content)
                    .HasColumnType("ntext")
                    .HasColumnName("CONTENT");

                entity.Property(e => e.Description)
                    .HasMaxLength(3000)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Doorman).HasColumnName("DOORMAN");

                entity.Property(e => e.Elevatorinbuilding).HasColumnName("ELEVATORINBUILDING");

                entity.Property(e => e.Entertainment).HasColumnName("ENTERTAINMENT");

                entity.Property(e => e.Fireplace).HasColumnName("FIREPLACE");

                entity.Property(e => e.Fitnesscenter).HasColumnName("FITNESSCENTER");

                entity.Property(e => e.Freeparking).HasColumnName("FREEPARKING");

                entity.Property(e => e.Golf).HasColumnName("GOLF");

                entity.Property(e => e.Handicapaccessible).HasColumnName("HANDICAPACCESSIBLE");

                entity.Property(e => e.Hottub).HasColumnName("HOTTUB");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdQ).HasColumnName("ID_Q");

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE");

                entity.Property(e => e.Khuvuc).HasColumnName("KHUVUC");

                entity.Property(e => e.Latmap).HasColumnName("LATMAP");

                entity.Property(e => e.Lonmap).HasColumnName("LONMAP");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("NAME");

                entity.Property(e => e.Petsallowed).HasColumnName("PETSALLOWED");

                entity.Property(e => e.Photo).HasColumnName("PHOTO");

                entity.Property(e => e.Pickanddrop).HasColumnName("PICKANDDROP");

                entity.Property(e => e.Playplace).HasColumnName("PLAYPLACE");

                entity.Property(e => e.Quanhuyen).HasColumnName("QUANHUYEN");

                entity.Property(e => e.Rating).HasColumnName("RATING");

                entity.Property(e => e.Roomservice).HasColumnName("ROOMSERVICE");

                entity.Property(e => e.Securevault).HasColumnName("SECUREVAULT");

                entity.Property(e => e.Smokingallowed).HasColumnName("SMOKINGALLOWED");

                entity.Property(e => e.Suitableforevents).HasColumnName("SUITABLEFOREVENTS");

                entity.Property(e => e.TenTt)
                    .HasMaxLength(50)
                    .HasColumnName("TenTT");

                entity.Property(e => e.Tenis).HasColumnName("TENIS");

                entity.Property(e => e.Tenkv)
                    .HasMaxLength(50)
                    .HasColumnName("TENKV");

                entity.Property(e => e.Tenquan)
                    .HasMaxLength(50)
                    .HasColumnName("TENQUAN");

                entity.Property(e => e.Tinhthanh).HasColumnName("TINHTHANH");

                entity.Property(e => e.WiFi).HasColumnName("WI_FI");

                entity.Property(e => e.Winebar).HasColumnName("WINEBAR");
            });

            modelBuilder.Entity<ViewDirection>(entity =>
            {
                entity.ToTable("ViewDirection");

                entity.Property(e => e.NameView)
                    .HasMaxLength(50)
                    .HasColumnName("ViewDirection");
            });

            modelBuilder.Entity<Viewhotelservice>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VIEWHOTELSERVICES");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idhotel).HasColumnName("IDHOTEL");

                entity.Property(e => e.ServicesVn)
                    .HasMaxLength(100)
                    .HasColumnName("SERVICES_VN");
            });
        }
    }
}
