USE [master]
GO
/****** Object:  Database [MostafidDatabase]    Script Date: 3/18/2021 3:18:39 PM ******/
CREATE DATABASE [MostafidDatabase]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MostafidDatabase', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Mostafid Database\MostafidDatabase.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MostafidDatabase_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Mostafid Database\MostafidDatabase_log.ldf' , SIZE = 2304KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [MostafidDatabase] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MostafidDatabase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MostafidDatabase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MostafidDatabase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MostafidDatabase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MostafidDatabase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MostafidDatabase] SET ARITHABORT OFF 
GO
ALTER DATABASE [MostafidDatabase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MostafidDatabase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MostafidDatabase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MostafidDatabase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MostafidDatabase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MostafidDatabase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MostafidDatabase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MostafidDatabase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MostafidDatabase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MostafidDatabase] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MostafidDatabase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MostafidDatabase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MostafidDatabase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MostafidDatabase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MostafidDatabase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MostafidDatabase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MostafidDatabase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MostafidDatabase] SET RECOVERY FULL 
GO
ALTER DATABASE [MostafidDatabase] SET  MULTI_USER 
GO
ALTER DATABASE [MostafidDatabase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MostafidDatabase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MostafidDatabase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MostafidDatabase] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [MostafidDatabase] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MostafidDatabase] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'MostafidDatabase', N'ON'
GO
ALTER DATABASE [MostafidDatabase] SET QUERY_STORE = OFF
GO
USE [MostafidDatabase]
GO
/****** Object:  User [user]    Script Date: 3/18/2021 3:18:39 PM ******/
CREATE USER [user] FOR LOGIN [user] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [user]
GO
/****** Object:  Table [dbo].[Applicant_Type]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Applicant_Type](
	[Applicant_Type_ID] [int] NOT NULL,
	[Applicant_Type_Name_EN] [nvarchar](250) NULL,
	[Applicant_Type_Name_AR] [nvarchar](250) NULL,
	[IS_Action] [nvarchar](20) NULL,
 CONSTRAINT [PK_Applicant_Type] PRIMARY KEY CLUSTERED 
(
	[Applicant_Type_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Provider_Academic_Services]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Provider_Academic_Services](
	[Provider_Academic_Services_ID] [int] NOT NULL,
	[Provider_Academic_Services_Name_EN] [nvarchar](max) NULL,
	[Provider_Academic_Services_Name_AR] [nvarchar](max) NULL,
	[IS_Action] [nvarchar](20) NULL,
 CONSTRAINT [PK_SELECT_PROVIDER_OF_ACADMIC_SERVICES] PRIMARY KEY CLUSTERED 
(
	[Provider_Academic_Services_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Main_Services]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Main_Services](
	[Main_Services_ID] [int] NOT NULL,
	[Main_Services_Name_EN] [nvarchar](max) NULL,
	[Main_Services_Name_AR] [nvarchar](max) NULL,
	[Provider_Academic_Services_ID] [int] NULL,
	[IS_Action] [nvarchar](20) NULL,
 CONSTRAINT [PK_Main_Services] PRIMARY KEY CLUSTERED 
(
	[Main_Services_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sub_Services]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sub_Services](
	[Sub_Services_ID] [int] NOT NULL,
	[Sub_Services_Name_EN] [nvarchar](max) NULL,
	[Sub_Services_Name_AR] [nvarchar](max) NULL,
	[Main_Services_ID] [int] NULL,
	[IS_Action] [bit] NULL,
 CONSTRAINT [PK_Sub_Services] PRIMARY KEY CLUSTERED 
(
	[Sub_Services_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supporting_Documents]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supporting_Documents](
	[Supporting_Documents_ID] [int] NOT NULL,
	[Supporting_Documents_Name_EN] [nvarchar](max) NULL,
	[Supporting_Documents_Name_AR] [nvarchar](max) NULL,
	[IS_Action] [bit] NULL,
 CONSTRAINT [PK_Supporting_Documents] PRIMARY KEY CLUSTERED 
(
	[Supporting_Documents_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Request_Data]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Request_Data](
	[Request_Data_ID] [int] IDENTITY(1,1) NOT NULL,
	[Personel_Data_ID] [int] NULL,
	[Provider_Academic_Services_ID] [int] NULL,
	[Sub_Services_ID] [int] NULL,
	[Supporting_Documents_ID] [int] NULL,
	[Required_Fields_Notes] [nvarchar](max) NULL,
	[Request_File_ID] [int] NULL,
	[Service_Type_ID] [int] NULL,
	[Request_Type_ID] [int] NULL,
	[Request_Data_Date] [datetime] NULL,
	[Code_Generate] [nvarchar](100) NULL,
	[IS_Action] [bit] NULL,
	[Request_State_ID] [tinyint] NOT NULL,
	[Required_Documents_File] [varchar](max) NULL,
 CONSTRAINT [PK_Request_Data] PRIMARY KEY CLUSTERED 
(
	[Request_Data_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Title_Middle_Names]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Title_Middle_Names](
	[Title_Middle_Names_ID] [int] NOT NULL,
	[Title_Middle_Names_Name_EN] [nvarchar](250) NULL,
	[Title_Middle_Names_Name_AR] [nvarchar](250) NULL,
	[IS_Action] [bit] NULL,
 CONSTRAINT [PK_Title_Middle_Names] PRIMARY KEY CLUSTERED 
(
	[Title_Middle_Names_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Nationality]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nationality](
	[Nationality_ID] [int] NOT NULL,
	[Nationality_Name_EN] [nvarchar](250) NULL,
	[Nationality_Name_AR] [nvarchar](250) NULL,
	[IS_Action] [bit] NULL,
 CONSTRAINT [PK_Nationality] PRIMARY KEY CLUSTERED 
(
	[Nationality_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Country]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[Country_ID] [int] NOT NULL,
	[Country_Name_EN] [nvarchar](250) NULL,
	[Country_Name_AR] [nvarchar](250) NULL,
	[IS_Action] [bit] NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[Country_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Request_Type]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Request_Type](
	[Request_Type_ID] [int] NOT NULL,
	[Request_Type_Name_EN] [nvarchar](250) NULL,
	[Request_Type_Name_AR] [nvarchar](250) NULL,
	[IS_Action] [bit] NULL,
	[ImagePath] [varchar](max) NULL,
 CONSTRAINT [PK_Request_Type_ID] PRIMARY KEY CLUSTERED 
(
	[Request_Type_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service_Type]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service_Type](
	[Service_Type_ID] [int] NOT NULL,
	[Service_Type_Name_EN] [nvarchar](250) NULL,
	[Service_Type_Name_AR] [nvarchar](250) NULL,
	[IS_Action] [bit] NULL,
	[ImagePath] [varchar](max) NULL,
 CONSTRAINT [PK_Service_Type] PRIMARY KEY CLUSTERED 
(
	[Service_Type_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ID_Document]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ID_Document](
	[ID_Document] [int] NOT NULL,
	[Document_Name_EN] [nvarchar](250) NULL,
	[Document_Name_AR] [nvarchar](250) NULL,
	[IS_Action] [bit] NULL,
 CONSTRAINT [PK_ID_Document] PRIMARY KEY CLUSTERED 
(
	[ID_Document] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Personel_Data]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Personel_Data](
	[Personel_Data_ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_Document] [int] NOT NULL,
	[ID_Number] [nvarchar](100) NOT NULL,
	[IAU_Affiliate_ID] [int] NULL,
	[IAU_ID_Number] [nvarchar](100) NULL,
	[Applicant_Type_ID] [int] NULL,
	[Title_Middle_Names_ID] [int] NULL,
	[First_Name] [nvarchar](100) NULL,
	[Middle_Name] [nvarchar](100) NULL,
	[Last_Name] [nvarchar](100) NULL,
	[Nationality_ID] [int] NULL,
	[Country_ID] [int] NULL,
	[City_Country_1] [nvarchar](200) NULL,
	[City_Country_2] [nvarchar](200) NULL,
	[Region_Postal_Code_1] [nvarchar](100) NULL,
	[Region_Postal_Code_2] [nvarchar](100) NULL,
	[Email] [nvarchar](200) NULL,
	[Mobile] [nvarchar](20) NULL,
	[IS_Action] [bit] NULL,
 CONSTRAINT [PK_Personel_Data] PRIMARY KEY CLUSTERED 
(
	[Personel_Data_ID] ASC,
	[ID_Document] ASC,
	[ID_Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[View_Personel_Data]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Personel_Data]
AS
SELECT dbo.Request_Data.Request_Data_ID, dbo.Request_Data.Personel_Data_ID, dbo.Personel_Data.IAU_ID_Number, dbo.Personel_Data.Title_Middle_Names, dbo.Personel_Data.First_Name, dbo.Personel_Data.Middle_Name, 
                  dbo.Personel_Data.Last_Name, dbo.Personel_Data.ID_Number, dbo.Personel_Data.City_Country_1, dbo.Personel_Data.City_Country_2, dbo.Personel_Data.Region_Postal_Code_1, dbo.Personel_Data.Region_Postal_Code_2, 
                  dbo.Personel_Data.Email, dbo.Personel_Data.Mobile, dbo.Request_Data.Required_Fields_Notes, dbo.Request_Data.Required_Documents_File, dbo.Applicant_Type.Applicant_Type_Name_EN, dbo.ID_Document.Document_Name_EN, 
                  dbo.Main_Services.Main_Services_Name_EN, dbo.Service_Type.Service_Type_Name_EN, dbo.Request_Type.Request_Type_Name_EN, dbo.Country.Country_Name_EN, dbo.Title_Middle_Names.Title_Middle_Names_Name_EN, 
                  dbo.Sub_Services.Sub_Services_Name_EN, dbo.Supporting_Documents.Supporting_Documents_Name_EN, dbo.Provider_Academic_Services.Provider_Academic_Services_Name_EN
FROM     dbo.Personel_Data INNER JOIN
                  dbo.Request_Data ON dbo.Personel_Data.Personel_Data_ID = dbo.Request_Data.Personel_Data_ID INNER JOIN
                  dbo.Applicant_Type ON dbo.Personel_Data.Applicant_Type_ID = dbo.Applicant_Type.Applicant_Type_ID INNER JOIN
                  dbo.Country ON dbo.Personel_Data.Country_ID = dbo.Country.Country_ID INNER JOIN
                  dbo.ID_Document ON dbo.Personel_Data.ID_Document = dbo.ID_Document.ID_Document INNER JOIN
                  dbo.Main_Services ON dbo.Request_Data.Main_Services_ID = dbo.Main_Services.Main_Services_ID INNER JOIN
                  dbo.Nationality ON dbo.Personel_Data.Nationality_ID = dbo.Nationality.Nationality_ID INNER JOIN
                  dbo.Provider_Academic_Services ON dbo.Request_Data.Provider_Academic_Services_ID = dbo.Provider_Academic_Services.Provider_Academic_Services_ID INNER JOIN
                  dbo.Request_Type ON dbo.Personel_Data.Request_Type_ID = dbo.Request_Type.Request_Type_ID INNER JOIN
                  dbo.Service_Type ON dbo.Personel_Data.Service_Type_ID = dbo.Service_Type.Service_Type_ID INNER JOIN
                  dbo.Sub_Services ON dbo.Request_Data.Sub_Services_ID = dbo.Sub_Services.Sub_Services_ID INNER JOIN
                  dbo.Supporting_Documents ON dbo.Request_Data.Supporting_Documents_ID = dbo.Supporting_Documents.Supporting_Documents_ID INNER JOIN
                  dbo.Title_Middle_Names ON dbo.Personel_Data.Title_Middle_Names_ID = dbo.Title_Middle_Names.Title_Middle_Names_ID
GO
/****** Object:  Table [dbo].[Request_File]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Request_File](
	[Request_File_ID] [int] IDENTITY(1,1) NOT NULL,
	[Request_File_Name] [nvarchar](max) NULL,
	[Request_File_Path] [nvarchar](max) NULL,
	[Request_File_Date] [datetime] NULL,
 CONSTRAINT [PK_Request_File] PRIMARY KEY CLUSTERED 
(
	[Request_File_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Request_State]    Script Date: 3/18/2021 3:18:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Request_State](
	[Request_State_ID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Request_StateName_AR] [nvarchar](100) NOT NULL,
	[Request_StateName_EN] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Request_State] PRIMARY KEY CLUSTERED 
(
	[Request_State_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Applicant_Type] ([Applicant_Type_ID], [Applicant_Type_Name_EN], [Applicant_Type_Name_AR], [IS_Action]) VALUES (1, N'Parent- Undergraduate Student', NULL, N'1')
INSERT [dbo].[Applicant_Type] ([Applicant_Type_ID], [Applicant_Type_Name_EN], [Applicant_Type_Name_AR], [IS_Action]) VALUES (2, N'Parent- Graduate Student', NULL, N'1')
INSERT [dbo].[Applicant_Type] ([Applicant_Type_ID], [Applicant_Type_Name_EN], [Applicant_Type_Name_AR], [IS_Action]) VALUES (3, N'Undergraduate Student', NULL, N'1')
INSERT [dbo].[Applicant_Type] ([Applicant_Type_ID], [Applicant_Type_Name_EN], [Applicant_Type_Name_AR], [IS_Action]) VALUES (4, N'Graduate Student', NULL, N'1')
INSERT [dbo].[Applicant_Type] ([Applicant_Type_ID], [Applicant_Type_Name_EN], [Applicant_Type_Name_AR], [IS_Action]) VALUES (5, N'Faculty Member', NULL, N'1')
INSERT [dbo].[Applicant_Type] ([Applicant_Type_ID], [Applicant_Type_Name_EN], [Applicant_Type_Name_AR], [IS_Action]) VALUES (6, N'Researcher', NULL, N'1')
INSERT [dbo].[Applicant_Type] ([Applicant_Type_ID], [Applicant_Type_Name_EN], [Applicant_Type_Name_AR], [IS_Action]) VALUES (7, N'Employee', NULL, N'1')
INSERT [dbo].[Applicant_Type] ([Applicant_Type_ID], [Applicant_Type_Name_EN], [Applicant_Type_Name_AR], [IS_Action]) VALUES (8, N'Other', NULL, N'1')
GO
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (1, N'Afghanistan', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (2, N'Albania', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (3, N'Angola', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (4, N'Argentina', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (5, N'Bahrain', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (6, N'Belgium', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (7, N'Brazil', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (8, N'Bulgaria', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (9, N'China', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (10, N'Denmark', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (11, N'Egypt', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (12, N'France', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (13, N'Germany', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (14, N'India', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (15, N'Iran', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (16, N'Iraq', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (17, N'Italy', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (18, N'Japan', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (19, N'Kuwait', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (20, N'Libya', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (21, N'Morocco', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (22, N'Oman', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (23, N'Qatar', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (24, N'Saudi Arabia
', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (25, N'Spain', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (26, N'Sudan', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (27, N'Tunisia', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (28, N'Turkey', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (29, N'United Arab Emirates
', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (30, N'United States
', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (31, N'Yemen', NULL, 1)
INSERT [dbo].[Country] ([Country_ID], [Country_Name_EN], [Country_Name_AR], [IS_Action]) VALUES (32, N'Zimbabwe', NULL, 1)
GO
INSERT [dbo].[ID_Document] ([ID_Document], [Document_Name_EN], [Document_Name_AR], [IS_Action]) VALUES (1, N'National ID card', NULL, 1)
INSERT [dbo].[ID_Document] ([ID_Document], [Document_Name_EN], [Document_Name_AR], [IS_Action]) VALUES (2, N'Iqama', NULL, 1)
INSERT [dbo].[ID_Document] ([ID_Document], [Document_Name_EN], [Document_Name_AR], [IS_Action]) VALUES (3, N'Passport', NULL, 1)
GO
INSERT [dbo].[Main_Services] ([Main_Services_ID], [Main_Services_Name_EN], [Main_Services_Name_AR], [Provider_Academic_Services_ID], [IS_Action]) VALUES (1, N'Acceptance Method', NULL, NULL, N'1')
INSERT [dbo].[Main_Services] ([Main_Services_ID], [Main_Services_Name_EN], [Main_Services_Name_AR], [Provider_Academic_Services_ID], [IS_Action]) VALUES (2, N'Transfer to the University', NULL, NULL, N'1')
INSERT [dbo].[Main_Services] ([Main_Services_ID], [Main_Services_Name_EN], [Main_Services_Name_AR], [Provider_Academic_Services_ID], [IS_Action]) VALUES (3, N'Visiting from outside the University', NULL, NULL, N'1')
INSERT [dbo].[Main_Services] ([Main_Services_ID], [Main_Services_Name_EN], [Main_Services_Name_AR], [Provider_Academic_Services_ID], [IS_Action]) VALUES (4, N'External Scholarships', NULL, NULL, N'1')
INSERT [dbo].[Main_Services] ([Main_Services_ID], [Main_Services_Name_EN], [Main_Services_Name_AR], [Provider_Academic_Services_ID], [IS_Action]) VALUES (5, N'Communicate', NULL, NULL, N'1')
INSERT [dbo].[Main_Services] ([Main_Services_ID], [Main_Services_Name_EN], [Main_Services_Name_AR], [Provider_Academic_Services_ID], [IS_Action]) VALUES (6, N'Course Enrollment', NULL, NULL, N'1')
INSERT [dbo].[Main_Services] ([Main_Services_ID], [Main_Services_Name_EN], [Main_Services_Name_AR], [Provider_Academic_Services_ID], [IS_Action]) VALUES (7, N'Rejoin', NULL, NULL, N'1')
GO
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (1, N'Afghans', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (2, N'Albanian', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (3, N'Algerian', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (4, N'Argentinean', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (5, N'Bahraini', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (6, N'Brazilian', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (7, N'British', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (8, N'Chinese', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (9, N'Egyptian', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (10, N'French', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (11, N'German', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (12, N'Dutch', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (13, N'Indian', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (14, N'Iranian', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (15, N'Iraqi', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (16, N'Italian', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (17, N'Japanese', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (18, N'Kuwaiti', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (19, N'Lebanese', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (20, N'Libyan', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (21, N'Malaysian', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (22, N'Mexican', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (23, N'Moroccan', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (24, N'Omani', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (25, N'Philippine', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (26, N'Portuguese', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (27, N'Qatari', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (28, N'Russian', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (29, N'Saudi Arabian', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (30, N'Somali', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (31, N'South African', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (32, N'Spanish', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (33, N'Sudanese', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (34, N'Swiss', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (35, N'Syrian', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (36, N'Tunisian', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (37, N'Turkish', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (38, N'Emirati', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (39, N'American', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (40, N'Yemeni', NULL, 1)
INSERT [dbo].[Nationality] ([Nationality_ID], [Nationality_Name_EN], [Nationality_Name_AR], [IS_Action]) VALUES (41, N'Zambian', NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[Personel_Data] ON 

INSERT [dbo].[Personel_Data] ([Personel_Data_ID], [ID_Document], [ID_Number], [IAU_Affiliate_ID], [IAU_ID_Number], [Applicant_Type_ID], [Title_Middle_Names_ID], [First_Name], [Middle_Name], [Last_Name], [Nationality_ID], [Country_ID], [City_Country_1], [City_Country_2], [Region_Postal_Code_1], [Region_Postal_Code_2], [Email], [Mobile], [IS_Action]) VALUES (1, 1, N'25544', 0, N'2514', 1, 1, N'yasser', N'mohamed', NULL, 9, 11, N'tanta', N'zefta', N'3265', N'36588', N'yasserprogramer@hotmail.com', N'0125595456', NULL)
INSERT [dbo].[Personel_Data] ([Personel_Data_ID], [ID_Document], [ID_Number], [IAU_Affiliate_ID], [IAU_ID_Number], [Applicant_Type_ID], [Title_Middle_Names_ID], [First_Name], [Middle_Name], [Last_Name], [Nationality_ID], [Country_ID], [City_Country_1], [City_Country_2], [Region_Postal_Code_1], [Region_Postal_Code_2], [Email], [Mobile], [IS_Action]) VALUES (6, 1, N'25544', 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Personel_Data] OFF
GO
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (1, N'Office of the Vice President for Academic Affairs', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (2, N'Office of the Vice President for Postgraduate Studies and Scientific Research', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (3, N'Office of the Vice President for Studies, Development and Scientific Research', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (4, N'Deanship of Admissions and Registration', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (5, N'Deanship of Preparatory Year and Supporting Studies', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (6, N'Deanship of Graduate Studies', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (7, N'Deanship of Scientific Research', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (8, N'Deanship of Library Affairs', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (9, N'Deanship of E-Learning and Distance Learning', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (10, N'College of Medicine', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (11, N'College of Dentistry', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (12, N'College of Nursing', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (13, N'College of Applied Medical Sciences', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (14, N'College of Public Health', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (15, N'College of Applied Medical Sciences- Jubail', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (16, N'College of Architecture and Planning', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (17, N'College of Design', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (18, N'College of Engineering', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (19, N'College of Applied Studies and Community Science', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (20, N'College of Business Administration', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (21, N'College of Computer Science and Information Technology', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (22, N'College of Science', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (23, N'Community College', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (24, N'College of Arts', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (25, N'College of Education', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (26, N'College of Science and Humanities- Jubail', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (27, N'College of Sharia and Law', NULL, N'1')
INSERT [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID], [Provider_Academic_Services_Name_EN], [Provider_Academic_Services_Name_AR], [IS_Action]) VALUES (28, N'Institute of Research and Medical Consultations (IRMC)', NULL, N'1')
GO
SET IDENTITY_INSERT [dbo].[Request_Data] ON 

INSERT [dbo].[Request_Data] ([Request_Data_ID], [Personel_Data_ID], [Provider_Academic_Services_ID], [Sub_Services_ID], [Supporting_Documents_ID], [Required_Fields_Notes], [Request_File_ID], [Service_Type_ID], [Request_Type_ID], [Request_Data_Date], [Code_Generate], [IS_Action], [Request_State_ID], [Required_Documents_File]) VALUES (1, 1, 1, 1, 1, N'2555555', NULL, NULL, NULL, NULL, N'1234567891234', NULL, 1, N'1')
INSERT [dbo].[Request_Data] ([Request_Data_ID], [Personel_Data_ID], [Provider_Academic_Services_ID], [Sub_Services_ID], [Supporting_Documents_ID], [Required_Fields_Notes], [Request_File_ID], [Service_Type_ID], [Request_Type_ID], [Request_Data_Date], [Code_Generate], [IS_Action], [Request_State_ID], [Required_Documents_File]) VALUES (2, 1, 2, 2, 2, N'6989', NULL, NULL, NULL, NULL, N'1234567891235', NULL, 1, N'1')
SET IDENTITY_INSERT [dbo].[Request_Data] OFF
GO
SET IDENTITY_INSERT [dbo].[Request_State] ON 

INSERT [dbo].[Request_State] ([Request_State_ID], [Request_StateName_AR], [Request_StateName_EN]) VALUES (1, N'Saved', N'Saved')
INSERT [dbo].[Request_State] ([Request_State_ID], [Request_StateName_AR], [Request_StateName_EN]) VALUES (2, N'Finished', N'Finished')
INSERT [dbo].[Request_State] ([Request_State_ID], [Request_StateName_AR], [Request_StateName_EN]) VALUES (3, N'Under Processing', N'Under Processing')
INSERT [dbo].[Request_State] ([Request_State_ID], [Request_StateName_AR], [Request_StateName_EN]) VALUES (4, N'Deleted', N'Deleted')
SET IDENTITY_INSERT [dbo].[Request_State] OFF
GO
INSERT [dbo].[Request_Type] ([Request_Type_ID], [Request_Type_Name_EN], [Request_Type_Name_AR], [IS_Action], [ImagePath]) VALUES (1, N'CONSULTATION', NULL, 1, NULL)
INSERT [dbo].[Request_Type] ([Request_Type_ID], [Request_Type_Name_EN], [Request_Type_Name_AR], [IS_Action], [ImagePath]) VALUES (2, N'INCIDENT', NULL, 1, NULL)
INSERT [dbo].[Request_Type] ([Request_Type_ID], [Request_Type_Name_EN], [Request_Type_Name_AR], [IS_Action], [ImagePath]) VALUES (3, N'INQUERY', NULL, 1, NULL)
INSERT [dbo].[Request_Type] ([Request_Type_ID], [Request_Type_Name_EN], [Request_Type_Name_AR], [IS_Action], [ImagePath]) VALUES (4, N'COMPLAINT', NULL, 1, NULL)
INSERT [dbo].[Request_Type] ([Request_Type_ID], [Request_Type_Name_EN], [Request_Type_Name_AR], [IS_Action], [ImagePath]) VALUES (5, N'APPOINTMENT', NULL, 1, NULL)
INSERT [dbo].[Request_Type] ([Request_Type_ID], [Request_Type_Name_EN], [Request_Type_Name_AR], [IS_Action], [ImagePath]) VALUES (6, N'SUGGESTION', NULL, 1, NULL)
INSERT [dbo].[Request_Type] ([Request_Type_ID], [Request_Type_Name_EN], [Request_Type_Name_AR], [IS_Action], [ImagePath]) VALUES (7, N'MEETINGS', NULL, 1, NULL)
GO
INSERT [dbo].[Service_Type] ([Service_Type_ID], [Service_Type_Name_EN], [Service_Type_Name_AR], [IS_Action], [ImagePath]) VALUES (1, N'ACADMIC

 SERVICES', NULL, 1, NULL)
INSERT [dbo].[Service_Type] ([Service_Type_ID], [Service_Type_Name_EN], [Service_Type_Name_AR], [IS_Action], [ImagePath]) VALUES (2, N'ADMINISTRATIVE
 
SERVICES', NULL, 1, NULL)
INSERT [dbo].[Service_Type] ([Service_Type_ID], [Service_Type_Name_EN], [Service_Type_Name_AR], [IS_Action], [ImagePath]) VALUES (3, N'RESIDENTIAL

 SERVICES', NULL, 1, NULL)
INSERT [dbo].[Service_Type] ([Service_Type_ID], [Service_Type_Name_EN], [Service_Type_Name_AR], [IS_Action], [ImagePath]) VALUES (4, N'MEDICAL 
SERVICES', NULL, 1, NULL)
INSERT [dbo].[Service_Type] ([Service_Type_ID], [Service_Type_Name_EN], [Service_Type_Name_AR], [IS_Action], [ImagePath]) VALUES (5, N'SPORTIVE

 ACTIVITIES', NULL, 1, NULL)
INSERT [dbo].[Service_Type] ([Service_Type_ID], [Service_Type_Name_EN], [Service_Type_Name_AR], [IS_Action], [ImagePath]) VALUES (6, N'AMENITIES', NULL, 1, NULL)
INSERT [dbo].[Service_Type] ([Service_Type_ID], [Service_Type_Name_EN], [Service_Type_Name_AR], [IS_Action], [ImagePath]) VALUES (7, N'MAINTENANCE

 & SERVICES', NULL, 1, NULL)
GO
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (1, N'Submit a new application', NULL, 1, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (2, N'Review your application', NULL, 1, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (3, N'Withdrawal from admission', NULL, 1, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (4, N'Academic Number Inquiry', NULL, 1, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (5, N'HOW to reachus', NULL, 1, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (6, N'Apply for a New Transfer Request', NULL, 2, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (7, N'Follow upon a previous Application', NULL, 2, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (8, N'password Reset', NULL, 2, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (9, N'Apply for a request to visit IAU', NULL, 3, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (10, N'Follow up on a previous request to visit IAU', NULL, 3, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (11, N'Submit a request for External scholarships', NULL, 4, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (12, N'Follow up on a previous Request', NULL, 4, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (13, N'Inquiry or inquiry', NULL, 5, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (14, N'Make a complaint', NULL, 5, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (15, N'Student Homepage', NULL, 6, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (16, N'Manage Classes', NULL, 6, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (17, N'Student Center', NULL, 6, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (18, N'Search for a Class', NULL, 6, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (19, N'Select the Courses', NULL, 6, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (20, N'Add to Shopping Cart', NULL, 6, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (21, N'Shopping Cart', NULL, 6, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (22, N'Enroll', NULL, 6, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (23, N'Student Homepage', NULL, 7, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (24, N'E-Services', NULL, 7, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (25, N'Status Change Applications', NULL, 7, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (26, N'Rejoin Application workflow', NULL, 7, 1)
INSERT [dbo].[Sub_Services] ([Sub_Services_ID], [Sub_Services_Name_EN], [Sub_Services_Name_AR], [Main_Services_ID], [IS_Action]) VALUES (27, N'Add a New Value', NULL, 7, 1)
GO
INSERT [dbo].[Supporting_Documents] ([Supporting_Documents_ID], [Supporting_Documents_Name_EN], [Supporting_Documents_Name_AR], [IS_Action]) VALUES (1, N'General Aptitude Test Result', NULL, 1)
INSERT [dbo].[Supporting_Documents] ([Supporting_Documents_ID], [Supporting_Documents_Name_EN], [Supporting_Documents_Name_AR], [IS_Action]) VALUES (2, N'Copies of diplomas from your previous studies.', NULL, 1)
INSERT [dbo].[Supporting_Documents] ([Supporting_Documents_ID], [Supporting_Documents_Name_EN], [Supporting_Documents_Name_AR], [IS_Action]) VALUES (3, N'Academic Transcripts from your Bachelor''s studies.', NULL, 1)
INSERT [dbo].[Supporting_Documents] ([Supporting_Documents_ID], [Supporting_Documents_Name_EN], [Supporting_Documents_Name_AR], [IS_Action]) VALUES (4, N'Academic Transcripts from your High School studies.', NULL, 1)
INSERT [dbo].[Supporting_Documents] ([Supporting_Documents_ID], [Supporting_Documents_Name_EN], [Supporting_Documents_Name_AR], [IS_Action]) VALUES (5, N'Proof of language proficiency.', NULL, 1)
INSERT [dbo].[Supporting_Documents] ([Supporting_Documents_ID], [Supporting_Documents_Name_EN], [Supporting_Documents_Name_AR], [IS_Action]) VALUES (6, N'Motivation letter or Statement of Purpose.', NULL, 1)
INSERT [dbo].[Supporting_Documents] ([Supporting_Documents_ID], [Supporting_Documents_Name_EN], [Supporting_Documents_Name_AR], [IS_Action]) VALUES (7, N'Recent bank statement (within 3 months)', NULL, 1)
INSERT [dbo].[Supporting_Documents] ([Supporting_Documents_ID], [Supporting_Documents_Name_EN], [Supporting_Documents_Name_AR], [IS_Action]) VALUES (8, N'Proof of English proficiency by TOEFL or IELTS score', NULL, 1)
INSERT [dbo].[Supporting_Documents] ([Supporting_Documents_ID], [Supporting_Documents_Name_EN], [Supporting_Documents_Name_AR], [IS_Action]) VALUES (9, N'Passport identification page – scan or photocopy', NULL, 1)
INSERT [dbo].[Supporting_Documents] ([Supporting_Documents_ID], [Supporting_Documents_Name_EN], [Supporting_Documents_Name_AR], [IS_Action]) VALUES (10, N'Iqama Identification scan', NULL, 1)
INSERT [dbo].[Supporting_Documents] ([Supporting_Documents_ID], [Supporting_Documents_Name_EN], [Supporting_Documents_Name_AR], [IS_Action]) VALUES (11, N'Study permit (if currently studying in Saudi Arabia) ', NULL, 1)
INSERT [dbo].[Supporting_Documents] ([Supporting_Documents_ID], [Supporting_Documents_Name_EN], [Supporting_Documents_Name_AR], [IS_Action]) VALUES (12, N'Reference letters.', NULL, 1)
GO
INSERT [dbo].[Title_Middle_Names] ([Title_Middle_Names_ID], [Title_Middle_Names_Name_EN], [Title_Middle_Names_Name_AR], [IS_Action]) VALUES (1, N'Mr.', NULL, 1)
INSERT [dbo].[Title_Middle_Names] ([Title_Middle_Names_ID], [Title_Middle_Names_Name_EN], [Title_Middle_Names_Name_AR], [IS_Action]) VALUES (2, N'Mrs.', NULL, 1)
INSERT [dbo].[Title_Middle_Names] ([Title_Middle_Names_ID], [Title_Middle_Names_Name_EN], [Title_Middle_Names_Name_AR], [IS_Action]) VALUES (3, N'Ms.', NULL, 1)
INSERT [dbo].[Title_Middle_Names] ([Title_Middle_Names_ID], [Title_Middle_Names_Name_EN], [Title_Middle_Names_Name_AR], [IS_Action]) VALUES (4, N'Dr.', NULL, 1)
INSERT [dbo].[Title_Middle_Names] ([Title_Middle_Names_ID], [Title_Middle_Names_Name_EN], [Title_Middle_Names_Name_AR], [IS_Action]) VALUES (5, N'Prof.', NULL, 1)
INSERT [dbo].[Title_Middle_Names] ([Title_Middle_Names_ID], [Title_Middle_Names_Name_EN], [Title_Middle_Names_Name_AR], [IS_Action]) VALUES (6, N'Other.', NULL, 1)
GO
ALTER TABLE [dbo].[Applicant_Type] ADD  CONSTRAINT [DF_Applicant_Type_IS_Action]  DEFAULT (N'True') FOR [IS_Action]
GO
ALTER TABLE [dbo].[Country] ADD  CONSTRAINT [DF_Country_IS_Action]  DEFAULT ((1)) FOR [IS_Action]
GO
ALTER TABLE [dbo].[ID_Document] ADD  CONSTRAINT [DF_ID_Document_IS_Action]  DEFAULT ((1)) FOR [IS_Action]
GO
ALTER TABLE [dbo].[Main_Services] ADD  CONSTRAINT [DF_Main_Services_IS_Action]  DEFAULT (N'True') FOR [IS_Action]
GO
ALTER TABLE [dbo].[Nationality] ADD  CONSTRAINT [DF_Nationality_IS_Action]  DEFAULT ((1)) FOR [IS_Action]
GO
ALTER TABLE [dbo].[Personel_Data] ADD  CONSTRAINT [DF_Personel_Data_IAU_Affiliate_ID]  DEFAULT ((0)) FOR [IAU_Affiliate_ID]
GO
ALTER TABLE [dbo].[Personel_Data] ADD  CONSTRAINT [DF_Personel_Data_IS_Action]  DEFAULT ((1)) FOR [IS_Action]
GO
ALTER TABLE [dbo].[Provider_Academic_Services] ADD  CONSTRAINT [DF_Provider_Academic_Services_IS_Action]  DEFAULT (N'True') FOR [IS_Action]
GO
ALTER TABLE [dbo].[Request_Data] ADD  CONSTRAINT [DF_Request_Data_IS_Action]  DEFAULT ((1)) FOR [IS_Action]
GO
ALTER TABLE [dbo].[Request_Type] ADD  CONSTRAINT [DF_Request_Type_IS_Action]  DEFAULT ((1)) FOR [IS_Action]
GO
ALTER TABLE [dbo].[Service_Type] ADD  CONSTRAINT [DF_Service_Type_IS_Action]  DEFAULT ((1)) FOR [IS_Action]
GO
ALTER TABLE [dbo].[Sub_Services] ADD  CONSTRAINT [DF_Sub_Services_IS_Action]  DEFAULT ((1)) FOR [IS_Action]
GO
ALTER TABLE [dbo].[Supporting_Documents] ADD  CONSTRAINT [DF_Supporting_Documents_IS_Action]  DEFAULT ((1)) FOR [IS_Action]
GO
ALTER TABLE [dbo].[Title_Middle_Names] ADD  CONSTRAINT [DF_Title_Middle_Names_IS_Action]  DEFAULT ((1)) FOR [IS_Action]
GO
ALTER TABLE [dbo].[Main_Services]  WITH CHECK ADD  CONSTRAINT [FK_Main_Services_Provider_Academic_Services] FOREIGN KEY([Provider_Academic_Services_ID])
REFERENCES [dbo].[Provider_Academic_Services] ([Provider_Academic_Services_ID])
GO
ALTER TABLE [dbo].[Main_Services] CHECK CONSTRAINT [FK_Main_Services_Provider_Academic_Services]
GO
ALTER TABLE [dbo].[Personel_Data]  WITH CHECK ADD  CONSTRAINT [FK_Personel_Data_Applicant_Type] FOREIGN KEY([Applicant_Type_ID])
REFERENCES [dbo].[Applicant_Type] ([Applicant_Type_ID])
GO
ALTER TABLE [dbo].[Personel_Data] CHECK CONSTRAINT [FK_Personel_Data_Applicant_Type]
GO
ALTER TABLE [dbo].[Personel_Data]  WITH CHECK ADD  CONSTRAINT [FK_Personel_Data_Country] FOREIGN KEY([Country_ID])
REFERENCES [dbo].[Country] ([Country_ID])
GO
ALTER TABLE [dbo].[Personel_Data] CHECK CONSTRAINT [FK_Personel_Data_Country]
GO
ALTER TABLE [dbo].[Personel_Data]  WITH CHECK ADD  CONSTRAINT [FK_Personel_Data_ID_Document] FOREIGN KEY([ID_Document])
REFERENCES [dbo].[ID_Document] ([ID_Document])
GO
ALTER TABLE [dbo].[Personel_Data] CHECK CONSTRAINT [FK_Personel_Data_ID_Document]
GO
ALTER TABLE [dbo].[Personel_Data]  WITH CHECK ADD  CONSTRAINT [FK_Personel_Data_Nationality] FOREIGN KEY([Nationality_ID])
REFERENCES [dbo].[Nationality] ([Nationality_ID])
GO
ALTER TABLE [dbo].[Personel_Data] CHECK CONSTRAINT [FK_Personel_Data_Nationality]
GO
ALTER TABLE [dbo].[Personel_Data]  WITH CHECK ADD  CONSTRAINT [FK_Personel_Data_Title_Middle_Names] FOREIGN KEY([Title_Middle_Names_ID])
REFERENCES [dbo].[Title_Middle_Names] ([Title_Middle_Names_ID])
GO
ALTER TABLE [dbo].[Personel_Data] CHECK CONSTRAINT [FK_Personel_Data_Title_Middle_Names]
GO
ALTER TABLE [dbo].[Request_Data]  WITH CHECK ADD  CONSTRAINT [FK_Request_Data_Request_State] FOREIGN KEY([Request_State_ID])
REFERENCES [dbo].[Request_State] ([Request_State_ID])
GO
ALTER TABLE [dbo].[Request_Data] CHECK CONSTRAINT [FK_Request_Data_Request_State]
GO
ALTER TABLE [dbo].[Sub_Services]  WITH CHECK ADD  CONSTRAINT [FK_Sub_Services_Main_Services] FOREIGN KEY([Main_Services_ID])
REFERENCES [dbo].[Main_Services] ([Main_Services_ID])
GO
ALTER TABLE [dbo].[Sub_Services] CHECK CONSTRAINT [FK_Sub_Services_Main_Services]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[25] 4[39] 2[6] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Personel_Data"
            Begin Extent = 
               Top = 14
               Left = 546
               Bottom = 369
               Right = 827
            End
            DisplayFlags = 280
            TopColumn = 10
         End
         Begin Table = "Request_Data"
            Begin Extent = 
               Top = 0
               Left = 998
               Bottom = 307
               Right = 1300
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Applicant_Type"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 148
               Right = 316
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Country"
            Begin Extent = 
               Top = 570
               Left = 655
               Bottom = 711
               Right = 872
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ID_Document"
            Begin Extent = 
               Top = 154
               Left = 48
               Bottom = 295
               Right = 283
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Main_Services"
            Begin Extent = 
               Top = 301
               Left = 48
               Bottom = 442
               Right = 306
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Nationality"
            Begin Extent = 
               Top = 389
               Left = 478
               Bottom = 530
               Right = 717
            End' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Personel_Data'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Provider_Academic_Services"
            Begin Extent = 
               Top = 13
               Left = 1400
               Bottom = 154
               Right = 1752
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Request_Type"
            Begin Extent = 
               Top = 539
               Left = 316
               Bottom = 680
               Right = 573
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Service_Type"
            Begin Extent = 
               Top = 448
               Left = 48
               Bottom = 589
               Right = 299
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Sub_Services"
            Begin Extent = 
               Top = 384
               Left = 1452
               Bottom = 547
               Right = 1702
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Supporting_Documents"
            Begin Extent = 
               Top = 177
               Left = 1421
               Bottom = 318
               Right = 1742
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Title_Middle_Names"
            Begin Extent = 
               Top = 432
               Left = 830
               Bottom = 573
               Right = 1130
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 26
         Width = 284
         Width = 1200
         Width = 1200
         Width = 1992
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1728
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 4668
         Alias = 900
         Table = 2700
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1356
         SortOrder = 1416
         GroupBy = 1350
         Filter = 1356
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Personel_Data'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Personel_Data'
GO
USE [master]
GO
ALTER DATABASE [MostafidDatabase] SET  READ_WRITE 
GO
