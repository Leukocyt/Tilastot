CREATE TABLE [dbo].[kulutyypit] (
    [id]     INT            IDENTITY (1, 1) NOT NULL,
    [kuvaus] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_kulutyypit] PRIMARY KEY CLUSTERED ([id] ASC)
);

