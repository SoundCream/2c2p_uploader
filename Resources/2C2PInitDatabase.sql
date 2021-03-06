
/****** Object:  Table [dbo].[Transactions]    Script Date: 9/4/2020 1:06:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[Id] [nvarchar](50) NOT NULL,
	[Amount] [decimal](18, 8) NOT NULL,
	[CurrencyCode] [nvarchar](3) NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[StatusId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TransactionStatus]    Script Date: 9/4/2020 1:06:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionStatus](
	[Id] [int] NOT NULL,
	[StatusName] [nvarchar](50) NOT NULL,
	[StatusCode] [nvarchar](1) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_TransactionStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[TransactionStatus] ([Id], [StatusName], [StatusCode], [IsActive]) VALUES (1, N'Approved', N'A', 1)
GO
INSERT [dbo].[TransactionStatus] ([Id], [StatusName], [StatusCode], [IsActive]) VALUES (2, N'Rejected', N'R', 1)
GO
INSERT [dbo].[TransactionStatus] ([Id], [StatusName], [StatusCode], [IsActive]) VALUES (3, N'Done', N'D', 1)
GO
