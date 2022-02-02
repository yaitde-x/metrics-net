
CREATE TABLE [dbo].[raw-metrics](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Period] [datetime] NOT NULL,
	[Module] [varchar](4096) NOT NULL,
	[Namespace] [varchar](4096) NOT NULL,
	[Type] [varchar](4096) NOT NULL,
	[Member] [varchar](4096) NOT NULL,
	[Raw] [varchar](4096) NOT NULL,
	[MemberName] [varchar](500) NOT NULL,
	[ReturnType] [varchar](100) NOT NULL,
	[Language] [varchar](20) NOT NULL,
	[MaintainabilityIndex] [int] NOT NULL,
	[CyclomaticComplexity] [int] NOT NULL,
	[ClassCoupling] [int] NOT NULL,
	[DepthOfInheritance] [int] NOT NULL,
	[LinesOfCode] [int] NOT NULL
) ON [PRIMARY]
