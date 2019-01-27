CREATE TABLE [dbo].[kaupungit] (
    [row_id]     INT            IDENTITY (1, 1) NOT NULL,
    [name]       NVARCHAR (90)  NULL,
    [state]      NVARCHAR (500) NULL,
    [country]    NVARCHAR (500) NULL,
    [created_at] DATETIME       CONSTRAINT [DF_kaupungit_created_at] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_kaupungit] PRIMARY KEY CLUSTERED ([row_id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [nimi]
    ON [dbo].[kaupungit]([name] ASC);

