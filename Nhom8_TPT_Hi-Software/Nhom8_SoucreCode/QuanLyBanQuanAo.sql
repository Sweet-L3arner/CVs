USE master
GO

-- Drop the database if it already exists
IF  EXISTS (
	SELECT name 
		FROM sys.databases 
		WHERE name = N'QuanLyBanQuanAo'
)
DROP DATABASE QuanLyBanQuanAo
GO

CREATE DATABASE QuanLyBanQuanAo
GO

-- =========================================
-- Create table
-- =========================================
USE QuanLyBanQuanAo
GO
-- LoaiSanPham table
CREATE TABLE LoaiSanPham
(
	MaLoaiSanPham	varchar(20)	NOT NULL,
	TenLoaiSanPham	nvarchar(255)	NOT NULL,
	DVT				nvarchar(255)	NOT NULL,

	CONSTRAINT PK_LoaiSanPham PRIMARY KEY (MaLoaiSanPham)
)
GO
-- NhanHieu table
CREATE TABLE NhanHieu
(
	MaNhanHieu	varchar(20)		NOT NULL,
	TenNhanHieu	varchar(255)	NOT NULL,
	GhiChu		nvarchar(255)	DEFAULT(N'Đang cập nhật'),
	AnhNH		nvarchar(255),

	CONSTRAINT PK_NhanHieu PRIMARY KEY (MaNhanHieu)
)
GO
-- SanPham table
CREATE TABLE SanPham
(
	MaSanPham		varchar(20)		NOT NULL,
	TenSanPham		nvarchar(255)	NOT NULL,
	MaNhanHieu		varchar(20)		NOT NULL,
	MaLoaiSanPham	varchar(20)		NOT NULL,
	NoiSanXuat		nvarchar(255)	NOT NULL,
	DonGia			int				DEFAULT(0) NOT NULL,
	TinhTrang		nvarchar(255),
	MoTa			ntext,
	AnhSP			nvarchar(255),

	CONSTRAINT PK_SanPham				PRIMARY KEY (MaSanPham),
	CONSTRAINT FK_SanPham_LoaiSanPham	FOREIGN KEY (MaLoaiSanPham) REFERENCES LoaiSanPham	(MaLoaiSanPham)
	ON UPDATE CASCADE
	ON DELETE CASCADE,
	CONSTRAINT FK_SanPham_NhanHieu		FOREIGN KEY (MaNhanHieu)	REFERENCES NhanHieu	(MaNhanHieu)
	ON UPDATE CASCADE
	ON DELETE CASCADE
)
GO
-- KhachHang table
CREATE TABLE dbo.KhachHang
(
	MaKhachHang	varchar(20)		NOT NULL,
	Ho			nvarchar(255)	NOT NULL,
	Ten			nvarchar(255)	NOT NULL,
	GioiTinh	bit				DEFAULT(1),
	SDT			varchar(255)	NOT NULL,
	Email		varchar(255)	NOT NULL,
	MatKhau		varchar(255)	NOT NULL,
	TinhOrTP	nvarchar(255)	NOT NULL,
	DiaChi		nvarchar(255)	NOT NULL,

	CONSTRAINT PK_KhachHang PRIMARY KEY (MaKhachHang)
)
GO
-- NhanVien table
CREATE TABLE dbo.NhanVien
(
	MaNhanVien	varchar(20)		NOT NULL,
	Ho			nvarchar(255)	NOT NULL,
	Ten			nvarchar(255)	NOT NULL,
	GioiTinh	bit				DEFAULT(1),
	SDT			varchar(15)		NOT NULL,
	Email		varchar(255)	NOT NULL,
	MatKhau		varchar(255)	NOT NULL,
	TrangThai	nvarchar(15),
	QuanLy		bit				DEFAULT(0),
	AnhNV		nvarchar(255),

	CONSTRAINT PK_NhanVien PRIMARY KEY (MaNhanVien),
)
GO
-- DonViVanChuyen table
CREATE TABLE DonViVanChuyen
(
	MaDonVi		varchar(20)		NOT NULL,
	TenDonVi	nvarchar(255)	NOT NULL,
	SDT			varchar(255)		NOT NULL,
	Email		varchar(255)	NOT NULL,
	AnhDVVC		nvarchar(255),

	CONSTRAINT PK_DonViVanChuyen PRIMARY KEY (MaDonVi),
)

