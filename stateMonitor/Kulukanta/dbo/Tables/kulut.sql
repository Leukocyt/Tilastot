CREATE TABLE [dbo].[kulut] (
    [rowid]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [timestamp] DATETIME       NOT NULL,
    [tyyppi]    INT            NULL,
    [paikkaID]  INT            NULL,
    [maara]     FLOAT (53)     NULL,
    [selite]    NVARCHAR (MAX) NULL,
    [cityID]    INT            NULL,
    [added_at]  DATETIME       CONSTRAINT [DF_kulut_added_at] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_kulut] PRIMARY KEY CLUSTERED ([rowid] ASC),
    CONSTRAINT [FK_kulut_kaupungit] FOREIGN KEY ([rowid]) REFERENCES [dbo].[kulut] ([rowid]),
    CONSTRAINT [FK_kulut_kulutyypit] FOREIGN KEY ([tyyppi]) REFERENCES [dbo].[kulutyypit] ([id]),
    CONSTRAINT [FK_kulut_paikat] FOREIGN KEY ([cityID]) REFERENCES [dbo].[kaupungit] ([row_id])
);



