
CREATE TABLE [dbo].[raw-metrics](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Period] [datetime] NULL,
	[Module] [varchar](4096) NULL,
	[Namespace] [varchar](4096) NULL,
	[Type] [varchar](4096) NULL,
	[Member] [varchar](4096) NULL,
	[Raw] [varchar](4096) NULL,
	[MemberName] [varchar](500) NULL,
	[ReturnType] [varchar](100) NULL,
	[Language] [varchar](20) NULL,
	[MaintainabilityIndex] [int] NULL,
	[CyclomaticComplexity] [int] NULL,
	[ClassCoupling] [int] NULL,
	[DepthOfInheritance] [int] NULL,
	[LinesOfCode] [int] NULL
) ON [PRIMARY]
