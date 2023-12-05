CREATE DATABASE MUSICAPP

GO
USE MUSICAPP

CREATE TABLE PLAYLIST (
	idPlaylist int primary key IDENTITY(1,1),
	PlaylistName NVARCHAR(MAX) not null,
	TotalSong int not null,
	Created datetime not null,
);

CREATE TABLE SONG (
	idSong int primary key IDENTITY(1,1),
	SongName NVARCHAR(MAX) not null,
	Artist NVARCHAR(MAX),
	TotalTime time not null,
	FilePath NVARCHAR(MAX) not null,
	Created Datetime not null,
	idPlaylist INT FOREIGN KEY REFERENCES PLAYLIST on delete cascade,
);