-- DonDatHang table
CREATE TABLE DonDatHang
(
	MaDonDatHang		varchar(20)	NOT NULL,
	MaKhachHang			varchar(20)		NOT NULL,
	MaNhanVien			varchar(20),
	MaDonViVanChuyen	varchar(20),
	NgayDatHang			date,
	NgayGiaoHang		date,
	DiaChiGiao			varchar(255),
	TinhTrang			int,

	CONSTRAINT PK_DonDatHang			PRIMARY KEY (MaDonDatHang),
	CONSTRAINT FK_DonDatHang_KhachHang	FOREIGN KEY (MaKhachHang)	REFERENCES KhachHang(MaKhachHang)
	ON UPDATE CASCADE
	ON DELETE CASCADE,
	CONSTRAINT FK_DonDatHang_NhanVien	FOREIGN KEY	(MaNhanVien)		REFERENCES NhanVien(MaNhanVien)
	ON DELETE SET NULL,
	CONSTRAINT FK_DonDatHang_Shipper	FOREIGN KEY (MaDonViVanChuyen)	REFERENCES DonViVanChuyen(MaDonVi)
	ON DELETE SET NULL,
)
GO
-- ChiTietDatHang table
CREATE TABLE ChiTietDatHang
(
	MaDonDatHang	varchar(20)	NOT NULL,
	MaSanPham		varchar(20)	NOT NULL,
	SoLuong			int			NOT NULL,
	ThanhTien		int			NOT NULL,

	CONSTRAINT PK_ChiTietDatHang			PRIMARY KEY (MaDonDatHang, MaSanPham),
	CONSTRAINT FK_ChiTietDatHang_DonDatHang	FOREIGN KEY (MaDonDatHang) REFERENCES DonDatHang(MaDonDatHang)
	ON UPDATE CASCADE
	ON DELETE CASCADE,
	CONSTRAINT FK_ChiTietDatHang_SanPham	FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
	ON UPDATE CASCADE
	ON DELETE CASCADE,
)
GO
-- Phanhoi table
CREATE TABLE PhanHoi
(
	MaPhanHoi	int		IDENTITY(1,1)	NOT NULL,
	MaSanPham	varchar(20)				NOT NULL,
	PhanHoi		nvarchar(255)			DEFAULT(N'Không có phản hồi'),
	MaKhachHang	varchar(20)				NOT NULL,

	CONSTRAINT PK_PhanHoi				PRIMARY KEY (MaPhanHoi),
	CONSTRAINT FK_PhanHoi_SanPham		FOREIGN KEY (MaSanPham)		REFERENCES SanPham(MaSanPham)
	ON UPDATE CASCADE
	ON DELETE CASCADE,
	CONSTRAINT FK_PhanHoi_MaKhachHang	FOREIGN KEY (MaKhachHang)	REFERENCES KhachHang(MaKhachHang)
	ON UPDATE CASCADE
	ON DELETE CASCADE
)
GO
-- NhatKy table
CREATE TABLE NhatKy
(
	ID			bigint IDENTITY(1,1),
	Username	varchar(255),
	TinhTrang	nvarchar(255),
	GhiNho		datetime DEFAULT(getdate()),

	CONSTRAINT PK_NhatKy PRIMARY KEY(ID)
)
GO
-- =========================================
-- CREATE PROCEDURE
-- =========================================

-- Nhân viên
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Sp_QuanLy_Login]
	@Email [varchar](250),
	@Password [varchar](250),
	@QuanLy	bit
as
begin
	declare @count int
	declare @res bit
	select @count = count(*) from NhanVien where Email = @Email and MatKhau = @Password and QuanLy = @QuanLy
	if @count > 0
		set @res = 1
	else
		set @res = 0

	select @res
end

-- khách hàng
-- login
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[Sp_KhachHang_Login]
	@Email [varchar](250),
	@Password [varchar](250)
as
begin
	declare @count int
	declare @res bit
	select @count = count(*) from KhachHang where Email = @Email and MatKhau = @Password
	if @count > 0
		set @res = 1
	else
		set @res = 0

	select @res
end
-- regis
create proc [dbo].[Sp_KhachHang_Regis]
	@Email [varchar](250)
