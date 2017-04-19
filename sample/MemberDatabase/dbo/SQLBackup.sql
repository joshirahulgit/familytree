USE [master]
GO
/****** Object:  Database [Library]    Script Date: 4/18/2017 9:01:09 PM ******/
CREATE DATABASE [Library]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Library', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\Library.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Library_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\Library_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Library] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Library].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Library] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Library] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Library] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Library] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Library] SET ARITHABORT OFF 
GO
ALTER DATABASE [Library] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Library] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Library] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Library] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Library] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Library] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Library] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Library] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Library] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Library] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Library] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Library] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Library] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Library] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Library] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Library] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Library] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Library] SET RECOVERY FULL 
GO
ALTER DATABASE [Library] SET  MULTI_USER 
GO
ALTER DATABASE [Library] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Library] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Library] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Library] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Library] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Library', N'ON'
GO
ALTER DATABASE [Library] SET QUERY_STORE = OFF
GO
USE [Library]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [Library]
GO
/****** Object:  UserDefinedFunction [dbo].[getChildMostMembers]    Script Date: 4/18/2017 9:01:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create FUNCTION [dbo].[getChildMostMembers] ()
RETURNS @Children TABLE 
(
    -- Columns returned by the function
    MemberID int PRIMARY KEY NOT NULL, 
    FirstName nvarchar(50) NOT NULL, 
    LastName nvarchar(50) NULL, 
    DateOfBirth datetime NOT NULL, 
    Sex nvarchar(1) NOT NULL,
	MMemberID int NULL,
	FMemberID int NULL,
	hasChild bit,
	hasParent bit
)
AS 
-- Returns the first name, last name, date of birth, sex, mother member id and father member id.
BEGIN 
	INSERT INTO @Children 
	SELECT m.Id as MemberID, m.FirstName,m.LastName,m.DateOfBirth, m.Sex, m.MotherId as MMemberID , m.FatherId as FMemberID, dbo.hasChild(m.Id) as hasChild, dbo.hasParent(m.Id) as hasParent 
    from Member m
    where not exists (select null from Member where FatherId = m.Id or MotherId =m.Id);
	return
End


GO
/****** Object:  UserDefinedFunction [dbo].[getChildrenMembers]    Script Date: 4/18/2017 9:01:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create FUNCTION [dbo].[getChildrenMembers] (	@MemberId int)
RETURNS @Children TABLE 
(
    -- Columns returned by the function
    MemberID int PRIMARY KEY NOT NULL, 
    FirstName nvarchar(50) NOT NULL, 
    LastName nvarchar(50) NULL, 
    DateOfBirth datetime NOT NULL, 
    Sex nvarchar(1) NOT NULL,
	MMemberID int NULL,
	FMemberID int NULL,
	hasChild bit,
	hasParent bit
)
AS 
-- Returns the first name, last name, date of birth, sex, mother member id and father member id.
BEGIN
DECLARE 
        @Sex nvarchar(1);
		Select @Sex= (select m.sex from dbo.Member m where m.Id=@MemberID);

		if (@Sex = 'M')
			INSERT INTO @Children 
			SELECT m.Id as MemberID, m.FirstName,m.LastName,m.DateOfBirth, m.Sex, m.MotherId as MMemberID , 
					m.FatherId as FMemberID , dbo.hasChild(m.Id) as hasChild, dbo.hasParent(m.Id) as hasParent 
			from dbo.Member m 
			where m.FatherId=@MemberId;

		else
			INSERT INTO @Children 
			SELECT m.Id as MemberID, m.FirstName,m.LastName,m.DateOfBirth, m.Sex, m.MotherId as MMemberID , 
					m.FatherId as FMemberID , dbo.hasChild(m.Id) as hasChild, dbo.hasParent(m.Id) as hasParent 
			from dbo.Member m 
			where m.MotherId=@MemberId;

		return
End

GO
/****** Object:  UserDefinedFunction [dbo].[getMember]    Script Date: 4/18/2017 9:01:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[getMember](@MemberID int)
RETURNS @Member TABLE 
(
    -- Columns returned by the function
    MemberID int PRIMARY KEY NOT NULL, 
    FirstName nvarchar(50) NOT NULL, 
    LastName nvarchar(50) NULL, 
    DateOfBirth datetime NOT NULL, 
    Sex nvarchar(1) NOT NULL,
	MMemberID int NULL,
	FMemberID int NULL,
	hasChild bit,
	hasParent bit
)
AS 
-- Returns the first name, last name, date of birth, sex, mother member id and father member id.
BEGIN
    INSERT into @Member
    SELECT m.Id as MemberID, m.FirstName,m.LastName,m.DateOfBirth, m.Sex, m.MotherId as MMemberID , 
	m.FatherId as FMemberID, dbo.hasChild(m.Id) as hasChild, dbo.hasParent(m.Id) as hasParent 
	from dbo.Member m 
	where m.Id=@MemberId;
    
    RETURN;
END;


GO
/****** Object:  UserDefinedFunction [dbo].[getMemberIncludingParentAndChildren]    Script Date: 4/18/2017 9:01:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Create FUNCTION [dbo].[getMemberIncludingParentAndChildren](@MemberID int)
RETURNS @Member TABLE 
(
    -- Columns returned by the function
    MemberID int PRIMARY KEY NOT NULL, 
    FirstName nvarchar(50) NOT NULL, 
    LastName nvarchar(50) NULL, 
    DateOfBirth datetime NOT NULL, 
    Sex nvarchar(1) NOT NULL,
	MMemberID int NULL,
	FMemberID int NULL,
	hasChild bit,
	hasParent bit
)
AS 
-- Returns the first name, last name, date of birth, sex, mother member id and father member id.
BEGIN 
    if (dbo.hasParent(@MemberID)=1)
		INSERT into @Member
		SELECT *
		from dbo.getParentMembers(@MemberId);

    INSERT into @Member
    SELECT *
	from dbo.getMember(@MemberId);

	if (dbo.hasChild(@MemberID)=1)
		INSERT into @Member
		SELECT *
		from dbo.getChildrenMembers(@MemberId);

    RETURN;
END;



GO
/****** Object:  UserDefinedFunction [dbo].[getParentMembers]    Script Date: 4/18/2017 9:01:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[getParentMembers] (	@MemberId int)
RETURNS @Parents TABLE 
(
    -- Columns returned by the function
    MemberID int PRIMARY KEY NOT NULL, 
    FirstName nvarchar(50) NOT NULL, 
    LastName nvarchar(50) NULL, 
    DateOfBirth datetime NOT NULL, 
    Sex nvarchar(1) NOT NULL,
	MMemberID int NULL,
	FMemberID int NULL,
	hasChild bit,
	hasParent bit
)
AS 
-- Returns the first name, last name, date of birth, sex, mother member id and father member id.
BEGIN
	declare @MId int,
			@FId int;
	Select @MId=(select m.MotherId from dbo.Member m where m.Id=@MemberId);
	Select @FId=(select m.FatherId from dbo.Member m where m.Id=@MemberId);
	INSERT INTO @Parents 
	SELECT m.Id as MemberID, m.FirstName,m.LastName,m.DateOfBirth, m.Sex, m.MotherId as MMemberID , 
	m.FatherId as FMemberID , dbo.hasChild(m.Id) as hasChild, dbo.hasParent(m.Id) as hasParent
	from dbo.Member m where m.Id in (@FId,@MId);
	return
End



GO
/****** Object:  UserDefinedFunction [dbo].[getParentMostMembers]    Script Date: 4/18/2017 9:01:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



create FUNCTION [dbo].[getParentMostMembers] ()
RETURNS @Parents TABLE 
(
    -- Columns returned by the function
    MemberID int PRIMARY KEY NOT NULL, 
    FirstName nvarchar(50) NOT NULL, 
    LastName nvarchar(50) NULL, 
    DateOfBirth datetime NOT NULL, 
    Sex nvarchar(1) NOT NULL,
	MMemberID int NULL,
	FMemberID int NULL,
	hasChild bit,
	hasParent bit
)
AS 
-- Returns the first name, last name, date of birth, sex, mother member id and father member id.
BEGIN 
	INSERT INTO @Parents
	SELECT m.Id as MemberID, m.FirstName, m.LastName,
		m.DateOfBirth, m.Sex, m.MotherId as MMemberID , 
		m.FatherId as FMemberID, dbo.hasChild(m.Id) as hasChild, 
		dbo.hasParent(m.Id) as hasParent 
    from Member m
    where m.FatherId is null and m.MotherId is null;
	return
End



GO
/****** Object:  UserDefinedFunction [dbo].[getRootMember]    Script Date: 4/18/2017 9:01:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[getRootMember](@MemberID int)
returns int
as 
BEGIN 
	declare @RootMemberId int;
	declare @SearchId int;
	declare @Sex varchar(1);
	Select @Sex= (select m.sex from dbo.Member m where m.MotherId=@MemberID);

	select @RootMemberId=(select MotherId from dbo.Member m where m.Id=@MemberID);	

	return @RootMemberId; 
END;

GO
/****** Object:  UserDefinedFunction [dbo].[hasChild]    Script Date: 4/18/2017 9:01:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rahul
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
create FUNCTION [dbo].[hasChild](@MemberId int)
RETURNS bit
AS
BEGIN
	declare @Sex varchar(1);
	declare @Res bit
	select @Sex=(select m.Sex from dbo.Member m where m.Id=@MemberId);
	-- Declare the return variable here
	if(@Sex='M' and (select COUNT(m.id) from dbo.Member m where m.FatherId=@MemberId)>0)
		select @Res = 1;
	else if(@Sex='F' and (select COUNT(m.id) from dbo.Member m where m.MotherId=@MemberId)>0)
		select @Res = 1;
	else 
		select @Res = 0;
	return @Res
END

GO
/****** Object:  UserDefinedFunction [dbo].[hasParent]    Script Date: 4/18/2017 9:01:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Rahul
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
create FUNCTION [dbo].[hasParent](@MemberId int)
RETURNS bit
AS
BEGIN
	declare @Res bit,
			@MId int,
			@FId int;
	Select @MId=(select m.MotherId from dbo.Member m where m.Id=@MemberId);
	Select @FId=(select m.FatherId from dbo.Member m where m.Id=@MemberId);
	if( @MId is not null or @FId is not null)
		select @Res = 1;
	else 
		select @Res = 0;
	return @Res
END


GO
/****** Object:  UserDefinedFunction [dbo].[isMemberExist]    Script Date: 4/18/2017 9:01:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Rahul
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
create FUNCTION [dbo].[isMemberExist](@MemberId int)
RETURNS bit
AS
BEGIN
	Declare @Res bit;
	if ((select COUNT(m.id) from dbo.Member m where m.Id=@MemberId)>0)
	Select @Res = 1;
	else
	Select @Res = 0;
	return @Res;
END


GO
/****** Object:  Table [dbo].[Member]    Script Date: 4/18/2017 9:01:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Member](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[Sex] [char](1) NOT NULL,
	[MotherId] [bigint] NULL,
	[FatherId] [bigint] NULL,
 CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Member] ON 

INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (1, N'Ramanugrah', N'Pramad', CAST(N'1940-11-28T00:00:00.000' AS DateTime), N'M', NULL, NULL)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (2, N'Urmil', N'Dovita', CAST(N'1961-03-23T00:00:00.000' AS DateTime), N'F', NULL, NULL)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (3, N'Tara', N'Devi', CAST(N'1961-03-31T00:00:00.000' AS DateTime), N'F', NULL, NULL)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (4, N'RamGovind', N'Ram', CAST(N'1965-03-23T00:00:00.000' AS DateTime), N'M', NULL, NULL)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (5, N'Anil', N'Shah', CAST(N'1972-03-03T00:00:00.000' AS DateTime), N'M', 3, 4)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (7, N'Vershaben', N'Patel', CAST(N'1959-03-04T00:00:00.000' AS DateTime), N'F', NULL, NULL)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (8, N'Udatai', N'Josef', CAST(N'1964-10-26T00:00:00.000' AS DateTime), N'M', 2, 1)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (9, N'Ramana', N'Jose', CAST(N'1965-10-29T00:00:00.000' AS DateTime), N'F', 3, 4)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (10, N'Sunil', N'Shah', CAST(N'1962-07-13T00:00:00.000' AS DateTime), N'M', 3, 4)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (12, N'Mita', N'Jate', CAST(N'1900-01-01T00:00:00.000' AS DateTime), N'F', 7, NULL)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (13, N'Raul', N'Thomos', CAST(N'1987-10-02T00:00:00.000' AS DateTime), N'M', 9, 8)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (14, N'Ramata', N'', CAST(N'1900-01-01T00:00:00.000' AS DateTime), N'M', 12, 13)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (18, N'KantiBhai', N'Patel', CAST(N'1963-11-01T00:00:00.000' AS DateTime), N'M', NULL, NULL)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (26, N'Sushil', N'Shah', CAST(N'1973-04-18T00:00:00.000' AS DateTime), N'M', 3, 4)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (27, N'Mita', N'Patil', CAST(N'1988-04-18T00:00:00.000' AS DateTime), N'F', 32, 31)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (28, N'mira', N'patil', CAST(N'1999-04-18T00:00:00.000' AS DateTime), N'F', 27, 30)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (29, N'Ram', N'Patil', CAST(N'1999-04-18T00:00:00.000' AS DateTime), N'M', 27, 30)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (30, N'Rama', N'Patil', CAST(N'1988-03-18T00:00:00.000' AS DateTime), N'M', NULL, NULL)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (31, N'Thomas', N'Cook', CAST(N'1955-04-18T00:00:00.000' AS DateTime), N'M', NULL, NULL)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (32, N'Liz', N'Beham', CAST(N'1956-04-18T00:00:00.000' AS DateTime), N'F', 34, 33)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (33, N'Trover', N'Cook', CAST(N'1920-04-18T00:00:00.000' AS DateTime), N'M', NULL, NULL)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (34, N'Irene', N'Cook', CAST(N'1920-04-18T00:00:00.000' AS DateTime), N'F', NULL, NULL)
INSERT [dbo].[Member] ([Id], [FirstName], [LastName], [DateOfBirth], [Sex], [MotherId], [FatherId]) VALUES (35, N'Sameul', N'Kadam', CAST(N'1954-09-18T00:00:00.000' AS DateTime), N'M', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Member] OFF
ALTER TABLE [dbo].[Member]  WITH CHECK ADD  CONSTRAINT [FK_Father_Member] FOREIGN KEY([FatherId])
REFERENCES [dbo].[Member] ([Id])
GO
ALTER TABLE [dbo].[Member] CHECK CONSTRAINT [FK_Father_Member]
GO
ALTER TABLE [dbo].[Member]  WITH CHECK ADD  CONSTRAINT [FK_Mother_Member] FOREIGN KEY([MotherId])
REFERENCES [dbo].[Member] ([Id])
GO
ALTER TABLE [dbo].[Member] CHECK CONSTRAINT [FK_Mother_Member]
GO
/****** Object:  StoredProcedure [dbo].[sp_deleteMember]    Script Date: 4/18/2017 9:01:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rahul Joshi
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_deleteMember] 
	-- input parameters
	@MemberId int,
	-- output parameters
	@IsSuccess bit output
AS
BEGIN
	BEGIN TRAN
	-- declare a table to keep deep-parent id.
		DECLARE @Delete TABLE
		(
			id int
		);
		WITH IdsToDelete (id)
		AS (
		  SELECT Id
		  FROM Member
		  WHERE Id = @MemberId
		  UNION ALL
		  SELECT s.Id
		  FROM Member s
		  INNER JOIN IdsToDelete I ON I.id = s.MotherId or I.id=s.FatherId
		)
		INSERT INTO @Delete (id)
		SELECT id FROM IdsToDelete
		DELETE FROM Member WHERE Id IN (Select DISTINCT id from @Delete)
		--Select * from @Delete

	IF (@@Error != 0)
			Begin
			ROLLBACK TRAN
			select @IsSuccess=0;
			End
        ELSE
			Begin
			COMMIT TRAN
			select @IsSuccess=1;
			end
        END
	Return @MemberId;

GO
/****** Object:  StoredProcedure [dbo].[sp_getMember]    Script Date: 4/18/2017 9:01:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Create Procedure [dbo].[sp_getMember]
@MemberID int,

    -- Columns returned by the function
    @Id int output, 
    @FirstName nvarchar(50) output, 
    @LastName nvarchar(50) output, 
    @DateOfBirth datetime output, 
    @Sex nvarchar(1) output,
	@MMemberID int output,
	@FMemberID int output,
	@hasChild bit output,
	@hasParent bit output
AS 
-- Returns the first name, last name, date of birth, sex, mother member id and father member id.

    SELECT @Id = m.Id, @FirstName = m.FirstName, @LastName = m.LastName, @DateOfBirth = m.DateOfBirth, @Sex = m.Sex,@MMemberID =  m.MotherId , 
	@FMemberID = m.FatherId , @hasChild = dbo.hasChild(m.Id) , @hasParent = dbo.hasParent(m.Id) 
	from dbo.Member m 
	where m.Id=@MemberId;
    
    RETURN;



GO
/****** Object:  StoredProcedure [dbo].[sp_saveMember]    Script Date: 4/18/2017 9:01:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE Procedure [dbo].[sp_saveMember]
-- input parameters
@MemberID int output, 
@FirstName nvarchar(50), 
@LastName nvarchar(50), 
@DateOfBirth datetime, 
@Sex nvarchar(1),
@MMemberID int,
@FMemberID int,

-- output parameters
@IsSuccess bit output
AS 

IF EXISTS(SELECT * FROM Member WHERE ID = @MemberID)
  -- Member exists, update details
  BEGIN
    BEGIN TRAN
      UPDATE Member
        SET 
          FirstName = @FirstName,
		  LastName=@LastName,
		  DateOfBirth=@DateOfBirth,
		  Sex=@Sex,
		  MotherId=@MMemberID,
		  FatherId=@FMemberID
        WHERE ID = @MemberID

        IF (@@Error != 0)
			Begin
			ROLLBACK TRAN
			select @IsSuccess=0;
			End
        ELSE
			Begin
			COMMIT TRAN
			select @IsSuccess=1;
			end
        END
ELSE
  -- New user
  BEGIN
    BEGIN TRAN
      INSERT Member(
        FirstName,
		LastName,
		DateOfBirth,
		Sex,
		MotherId,
		FatherId
      )
        VALUES (
          @FirstName,
		  @LastName,
		  @DateOfBirth,
		  @Sex,
		  @MMemberID,
		  @FMemberID
      ) 

      SELECT @MemberID = @@IDENTITY

      IF (@@Error != 0)
			Begin
			ROLLBACK TRAN
			select @IsSuccess=0;
			End
        ELSE
			Begin
			COMMIT TRAN
			select @IsSuccess=1;
			end
      END    
    RETURN @MemberID;

GO
USE [master]
GO
ALTER DATABASE [Library] SET  READ_WRITE 
GO
