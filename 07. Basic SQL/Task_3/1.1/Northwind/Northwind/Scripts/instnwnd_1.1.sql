/*
** Northwind -> 1.1
*/
SET NOCOUNT ON
GO

USE "Northwind"
GO

SET DATEFORMAT mdy
GO

SET quoted_identifier ON
GO

if exists (select * from sysobjects where id = object_id('dbo.EmployeeCreditCards') and sysstat & 0xf = 3)
	drop table "dbo"."EmployeeCreditCards"
GO

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