as
begin
	declare @count int
	declare @res bit
	select @count = count(*) from KhachHang where Email = @Email
	if @count > 0
		set @res = 1
	else
		set @res = 0

	select @res
end

-- =========================================
-- INSERT INTO
-- =========================================
-- Loại sản phẩm
INSERT INTO LoaiSanPham 
(MaLoaiSanPham, TenLoaiSanPham, DVT)
VALUES 
('LSP001',N'Áo phông', N'Chiếc'),
('LSP002',N'Áo khoác', N'Chiếc'),
('LSP003',N'Quần dài', N'Chiếc'),
('LSP004',N'Quần ngắn', N'Chiếc'),
('LSP005',N'Túi xách', N'Chiếc'),
('LSP006',N'Thắt lưng', N'Cái'),
('LSP007',N'Nón', N'Cái'),
('LSP008',N'Giày', N'Cái'),
('LSP009',N'Mắt kính', N'Cặp'),
('LSP010',N'Áo sơ mi', N'Chiếc')
GO

-- Nhãn hiệu
INSERT INTO NhanHieu
(MaNhanHieu, TenNhanHieu, GhiChu, AnhNH)
VALUES 
('NH001', N'Dior' , '...', 'dior.jpg'),
('NH002', N'Celine', '...', 'celine.jpg'),
('NH003', N'Saint Laurent', '...', 'saintlaurent.jpg'),
('NH004', N'Chanel', '...', 'chanel.jpg'),
('NH005', N'Gucci', '...', 'gucci.jpg'),
('NH006', N'Adidas', '...', 'adidas.jpg'),
('NH007', N'Nike', '...', 'nike.jpg'),
('NH008', N'Calvin Klein', '...', 'calvinklein.jpg'),
('NH009', N'Puma', '...' , 'puma.jpg')
GO

