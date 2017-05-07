CREATE TABLE [dbo].[kulut] (
    [rowid]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [timestamp] DATETIME       NULL,
    [tyyppi]    INT            NULL,
    [paikkaID]  INT            NULL,
    [maara]     FLOAT (53)     NULL,
    [selite]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_kulut] PRIMARY KEY CLUSTERED ([rowid] ASC),
    CONSTRAINT [FK_kulut_kulutyypit] FOREIGN KEY ([tyyppi]) REFERENCES [dbo].[kulutyypit] ([id]),
    CONSTRAINT [FK_kulut_paikat] FOREIGN KEY ([paikkaID]) REFERENCES [dbo].[paikat] ([rowid])
);

