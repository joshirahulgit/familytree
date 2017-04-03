CREATE TABLE [dbo].[Member] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [FirstName]   NVARCHAR (50) NOT NULL,
    [LastName]    NVARCHAR (50) NULL,
    [DateOfBirth] DATETIME      NOT NULL,
    [Sex]         CHAR (1)      NOT NULL,
    [MotherId]    BIGINT        NULL,
    [FatherId]    BIGINT        NULL,
    CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = ON),
    CONSTRAINT [FK_Father_Member] FOREIGN KEY ([FatherId]) REFERENCES [dbo].[Member] ([Id]),
    CONSTRAINT [FK_Mother_Member] FOREIGN KEY ([MotherId]) REFERENCES [dbo].[Member] ([Id])
);