DELETE FROM SanPham
-- SanPham
INSERT INTO SanPham
(MaSanPham, TenSanPham, MaNhanHieu, MaLoaiSanPham, NoiSanXuat, DonGia, TinhTrang, MoTa, AnhSP)
VALUES
('SP0001', N'OVERSIZED DIOR AND PETER DOIG T-SHIRT','NH001','LSP001',N'Ý',550000,N'Còn hàng',N'Nam', 'sp1.png'),
('SP0002', N'CELINE LOOSE T-SHIRT IN COTTON JERSEY WITH STUDS BLACK','NH002','LSP001',N'Ý',650000,N'Còn hàng',N'Nam', 'sp2.png'),
('SP0003', N'"SAINT LAURENT HEART" T-SHIRT','NH003','LSP001',N'Ý',450000,N'Còn hàng',N'Nữ', 'sp3.png'),
('SP0004', N'QUILTED CARDIGAN IN VELVET JERSEY BLACK','NH002','LSP002',N'Mỹ',354000,N'Còn hàng',N'Nữ', 'sp4.png'),
('SP0005', N'OFFICER COLLAR COAT','NH001','LSP002',N'Ý',3300000,N'Còn hàng',N'Nữ', 'sp5.png'),
('SP0006', N'MASCULINE WOOL COAT','NH003','LSP002',N'Ý',4500000,N'Còn hàng',N'Nữ', 'sp6.png'),
('SP0007', N'PANTABOOTS IN SUEDE','NH003','LSP003',N'Pháp',10500000,N'Còn hàng',N'Nữ', 'sp7.png'),
('SP0008', N'TOILE DE JOUY PAJAMA PANTS','NH001','LSP003',N'Ý',1550000,N'Còn hàng',N'Nam', 'sp8.png'),
('SP0009', N'SKATE PANTS IN STRIPED WOOL GABARDINE BLACK/ANTHRACITE','NH002','LSP003',N'Ý',2450000,N'Còn hàng',N'Nam', 'sp9.png'),
('SP0010', N'Channel Shorts','NH004','LSP004',N'Đức',2000000,N'Còn hàng',N'Nam', 'sp10.png'),
('SP0011', N'Cady crêpe silk wool shorts','NH005','LSP004',N'Ý',1355000,N'Còn hàng',N'Nữ', 'sp11.png'),
('SP0012', N'QUẦN SHORT 3 SỌC KẺ','NH006','LSP004',N'Đức',1200000,N'Còn hàng',N'Nam', 'sp12.png'),
('SP0013', N'2017 Re-Edition Dionysus python bag','NH005','LSP005',N'Pháp',3650000,N'Còn hàng',N'Nữ', 'sp13.png'),
('SP0014', N'TÚI SHOPPING','NH004','LSP005',N'Pháp',4050000,N'Còn hàng',N'Nữ', 'sp14.png'),
('SP0015', N'TÚI HER STUDIO LONDON','NH006','LSP005',N'Pháp',650000,N'Còn hàng',N'Nữ', 'sp15.png'),
('SP0016', N'FQ2163','NH006','LSP006',N'Mỹ',9250000,N'Còn hàng',N'Nam', 'sp16.png'),
('SP0017', N'THẮT LƯNG CHANEL','NH004','LSP006',N'Mỹ',1125000,N'Còn hàng',N'Nam', 'sp17.png'),
('SP0018', N'Belt with G buckle','NH005','LSP006',N'Pháp',1250000,N'Còn hàng',N'Nữ', 'sp18.png'),
('SP0019', N'Nike Sportswear','NH007','LSP007',N'Ý',750000,N'Còn hàng',N'Nam/Nữ', 'sp19.png'),
('SP0020', N'Organic Cotton Canvas Monogram Logo Baseball Cap','NH008','LSP007',N'Ý',1450000,N'Còn hàng',N'Nam/Nữ', 'sp20.png'),
('SP0021', N'Estate 2.0 Snapback','NH009','LSP007',N'Ý',350000,N'Còn hàng',N'Nam', 'sp21.png'),
('SP0022', N'Emmitt Retro Logo Sneaker','NH008','LSP008',N'Ý',450000,N'Còn hàng',N'Nam', 'sp22.png'),
('SP0023', N'Nike React Infinity Run FK 2 BeTrue','NH007','LSP008',N'Ý',730000,N'Còn hàng',N'Nam', 'sp23.png'),
('SP0024', N'Future Rider NYC Womens Sneakers','NH009','LSP008',N'Ý',750000,N'Còn hàng',N'Nữ', 'sp24.png'),
('SP0025', N'Ruby Oval Sunglasses','NH009','LSP009',N'Ý',1060000,N'Còn hàng',N'Nam', 'sp25.png'),
('SP0026', N'Nike NIKE 5002 Eyeglasses','NH007','LSP009',N'Ý',2550000,N'Còn hàng',N'Nam', 'sp26.png'),
('SP0027', N'Modified Rectangle Acetate Sunglasses','NH008','LSP009',N'Ý',950000,N'Còn hàng',N'Nữ', 'sp27.png'),
('SP0028', N'DIOR AND PETER DOIG SHIRT','NH001','LSP010',N'Ý',1550000,N'Còn hàng',N'Nam', 'sp28.png'),
('SP0029', N'HAWAIIAN SHIRT IN PRINTED VISCOSE NOIR/CRAIE','NH002','LSP010',N'Pháp',3450000,N'Còn hàng',N'Nam', 'sp29.png'),
('SP0030', N'OVERSIZED SHIRT IN COTTON TWILL','NH003','LSP010',N'Pháp',2190000,N'Còn hàng',N'Nữ', 'sp30.png')

