CREATE TABLE [dbo].[Department]
(
	[Id] INT NOT NULL IDENTITY , 
    [Name] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [AK_Table_Column] UNIQUE ([Name]), 
    CONSTRAINT [PK_Table] PRIMARY KEY ([Id])
)
