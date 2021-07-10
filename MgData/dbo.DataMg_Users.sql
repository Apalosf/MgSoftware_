CREATE TABLE [dbo].[DataMg_Users] (
    [Users_Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Users_UserName] VARCHAR (50) NOT NULL,
    [Users_Password] VARCHAR (50) NOT NULL,
    [Users_FirsName] VARCHAR (50) NOT NULL,
    [Users_LastName] VARCHAR (50) NOT NULL,
    [Users_Position] VARCHAR (50) NOT NULL,
    [Users_Email]    NCHAR (100)  NOT NULL,
    [Users_Photo]    BIT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Users_Id] ASC)
)
go
--PROCEDIMIENTOS ALMACENADOS
create proc AddUser
	@Users_UserName nvarchar(100),
	@Users_Password nvarchar(100),
	@Users_FirsName nvarchar(100),
	@Users_LastName nvarchar(100),
	@Users_Position nvarchar(100),
	@Users_Email nvarchar(100),
	@Users_Photo varbinary(max)--foto de perfil
	as
	insert into DataMg_Users 
	values (@Users_UserName,@Users_Password,@Users_FirsName,@Users_LastName,@Users_Position,@Users_Email,@Users_Photo)
go

create proc EditUser
	@Users_UserName nvarchar(100),
	@Users_Password nvarchar(100),
	@Users_FirsName nvarchar(100),
	@Users_LastName nvarchar(100),
	@Users_Position nvarchar(100),
	@Users_Email nvarchar(100),
	@Users_Photo varbinary(max),--foto de perfil
	@Users_Id int
	as
	update  DataMg_Users	
	set Users_UserName=@Users_UserName,Users_Password=@Users_Password,Users_FirsName=@Users_FirsName,Users_LastName=@Users_LastName,Users_Position= @Users_Position,Users_Email=@Users_Email,Users_Photo=@Users_Photo  
	where Users_Id=@Users_Id 
go

create proc RemoveUser
	@Users_Id int
	as
	delete from DataMg_Users where Users_Id=@Users_Id 
go

create proc LoginUser
	@Users_UserName nvarchar (100),
	@Users_Password nvarchar (100)
	as
	select *from DataMg_Users 
	where (Users_UserName=@Users_UserName and Users_Password=@Users_Password ) or (Users_Email=@Users_UserName and Users_Password=@Users_Password)
go

create proc SelectAllUsers
	as
	select *from DataMg_Users  
go

create proc SelectUser
	@findValue nvarchar (100)
	as
	select *from Users 
	where userName= @findValue or firstName like @findValue+'%' or email=@findValue
go