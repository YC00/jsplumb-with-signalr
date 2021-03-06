SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_Link]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_Link](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[LinkID] [varchar](50) NULL,
	[LinkLabel] [varchar](50) NULL,
	[StartNode] [varchar](50) NULL,
	[EndNode] [varchar](50) NULL,
	[CID] [varchar](50) NULL,
	[Color] [varchar](50) NULL,
	[CreatedDateTime] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_RootLeafPath]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_RootLeafPath](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CID] [varchar](50) NULL,
	[nodepath] [text] NULL,
	[linkpath] [text] NULL,
	[StartNode] [varchar](50) NULL,
	[EndNode] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_PathCompareResult]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_PathCompareResult](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sCID] [varchar](50) NULL,
	[tCID] [varchar](50) NULL,
	[rootleaf] [varchar](50) NULL,
	[path] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_Application]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_Application](
	[ApplicationID] [bigint] NULL,
	[Description] [varchar](50) NULL,
	[Type] [varchar](50) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_Info]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_Info](
	[CID] [int] IDENTITY(1,1) NOT NULL,
	[CName] [varchar](50) NULL,
	[SystemName] [varchar](50) NULL,
	[SystemID] [varchar](50) NULL,
	[GroupID] [varchar](50) NULL,
 CONSTRAINT [PK_ConceptMap_Info] PRIMARY KEY CLUSTERED 
(
	[CID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_NodeApplication]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_NodeApplication](
	[NodeApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[NodeID] [varchar](50) NULL,
	[CID] [varchar](50) NULL,
	[ApplicationID] [varchar](50) NULL,
	[FieldName] [varchar](50) NULL,
	[FieldValue] [varchar](50) NULL,
	[OptionName] [varchar](50) NULL,
	[OptionValue] [varchar](50) NULL,
 CONSTRAINT [PK_ConceptMap_NodeApplication] PRIMARY KEY CLUSTERED 
(
	[NodeApplicationID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_TemplateNode]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_TemplateNode](
	[TemplateNodeID] [int] IDENTITY(1,1) NOT NULL,
	[TemplateID] [int] NULL,
	[FieldName] [varchar](50) NULL,
	[FieldValue] [varchar](50) NULL,
	[Type] [varchar](50) NULL,
	[Show] [int] NULL,
 CONSTRAINT [PK_ConceptMap_TemplateNode] PRIMARY KEY CLUSTERED 
(
	[TemplateNodeID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_Template]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_Template](
	[TemplateID] [int] IDENTITY(1,1) NOT NULL,
	[TemplateName] [varchar](50) NULL,
 CONSTRAINT [PK_ConceptMap_Template] PRIMARY KEY CLUSTERED 
(
	[TemplateID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_TemplateGroup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_TemplateGroup](
	[TemplateGroupID] [int] IDENTITY(1,1) NOT NULL,
	[TemplateGroupName] [varchar](255) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_TemplateGroupValue]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_TemplateGroupValue](
	[TemplateGroupValueID] [int] IDENTITY(1,1) NOT NULL,
	[TemplateGroupID] [int] NULL,
	[TemplateID] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_LinkTemplate]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_LinkTemplate](
	[TemplateID] [int] IDENTITY(1,1) NOT NULL,
	[TemplateName] [varchar](50) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_TemplateLinkData]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_TemplateLinkData](
	[TemplateNodeID] [int] IDENTITY(1,1) NOT NULL,
	[TemplateID] [int] NULL,
	[FieldName] [varchar](50) NULL,
	[FieldValue] [varchar](50) NULL,
	[Type] [varchar](50) NULL,
	[Show] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_NodeFieldsData]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_NodeFieldsData](
	[FieldsID] [int] IDENTITY(1,1) NOT NULL,
	[NodeID] [varchar](100) NULL,
	[TemplateID] [int] NULL,
	[FieldName] [varchar](50) NULL,
	[FieldValue] [varchar](50) NULL,
	[Type] [varchar](50) NULL,
	[Show] [int] NULL,
	[CreatedDateTime] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_Node]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_Node](
	[NodeID] [varchar](50) NULL,
	[CID] [varchar](50) NULL,
	[UserID] [varchar](50) NULL,
	[NodeLabel] [varchar](50) NULL,
	[Xcoordinate] [varchar](50) NULL,
	[Ycoordinate] [varchar](50) NULL,
	[Width] [varchar](50) NULL,
	[Height] [varchar](50) NULL,
	[ParentNode] [varchar](50) NULL,
	[Level] [varchar](50) NULL,
	[Color] [varchar](50) NULL,
	[CreatedDateTime] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_User](
	[cUserID] [varchar](20) NOT NULL,
	[cPassword] [varchar](20) NOT NULL,
	[cUserName] [varchar](50) NOT NULL,
	[cAuthority] [varchar](10) NOT NULL,
	[cEmail] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[LastLoginDate] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_SelectedTemplate]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_SelectedTemplate](
	[ORCSGroupID] [varchar](50) NULL,
	[TemplateID] [varchar](50) NULL,
	[TemplateColor] [varchar](50) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_SelectedLinkTemplate]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_SelectedLinkTemplate](
	[ORCSGroupID] [varchar](50) NULL,
	[TemplateID] [varchar](50) NULL,
	[TemplateColor] [varchar](50) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_LinkFieldsData]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_LinkFieldsData](
	[FieldsID] [int] IDENTITY(1,1) NOT NULL,
	[NodeID] [varchar](100) NULL,
	[CID] [varchar](50) NULL,
	[TemplateID] [int] NULL,
	[StartNode] [varchar](50) NULL,
	[EndNode] [varchar](50) NULL,
	[FieldName] [varchar](50) NULL,
	[FieldValue] [varchar](50) NULL,
	[Type] [varchar](50) NULL,
	[Show] [int] NULL,
	[CreatedDateTime] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_tmpuserlist]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_tmpuserlist](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NULL,
	[connectionid] [varchar](50) NULL,
	[groupid] [varchar](50) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_Keyword]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_Keyword](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](100) NULL,
	[description] [text] NULL,
	[url] [varchar](1000) NULL,
	[image] [varchar](100) NULL,
	[video] [varchar](100) NULL,
	[ORCSGroupID] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConceptMap_Path]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConceptMap_Path](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CID] [varchar](50) NULL,
	[nodepath] [text] NULL,
	[linkpath] [text] NULL,
	[StartNode] [varchar](50) NULL,
	[EndNode] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