-- Khách hàng
-- Mật khẩu demo: 123 sau khi mã khóa md5: 202cb962ac59075b964b07152d234b70
INSERT INTO KhachHang 
(MaKhachHang, Ho, Ten, GioiTinh, SDT, Email, MatKhau, TinhOrTP, DiaChi)
VALUES 
('KH0001',N'Lê Xuân',N'Tài',1,'0987456123','phuclh@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang',N'19 Nguyễn Khuyến'),
('KH0002',N'Lê Quốc',N'Duy',1,'0987456111','duylq@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Phú Yên',N'23 Nguyễn Du, Đông Hòa, Tuy Hòa'),
('KH0003',N'Võ Tuấn',N'Kiệt',1,'0987452222','kietvt@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang', N'21 Nguyễn Khuyến'),
('KH0004',N'Lê Đức',N'Thắng',1,'0987453333','thangld@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Vũng Tàu', N'30 Nguyễn Thị Minh Khai'),
('KH0005',N'Lê Thị Hoàng',N'Yến',0,'0987454444','yenlth@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Cam Ranh', N'03 Lê Khuyến'),
('KH0006',N'Bùi Thị Ngọc',N'Mai',0,'0987454545','maibtn@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Khánh Hòa', N'Dục Mỹ, Ninh Hòa'),
('KH0007',N'Lê Thị Thu',N'Trang',0,'0987455555','trangltt@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang',N'25 Nguyễn Trãi'),
('KH0008',N'Mai Quỳnh',N'Hương',0,'0987456611','huongmq@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang', N'18 Nguyễn Khuyến'),
('KH0009',N'Ngô Thị Kim',N'Yến',0,'0987456133','yenntk@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang',N'34 Nguyễn Khuyến'),
('KH0010',N'Thái Hữu',N'Nhật',1,'0987454534','nhatth@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang',N'40 Nguyễn Khuyến'),
('KH0011',N'Lê Kim Thanh',N'Nga',0,'0987455656','ngalkt@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Khánh Hòa', N'Bảy Búa, Ninh Thân'),
('KH0012',N'Nguyễn Thị Ngọc',N'Diệu',0,'0987456666','dieuntn@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang', N'40 Đoàn Trần Nghiệp'),
('KH0013',N'La Thị Hoài',N'Hoa',0,'0987455612','hoalth@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang', N'20 Đoàn Trần Nghiệp'),
('KH0014',N'Lê Thị',N'Mỹ',0,'0987458787','mylt@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang', N'19 Nguyễn Khuyến'),
('KH0015',N'Nguyễn Khánh',N'Hạ',0,'0987458888','hank@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đà Nẵng', N'15 Nguyễn Thiện Thuật'),
('KH0016',N'Trần Thị Ni',N'Na',0,'0987459999','nattnn@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Hà Nội', N'09 Hàng Bài'),
('KH0017',N'Lê Thảo',N'Vy',0,'0987458989','vylt@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đà Nẵng',N'30 Nguyễn Trãi'),
('KH0018',N'Lê Thị Yến',N'Vi',0,'0987458008','vilty@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang', N'19 Nguyễn Đình Chiểu'),
('KH0019',N'Lê Thị Thanh',N'Vy',0,'0987453003','vyltt@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang',N'40 Nguyễn Khuyến'),
('KH0020',N'Trần Thị',N'Vân',0,'0987450081','vantt@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang',N'19 Nguyễn Khuyến'),
('KH0021',N'Trương Đình',N'Hoàng',1,'0987450980','hoangtd@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang',N'21 Nguyễn Du'),
('KH0022',N'Lê Bảo',N'Long',1,'0987451029','longlb@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Hồ Chí Minh',N'01 Nguyễn Huệ'),
('KH0023',N'Trần Minh',N'Lâm',1,'0987452340','lamtm@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Phú Yên',N'19 Nguyễn Trãi, Đông Hòa'),
('KH0024',N'Trương Hoàng',N'Yến',0,'0987454051','yenth@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang',N'19 Yersin'),
('KH0025',N'Lê Ngọc',N'Tâm',0,'0987454770','tamln@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang',N'40 Mai Xuân Thưởng'),
('KH0026',N'Võ Thị Ngọc',N'Yến',0,'0987458189','yenvtn@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang',N'40 Nguyễn Khuyến'),
('KH0027',N'Lê Thị Như',N'Ý',0,'0987455650','yltn@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang',N'21 Nguyễn Khuyến'),
('KH0028',N'Nguyễn Văn',N'Phước',1,'0987450911','phuocnv@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang',N'40 Nguyễn Du'),
('KH0029',N'Lê Thanh',N'Phong',1,'0987451919','phonglt@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang',N'21 Nguyễn Thị Minh Khai'),
('KH0030',N'Mai Xuân',N'Huy',1,'0987452919','huymx@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang',N'05 Nguyễn Trãi'),
('KH0031',N'Lê Hữu',N'Phước',1,'0794508947','phuoc@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Nha Trang',N'132/Lưu Hữu Phước')

