create table Actors
(
    Id        int identity
        primary key,
    Name      nvarchar(255) not null,
    PhotoUrl  nvarchar(max),
    Biography nvarchar(max)
)
go

create table Genres
(
    Id   int identity
        primary key,
    Name nvarchar(100) not null
)
go

create table Movies
(
    Id                  int identity
        primary key,
    Title               nvarchar(255) not null,
    Description         nvarchar(max),
    VerticalImageUrl    nvarchar(max),
    HorizontalImageUrl  nvarchar(max),
    IsSubscriptionBased bit           not null,
    GenreId             int
        references Genres,
    Rating              decimal(3, 1),
    ReleaseYear         int,
    Duration            nvarchar(20)
)
go

create table MovieActors
(
    Id      int identity
        primary key,
    MovieId int not null
        references Movies
            on delete cascade,
    ActorId int not null
        references Actors
            on delete cascade
)
go

create table Reviews
(
    Id        int identity
        primary key,
    MovieId   int not null
        references Movies
            on delete cascade,
    UserName  nvarchar(255),
    Rating    int
        check ([Rating] >= 1 AND [Rating] <= 10),
    Comment   nvarchar(max),
    CreatedAt datetime default getdate()
)
go

create table Users
(
    Id       int identity
        primary key,
    UserName nvarchar(255) not null
        unique,
    Password nvarchar(max) not null,
    IsAdmin  bit default 0
)
go
