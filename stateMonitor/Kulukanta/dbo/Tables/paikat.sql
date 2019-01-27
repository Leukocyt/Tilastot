CREATE TABLE [dbo].[paikat] (
    [rowid]    INT            IDENTITY (1, 1) NOT NULL,
    [selite]   NVARCHAR (MAX) NULL,
    [kaupunki] NVARCHAR (MAX) NULL,
    [cityID]   INT            NULL,
    CONSTRAINT [PK_paikat] PRIMARY KEY CLUSTERED ([rowid] ASC)
);



