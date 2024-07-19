if object_id(N'dbo.Users',N'U')  is null begin create table Users (
 UserID INT PRIMARY KEY IDENTITY(1,1),
 UserName varchar(255) unique not null , 
 Passphrase varchar(255) 

); 


if object_id(N'dbo.Cats',N'U')  is null begin  CREATE TABLE Cats (
    CatID INT PRIMARY KEY IDENTITY(1,1), 
    Tags VARCHAR(255) NOT NULL, 
    UserID INT FOREIGN KEY REFERENCES Users(UserID) on delete cascade, 
    DateRequested DATETIME, 
    Favorite BIT
)