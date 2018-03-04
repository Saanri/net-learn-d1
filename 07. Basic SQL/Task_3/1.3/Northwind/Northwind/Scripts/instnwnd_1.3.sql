/*
** Northwind -> 1.3
*/

SET NOCOUNT ON
GO

USE "Northwind"
GO

SET quoted_identifier ON
GO

if exists (select * from sysobjects where id = object_id('dbo.Region') and sysstat & 0xf = 3)
    exec sp_rename "Region", "Regions"
GO

if exists(select * from information_schema.columns where table_name = 'Customers' and column_name = 'BegDate') 
    alter table Customers
        add BegDate "datetime" NULL
GO

if not exists (select * from sysobjects where id = object_id('dbo.EmployeeCreditCards') and sysstat & 0xf = 3)
    CREATE TABLE "EmployeeCreditCards" (
        "CreditCardsID" "int" IDENTITY (1, 1) NOT NULL,
        "EndDate" "datetime" NULL,
        "CardHolderName" nvarchar(50) NULL,
        "EmployeeID" "int" NULL,
        CONSTRAINT "PK_EmployeeCreditCards" PRIMARY KEY CLUSTERED 
        (
            "CreditCardsID"
        ),
        CONSTRAINT "FK_EmployeeCreditCards_Employees" FOREIGN KEY 
        (
            "EmployeeID"
        ) REFERENCES "dbo"."Employees" (
            "EmployeeID"
        )
    )
GO
