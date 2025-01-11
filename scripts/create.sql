create table IviDb.dbo.Actors (
  Id int primary key not null,
  Name nvarchar(255) not null,
  PhotoUrl nvarchar(max),
  Biography nvarchar(max)
);
GO

create table IviDb.dbo.Genres (
  Id int primary key not null,
  Name nvarchar(100) not null
);
GO

create table IviDb.dbo.MovieActors (
  Id int primary key not null,
  MovieId int not null,
  ActorId int not null,
  foreign key (MovieId) references Movies (Id),
  foreign key (ActorId) references Actors (Id)
);
GO

create table IviDb.dbo.Movies (
  Id int primary key not null,
  Title nvarchar(255) not null,
  Description nvarchar(max),
  VerticalImageUrl nvarchar(max),
  HorizontalImageUrl nvarchar(max),
  IsSubscriptionBased bit not null,
  GenreId int,
  Rating decimal(3,1),
  ReleaseYear int,
  Duration nvarchar(20),
  foreign key (GenreId) references Genres (Id)
);
GO

create table IviDb.dbo.Reviews (
);
GO

create table IviDb.dbo.Users (
  Id int primary key not null,
  UserName nvarchar(255) not null,
  Password nvarchar(max) not null,
  IsAdmin bit default ((0))
);
create unique index UQ__Users__C9F284569056D2CC on Users (UserName);
GO