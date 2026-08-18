-- Application sql server table definitions:
CREATE TABLE [dbo].[tbl_Destination] -- Warehouses
(
	[DestinationId] [int] IDENTITY(1,1) NOT NULL,
	[DestinationTitle] [nvarchar](50) NULL,
	[DestinationSt] [int] NULL,
	[DestinationDesc] [nvarchar](max) NULL,
	[DestinationCode] [nvarchar](50) NULL,
	[DestinationType] [int] NULL,
	[DestinationParentId] [int] NULL,
	[DestinationParentsId] [nvarchar](max) NULL,
	[DestinationEpc] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[tbl_DestinationType](
	[fld_Id] [int] IDENTITY(1,1) NOT NULL,
	[fld_DestinationTypeCode] [nvarchar](50) NULL,
	[fld_DestinationTypeName] [nvarchar](250) NULL
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[tbl_ProductBrand](
	[fld_ProductBrandId] [int] IDENTITY(1,1) NOT NULL,
	[fld_ProductBrandCode] [nvarchar](128) NOT NULL,
	[fld_ProductBrandTitle] [nvarchar](128) NOT NULL,
	[fld_ProductBrandData] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[fld_ProductBrandId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[tbl_ProductClass](
	[fld_ProductClassId] [int] IDENTITY(1,1) NOT NULL,
	[fld_ProductClassCode] [nvarchar](128) NOT NULL,
	[fld_ProductClassTitle] [nvarchar](256) NOT NULL,
	[fld_ProductClassSubTitle] [nvarchar](512) NULL,
	[fld_ProductClassDesc] [nvarchar](512) NULL
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[tbl_ProductGroup](
	[fld_ProductGroupId] [int] IDENTITY(1,1) NOT NULL,
	[fld_ProductGroupCode] [nvarchar](128) NOT NULL,
	[fld_ProductGroupTitle] [nvarchar](128) NOT NULL,
	[fld_ProductGroupData] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[fld_ProductGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[tbl_ProductPropertyA](
	[fld_ProductPropertyAId] [nvarchar](128) NOT NULL,
	[fld_ProductPropertyATitle] [nvarchar](256) NOT NULL,
	[fld_ProductPropertyADesc] [nvarchar](max) NULL,
	[fld_ProductPropertyAData] [nvarchar](max) NOT NULL,
 CONSTRAINT [IX_tbl_ProductPropertyATitleUnique] UNIQUE NONCLUSTERED 
(
	[fld_ProductPropertyATitle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[tbl_ProductPropertyB](
	[fld_ProductPropertyBId] [nvarchar](128) NOT NULL,
	[fld_ProductPropertyBTitle] [nvarchar](512) NOT NULL,
	[fld_ProductPropertyBDesc] [nvarchar](max) NULL,
	[fld_ProductPropertyBData] [nvarchar](max) NOT NULL,
	[fld_ProductPropertyAId] [nvarchar](128) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[tbl_ProductPropertyC](
	[fld_ProductPropertyCId] [nvarchar](128) NOT NULL,
	[fld_ProductPropertyCTitle] [nvarchar](256) NOT NULL,
	[fld_ProductPropertyCDesc] [nvarchar](max) NULL,
	[fld_ProductPropertyCData] [nvarchar](max) NOT NULL,
	[fld_ProductPropertyCIdentity] [int] IDENTITY(1,1) NOT NULL,
	[fld_ProductPropertyCTemp] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[tbl_ProductStatus] -- Quality
(
	[ProductStatusId] [int] IDENTITY(1,1) NOT NULL,
	[ProductStatusTitle] [nvarchar](50) NULL,
	[ProductStatusCode] [nvarchar](50) NULL,
	[ProductStatusDesc] [nvarchar](max) NULL,
 CONSTRAINT [PK__tbl_Prod__2082058B59904A2C] PRIMARY KEY CLUSTERED 
(
	[ProductStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[tbl_ProductSubGroup](
	[fld_ProductSubGroupId] [int] IDENTITY(1,1) NOT NULL,
	[fld_ProductSubGroupCode] [nvarchar](128) NOT NULL,
	[fld_ProductSubGroupTitle] [nvarchar](256) NOT NULL,
	[fld_ProductSubGroupSubTitle] [nvarchar](512) NULL,
	[fld_ProductGroupCode] [nvarchar](128) NOT NULL,
	[fld_ProductSubGroupDesc] [nvarchar](512) NULL
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[tbl_ProductType](
	[ProductTypeId] [int] IDENTITY(1,1) NOT NULL,
	[ProductTypeTitle] [nvarchar](50) NULL,
	[ProductTypeParentId] [nvarchar](10) NULL,
	[ProductTypeParentsId] [nvarchar](max) NULL,
	[ProductTypeCode] [nvarchar](50) NULL,
 CONSTRAINT [PK__tbl_Prod__A1312F6E5C6CB6D7] PRIMARY KEY CLUSTERED (
	[ProductTypeId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[tbl_Tags] -- Product that are in Warehouse
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductSerial] [nvarchar](50) NOT NULL,
	[ProductCode] [nvarchar](50) NULL,
	[TagEpc] [nvarchar](50) NOT NULL,
	[ProjectCode] [nvarchar](50) NULL,
	[ProductCount] [decimal](18, 2) NULL,
	[ProductName] [nvarchar](max) NULL,
	[ProductType] [nvarchar](256) NULL,
	[ProductStatus] [nvarchar](256) NULL,
	[TagStatus] [nvarchar](50) NULL,
	[TagRegisterShamsiUnixDate] [nvarchar](50) NULL, -- Data has format "{persian year - 4 digits}{persian month - 2 digits}{persian day - 2 digits}{hour - 2 digits}{minute - 2 digits}{second - 2 digits}" for example 14020618131759  
	[TagRegisterUser] [nvarchar](50) NULL,
	[TagTreeParentId] [int] NULL,
	[TagTreeSecondParentId] [int] NULL,
	[TagTreeParentsId] [nvarchar](max) NULL,
	[NewProductSerial] [nvarchar](50) NULL,
	[ProductProperties] [nvarchar](max) NULL,
	[Lock] [bit] NULL,
	[Username] [nvarchar](50) NULL,
	[DeviceId] [nvarchar](50) NULL,
	[DeviceIp] [nvarchar](50) NULL,
	[Freeze] [bit] NULL,
	[Deactivate] [bit] NULL,
	[TagInActionId] [int] NULL,
	[TagInDestinationId] [nvarchar](50) NULL, -- Warehouse code
	[TagInActionId2] [int] NULL,
	[TagInDestinationId2] [nvarchar](50) NULL,
	[fld_ProductPropertyAId] [nvarchar](50) NULL,
	[fld_ProductPropertyBId] [nvarchar](50) NULL,
	[fld_ProductPropertyCId] [nvarchar](50) NULL,
	[RegCode] [nvarchar](50) NULL,
	[fld_LastModifierUser] [nvarchar](128) NULL,
	[ContractStatus] [nvarchar](50) NULL,
	[TagZone] [nvarchar](50) NULL,
	[TagRegisterDateTime] [datetime] NULL, -- Register datetime 
	[ReProduct] [bit] NULL,
	[fld_InspectActionId] [int] NULL,
	[fld_LastInspectResult] [nvarchar](max) NULL,
	[fld_ProductGroup] [nvarchar](128) NULL,
	[fld_ProductBrand] [nvarchar](128) NULL,
	[fld_ProductSubGroup] [nvarchar](128) NULL,
	[fld_ProductClass] [nvarchar](128) NULL,
	[TagTreeParentSerial] [nvarchar](50) NULL,
	[TagTreeParentsEpc] [nvarchar](128) NULL,
 CONSTRAINT [PK_tbl_Tags_1] PRIMARY KEY CLUSTERED 
(
	[ProductSerial] ASC,
	[TagEpc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[tbl_User](
	[Id] [nvarchar](128) NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatorIdentityID] [nvarchar](128) NOT NULL,
	[LastModifierIdentityID] [nvarchar](128) NULL,
	[Name] [nvarchar](512) NOT NULL,
	[Row] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[Username] [nvarchar](256) NOT NULL,
	[Details] [nvarchar](max) NULL,
	[Image] [nvarchar](50) NULL,
 CONSTRAINT [PK_dbo.tbl_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[tbl_Station](
	[fld_StationId] [int] IDENTITY(1,1) NOT NULL,
	[fld_StationCode] [nvarchar](128) NULL,
	[fld_StationName] [nvarchar](512) NULL,
	[fld_StationType] [int] NULL,
	[fld_StationActionType] [int] NULL,
	[fld_StationStatus] [int] NULL,
	[fld_StationReaders] [nvarchar](max) NULL,
	[fld_StationDescription] [nvarchar](1024) NULL,
	[fld_StationSettings] [nvarchar](max) NULL,
	[fld_StationFromDestination] [nvarchar](50) NULL,
	[fld_StationToDestination] [nvarchar](50) NULL,
	[fld_StationMacAddress] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[tbl_ActionTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[fld_ActionTypeId] [int] NULL,
	[fld_ActionTypeFromDestinationType] [nvarchar](50) NULL,
	[fld_ActionTypeToTypeDestinationType] [nvarchar](50) NULL,
	[fld_ActionTypeTitle] [nvarchar](50) NULL,
	[fld_ActionTypePermitedDocStatus] [nvarchar](50) NULL,
	[fld_ActionTypeChangeDocStatus] [nvarchar](50) NULL,
	[fld_ActionTypeActiveControls] [nvarchar](max) NULL,
	[fld_ActionTypeRfidPower] [int] NULL,
	[fld_ActionTypeChangeTagLocation] [int] NULL,
	[fld_ActionTypeProductType] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[tbl_TagsMovement](
	[TagsMovementId] [int] IDENTITY(1,1) NOT NULL,
	[TagEpc] [nvarchar](50) NULL,
	[ProductCode] [nvarchar](50) NULL,
	[ProductSerial] [nvarchar](50) NULL,
	[ProductCount] [decimal](18, 2) NULL,
	[HMovementActionId] [int] NULL,
	[HTagsMovementDate] [nvarchar](10) NULL,
	[HTagsMovementTime] [nvarchar](5) NULL,
	[HTagsMovementDateTime] [datetime] NULL,
	[HTagsMovementSt] [int] NULL,
	[RMovementActionId] [int] NULL,
	[RTagsMovementDate] [nvarchar](10) NULL,
	[RTagsMovementTime] [nvarchar](5) NULL,
	[RTagsMovementDateTime] [datetime] NULL,
	[RTagsMovementSt] [int] NULL,
	[MovementData] [nvarchar](max) NULL,
	[ApiSendStatus] [int] NULL,
	[ApiSendUser] [nvarchar](128) NULL,
	[ApiSendDateTime] [datetime] NULL,
	[ApiSendData] [nvarchar](max) NULL,
	[RMovementActionType] [int] NULL,
	[RMovementActionDocumentId] [nvarchar](512) NULL,
	[RMovementActionUHFLogId] [nvarchar](50) NULL,
	[RMovementActionDestinationId] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tbl_TagsMovement] ADD  CONSTRAINT [DF_tbl_TagsMovement_HTagsMovementSt]  DEFAULT ((0)) FOR [HTagsMovementSt]
GO

ALTER TABLE [dbo].[tbl_TagsMovement] ADD  CONSTRAINT [DF_tbl_TagsMovement_ApiSendStatus]  DEFAULT ((0)) FOR [ApiSendStatus]
GO
CREATE TABLE [dbo].[tbl_MovementActions](
	[MovementActionId] [int] NULL,
	[MovementActionTp] [int] NULL,
	[MovementActionStore] [nvarchar](50) NULL,
	[MovementActionUserId] [nvarchar](max) NULL,
	[MovementActionDate] [nvarchar](10) NULL,
	[MovementActionTime] [nvarchar](5) NULL,
	[MovementActionDateTime] [datetime] NULL,
	[MovementActionCountTags] [int] NULL,
	[MovementActionDestinationId] [nvarchar](50) NULL,
	[MovementActionCarPlaque] [nvarchar](16) NULL,
	[MovementActionDriverName] [nvarchar](50) NULL,
	[MovementActionDriverMobile] [nvarchar](50) NULL,
	[MovementActionData] [nvarchar](max) NULL,
	[MovementActionUHFLogId] [nvarchar](50) NULL,
	[MovementActionLinkId] [int] NULL,
	[MovementActionLinkDestId] [nvarchar](50) NULL,
	[MovementActionTruckCrossId] [bigint] NULL,
	[MovementActionDesc] [nvarchar](max) NULL,
	[MovementActionDocumentId] [nvarchar](512) NULL,
	[MovementActionUHFLogGate] [nvarchar](30) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_MovementActions] ADD  CONSTRAINT [DF_tbl_MovementActions_MovementActionTruckCrossId]  DEFAULT ((0)) FOR [MovementActionTruckCrossId]
GO
CREATE TABLE [dbo].[tbl_Products](
	[ProductCode] [nvarchar](50) NOT NULL,
	[ProductTitle] [nvarchar](250) NULL,
	[ProductTechnicalCode] [nvarchar](50) NULL,
	[ProductSize] [nvarchar](50) NULL,
	[ProductType] [nvarchar](50) NULL,
	[ProductStatus] [nvarchar](50) NULL,
	[fld_ProductGroup] [nvarchar](128) NULL,
	[fld_ProductBrand] [nvarchar](128) NULL,
	[fld_ProductSubGroup] [nvarchar](128) NULL,
	[fld_ProductClass] [nvarchar](128) NULL,
	[ProductProperties] [nvarchar](max) NULL,
	[ProductTechnicalData] [nvarchar](max) NULL
)
CREATE TABLE [dbo].[tbl_Zones] -- Locations of warehouse
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ZoneCode] [nvarchar](50) NULL,
	[ZoneTitle] [nvarchar](50) NULL,
	[ZoneCapacity] [decimal](18, 2) NULL,
	[ZoneDimention] [nvarchar](50) NULL,
	[ZoneParentCode] [nvarchar](50) NULL,
	[ZoneParentLayer] [int] NULL,
	[ZoneStoreCode] [nvarchar](50) NULL, -- Warehouse code
	[ZoneCountPixle] [int] NULL,
	[ZoneOccupiedCapacity] [int] NULL,
	[MinZoneCapacity] [decimal](18, 2) NULL,
	[MaxZoneCapacity] [decimal](18, 2) NULL,
	[ZoneRowIndex] [int] NULL,
	[ZoneType] [int] NULL,
	[ZoneAddress] [nvarchar](250) NULL,
	[ZoneCorridorId] [int] NULL,
	[ZoneMapXPosition] [int] NULL,
	[ZoneMapYPosition] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_Tags] ADD  CONSTRAINT [DF_tbl_Tags_TagStatus]  DEFAULT ((0)) FOR [TagStatus]
GO
ALTER TABLE [dbo].[tbl_Tags] ADD  CONSTRAINT [DF_tbl_Tags_Lock]  DEFAULT ((0)) FOR [Lock]
GO
ALTER TABLE [dbo].[tbl_Tags] ADD  CONSTRAINT [DF_tbl_Tags_TagZone]  DEFAULT ((0)) FOR [TagZone]
GO
ALTER TABLE [dbo].[tbl_Tags] ADD  CONSTRAINT [DF_tbl_Tags_TagRegisterDateTime]  DEFAULT (getdate()) FOR [TagRegisterDateTime]
GO
ALTER TABLE [dbo].[tbl_Tags] ADD  CONSTRAINT [DF_tbl_Tags_ReProduct]  DEFAULT ((0)) FOR [ReProduct]
GO
ALTER TABLE [dbo].[tbl_Tags] ADD  CONSTRAINT [DF_tbl_Tags_fld_InspectActionId]  DEFAULT ((0)) FOR [fld_InspectActionId]
GO
ALTER TABLE [dbo].[tbl_Zones] ADD  CONSTRAINT [DF_tbl_Zones_ZoneOccupiedCapacity]  DEFAULT ((0)) FOR [ZoneOccupiedCapacity]
GO
ALTER TABLE [dbo].[tbl_Zones] ADD  CONSTRAINT [DF_tbl_Zones_ZoneRowIndex]  DEFAULT ((0)) FOR [ZoneRowIndex]
GO
ALTER TABLE [dbo].[tbl_Zones] ADD  CONSTRAINT [DF_tbl_Zones_ZoneCorridorId]  DEFAULT ((0)) FOR [ZoneCorridorId]


