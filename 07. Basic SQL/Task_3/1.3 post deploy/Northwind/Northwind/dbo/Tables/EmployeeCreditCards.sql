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