-- NhanVien
INSERT INTO NhanVien
(MaNhanVien, Ho, Ten, GioiTinh, SDT, Email, MatKhau, TrangThai, QuanLy, AnhNV)
VALUES
('NV000', 'Check', 'Check', 1, 00000000, 'check@gmail.com', '00000000', N'Dùng để check', 1, null),
('NV001', N'Nguyễn Thị',N'Thắm',1,0931132321,'thamnt@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',1,N'employee.png'),
('NV002', N'Nguyễn Thị',N'Mai',1,0931132354,'maint@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV003', N'Nguyễn Thị',N'Nga',1,0931265321,'ngant@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV004', N'Nguyễn Thị',N'Đào',1,0931210031,'daont@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV005', N'Nguyễn Thị',N'Thủy',1,0949265321,'thuynt@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV006', N'Nguyễn Thị',N'Liễu',1,0831265321,'lieunt@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV007', N'Nguyễn Thị',N'Tuyết',1,0131265321,'tuyetnt@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV008', N'Nguyễn Thị',N'Ngân',1,0231565321,'ngannt@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV009', N'Nguyễn Thị',N'Hồng',1,0123155321,'hongnt@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV010', N'Nguyễn Thị',N'Ngân',1,0231565321,'ngannt@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV011', N'Bùi Ngọc',N'Hoa',1,0231995321,'hoabn@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',1,N'employee.png'),
('NV012', N'Bùi Ngọc',N'Thúy',1,0131995211,'thuybn@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV013', N'Bùi Ngọc',N'Thọ',1,0531995211,'thobn@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV014', N'Bùi Ngọc',N'Thoa',1,0431995211,'thoabn@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV015', N'Bùi Ngọc',N'My',1,0531795211,'mybn@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV016', N'Bùi Ngọc',N'Mỹ',1,0536995211,'mymybn@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV017', N'Bùi Ngọc',N'Hương',1,0131995211,'huongbn@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV018', N'Bùi Ngọc',N'Hường',1,0531988211,'huonghoabn@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV019', N'Bùi Ngọc',N'Bích',1,0231995211,'bichbn@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV020', N'Lê Nguyễn',N'Thương',1,0731995211,'thuongln@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',1,N'employee.png'),
('NV021', N'Lê Nguyễn',N'Trà',1,0777995211,'traln@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV022', N'Lê Nguyễn',N'Trân',1,0931995211,'tranln@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV023', N'Lê Nguyễn',N'Trâm',1,0131995211,'tram@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV024', N'Lê Nguyễn',N'Vân',1,0231988211,'van@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV025', N'Lê Nguyễn',N'Vy',1,0111995211,'vy@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV026', N'Lê Nguyễn',N'Vi',1,0161996211,'vi@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV027', N'Lê Nguyễn',N'Trinh',1,0181999211,'trinh@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV028', N'Lê Nguyễn',N'Uyên',1,0161995291,'uyen@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV029', N'Lê Nguyễn',N'Ân',1,0136695211,'an@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png'),
('NV030', N'Lê Nguyễn',N'Ánh',1,0131335211,'anh@gmail.com',N'202cb962ac59075b964b07152d234b70',N'Đang làm việc',0,N'employee.png')

--- Đơn vị vận chuyển
INSERT INTO DonViVanChuyen
(MaDonVi, TenDonVi, SDT, Email, AnhDVVC)
VALUES
('DVVC001', N'VNPost – EMS', '1900 545433', 'vnpostems@gmail.com', 'vietnampost.jpg'),
('DVVC002', N'Viettel Post', '1800 8198', 'viettelpost@gmail.com', 'viettelpost.jpg'),
('DVVC003', N'Giaohangnhanh', '1900 0218', 'giaohangnhanh@gmail.com', 'ghn.jpg'),
('DVVC004', N'Giaohangtietkiem', '1800 6092', 'giaohangtietkiem@gmail.com', 'giaohangtietkiem.jpg'),
('DVVC005', N'J&T Express', '1900 1088', 'jtexpress@gmail.com', 'jtexpress.jpeg')
GO

-- Đơn đặt hàng
INSERT INTO DonDatHang
(MaDonDatHang, MaKhachHang, MaNhanVien, MaDonViVanChuyen, NgayDatHang, NgayGiaoHang, DiaChiGiao)
VALUES
('DH007', 'KH001', 'NV001', NULL, '2021-06-30', '2021-07-30', 'A')