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
        public virtual DbSet<Banner> Banners { get; set; } = null!;
        public virtual DbSet<Hotel> Hotels { get; set; } = null!;
        public virtual DbSet<HotelMameniti> HotelMamenitis { get; set; } = null!;
        public virtual DbSet<HotelPromotion> HotelPromotions { get; set; } = null!;
        public virtual DbSet<HotelRating> HotelRatings { get; set; } = null!;
        public virtual DbSet<HotelReview> HotelReviews { get; set; } = null!;
        public virtual DbSet<HotelRoom> HotelRooms { get; set; } = null!;
        public virtual DbSet<HotelRoomComment> HotelRoomComments { get; set; } = null!;
        public virtual DbSet<KhuVuc> KhuVucs { get; set; } = null!;
        public virtual DbSet<MulltiKeyValue> MulltiKeyValues { get; set; } = null!;
        public virtual DbSet<NearbyHotel> NearbyHotels { get; set; } = null!;
        public virtual DbSet<News> News { get; set; } = null!;
        public virtual DbSet<NewsCategory> NewsCategories { get; set; } = null!;
        public virtual DbSet<NewsGallery> NewsGalleries { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderPhuongThucTt> OrderPhuongThucTts { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<Palletbed> Palletbeds { get; set; } = null!;
        public virtual DbSet<Phuong> Phuongs { get; set; } = null!;
        public virtual DbSet<Picture> Pictures { get; set; } = null!;
        public virtual DbSet<PriceRange> PriceRanges { get; set; } = null!;
        public virtual DbSet<Quan> Quans { get; set; } = null!;
        public virtual DbSet<RoomMameniti> RoomMamenitis { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<TinhThanh> TinhThanhs { get; set; } = null!;
        public virtual DbSet<Tour> Tours { get; set; } = null!;
        public virtual DbSet<TourDetail> TourDetails { get; set; } = null!;
        public virtual DbSet<TourDetailsStyle> TourDetailsStyles { get; set; } = null!;
        public virtual DbSet<TourDistrictTo> TourDistrictTos { get; set; } = null!;
        public virtual DbSet<TourStyle> TourStyles { get; set; } = null!;
        public virtual DbSet<TourTopic> TourTopics { get; set; } = null!;
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

				entity.HasOne(d => d.IdTypeNavigation)
					.WithMany(p => p.Albums)
					.HasForeignKey(d => d.IdType)
					.HasConstraintName("FK_Album_Hotel");
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

            modelBuilder.Entity<Banner>(entity =>
            {
                entity.ToTable("Banner");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Image)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Link)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Stt).HasDefaultValueSql("((10))");

                entity.Property(e => e.Title).HasMaxLength(50);
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

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(3000);

				entity.Property(e => e.IdPhuong).HasDefaultValueSql("((2))");

				entity.Property(e => e.IdPriceRange).HasDefaultValueSql("((0))");

				entity.Property(e => e.IdQuan).HasDefaultValueSql("((14))");

				entity.Property(e => e.IdTinhThanh).HasDefaultValueSql("((5))");

                entity.Property(e => e.IntDate)
                    .HasColumnName("intDate")
                    .HasDefaultValueSql("(datediff(second,'2022-1-1',getdate()))");

				entity.Property(e => e.LatMap).HasColumnName("Lat_map");

				entity.Property(e => e.LocationScore)
					.HasColumnType("decimal(2, 1)")
					.HasColumnName("Location_score");

				entity.Property(e => e.LonMap).HasColumnName("Lon_map");

				entity.Property(e => e.Meals).HasDefaultValueSql("((0))");

				entity.Property(e => e.Name).HasMaxLength(120);

				entity.Property(e => e.NameSeo)
					.HasMaxLength(50)
					.HasColumnName("Name_seo");

                entity.Property(e => e.NumViews)
                    .HasColumnName("numViews")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Rating).HasDefaultValueSql("((1))");

				entity.Property(e => e.ReviewScore).HasColumnName("Review_score");

                entity.Property(e => e.RoomsScore)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("Rooms_score");

                entity.Property(e => e.SearchKey).HasMaxLength(200);

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

				entity.HasOne(d => d.IdPhuongNavigation)
					.WithMany(p => p.Hotels)
					.HasForeignKey(d => d.IdPhuong)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Hotel_Phuong");

				entity.HasOne(d => d.IdPriceRangeNavigation)
					.WithMany(p => p.Hotels)
					.HasForeignKey(d => d.IdPriceRange)
					.HasConstraintName("FK_Hotel_PRICE_RANGE");

				entity.HasOne(d => d.IdQuanNavigation)
					.WithMany(p => p.Hotels)
					.HasForeignKey(d => d.IdQuan)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Hotel_Quan");

				entity.HasOne(d => d.IdTinhThanhNavigation)
					.WithMany(p => p.Hotels)
					.HasForeignKey(d => d.IdTinhThanh)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Hotel_TinhThanh");

				entity.HasOne(d => d.TypeNavigation)
					.WithMany(p => p.Hotels)
					.HasForeignKey(d => d.Type)
					.HasConstraintName("FK_Hotel_TypeHotel");
			});

			modelBuilder.Entity<HotelMameniti>(entity =>
			{
				entity.ToTable("HotelMameniti");

				entity.Property(e => e.Idhotel).HasColumnName("IDHOTEL");

				entity.Property(e => e.Idservices).HasColumnName("IDSERVICES");

				entity.HasOne(d => d.IdhotelNavigation)
					.WithMany(p => p.HotelMamenitis)
					.HasForeignKey(d => d.Idhotel)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_HotelMameniti_Hotel");

				entity.HasOne(d => d.IdservicesNavigation)
					.WithMany(p => p.HotelMamenitis)
					.HasForeignKey(d => d.Idservices)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_HotelMameniti_Services");
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

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(1000);

				entity.Property(e => e.Idhotel).HasColumnName("IDHOTEL");

                entity.Property(e => e.Iduser).HasColumnName("IDUser");

                entity.Property(e => e.IntDate)
                    .HasColumnName("intDate")
                    .HasDefaultValueSql("(datediff(second,'2022-1-1',getdate()))");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.NewPrice).HasColumnType("decimal(18, 0)");

				entity.Property(e => e.PriceValue)
					.HasColumnType("decimal(18, 0)")
					.HasColumnName("Price_Value");

                entity.Property(e => e.RemainNum).HasDefaultValueSql("((1))");

                entity.Property(e => e.RoomSize).HasColumnType("decimal(6, 2)");

                entity.HasOne(d => d.IdhotelNavigation)
                    .WithMany(p => p.HotelRooms)
                    .HasForeignKey(d => d.Idhotel)
                    .HasConstraintName("FK_TBLROOM_TBLHOTEL");

                entity.HasOne(d => d.PalletbedNavigation)
                    .WithMany(p => p.HotelRooms)
                    .HasForeignKey(d => d.Palletbed)
                    .HasConstraintName("FK_HotelRoom_Palletbed");

                entity.HasOne(d => d.ViewDirectionNavigation)
                    .WithMany(p => p.HotelRooms)
                    .HasForeignKey(d => d.ViewDirection)
                    .HasConstraintName("FK_HotelRoom_ViewDirection");
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

			modelBuilder.Entity<KhuVuc>(entity =>
			{
				entity.HasKey(e => e.IdKv)
					.HasName("PK_TB_KV");

				entity.ToTable("KhuVuc");

				entity.Property(e => e.IdKv).HasColumnName("ID_KV");

				entity.Property(e => e.Diengiai)
					.HasMaxLength(500)
					.HasColumnName("DIENGIAI");

				entity.Property(e => e.IdQ).HasColumnName("ID_Q");

				entity.Property(e => e.Stt).HasColumnName("STT");

				entity.Property(e => e.Tenkv)
					.HasMaxLength(50)
					.HasColumnName("TENKV");
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

            modelBuilder.Entity<NearbyHotel>(entity =>
            {
                entity.Property(e => e.IdPlace)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.IdHotelNavigation)
                    .WithMany(p => p.NearbyHotels)
                    .HasForeignKey(d => d.IdHotel)
                    .HasConstraintName("FK_NearbyHotels_Hotel");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.DateEdit).HasColumnType("datetime");

                entity.Property(e => e.Keysearch)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Title).HasMaxLength(150);

                entity.HasOne(d => d.IdcatNavigation)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.Idcat)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_News_NewsCategory");
            });

            modelBuilder.Entity<NewsCategory>(entity =>
            {
                entity.ToTable("NewsCategory");

                entity.Property(e => e.Category).HasMaxLength(100);

                entity.Property(e => e.Keysearch)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");
            });

            modelBuilder.Entity<NewsGallery>(entity =>
            {
                entity.ToTable("NewsGallery");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Idpic).ValueGeneratedOnAdd();

                entity.HasOne(d => d.IdnewsNavigation)
                    .WithMany(p => p.NewsGalleries)
                    .HasForeignKey(d => d.Idnews)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewsGallery_News");

                entity.HasOne(d => d.IdpicNavigation)
                    .WithMany(p => p.NewsGalleries)
                    .HasForeignKey(d => d.Idpic)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewsGallery_Pictures");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.DateCreate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateUpdate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdPaymentMethod).HasColumnName("IdPTThanhToan");

                entity.Property(e => e.MoreInfo).HasMaxLength(500);

                entity.Property(e => e.Ordercode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Session)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.IdPaymentMethodNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdPaymentMethod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_OrderPhuongThucTT");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_users");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK_Orders_OrderStatus");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.ChechIn).HasColumnType("datetime");

                entity.Property(e => e.CheckOut).HasColumnType("datetime");

                entity.Property(e => e.DateCreate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DetailStyle).HasDefaultValueSql("((1))");

                entity.Property(e => e.MoreInfo).HasMaxLength(500);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdOrderNavigation)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.IdOrder)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Orders");

                entity.HasOne(d => d.IdRoomNavigation)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.IdRoom)
                    .HasConstraintName("FK_OrderDetail_HotelRoom");

                entity.HasOne(d => d.IdTourNavigation)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.IdTour)
                    .HasConstraintName("FK_OrderDetail_Tours");
            });

            modelBuilder.Entity<OrderPhuongThucTt>(entity =>
            {
                entity.ToTable("OrderPhuongThucTT");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.PhuongThuc).HasMaxLength(50);
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.ToTable("OrderStatus");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<Palletbed>(entity =>
            {
                entity.ToTable("Palletbed");

                entity.Property(e => e.Text).HasMaxLength(50);
            });

            modelBuilder.Entity<Phuong>(entity =>
            {
                entity.ToTable("Phuong");

                entity.Property(e => e.SearchKey).HasMaxLength(200);

				entity.Property(e => e.Stt).HasColumnName("STT");

				entity.Property(e => e.Tenphuong)
					.HasMaxLength(250)
					.HasColumnName("TENPHUONG");

				entity.Property(e => e.Tenphuong2)
					.HasMaxLength(50)
					.IsUnicode(false)
					.HasColumnName("TENPHUONG2");

				entity.HasOne(d => d.IdQuanNavigation)
					.WithMany(p => p.Phuongs)
					.HasForeignKey(d => d.IdQuan)
					.HasConstraintName("FK_Phuong_Quan");
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

				entity.HasOne(d => d.Hotel)
					.WithMany(p => p.Pictures)
					.HasForeignKey(d => d.HotelId)
					.HasConstraintName("Fk_Pictures_Hotel");

				entity.HasOne(d => d.HotelRoom)
					.WithMany(p => p.Pictures)
					.HasForeignKey(d => d.HotelRoomId)
					.HasConstraintName("Fk_Pictures_HotelRoom");

                entity.HasOne(d => d.IdAlbumNavigation)
                    .WithMany(p => p.Pictures)
                    .HasForeignKey(d => d.IdAlbum)
                    .HasConstraintName("FK_Pictures_Album");

                entity.HasOne(d => d.Tour)
                    .WithMany(p => p.Pictures)
                    .HasForeignKey(d => d.TourId)
                    .HasConstraintName("FK_Pictures_Tours");
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

			modelBuilder.Entity<Quan>(entity =>
			{
				entity.ToTable("Quan");

                entity.Property(e => e.Diengiai)
                    .HasMaxLength(50)
                    .HasColumnName("DIENGIAI");

                entity.Property(e => e.SanitizedName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SearchKey).HasMaxLength(200);

				entity.Property(e => e.Stt).HasColumnName("stt");

				entity.Property(e => e.Tenquan)
					.HasMaxLength(50)
					.HasColumnName("TENQUAN");

                entity.HasOne(d => d.IdTinhThanhNavigation)
                    .WithMany(p => p.Quans)
                    .HasForeignKey(d => d.IdTinhThanh)
                    .HasConstraintName("FK_Quan_TinhThanh");
            });

			modelBuilder.Entity<RoomMameniti>(entity =>
			{
				entity.ToTable("RoomMameniti");

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

           

			modelBuilder.Entity<TinhThanh>(entity =>
			{
				entity.ToTable("TinhThanh");

                entity.Property(e => e.Diengiai).HasMaxLength(50);

                entity.Property(e => e.ECode)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("eCode");

				entity.Property(e => e.Locality).HasMaxLength(50);

                entity.Property(e => e.SanitizedName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SearchKey).HasMaxLength(200);

                entity.Property(e => e.Stt).HasColumnName("STT");

				entity.Property(e => e.TenTt)
					.HasMaxLength(50)
					.HasColumnName("TenTT");

				entity.Property(e => e.Tentt2)
					.HasMaxLength(50)
					.IsUnicode(false)
					.HasColumnName("TENTT2");
			});

            modelBuilder.Entity<Tour>(entity =>
            {
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Descriptions).HasMaxLength(350);

                entity.Property(e => e.Discount).HasDefaultValueSql("((0))");

                entity.Property(e => e.IdTourStyle).HasDefaultValueSql("((1))");

                entity.Property(e => e.IdTourTopic).HasDefaultValueSql("((1))");

                entity.Property(e => e.IdUser).HasDefaultValueSql("((9))");

                entity.Property(e => e.Locations).HasMaxLength(150);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.TourName).HasMaxLength(150);

                entity.HasOne(d => d.IdDistrictFromNavigation)
                    .WithMany(p => p.Tours)
                    .HasForeignKey(d => d.IdDistrictFrom)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tours_Quan");

                entity.HasOne(d => d.IdTourStyleNavigation)
                    .WithMany(p => p.Tours)
                    .HasForeignKey(d => d.IdTourStyle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tours_TourStyle");

                entity.HasOne(d => d.IdTourTopicNavigation)
                    .WithMany(p => p.Tours)
                    .HasForeignKey(d => d.IdTourTopic)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tours_TourTopic");
            });

            modelBuilder.Entity<TourDetail>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(200);

                entity.HasOne(d => d.IdStyleNavigation)
                    .WithMany(p => p.TourDetails)
                    .HasForeignKey(d => d.IdStyle)
                    .HasConstraintName("FK_TourDetails_TourDetailsStyle");

                entity.HasOne(d => d.IdToursNavigation)
                    .WithMany(p => p.TourDetails)
                    .HasForeignKey(d => d.IdTours)
                    .HasConstraintName("FK_TourDetails_Tours");
            });

            modelBuilder.Entity<TourDetailsStyle>(entity =>
            {
                entity.ToTable("TourDetailsStyle");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Style).HasMaxLength(50);
            });

            modelBuilder.Entity<TourDistrictTo>(entity =>
            {
                entity.ToTable("TourDistrictTo");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.HasOne(d => d.IdDistrictToNavigation)
                    .WithMany(p => p.TourDistrictTos)
                    .HasForeignKey(d => d.IdDistrictTo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TourDistrictTo_Quan");

                entity.HasOne(d => d.IdTourNavigation)
                    .WithMany(p => p.TourDistrictTos)
                    .HasForeignKey(d => d.IdTour)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TourDistrictTo_Tours");
            });

            modelBuilder.Entity<TourStyle>(entity =>
            {
                entity.ToTable("TourStyle");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.TourStyle1)
                    .HasMaxLength(100)
                    .HasColumnName("TourStyle");
            });

            modelBuilder.Entity<TourTopic>(entity =>
            {
                entity.ToTable("TourTopic");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.TourTopic1)
                    .HasMaxLength(100)
                    .HasColumnName("TourTopic");
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
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

				entity.Property(e => e.CreatedBy).HasColumnName("created_by");

				entity.Property(e => e.Email)
					.HasMaxLength(150)
					.HasColumnName("email");

				entity.Property(e => e.FirstName)
					.HasMaxLength(70)
					.HasColumnName("first_name");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

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

                entity.Property(e => e.UserType)
                    .HasColumnName("user_type")
                    .HasDefaultValueSql("((1))");
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

				entity.Property(e => e.ViewDirection1)
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

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
