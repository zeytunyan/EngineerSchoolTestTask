CREATE TABLE [dbo].[Employee]
(
	[Id] INT NOT NULL IDENTITY, 
    [DepartmentId] INT NOT NULL, 
    [ChiefId] INT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Salary] MONEY NOT NULL, 
    CONSTRAINT [PK_Employee] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Employee_Employee] FOREIGN KEY ([ChiefId]) REFERENCES [Employee]([Id]), 
    CONSTRAINT [FK_Employee_ToTable] FOREIGN KEY ([DepartmentId]) REFERENCES [Department]([Id]) 
)
