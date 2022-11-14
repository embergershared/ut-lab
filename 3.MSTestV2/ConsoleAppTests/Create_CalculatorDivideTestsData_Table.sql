CREATE TABLE [dbo].[CalculatorDivideTestsData](
 [Id] [int] NOT NULL PRIMARY KEY IDENTITY(1,1),
 [Arg1Value] [float] NULL,
 [Arg2Value] [float] NULL,
 [ExpectedValue] [float] NULL
) ON [PRIMARY]
GO

INSERT INTO [dbo].[CalculatorDivideTestsData] ( [Arg1Value], [Arg2Value], [ExpectedValue])
VALUES (10, 2, 5);

INSERT INTO [dbo].[CalculatorDivideTestsData] ( [Arg1Value], [Arg2Value], [ExpectedValue])
VALUES (34.67, 9.6, 3.6114);

INSERT INTO [dbo].[CalculatorDivideTestsData] ( [Arg1Value], [Arg2Value], [ExpectedValue])
VALUES (56, 0, 0);

INSERT INTO [dbo].[CalculatorDivideTestsData] ( [Arg1Value], [Arg2Value], [ExpectedValue])
VALUES (10, -5, -2);