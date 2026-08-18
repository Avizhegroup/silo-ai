## DATABASE SCHEMA and RELATIONSHIPS
{
  "tableName": "tbl_Tags",
  "primaryKey": ["ProductSerial", "TagEpc"],

  "columns": [
    {"name":"Id","type":"int","nullable":false,"identity":true},
    {"name":"ProductSerial","type":"nvarchar(50)","nullable":false},
    {"name":"ProductCode","type":"nvarchar(50)","nullable":true},
    {"name":"TagEpc","type":"nvarchar(50)","nullable":false},
    {"name":"ProjectCode","type":"nvarchar(50)","nullable":true},
    {"name":"ProductCount","type":"decimal(18,2)","nullable":true},
    {"name":"ProductName","type":"nvarchar(max)","nullable":true},
    {"name":"ProductType","type":"nvarchar(256)","nullable":true},
    {"name":"ProductStatus","type":"nvarchar(256)","nullable":true},
    {"name":"TagStatus","type":"nvarchar(50)","nullable":true,"default":"0"},
    {"name":"TagRegisterDateTime","type":"datetime","nullable":true,"default":"GETDATE()"},
    {"name":"TagRegisterUser","type":"nvarchar(50)","nullable":true},
    {"name":"TagTreeParentId","type":"int","nullable":true},
    {"name":"TagTreeSecondParentId","type":"int","nullable":true},
    {"name":"TagTreeParentsId","type":"nvarchar(max)","nullable":true},
    {"name":"NewProductSerial","type":"nvarchar(50)","nullable":true},
    {"name":"ProductProperties","type":"nvarchar(max)","nullable":true},
    {"name":"Lock","type":"bit","nullable":true,"default":0},
    {"name":"Username","type":"nvarchar(50)","nullable":true},
    {"name":"DeviceId","type":"nvarchar(50)","nullable":true},
    {"name":"DeviceIp","type":"nvarchar(50)","nullable":true},
    {"name":"Freeze","type":"bit","nullable":true},
    {"name":"Deactivate","type":"bit","nullable":true},
    {"name":"TagInActionId","type":"int","nullable":true},
    {"name":"TagInDestinationId","type":"nvarchar(50)","nullable":true},
    {"name":"TagInActionId2","type":"int","nullable":true},
    {"name":"TagInDestinationId2","type":"nvarchar(50)","nullable":true},
    {"name":"fld_ProductPropertyAId","type":"nvarchar(50)","nullable":true},
    {"name":"fld_ProductPropertyBId","type":"nvarchar(50)","nullable":true},
    {"name":"fld_ProductPropertyCId","type":"nvarchar(50)","nullable":true},
    {"name":"RegCode","type":"nvarchar(50)","nullable":true},
    {"name":"fld_LastModifierUser","type":"nvarchar(128)","nullable":true},
    {"name":"ContractStatus","type":"nvarchar(50)","nullable":true},
    {"name":"TagZone","type":"nvarchar(50)","nullable":true,"default":"0"},
    {"name":"ReProduct","type":"bit","nullable":true,"default":0},
    {"name":"fld_InspectActionId","type":"int","nullable":true,"default":0},
    {"name":"fld_LastInspectResult","type":"nvarchar(max)","nullable":true},
    {"name":"fld_ProductGroup","type":"nvarchar(128)","nullable":true},
    {"name":"fld_ProductBrand","type":"nvarchar(128)","nullable":true},
    {"name":"fld_ProductSubGroup","type":"nvarchar(128)","nullable":true},
    {"name":"fld_ProductClass","type":"nvarchar(128)","nullable":true},
    {"name":"TagTreeParentsEpc","type":"nvarchar(128)","nullable":true},
    {"name":"TagEpc2","type":"nvarchar(128)","nullable":true}
  ],

 "relationships": {
  "children": ["tbl_TagsMovement"],

  "parents": [
    {
      "table": "tbl_Products",
      "type": "Many-to-One",
      "via": "ProductCode (logical FK)"
    },
    {
    "table": "tbl_ProductBrand",
    "type": "Many-to-One",
    "via": "tbl_Tags.fld_ProductBrand → "tbl_ProductBrand.fld_ProductBrandCode",
    "keyType": "Business Key (string)"
    },
    {
    "table": "tbl_ProductGroup",
      "type": "Many-to-One",
      "via": "tbl_Tags.fld_ProductGroup → "tbl_ProductGroup.fld_ProductGroupCode",
      "keyType": "Business Key (string)"
    },
    {
    "table": "tbl_ProductClass",
      "type": "Many-to-One",
      "via": "tbl_Tags.fld_ProductClass → "tbl_ProductClass.fld_ProductClassCode",
      "keyType": "Business Key (string)"
    },
    {
      "table": "tbl_ProductSubGroup",
      "type": "Many-to-One",
      "via": "tbl_Tags.fld_ProductSubGroup → tbl_ProductSubGroup.fld_ProductSubGroupCode",
      "keyType": "Business Key (string)"
    },
     {
      "table": "tbl_ProductStatus",
      "type": "Many-to-One",
      "via": "tbl_Tags.ProductStatus → tbl_ProductStatus.ProductStatusCode",
      "keyType": "Business Key (string)"
    },
    {
      "table": "tbl_ProductType",
      "type": "Many-to-One",
      "via": "tbl_Tags.ProductType → tbl_ProductType.ProductTypeCode",
      "keyType": "Business Key (string)"
    },
     {
  "table": "tbl_User",
  "type": "Many-to-One",
  "via": "tbl_Tags.TagRegisterUser → tbl_User.Id",
  "keyType": "Business Key (string)"
   },
   {
   "table": "tbl_ProductPropertyA",
  "type": "Many-to-One",
  "via": "tbl_Tags.fld_ProductPropertyAId → tbl_ProductPropertyA.fld_ProductPropertyAId",
  "keyType": "Business Key (string)"
   },
   {
   "table": "tbl_ProductPropertyB",
  "type": "Many-to-One",
  "via": "tbl_Tags.fld_ProductPropertyBId → tbl_ProductPropertyB.fld_ProductPropertyBId",
  "keyType": "Business Key (string)"
   },
    {
 "table": "tbl_ProductPropertyC",
"type": "Many-to-One",
"via": "tbl_Tags.fld_ProductPropertyCId → tbl_ProductPropertyC.fld_ProductPropertyCId",
"keyType": "Business Key (string)"
 },
   {
   "table": "tbl_Destination",
  "type": "Many-to-One",
  "via": "tbl_Tags.TagInDestinationId → tbl_Destination.DestinationCode",
  "keyType": "Business Key (string)"
   },
    {
   "table": "tbl_Zones",
   "type": "Many-to-One",
   "via": "tbl_Tags.TagZone → tbl_Zones.ZoneCode",
   "keyType": "Business Key (string)"
   },

  ]
},

  "businessMeaning": "RFID Tag is a physical product instance in production lifecycle. It represents a single tracked unit of a Product, enriched with operational metadata, classification snapshot, and lifecycle state.",
  
  "role": "FACT + MASTER HYBRID (Entity Instance Layer)",

  "timeField": "TagRegisterDateTime",

  "semanticNotes": [
    "ProductGroup/Brand/Class/SubGroup inside Tag are denormalized snapshots of Product state",
    "True source of Product hierarchy is tbl_Products",
    "Tag is NOT a product definition; it is a product instance",
    "Movement history is stored in tbl_TagsMovement"
    "CRITICAL: When the user asks about ANY specific warehouse or location by name (e.g., 'انبار محصول', 'انبار درحال بازسازی', 'انبار ضایعات'), YOU MUST INNER JOIN `tbl_Tags` with `tbl_Destination` on `T.TagInDestinationId = D.DestinationCode` and filter by `D.DestinationTitle = N'...'`."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_ProductStatus",
  "primaryKey": "ProductStatusId",

  "columns": [
    {"name":"ProductStatusId","type":"int","identity":true},
    {"name":"ProductStatusCode","type":"nvarchar(128)"},
    {"name":"ProductStatusTitle","type":"nvarchar(128)"},
    {"name":"ProductStatusDesc","type":"nvarchar(max)"}
  ],

  "relationships": {
    "children": ["tbl_Products","tbl_Tags"]
  },

  "businessMeaning": "Defines QC / status state of product (e.g. approved, rejected, pending).",
  "role": "DIMENSION (Quality Control)"
}

---------------------------------------------------------
{
  "tableName": "tbl_ProductSubGroup",
  "primaryKey": "fld_ProductSubGroupId",

  "columns": [
    {"name":"fld_ProductSubGroupCode","type":"nvarchar(128)"},
    {"name":"fld_ProductSubGroupTitle","type":"nvarchar(256)"},
    {"name":"fld_ProductSubGroupSubTitle","type":"nvarchar(512)"},
    {"name":"fld_ProductSubGroupDesc","type":"nvarchar(512)"},
    {"name":"fld_ProductGroupCode","type":"nvarchar(128)"}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_ProductGroup",
        "type": "Many-to-One",
        "via": "tbl_ProductSubGroup.fld_ProductGroupCode → tbl_ProductGroup.fld_ProductGroupCode"
      }
    ],
    "children": ["tbl_Products", "tbl_Tags"]
  },

  "businessMeaning": "Hierarchical subdivision of product groups used for classification.",
  "role": "DIMENSION (Hierarchy Level 2)"
}

---------------------------------------------------------
{
  "tableName": "tbl_ProductType",
  "primaryKey": "ProductTypeId",

  "columns": [
    {"name":"ProductTypeCode","type":"nvarchar(50)"},
    {"name":"ProductTypeTitle","type":"nvarchar(50)"},
    {"name":"ProductTypeParentId","type":"nvarchar(50)"},
    {"name":"ProductTypeParentsId","type":"nvarchar(max)"}
  ],

  "relationships": {
    "children": ["tbl_Products"]
  },

  "businessMeaning": "Defines product type hierarchy (tree structure).",
  "role": "DIMENSION (Hierarchical Type Tree)"
}

---------------------------------------------------------
{
  "tableName": "tbl_ProductPropertyC",

  "primaryKey": "fld_ProductPropertyCIdentity",

  "columns": [
    {"name":"fld_ProductPropertyCId","type":"nvarchar(128)"},
    {"name":"fld_ProductPropertyCTitle","type":"nvarchar(256)"},
    {"name":"fld_ProductPropertyCDesc","type":"nvarchar(max)"},
    {"name":"fld_ProductPropertyCData","type":"nvarchar(max)"},
    {"name":"fld_ProductPropertyCTemp","type":"nvarchar(max)"}
  ],

  "relationships": {
    "children": [
      "tbl_Products",
      "tbl_Tags"
    ]
  },

   "businessMeaning": "Defines product sizes. Each record represents a product size such as Small, Medium, Large or other size classifications.",
   "role": "DIMENSION",
   "semanticNotes": [
    "Represents product sizes.",
    "Products and RFID tags can be associated with a size.",
    "When the user asks about size or product size, use this table."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_ProductPropertyB",

  "primaryKey": "fld_ProductPropertyBId",

  "columns": [
    {"name":"fld_ProductPropertyBId","type":"nvarchar(128)"},
    {"name":"fld_ProductPropertyBTitle","type":"nvarchar(512)"},
    {"name":"fld_ProductPropertyBDesc","type":"nvarchar(512)"},
    {"name":"fld_ProductPropertyBData","type":"nvarchar(max)"},
    {"name":"fld_ProductPropertyAId","type":"nvarchar(128)"}
  ],

  "relationships": {
    "parents": [
      {
        "table":"tbl_ProductPropertyA",
        "type":"Many-to-One",
        "via":"tbl_ProductPropertyB.fld_ProductPropertyAId → tbl_ProductPropertyA.fld_ProductPropertyAId"
      }
      ],
      "children": [
      "tbl_Products",
      "tbl_Tags"
    ]
  },

   "businessMeaning": "Defines production shifts. Each record represents a work shift (e.g. Shift 1, Shift 2, Night Shift). Shift working hours are stored in fld_ProductPropertyBDesc.",

  "role": "DIMENSION",

  "semanticNotes": [
    "Represents production shifts.",
    "Shift time range is stored in fld_ProductPropertyBDesc (e.g. 06:00-14:00).",
    "Products and RFID tags can be associated with a production shift.",
    "When the user asks about shift or work shift, use this table."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_ProductPropertyA",

  "primaryKey": "fld_ProductPropertyAId",

  "columns": [
    {"name":"fld_ProductPropertyAId","type":"nvarchar(128)"},
    {"name":"fld_ProductPropertyATitle","type":"nvarchar(256)"},
    {"name":"fld_ProductPropertyADesc","type":"nvarchar(512)"},
    {"name":"fld_ProductPropertyAData","type":"nvarchar(max)"}
  ],

  "relationships": {
    "children": [
      "tbl_ProductPropertyB", "tbl_Products",
      "tbl_Tags"
    ]
  },

  "businessMeaning": "Defines production lines. Each record represents a manufacturing line used during product production.",

  "role": "DIMENSION",

  "semanticNotes": [
    "Represents production lines.",
    "Products and RFID tags can be associated with a production line.",
    "When the user asks about production line or line, use this table."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_ProductExpire",

  "primaryKey": "fld_ProductExpireId",

  "columns": [
    {"name":"fld_ProductExpireId","type":"int","identity":true},
    {"name":"fld_ProductExpireProductSerial","type":"nvarchar(50)"},
    {"name":"fld_ProductExpireProductCode","type":"nvarchar(50)"},
    {"name":"fld_ProductExpireStatus","type":"int"},
    {"name":"fld_ProductExpireStartDate","type":"nvarchar(10)"},
    {"name":"fld_ProductExpireEndDate","type":"nvarchar(10)"},
    {"name":"fld_ProductExpireActivationType","type":"int"},
    {"name":"fld_ProductExpireLastModifiedDateTime","type":"datetime"},
    {"name":"fld_ProductExpireLastModifiedUserId","type":"nvarchar(128)"}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_User",
        "type": "Many-to-One",
        "via": "tbl_ProductExpire.fld_ProductExpireLastModifiedUserId → tbl_User.Id"
      }
    ]
  },

  "businessMeaning": "Stores expiration and warranty activation information for produced products.",

  "role": "TRANSACTIONAL / LIFECYCLE TABLE",

  "semanticNotes": [
    "Each record represents expiration information for a product.",
    "Product is identified by ProductCode (business key).",
    "StartDate and EndDate are stored as Persian date strings (nvarchar(10)).",
    "LastModifiedUserId references the user who last modified the record."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_ProductGroup",
  "primaryKey": "fld_ProductGroupId",
  "columns": [
    {"name":"fld_ProductGroupCode","type":"nvarchar(128)"},
    {"name":"fld_ProductGroupTitle","type":"nvarchar(128)"},
    {"name":"fld_ProductGroupData","type":"nvarchar(max)"}
  ],
  "relationships": {
    "children": ["tbl_Products", "tbl_ProductSubGroup","tbl_Tags"]
  },
  "businessMeaning": "Top-level product grouping used for categorization and reporting.",
  "role": "DIMENSION (Hierarchy Root)"
}

---------------------------------------------------------
{
  "tableName": "tbl_ProductClass",
  "primaryKey": "fld_ProductClassId",
  "columns": [
    {"name":"fld_ProductClassCode","type":"nvarchar(128)"},
    {"name":"fld_ProductClassTitle","type":"nvarchar(256)"},
    {"name":"fld_ProductClassSubTitle","type":"nvarchar(512)"},
    {"name":"fld_ProductClassDesc","type":"nvarchar(512)"}
  ],
  "relationships": {
    "children": ["tbl_Products","tbl_Tags"]
  },
  "businessMeaning": "Product classification layer defining category/type of product.",
  "role": "DIMENSION (Hierarchy Level 2)"
}

---------------------------------------------------------
{
  "tableName": "tbl_ProductBrand",
  "primaryKey": "fld_ProductBrandId",
  "columns": [
    {"name":"fld_ProductBrandCode","type":"nvarchar(128)"},
    {"name":"fld_ProductBrandTitle","type":"nvarchar(128)"},
    {"name":"fld_ProductBrandData","type":"nvarchar(max)"}
  ],
  "relationships": {
    "children": ["tbl_Products","tbl_Tags"]
  },
  "businessMeaning": "Represents product brand classification used for grouping products.",
  "role": "DIMENSION (Hierarchy Level 1)"
}

---------------------------------------------------------
{
  "tableName": "tbl_Products",
  "primaryKey": "Id",

  "columns": [
    {"name":"Id","type":"int","identity":true},
    {"name":"ProductCode","type":"nvarchar(50)"},
    {"name":"ProductTitle","type":"nvarchar(250)"},
    {"name":"ProductENTitle","type":"nvarchar(250)"},
    {"name":"ProductPackValue","type":"decimal"},
    {"name":"ProductPackWeight","type":"decimal"},
    {"name":"ProductPackVolume","type":"decimal"},
    {"name":"ProductCountInPack","type":"decimal"},
    {"name":"ProductValue","type":"decimal"},
    {"name":"ProductTechnicalCode","type":"nvarchar(50)"},
    {"name":"ProductProperties","type":"nvarchar(max)"},
    {"name":"ProductType","type":"nvarchar(50)"},
    {"name":"ProductStatus","type":"nvarchar(50)"},
    {"name":"ProductSize","type":"nvarchar(50)"},
    {"name":"ProductUnit","type":"nvarchar(50)"},
    {"name":"ProductRegUser","type":"nvarchar(50)"},
    {"name":"ProductRegDateTime","type":"datetime"},
    {"name":"ProductGalleryId","type":"int"},
    {"name":"ProductTechnicalData","type":"nvarchar(max)"},
    {"name":"fld_ProductGroup","type":"nvarchar(128)"},
    {"name":"fld_ProductBrand","type":"nvarchar(128)"},
    {"name":"fld_ProductSubGroup","type":"nvarchar(128)"},
    {"name":"fld_ProductClass","type":"nvarchar(128)"},
    {"name":"fld_ProductGuaranteeType","type":"int"},
    {"name":"fld_ProductGuaranteeDays","type":"int"},
    {"name":"fld_ProductExpireType","type":"int"},
    {"name":"fld_ProductExpireDays","type":"int"},
    {"name":"fld_ProductIsActive","type":"bit"},
    {"name":"fld_ProductGuaranteeEndDate","type":"nvarchar(10)"},
    {"name":"fld_ProductExpireEndDate","type":"nvarchar(10)"},
    {"name":"fld_ProductGuaranteeMonths","type":"int"},
    {"name":"fld_ProductExpireMonths","type":"int"},
    {"name":"fld_HasDoubleTag","type":"bit"}

  ],
  "relationships": {
    "parents": [
    {
 "table": "tbl_ProductGroup",
 "type": "Many-to-One",
 "via": "tbl_Products.fld_ProductGroup → tbl_ProductGroup.fld_ProductGroupCode",
 "keyType": "Business Key (string)"
},
     {
 "table": "tbl_ProductBrand",
 "type": "Many-to-One",
 "via": "tbl_Products.fld_ProductBrand → tbl_ProductBrand.fld_ProductBrandCode",
 "keyType": "Business Key (string)"
},
     {
 "table": "tbl_ProductSubGroup",
 "type": "Many-to-One",
 "via": "tbl_Products.fld_ProductSubGroup → tbl_ProductSubGroup.fld_ProductSubGroupCode",
 "keyType": "Business Key (string)"
},
      {
 "table": "tbl_ProductClass",
 "type": "Many-to-One",
 "via": "tbl_Products.fld_ProductClass → tbl_ProductClass.fld_ProductClassCode",
 "keyType": "Business Key (string)"
},
      {
 "table": "tbl_ProductType",
 "type": "Many-to-One",
 "via": "tbl_Products.ProductType → tbl_ProductType.ProductTypeCode",
 "keyType": "Business Key (string)"
},
    {
 "table": "tbl_ProductStatus",
 "type": "Many-to-One",
 "via": "tbl_Products.ProductStatus → tbl_ProductStatus.ProductStatusCode",
 "keyType": "Business Key (string)"
},
   {
  "table": "tbl_ProductPropertyC",
  "type": "Many-to-One",
  "via": "tbl_Products.ProductSize → tbl_ProductPropertyC.fld_ProductPropertyCId",
  "keyType": "Business Key (string)"
 },
 {
  "table": "tbl_User",
  "type": "Many-to-One",
  "via": "tbl_Products.ProductRegUser → tbl_User.Id",
  "keyType": "Business Key (string)"
 },
  {
 "table": "tbl_Gallery",
 "type": "Many-to-One",
 "via": "tbl_Products.ProductGalleryId → tbl_Gallery.GalleryId",
 "keyType": "Business Key (string)"
}
],
    "children": ["tbl_Tags", "tbl_TagsMovement","tbl_DocumentItem"]
  },
  "businessMeaning": "Master product definition (SKU-level entity) used as template for production and RFID tagging.",
  "role": "DIMENSION TABLE (Core Master Data)"
}

---------------------------------------------------------
{
  "tableName": "tbl_Gallery",
  "primaryKey": "fld_GalleryId",

  "columns": [
    {"name":"fld_GalleryId","type":"int","identity":true},
    {"name":"fld_GalleryUserId","type":"nvarchar(128)"},
    {"name":"fld_GalleryMediaName","type":"nvarchar(128)"},
    {"name":"fld_GalleryMediaPath","type":"nvarchar(512)"},
    {"name":"fld_GalleryUsageType","type":"int"},
    {"name":"fld_GalleryUploadDateTime","type":"datetime"},
    {"name":"fld_GalleryUsageId","type":"nvarchar(128)"},
    {"name":"fld_GalleryMediaExtensionType","type":"int"},
    {"name":"fld_GalleryAdditionalData","type":"nvarchar(max)"}
  ],

  "relationships": {
    "parents": [
      {
        "table":"tbl_User",
        "type":"Many-to-One",
        "via":"tbl_Gallery.fld_GalleryUserId → tbl_User.Id",
        "keyType":"Business Key (string)"
      }
    ],
    "children":[
      {
        "table":"tbl_Products",
        "type":"One-to-Many",
        "via":"tbl_Gallery.fld_GalleryId → tbl_Products.ProductGalleryId"
      }
    ]
  },

  "businessMeaning":"Stores uploaded media files (images/documents) used by different modules such as Products.",

  "role":"DIMENSION / SHARED RESOURCE",

  "semanticNotes":[
    "Media may be reused by different modules depending on UsageType.",
    "Product images are linked through ProductGalleryId.",
    "MediaPath stores the physical or virtual file location."
  ]
}

---------------------------------------------------------
{
  "tableName":"tbl_Destination",

  "primaryKey":"DestinationCode",

  "columns":[
    {"name":"DestinationId","type":"int","identity":true},
    {"name":"DestinationCode","type":"nvarchar(50)"},
    {"name":"DestinationTitle","type":"nvarchar(50)"},
    {"name":"DestinationSt","type":"int"},
    {"name":"DestinationParentId","type":"int"},
    {"name":"DestinationDesc","type":"nvarchar(max)"},
    {"name":"DestinationType","type":"int"},
    {"name":"DestinationParentsId","type":"nvarchar(max)"},
    {"name":"DestinationEpc","type":"nvarchar(max)"}
  ],

  "relationships":{
    "children":["tbl_Tags","tbl_Corridor","tbl_Zones"]
  },

  "businessMeaning":"Defines warehouses, storage locations and operational destinations used during product movement.",

  "role":"DIMENSION (Warehouse / Location)",

  "semanticNotes":[
    "DestinationCode is the business identifier used by most tables.",
    "Hierarchy is represented by DestinationParentId and DestinationParentsId.",
    "DestinationEpc identifies RFID-enabled locations.",
    "Tags store their current destination using TagInDestinationId."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_DestinationType",
  "primaryKey": "fld_Id",

  "columns": [
    {"name": "fld_Id", "type": "int", "identity": true, "nullable": false},
    {"name": "fld_DestinationTypeCode", "type": "nvarchar(50)", "nullable": true},
    {"name": "fld_DestinationTypeName", "type": "nvarchar(250)", "nullable": true}
  ],

  "relationships": {
    "children": [
      "tbl_Destination"
    ]
  },

  "businessMeaning": "Defines classification categories for warehouse destinations (e.g., Raw Materials Warehouse, Finished Goods Warehouse, Scrap Yard, Production Line).",
  "role": "DIMENSION / REFERENCE (Warehouse Classification Layer)",
  "semanticNotes": [
    "Categorizes destinations in tbl_Destination to differentiate workflow handling or material types.",
    "Though DestinationType in tbl_Destination is stored as an integer, it logically maps to this table to resolve destination class titles."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_Zones",
  "primaryKey": "id",

  "columns": [
    {"name":"id","type":"int","identity":true},
    {"name":"ZoneCode","type":"nvarchar(50)"},
    {"name":"ZoneTitle","type":"nvarchar(50)"},
    {"name":"ZoneCapacity","type":"decimal"},
    {"name":"ZoneDimention","type":"nvarchar(50)"},
    {"name":"ZoneParentCode","type":"nvarchar(50)"},
    {"name":"ZoneParentLayer","type":"int"},
    {"name":"ZoneStoreCode","type":"nvarchar(50)"},
    {"name":"ZoneCountPixle","type":"int"},
    {"name":"ZoneOccupiedCapacity","type":"int"},
    {"name":"MinZoneCapacity","type":"decimal"},
    {"name":"MaxZoneCapacity","type":"decimal"},
    {"name":"ZoneRowIndex","type":"int"},
    {"name":"ZoneAddress","type":"nvarchar(250)"},
    {"name":"ZoneCorridorId","type":"int"}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_Destination",
        "type": "Many-to-One",
        "via": "tbl_Zones.ZoneStoreCode → tbl_Destination.DestinationCode",
        "keyType": "Business Key (string)"
      },
      {
        "table": "tbl_Corridor",
        "type": "Many-to-One",
        "via": "tbl_Zones.ZoneCorridorId → tbl_Corridor.fld_CorridorId"
      }
    ],
    "children": [
      "tbl_Tags"
    ]
  },

  "businessMeaning": "Defines storage zones inside a warehouse for locating RFID-tagged products.",

  "role": "DIMENSION (Warehouse Layout)"
}

---------------------------------------------------------
{
  "tableName": "tbl_User",

  "primaryKey": "Id",

  "columns": [
    {"name":"Id","type":"nvarchar(128)"},
    {"name":"LastModifiedDate","type":"datetime"},
    {"name":"CreateDate","type":"datetime"},
    {"name":"IsActive","type":"bit"},
    {"name":"CreatorIdentityID","type":"nvarchar(128)"},
    {"name":"LastModifierIdentityID","type":"nvarchar(128)"},
    {"name":"Name","type":"nvarchar(512)"},
    {"name":"Row","type":"int","identity":true},
    {"name":"Email","type":"nvarchar(256)"},
    {"name":"EmailConfirmed","type":"bit"},
    {"name":"PasswordHash","type":"nvarchar(max)"},
    {"name":"SecurityStamp","type":"nvarchar(max)"},
    {"name":"PhoneNumber","type":"nvarchar(max)"},
    {"name":"PhoneNumberConfirmed","type":"bit"},
    {"name":"TwoFactorEnabled","type":"bit"},
    {"name":"LockoutEndDateUtc","type":"datetime"},
    {"name":"LockoutEnabled","type":"bit"},
    {"name":"AccessFailedCount","type":"int"},
    {"name":"Username","type":"nvarchar(256)"},
    {"name":"Details","type":"nvarchar(max)"},
    {"name":"Image","type":"nvarchar(50)"}
  ],

  "relationships": {
    "children": [
      "tbl_Products",
      "tbl_Tags",
      "tbl_ProductExpire",
      "tbl_Gallery"
    ]
  },

  "businessMeaning": "System user account used for authentication, authorization, auditing, and ownership across the RFID/WMS platform.",

  "role": "MASTER / SECURITY (Identity)"
}

---------------------------------------------------------
{
  "tableName": "tbl_MovementActions",

  "primaryKey": "MovementActionId",

  "columns": [
    {"name":"MovementActionId","type":"int"},
    {"name":"MovementActionTp","type":"int"},
    {"name":"MovementActionStore","type":"nvarchar(50)"},
    {"name":"MovementActionUserId","type":"nvarchar(128)"},
    {"name":"MovementActionDate","type":"nvarchar(10)"},
    {"name":"MovementActionTime","type":"nvarchar(5)"},
    {"name":"MovementActionDateTime","type":"datetime"},
    {"name":"MovementActionCountTags","type":"int"},
    {"name":"MovementActionDestinationId","type":"nvarchar(50)"},
    {"name":"MovementActionCarPlaque","type":"nvarchar(16)"},
    {"name":"MovementActionDriverName","type":"nvarchar(50)"},
    {"name":"MovementActionDriverMobile","type":"nvarchar(50)"},
    {"name":"MovementActionData","type":"nvarchar(max)"},
    {"name":"MovementActionLinkId","type":"int"},
    {"name":"MovementActionLinkDestId","type":"nvarchar(50)"},
    {"name":"MovementActionDocumentId","type":"nvarchar(128)"},
    {"name":"MovementActionDesc","type":"nvarchar(max)"},
    {"name":"MovementActionUHFLogId","type":"nvarchar(256)"},
    {"name":"MovementActionUHFLogGate","type":"nvarchar(30)"},
    {"name":"MovementActionTruckCrossId","type":"bigint"}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_User",
        "type": "Many-to-One",
        "via": "tbl_MovementActions.MovementActionUserId → tbl_User.Id",
      },
      {
        "table": "tbl_Destination",
        "type": "Many-to-One",
        "via": "tbl_MovementActions.MovementActionDestinationId → tbl_Destination.DestinationCode",
        "keyType": "Business Key (string)"
      },
      {
        "table": "tbl_TruckCross",
        "type": "Many-to-One",
        "via": "tbl_MovementActions.MovementActionTruckCrossId → tbl_TruckCross.fld_TruckCrossId"
      }
    ],
    "children": [
      "tbl_TagsMovement",
      "tbl_UHFReaderLogHeader"
    ]
  },

  "businessMeaning": "Represents a warehouse movement transaction such as receiving, dispatch, transfer, or inventory operation. It acts as the header/master record for a movement event.",

  "role": "TRANSACTION HEADER",

  "timeField": "MovementActionDateTime",

  "semanticNotes": [
    "One movement action can contain multiple tag movements.",
    "Stores operational metadata such as destination, operator, vehicle, and timestamps.",
    "Acts as the header entity for movement execution."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_TagsMovement",

  "primaryKey": "TagsMovementId",

  "columns": [
    {"name":"TagsMovementId","type":"int"},
    {"name":"TagEpc","type":"nvarchar(50)"},
    {"name":"ProductCode","type":"nvarchar(50)"},
    {"name":"ProductSerial","type":"nvarchar(50)"},
    {"name":"ProductCount","type":"decimal"},
    {"name":"HMovementActionId","type":"int"},
    {"name":"HTagsMovementDate","type":"nvarchar(10)"},
    {"name":"HTagsMovementTime","type":"nvarchar(5)"},
    {"name":"HTagsMovementDateTime","type":"datetime"},
    {"name":"HTagsMovementSt","type":"int"},
    {"name":"RMovementActionId","type":"int"},
    {"name":"RTagsMovementDate","type":"nvarchar(10)"},
    {"name":"RTagsMovementTime","type":"nvarchar(5)"},
    {"name":"RTagsMovementDateTime","type":"datetime"},
    {"name":"RTagsMovementSt","type":"int"},
    {"name":"MovementData","type":"nvarchar(max)"},
    {"name":"ApiSendStatus","type":"int"},
    {"name":"ApiSendUser","type":"nvarchar(128)"},
    {"name":"ApiSendDateTime","type":"datetime"},
    {"name":"ApiSendData","type":"nvarchar(max)"},
    {"name":"RMovementActionType","type":"int"},
    {"name":"RMovementActionDocumentId","type":"nvarchar(128)"},
    {"name":"RMovementActionUHFLogId","type":"nvarchar(256)"},
    {"name":"RMovementActionDestinationId","type":"nvarchar(50)"}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_Products",
        "type": "Many-to-One",
        "via": "tbl_TagsMovement.ProductCode → tbl_Products.ProductCode"
      },
      {
        "table": "tbl_Tags",
        "type": "Many-to-One",
        "via": "tbl_TagsMovement.ProductSerial → tbl_Tags.ProductSerial"
      },
      {
        "table": "tbl_MovementActions",
        "type": "Many-to-One",
        "via": "tbl_TagsMovement.RMovementActionId → tbl_MovementActions.MovementActionId"
      },
      {
        "table": "tbl_MovementActions",
        "type": "Many-to-One (History)",
        "via": "tbl_TagsMovement.HMovementActionId → tbl_MovementActions.MovementActionId"
      }
    ]
  },

  "businessMeaning": "Line-level RFID movement log. Each record represents movement of a single tag within or across movement actions.",

  "role": "FACT TABLE (Transaction Detail)",

  "timeFields": [
    "RTagsMovementDateTime"
  ],

  "semanticNotes": [
    "This is the DETAIL table of MovementAction (Header-Detail pattern)",
    "Core table for tracking tag lifecycle transitions"
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_Station",

  "primaryKey": "fld_StationId",

  "columns": [
    {"name":"fld_StationId","type":"int","identity":true},
    {"name":"fld_StationCode","type":"nvarchar(128)"},
    {"name":"fld_StationName","type":"nvarchar(512)"},
    {"name":"fld_StationType","type":"int"},
    {"name":"fld_StationStatus","type":"int"},
    {"name":"fld_StationReaders","type":"nvarchar(max)"},
    {"name":"fld_StationDescription","type":"nvarchar(1024)"},
    {"name":"fld_StationSettings","type":"nvarchar(max)"},
    {"name":"fld_StationFromDestination","type":"nvarchar(max)"},
    {"name":"fld_StationToDestination","type":"nvarchar(max)"},
    {"name":"fld_StationActionType","type":"int"},
    {"name":"fld_StationMacAddress","type":"nvarchar(50)"}
  ],

  "relationships": {
  "parent": [  
      {
        "table": "tbl_ActionTypes",
        "type": "Many-to-One",
        "via": "tbl_TagsMovement.fld_StationActionType → tbl_ActionTypes.fld_ActionTypeId"
       }
  ],
    "children": [
      "tbl_UHFReaderLogHeader"
    ]
  },

  "businessMeaning": "Represents a physical or logical RFID station (reader gate / checkpoint) used to capture tag movements in warehouse or logistics flows.",

  "role": "OPERATIONAL DEVICE / CAPTURE NODE",

  "semanticNotes": [
    "Station acts as a data capture point for UHF RFID readers",
    "Defines where tag events are generated in the system",
    "Can represent gates, corridors, or warehouse checkpoints depending on configuration",
    "Reader configuration is stored in StationReaders field"
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_ActionTypes",

  "primaryKey": "Id",

  "columns": [
    {"name":"Id","type":"int","identity":true},
    {"name":"fld_ActionTypeId","type":"int"},
    {"name":"fld_ActionTypeFromDestinationType","type":"nvarchar(max)"},
    {"name":"fld_ActionTypeToTypeDestinationType","type":"nvarchar(max)"},
    {"name":"fld_ActionTypeTitle","type":"nvarchar(max)"},
    {"name":"fld_ActionTypePermitedDocStatus","type":"nvarchar(max)"},
    {"name":"fld_ActionTypeChangeDocStatus","type":"nvarchar(max)"},
    {"name":"fld_ActionTypeActiveControls","type":"nvarchar(max)"},
    {"name":"fld_ActionTypeRFIDPower","type":"int"}
  ],

  "relationships": {
    "children": [
      "tbl_TruckCrossCause",
      "tbl_Station",
      "tbl_MovementActions"
    ]
  },

  "businessMeaning": "Defines system action definitions used in warehouse and logistics workflows, including allowed transitions between destinations, document status rules, and RFID configuration behavior.",

  "role": "DIMENSION (Workflow Control)",

  "semanticNotes": [
    "Each ActionType defines a business operation rule set",
    "Controls allowed from/to destination logic in movement flows",
    "Used for enforcing workflow constraints in RFID and truck cross operations",
    "Also drives document status transitions and UI control behavior",
    "RFIDPower is used for reader configuration per action context"
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_TruckCross",
  "primaryKey": "fld_TruckCrossId",

  "columns": [
    {"name":"fld_TruckCrossId","type":"bigint","identity":true},
    {"name":"fld_TruckCrossPlaque","type":"nvarchar(20)"},
    {"name":"fld_TruckCrossInternationalPlaque","type":"nvarchar(20)"},
    {"name":"fld_TruckCrossDriverName","type":"nvarchar(50)"},
    {"name":"fld_TruckCrossDriverPhone","type":"nvarchar(20)"},
    {"name":"fld_TruckCrossNationalCode","type":"nvarchar(20)"},
    {"name":"fld_TruckCrossPassportCode","type":"nvarchar(50)"},
    {"name":"fld_TruckCrossSerial","type":"nvarchar(max)"},
    {"name":"fld_TruckCrossCompany","type":"int"},
    {"name":"fld_TruckCrossType","type":"int"},
    {"name":"fld_TruckCrossTypeDesc","type":"nvarchar(256)"},
    {"name":"fld_TruckCrossLicenseCode","type":"nvarchar(50)"},
    {"name":"fld_TruckCrossStatus","type":"int"},
    {"name":"fld_TruckCrossPresentCause","type":"int"},
    {"name":"fld_TruckCrossPresentTurn","type":"int"},
    {"name":"fld_TruckCrossPresentDateTime","type":"datetime"},
    {"name":"fld_TruckCrossPresentDesc","type":"nvarchar(250)"},
    {"name":"fld_TruckCrossPresentUserId","type":"nvarchar(128)"},
    {"name":"fld_TruckCrossPresentOperationType","type":"int"},
    {"name":"fld_TruckCrossPresentOperationDestination","type":"int"},
    {"name":"fld_TruckCrossPresentShipment","type":"int"},
    {"name":"fld_TruckCrossPresentShipmentNumber","type":"nvarchar(50)"},
    {"name":"fld_TruckCrossPresentCustomer","type":"int"},
    {"name":"fld_TruckCrossEnterDateTime","type":"datetime"},
    {"name":"fld_TruckCrossEnterWeightTonage","type":"decimal"},
    {"name":"fld_TruckCrossEnterDesc","type":"nvarchar(250)"},
    {"name":"fld_TruckCrossEnterUserId","type":"nvarchar(128)"},
    {"name":"fld_TruckCrossEnterEpc","type":"nvarchar(50)"},
    {"name":"fld_TruckCrossEnterOtherEpcs","type":"nvarchar(250)"},
    {"name":"fld_TruckCrossEnterAcceptor","type":"nvarchar(50)"},
    {"name":"fld_TruckCrossEnterAcceptPlace","type":"int"},
    {"name":"fld_TruckCrossPresentRevokeDateTime","type":"datetime"},
    {"name":"fld_TruckCrossPresentRevokeUserId","type":"nvarchar(128)"},
    {"name":"fld_TruckCrossExitDateTime","type":"datetime"},
    {"name":"fld_TruckCrossExitDesc","type":"nvarchar(250)"},
    {"name":"fld_TruckCrossExitUserId","type":"nvarchar(128)"},
    {"name":"fld_TruckCrossExitWeightTonage","type":"decimal"},
    {"name":"fld_TruckCrossExitGateId","type":"int"},
    {"name":"fld_TruckCrossExitDestination","type":"nvarchar(128)"},
    {"name":"fld_TruckCrossExitPureWeightCargo","type":"decimal"},
    {"name":"fld_TruckCrossExitWeightbridgeReceiptNumber","type":"nvarchar(50)"},
    {"name":"fld_TruckCrossExitCargoOwnerName","type":"nvarchar(50)"},
    {"name":"fld_TruckCrossExitCargoOwnerPhone","type":"nvarchar(50)"},
    {"name":"fld_TruckCrossExitDeliveryAddress","type":"nvarchar(250)"},
    {"name":"fld_TruckCrossExitShipmentCost","type":"nvarchar(50)"},
    {"name":"fld_TruckCrossExitPaymentType","type":"int"},
    {"name":"fld_TruckCrossExitUnitPrice","type":"nvarchar(50)"},
    {"name":"fld_TruckCrossExitTotalCost","type":"nvarchar(50)"},
    {"name":"fld_TruckCrossExitDistance","type":"nvarchar(50)"},
    {"name":"fld_TruckCrossDynamicFields","type":"nvarchar(max)"}
  ],

  "relationships": {
    "parents": [
      { "table":"tbl_TruckCrossCompany",
        "type":"Many-to-One",
        "via":"tbl_TruckCross.fld_TruckCrossCompany → tbl_TruckCompany.fld_TruckCompanyId"},
      {"table":"tbl_TruckType",
         "type":"Many-to-One",
         "via":"tbl_TruckCross.fld_TruckCrossType → tbl_TruckType.fld_TruckTypeId"},
      {"table":"tbl_TruckCrossCause",
         "type":"Many-to-One",
         "via":"tbl_TruckCross.fld_TruckCrossPresentCause → tbl_TruckCrossCause.fld_TruckCrossCauseId"},
      {"table":"tbl_TruckCrossOperationType",
         "type":"Many-to-One",
         "via":"tbl_TruckCross.fld_TruckCrossPresentOperationType → tbl_TruckCrossOperationType.fld_TruckCrossOperationTypeId"},
      {"table":"tbl_TruckCrossOperationDestination",
         "type":"Many-to-One",
         "via":"tbl_TruckCross.fld_TruckCrossPresentOperationDestination → tbl_TruckCrossOperationDestination.fld_TruckCrossOperationDestinationId"},
      {"table":"tbl_TruckCrossShipment",
         "type":"Many-to-One",
         "via":"tbl_TruckCross.fld_TruckCrossPresentShipment → tbl_TruckCrossShipment.fld_TruckCrossShipmentId"},
      {"table":"tbl_TruckCrossCustomer",
        "type":"Many-to-One",
         "via":"tbl_TruckCross.fld_TruckCrossPresentCustomer → tbl_TruckCrossCustomer.fld_TruckCrossCustomerId"},
      {"table":"tbl_TruckCrossAcceptPlace",
         "type":"Many-to-One",
         "via":"tbl_TruckCross.fld_TruckCrossEnterAcceptPlace → tbl_TruckCrossAcceptPlace.fld_TruckCrossAcceptPlaceId"}
    ],
    "children": [
      "tbl_TruckCrossItem",
      "tbl_UHFReaderLogHeader"
    ]
  },

  "businessMeaning": "Core operational truck entry/exit lifecycle entity in warehouse/logistics system.",
  "role": "FACT / OPERATIONAL HEADER"
}

---------------------------------------------------------
{
  "tableName": "tbl_TruckCrossAcceptPlace",
  "primaryKey": "fld_TruckCrossAcceptPlaceId",

  "columns": [
    {"name":"fld_TruckCrossAcceptPlaceId","type":"int","identity":true},
    {"name":"fld_TruckCrossAcceptPlaceTitle","type":"nvarchar(256)"}
  ],

  "relationships": {
    "children": ["tbl_TruckCross"]
  },

  "businessMeaning": "Defines locations where truck entries are accepted.",
  "role": "DIMENSION"
}

---------------------------------------------------------
{
  "tableName": "tbl_TruckCompany",
  "primaryKey": "fld_TruckCompanyId",

  "columns": [
    {"name":"fld_TruckCompanyId","type":"int","identity":true},
    {"name":"fld_TruckCompanyTitle","type":"nvarchar(256)"}
  ],

  "relationships": {
    "children": [
      "tbl_TruckCross",
      "tbl_TruckCrossShippingFee"
    ]
  },

  "businessMeaning": "Represents transport/logistics companies operating trucks.",
  "role": "MASTER DATA"
}

---------------------------------------------------------
{
  "tableName": "tbl_TruckCrossCause",
  "primaryKey": "fld_TruckCrossCauseId",

  "columns": [
    {"name":"fld_TruckCrossCauseId","type":"int","identity":true},
    {"name":"fld_TruckCrossCauseTitle","type":"nvarchar(256)"},
    {"name":"fld_TruckCrossCauseEnterActionTypeId","type":"int"},
    {"name":"fld_TruckCrossCauseExitActionTypeId","type":"int"}
  ],

  "relationships": {
    "parents": [
      {"table":"tbl_ActionTypes",
      "type":"Many-to-One",
      "via":"tbl_TruckCrossCause.fld_TruckCrossCauseEnterActionTypeId → tbl_ActionTypes.fld_ActionTypeId"},
      {"table":"tbl_ActionTypes",
      "type":"Many-to-One",
      "via":"tbl_TruckCrossCause.fld_TruckCrossCauseExitActionTypeId → tbl_ActionTypes.fld_ActionTypeId"}
    ],
    "children": [
      "tbl_TruckCross",
      "tbl_TruckCrossOperationType"
    ]
  },

  "businessMeaning": "Defines reasons for truck entry/exit operations.",
  "role": "REFERENCE"
}

---------------------------------------------------------
{
  "tableName": "tbl_TruckCrossCustomer",
  "primaryKey": "fld_TruckCrossCustomerId",

  "columns": [
    {"name":"fld_TruckCrossCustomerId","type":"int","identity":true},
    {"name":"fld_TruckCrossCustomerTitle","type":"nvarchar(256)"}
  ],

  "relationships": {
    "children": [
      "tbl_TruckCross",
      "tbl_TruckCrossShippingFee"
    ]
  },

  "businessMeaning": "Represents customers receiving logistics services.",
  "role": "MASTER DATA"
}

---------------------------------------------------------
{
  "tableName": "tbl_TruckCrossItem",
  "primaryKey": "fld_TruckCrossItemId",

  "columns": [
    {"name":"fld_TruckCrossItemId","type":"int","identity":true},
    {"name":"fld_TruckCrossItemTitle","type":"nvarchar(256)"},
    {"name":"Type","type":"int"},
    {"name":"ProductUnit","type":"nvarchar(50)"},
    {"name":"ProductCount","type":"decimal"},
    {"name":"ProductSerial","type":"nvarchar(50)"},
    {"name":"ProductCode","type":"nvarchar(50)"},
    {"name":"TruckCrossProductTypeId","type":"int"},
    {"name":"TruckCrossId","type":"bigint"}
  ],

  "relationships": {
    "parents": [
      {"table":"tbl_TruckCross",
      "type":"Many-to-One",
      "via":"tbl_TruckCrossItem.fld_TruckCross → tbl_TruckCross.fld_TruckCrossId"},
      {"table":"tbl_TruckCrossProductType",
      "type":"Many-to-One",
      "via":"tbl_TruckCrossItem.fld_TruckCrossProductType → tbl_TruckCrossProductType.fld_TruckCrossProductTypeId"}
    ]
  },

  "businessMeaning": "Line-level items associated with a truck crossing operation.",
  "role": "FACT DETAIL"
}

---------------------------------------------------------
{
  "tableName": "tbl_TruckCrossOperationDestination",
  "primaryKey": "fld_TruckCrossOperationDestinationId",

  "columns": [
    {"name":"fld_TruckCrossOperationDestinationId","type":"int","identity":true},
    {"name":"fld_TruckCrossOperationDestinationTitle","type":"nvarchar(256)"}
  ],

  "relationships": {
    "children": ["tbl_TruckCross"]
  },

  "businessMeaning": "Defines destinations for truck operations.",
  "role": "DIMENSION"
}

---------------------------------------------------------
{
  "tableName": "tbl_TruckCrossOperationType",
  "primaryKey": "fld_TruckCrossOperationTypeId",

  "columns": [
    {"name":"fld_TruckCrossOperationTypeId","type":"int","identity":true},
    {"name":"fld_TruckCrossOperationTypeTitle","type":"nvarchar(256)"},
    {"name":"TruckCrossCauseId","type":"int"}
  ],

  "relationships": {
    "parents": [
      {"table":"tbl_TruckCrossCause",
      "type":"Many-to-One",
      "via":"tbl_TruckCrossOperationType.fld_TruckCrossCause → tbl_TruckCrossCause.fld_TruckCrossCauseId"}
    ],
    "children": ["tbl_TruckCross"]
  },

  "businessMeaning": "Defines types of truck operations linked to causes.",
  "role": "REFERENCE"
}

---------------------------------------------------------
{
  "tableName": "tbl_TruckCrossProductType",
  "primaryKey": "fld_TruckCrossProductTypeId",

  "columns": [
    {"name":"fld_TruckCrossProductTypeId","type":"int","identity":true},
    {"name":"fld_TruckCrossProductTypeTitle","type":"nvarchar(256)"},
    {"name":"TruckCrossCauseIdsArray","type":"nvarchar(max)"}
  ],

  "relationships": {
    "children": [
      "tbl_TruckCrossItem",
      "tbl_TruckCrossShippingFee"
    ]
  },

  "businessMeaning": "Defines product categories involved in truck operations.",
  "role": "DIMENSION"
}

---------------------------------------------------------
{
  "tableName": "tbl_TruckCrossShipment",
  "primaryKey": "fld_TruckCrossShipmentId",

  "columns": [
    {"name":"fld_TruckCrossShipmentId","type":"int","identity":true},
    {"name":"fld_TruckCrossShipmentTitle","type":"nvarchar(256)"}
  ],

  "relationships": {
    "children": [
      "tbl_TruckCross",
      "tbl_TruckCrossShippingFee"
    ]
  },

  "businessMeaning": "Represents shipment types or contracts.",
  "role": "DIMENSION"
}

---------------------------------------------------------
{
  "tableName": "tbl_TruckCrossShippingFee",
  "primaryKey": "fld_TruckCrossShippingFeeId",

  "columns": [
    {"name":"fld_TruckCrossShippingFeeId","type":"int","identity":true},
    {"name":"CompanyId","type":"int"},
    {"name":"CustomerId","type":"int"},
    {"name":"ProductTypeId","type":"int"},
    {"name":"ShipmentId","type":"int"},
    {"name":"FromDate","type":"nvarchar(50)"},
    {"name":"ToDate","type":"nvarchar(50)"},
    {"name":"FeeStatus","type":"bit"},
    {"name":"FeeAmount","type":"decimal"},
    {"name":"FeeWeight","type":"decimal"},
    {"name":"FeeDistance","type":"decimal"}
  ],

  "relationships": {
    "parents": [
      {"table":"tbl_TruckCrossCompany",
      "type":"Many-to-One",
      "via":"tbl_TruckCrossShippingFee.fld_TruckCrossShippingFeeCompanyId → tbl_TruckCrossCompany.fld_TruckCompanyId"},
      {"table":"tbl_TruckCrossCustomer",
      "type":"Many-to-One",
      "via":"tbl_TruckCrossShippingFee.fld_TruckCrossShippingFeeCustomerId → tbl_TruckCrossCustomer.fld_TruckCrossCustomerId"},
      {"table":"tbl_TruckCrossProductType",
      "type":"Many-to-One",
      "via":"tbl_TruckCrossShippingFee.fld_TruckCrossShippingFeeProductTypeId → tbl_TruckCrossProductType.fld_TruckCrossProductTypeId"},
      {"table":"tbl_TruckCrossShipment",
      "type":"Many-to-One",
      "via":"tbl_TruckCrossShippingFee.fld_TruckCrossShippingFeeShipmentId → tbl_TruckCrossShipment.fld_TruckCrossShipmentId"}
    ]
  },

  "businessMeaning": "Defines pricing rules for shipping based on company, customer, product type and shipment.",
  "role": "PRICING / FACT CONFIG"
}

---------------------------------------------------------
{
  "tableName": "tbl_TruckType",
  "primaryKey": "fld_TruckTypeId",

  "columns": [
    {"name":"fld_TruckTypeId","type":"int","identity":true},
    {"name":"fld_TruckTypeTitle","type":"nvarchar(256)"}
  ],

  "relationships": {
    "children": ["tbl_TruckCross"]
  },

  "businessMeaning": "Defines types of trucks used in operations.",
  "role": "DIMENSION"
}

---------------------------------------------------------
{
  "tableName": "tbl_NotificationEventTypes",

  "primaryKey": "Id",

  "columns": [
    {"name":"Id","type":"int","identity":true},
    {"name":"fld_NETitle","type":"nvarchar(50)"},
    {"name":"fld_NECommand","type":"nvarchar(max)"}
  ],

  "relationships": {
    "children": [
      "tbl_NotificationOrders"
    ]
  },

  "businessMeaning": "Defines the available notification event templates that trigger notification orders (e.g., inventory alert, movement completed, truck entered).",

  "role": "MASTER / CONFIGURATION",

  "semanticNotes": [
    "Stores notification event definitions.",
    "Command specifies the action or event that generates notifications."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_NotificationOrders",

  "primaryKey": "Id",

  "columns": [
    {"name":"Id","type":"int","identity":true},
    {"name":"fld_NOId","type":"int"},
    {"name":"fld_NOStatus","type":"int"},
    {"name":"fld_NODateTime","type":"datetime"},
    {"name":"fld_NOUserId","type":"nvarchar(50)"},
    {"name":"fld_NOType","type":"int"},
    {"name":"fld_NOTitle","type":"nvarchar(50)"},
    {"name":"fld_NOEventType","type":"int"},
    {"name":"fld_NOTimePeriod","type":"int"},
    {"name":"fld_NOSendDay","type":"nvarchar(50)"},
    {"name":"fld_NOSendClock","type":"nvarchar(5)"},
    {"name":"fld_NOSendType","type":"nvarchar(50)"},
    {"name":"fld_NOSendContacts","type":"nvarchar(max)"},
    {"name":"fld_NOContent","type":"nvarchar(max)"}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_User",
        "type": "Many-to-One",
        "via": "tbl_NotificationOrders.fld_NOUserId → tbl_User.Id"
      },
      {
        "table": "tbl_NotificationEventTypes",
        "type": "Many-to-One",
        "via": "tbl_NotificationOrders.fld_NOEventType → tbl_NotificationEventTypes.Id"
      }
    ],
    "children": [
      "tbl_NotificationQueue"
    ]
  },

  "businessMeaning": "Represents a notification definition or scheduled notification order created by users.",

  "role": "TRANSACTION HEADER",

  "timeField": "fld_NODateTime",

  "semanticNotes": [
    "One notification order may generate multiple queue records.",
    "Contains scheduling, recipients, event type and notification content."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_NotificationQueue",

  "primaryKey": "fld_Id",

  "columns": [
    {"name":"fld_Id","type":"int","identity":true},
    {"name":"fld_Text","type":"nvarchar(max)"},
    {"name":"fld_SendType","type":"int"},
    {"name":"fld_Contact","type":"nvarchar(256)"},
    {"name":"fld_SendDateTime","type":"datetime"},
    {"name":"fld_SendDate","type":"nvarchar(10)"},
    {"name":"fld_SendTime","type":"nvarchar(5)"},
    {"name":"fld_SendStatus","type":"int"},
    {"name":"fld_NotificationOrderId","type":"int"},
    {"name":"fld_QueueActionCode","type":"nvarchar(256)"},
    {"name":"fld_SaveDateTime","type":"datetime"}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_NotificationOrders",
        "type": "Many-to-One",
        "via": "tbl_NotificationQueue.fld_NotificationOrderId → tbl_NotificationOrders.Id"
      }
    ]
  },

  "businessMeaning": "Stores individual notification messages waiting to be sent or already processed through notification channels such as SMS, Email, or Messaging services.",

  "role": "FACT TABLE (Notification Detail)",

  "timeFields": [
    "fld_SaveDateTime",
    "fld_SendDateTime"
  ],

  "semanticNotes": [
    "Each record represents one outgoing notification.",
    "Generated from a Notification Order.",
    "Tracks delivery status and recipient information."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_Corridor",
  "primaryKey": "fld_CorridorId",

  "columns": [
    {"name": "fld_CorridorId", "type": "int", "identity": true, "nullable": false},
    {"name": "fld_CorridorWarehouseCode", "type": "nvarchar(50)", "nullable": false},
    {"name": "fld_CorridorName", "type": "nvarchar(128)", "nullable": false},
    {"name": "fld_CorridorDirection", "type": "int", "nullable": true},
    {"name": "fld_CorridorVerticalOrder", "type": "int", "nullable": true},
    {"name": "fld_CorridorHorizontalOrder", "type": "int", "nullable": true},
    {"name": "fld_CorridorWidth", "type": "int", "nullable": true},
    {"name": "fld_CorridorZoom", "type": "int", "nullable": true},
    {"name": "fld_CorridorIsFaken", "type": "bit", "nullable": false}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_Destination",
        "type": "Many-to-One",
        "via": "tbl_Corridor.fld_CorridorWarehouseCode → tbl_Destination.DestinationCode",
        "keyType": "Business Key (string)"
      }
    ],
    "children": [
      "tbl_Zones"
    ]
  },

  "businessMeaning": "Defines physical or logical warehouse corridors (aisles) used to structurally group storage zones within a specific warehouse destination.",
  "role": "DIMENSION (Warehouse Layout / Layout Layer 2)",
  "semanticNotes": [
    "Corridors belong to a Warehouse (stored in tbl_Destination where DestinationCode matches fld_CorridorWarehouseCode).",
    "A corridor contains multiple storage zones mapped in tbl_Zones."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_DocumentHeader",
  "primaryKey": "fld_DocumentKey",

  "columns": [
    {"name": "fld_Id", "type": "int", "identity": true, "nullable": false},
    {"name": "fld_DocumentKey", "type": "nvarchar(450)", "nullable": false},
    {"name": "fld_DocumentSaveUserId", "type": "nvarchar(50)", "nullable": true},
    {"name": "fld_DocumentImportType", "type": "int", "nullable": false},
    {"name": "fld_DocumentImportFileName", "type": "nvarchar(max)", "nullable": true},
    {"name": "fld_DocumentType", "type": "nvarchar(max)", "nullable": true},
    {"name": "fld_DocumentType1", "type": "nvarchar(max)", "nullable": true},
    {"name": "fld_DocumentType2", "type": "nvarchar(max)", "nullable": true},
    {"name": "fld_DocumentImportDatetime", "type": "datetime", "nullable": true},
    {"name": "fld_DocumentDesc", "type": "nvarchar(200)", "nullable": true},
    {"name": "fld_DocumentStatus", "type": "int", "nullable": false},
    {"name": "fld_DocumentHeaderData", "type": "nvarchar(max)", "nullable": true},
    {"name": "fld_DocumentParent", "type": "nvarchar(max)", "nullable": true},
    {"name": "fld_DocumentAggStatus", "type": "int", "nullable": true},
    {"name": "fld_DocumentDivideParent", "type": "nvarchar(max)", "nullable": true},
    {"name": "fld_DocumentChangeStatusLastUserId", "type": "nvarchar(50)", "nullable": true},
    {"name": "fld_DocumentCheckType", "type": "int", "nullable": true}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_User",
        "type": "Many-to-One",
        "via": "tbl_DocumentHeader.fld_DocumentSaveUserId → tbl_User.Id",
        "keyType": "Primary Key"
      },
      {
        "table": "tbl_DocumentStatus",
        "type": "Many-to-One",
        "via": "tbl_DocumentHeader.fld_DocumentStatus → tbl_DocumentStatus.fld_DocumentStatusId",
        "keyType": "Primary Key"
      }
    ],
    "children": [
      "tbl_DocumentItem"
    ]
  },

  "businessMeaning": "Stores master header records for system documents, invoices, purchase orders, or delivery permissions imported via API, Excel, or manual entry.",
  "role": "TRANSACTION HEADER (Document Master Layer)",
  "timeField": "fld_DocumentImportDatetime",
  "semanticNotes": [
    "fld_DocumentImportType mapping (Enum ImportType): 0=Excel, 1=Api, 2=Manual, 3=Other, 4=Aggregate, 5=Divide.",
    "This table links directly to movement workflows through DocumentId/DocumentKey mappings.",
    "When user asks for document owner or issuer, join with tbl_User."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_DocumentItem",
  "primaryKey": "fld_Id",

  "columns": [
    {"name": "fld_Id", "type": "int", "identity": true, "nullable": false},
    {"name": "fld_DocumentKey", "type": "nvarchar(450)", "nullable": true},
    {"name": "fld_DocumentType", "type": "nvarchar(max)", "nullable": false},
    {"name": "fld_DocumentType1", "type": "nvarchar(max)", "nullable": false},
    {"name": "fld_DocumentType2", "type": "nvarchar(max)", "nullable": false},
    {"name": "fld_DocumentItemProductCode", "type": "nvarchar(50)", "nullable": true},
    {"name": "fld_DocumentItemProductTitle", "type": "nvarchar(50)", "nullable": true},
    {"name": "fld_DocumentItemCount", "type": "decimal(18,2)", "nullable": false},
    {"name": "fld_DocumentItemProducUnit", "type": "nvarchar(50)", "nullable": true},
    {"name": "fld_DocumentItemsData", "type": "nvarchar(max)", "nullable": true}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_DocumentHeader",
        "type": "Many-to-One",
        "via": "tbl_DocumentItem.fld_DocumentKey → tbl_DocumentHeader.fld_DocumentKey",
        "keyType": "Business Key"
      },
      {
        "table": "tbl_Products",
        "type": "Many-to-One",
        "via": "tbl_DocumentItem.fld_DocumentItemProductCode → tbl_Products.ProductCode",
        "keyType": "Business Key"
      }
    ]
  },

  "businessMeaning": "Stores line-level detail items for documents, specifying target products, allowed quotas, or requested quantities for warehouse execution.",
  "role": "TRANSACTION DETAIL (Document Line Layer)",
  "semanticNotes": [
    "Core table to track expected vs actual logic. Contains the formal quantity allocated for a product serial/code.",
    "Always sum fld_DocumentItemCount when computing target document totals."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_DocumentStatus",
  "primaryKey": "fld_DocumentStatusId",

  "columns": [
    {"name": "fld_DocumentStatusId", "type": "int", "nullable": false},
    {"name": "fld_DocumentStatusTitle", "type": "nvarchar(256)", "nullable": false},
    {"name": "fld_DocumentStatusIsUpdatePermitted", "type": "bit", "nullable": false},
    {"name": "fld_DocumentStatusIsCartablePermitted", "type": "bit", "nullable": false}
  ],

  "relationships": {
    "children": [
      "tbl_DocumentHeader"
    ]
  },

  "businessMeaning": "Defines states and workflow lifecycle permissions for document headers (e.g., Draft, Approved, Executing, Closed).",
  "role": "DIMENSION (Workflow State Control)",
  "semanticNotes": [
    "Use fld_DocumentStatusTitle to filter headers by status strings like N'تایید شده' or N'موقت' based on user prompts."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_DynamicFields",
  "primaryKey": "fld_DynamicFieldId",

  "columns": [
    {"name": "fld_DynamicFieldId", "type": "int", "identity": true, "nullable": false},
    {"name": "fld_DynamicFieldUser", "type": "nvarchar(128)", "nullable": true},
    {"name": "fld_DynamicFieldTitle", "type": "nvarchar(50)", "nullable": true},
    {"name": "fld_DynamicFieldType", "type": "int", "nullable": false},
    {"name": "fld_IsSystematicField", "type": "bit", "nullable": false},
    {"name": "fld_IsHeaderKey", "type": "bit", "nullable": false},
    {"name": "fld_DynamicFieldDateTime", "type": "datetime", "nullable": true},
    {"name": "fld_DynamicFieldRelatedTitle1", "type": "nvarchar(50)", "nullable": true},
    {"name": "fld_DynamicFieldRelatedTitle2", "type": "nvarchar(50)", "nullable": true},
    {"name": "fld_DynamicFieldRelatedTitle3", "type": "nvarchar(50)", "nullable": true},
    {"name": "fld_DynamicFieldActionType", "type": "int", "nullable": true},
    {"name": "fld_DynamicFieldShowColumn", "type": "bit", "nullable": false},
    {"name": "fld_DynamicFieldShowColumnForAction", "type": "bit", "nullable": false},
    {"name": "fld_DynamicFieldDocGroupAggregate", "type": "bit", "nullable": false},
    {"name": "fld_DynamicFieldValueType", "type": "int", "nullable": true},
    {"name": "fld_DynamicFieldDefaultValue", "type": "nvarchar(128)", "nullable": true},
    {"name": "fld_DynamicFieldValueOptions", "type": "nvarchar(max)", "nullable": true},
    {"name": "fld_DynamicFieldRequirement", "type": "bit", "nullable": true},
    {"name": "fld_DynamicFieldOrder", "type": "int", "nullable": true},
    {"name": "fld_DynamicFieldSectionId", "type": "int", "nullable": true},
    {"name": "fld_DynamicFieldParentId", "type": "int", "nullable": true},
    {"name": "fld_DynamicFieldIsReadOnly", "type": "bit", "nullable": true}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_User",
        "type": "Many-to-One",
        "via": "tbl_DynamicFields.fld_DynamicFieldUser → tbl_User.Id",
        "keyType": "Primary Key"
      },
      {
        "table": "tbl_ActionTypes",
        "type": "Many-to-One",
        "via": "tbl_DynamicFields.fld_DynamicFieldActionType → tbl_ActionTypes.fld_ActionTypeId",
        "keyType": "Business Key"
      },
      {
        "table": "tbl_DynamicFieldSections",
        "type": "Many-to-One",
        "via": "tbl_DynamicFields.fld_DynamicFieldSectionId → tbl_DynamicFieldSections.fld_DynamicFieldSectionId",
        "keyType": "Primary Key"
      }
    ]
  },

  "businessMeaning": "Stores metadata definitions for dynamic metadata fields or custom attributes that can be attached to transactions, vehicles, or document headers across different deployment sites.",
  "role": "DIMENSION / METADATA DEFINITION (Dynamic UI/Schema Configuration)",
  "timeField": "fld_DynamicFieldDateTime",
  "semanticNotes": [
    "Defines custom metadata extensions. Values captured for these fields are typically stored inside stringified JSON payloads such as ProductProperties or MovementActionData across fact tables.",
    "fld_DynamicFieldActionType connects specific dynamic attributes to target workflows in tbl_ActionTypes."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_DynamicFieldSections",
  "primaryKey": "fld_DfSectionId",

  "columns": [
    {"name": "fld_DfSectionId", "type": "int", "nullable": false},
    {"name": "fld_DfSectionTitle", "type": "nvarchar(256)", "nullable": false},
    {"name": "fld_DfType", "type": "int", "nullable": false}
  ],

  "relationships": {
    "children": [
      "tbl_DynamicFields"
    ]
  },

  "businessMeaning": "Defines structural UI categories or layout sections used to visually group related custom dynamic fields together on forms and reports.",
  "role": "DIMENSION / METADATA HIERARCHY (UI Form Section Configuration)",
  "semanticNotes": [
    "Acts as a parent grouping table for tbl_DynamicFields via fld_DynamicFieldSectionId.",
    "fld_DfSectionTitle stores the display name of the grouping tab or panel (e.g., N'اطلاعات خودرو', N'مشخصات کیفی کالا')."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_FreezeHeader",
  "primaryKey": "fld_FreezeHeaderId",

  "columns": [
    {"name": "fld_FreezeHeaderId", "type": "int", "identity": true, "nullable": false},
    {"name": "fld_FreezeUserId", "type": "nvarchar(128)", "nullable": true},
    {"name": "fld_FreezeSaveDateTime", "type": "datetime", "nullable": true},
    {"name": "fld_FreezeDesc", "type": "nvarchar(256)", "nullable": true},
    {"name": "fld_FreezeResult", "type": "bit", "nullable": false}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_User",
        "type": "Many-to-One",
        "via": "tbl_FreezeHeader.fld_FreezeUserId → tbl_User.Id",
        "keyType": "Primary Key"
      }
    ],
    "children": [
      "tbl_FreezeItem"
    ]
  },

  "businessMeaning": "Stores master header records for inventory freezing operations, which lock specific product serials/tags from being moved or modified during audits, disputes, or quality control holds.",
  "role": "TRANSACTION HEADER (Inventory Lock Layer)",
  "timeField": "fld_FreezeSaveDateTime",
  "semanticNotes": [
    "Represents an inventory block or freeze event managed by a system user.",
    "fld_FreezeResult indicates the overall enforcement state or status of the freeze operation (e.g., active/inactive lock).",
    "Links to the specific inventory units through tbl_FreezeItem."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_FreezeItem",
  "primaryKey": "fld_FreezeItemId",

  "columns": [
    {"name": "fld_FreezeItemId", "type": "int", "identity": true, "nullable": false},
    {"name": "fld_FreezeProductSerial", "type": "nvarchar(50)", "nullable": true},
    {"name": "fld_FreezeHeaderId", "type": "int", "nullable": false}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_FreezeHeader",
        "type": "Many-to-One",
        "via": "tbl_FreezeItem.fld_FreezeHeaderId → tbl_FreezeHeader.fld_FreezeHeaderId",
        "keyType": "Primary Key"
      },
      {
        "table": "tbl_Tags",
        "type": "Many-to-One",
        "via": "tbl_FreezeItem.fld_FreezeProductSerial → tbl_Tags.ProductSerial",
        "keyType": "Business Key (string)"
      }
    ]
  },

  "businessMeaning": "Stores line-level details of frozen operations, mapping individual product serials (RFID tags) to their respective freeze lock header rule.",
  "role": "TRANSACTION DETAIL (Inventory Lock Detail)",
  "semanticNotes": [
    "Core table to identify exactly which physical pallets or products are under an active freeze hold.",
    "Always join with tbl_Tags on fld_FreezeProductSerial to resolve current location, status, or properties of the locked units."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_Print",
  "primaryKey": "Id",

  "columns": [
    {"name": "Id", "type": "int", "identity": true, "nullable": false},
    {"name": "ProductSerial", "type": "nvarchar(50)", "nullable": true},
    {"name": "ProductCode", "type": "nvarchar(50)", "nullable": true},
    {"name": "ProductName", "type": "nvarchar(500)", "nullable": true},
    {"name": "ProductDescription", "type": "nvarchar(300)", "nullable": true},
    {"name": "ProductType", "type": "nvarchar(50)", "nullable": true},
    {"name": "ProductCount", "type": "decimal(18,2)", "nullable": true},
    {"name": "ProductItemCount", "type": "decimal(18,2)", "nullable": true},
    {"name": "ProductCountInPack", "type": "decimal(18,2)", "nullable": true},
    {"name": "ProductUnit", "type": "nvarchar(50)", "nullable": true},
    {"name": "ProductSize", "type": "nvarchar(50)", "nullable": true},
    {"name": "ProductRegCode", "type": "nvarchar(50)", "nullable": true},
    {"name": "ProductWeight", "type": "decimal(18,2)", "nullable": true},
    {"name": "ProductVolume", "type": "decimal(18,2)", "nullable": true},
    {"name": "ProductStatus", "type": "nvarchar(50)", "nullable": true},
    {"name": "ProjectCode", "type": "nvarchar(50)", "nullable": true},
    {"name": "TagEpc", "type": "nvarchar(50)", "nullable": true},
    {"name": "ProductProductionShift", "type": "nvarchar(50)", "nullable": true},
    {"name": "ProductProductionLine", "type": "nvarchar(50)", "nullable": true},
    {"name": "ProductContractType", "type": "int", "nullable": true},
    {"name": "PackageId", "type": "int", "nullable": true},
    {"name": "Location", "type": "nvarchar(50)", "nullable": true},
    {"name": "PrintActionId", "type": "int", "nullable": true},
    {"name": "PrintType", "type": "nvarchar(50)", "nullable": true},
    {"name": "PrintActionDateTime", "type": "datetime", "nullable": true},
    {"name": "PrintUser", "type": "nvarchar(50)", "nullable": true},
    {"name": "PrintFlag", "type": "int", "nullable": true},
    {"name": "RegisterActionDateTime", "type": "datetime", "nullable": true},
    {"name": "RegisterFlag", "type": "int", "nullable": true},
    {"name": "RegisterType", "type": "int", "nullable": true},
    {"name": "InputFileName", "type": "nvarchar(50)", "nullable": true},
    {"name": "ErrorTime", "type": "nvarchar(50)", "nullable": true},
    {"name": "ErrorDesc", "type": "nvarchar(200)", "nullable": true},
    {"name": "SoftDelete", "type": "int", "nullable": true},
    {"name": "ReRegister", "type": "bit", "nullable": true},
    {"name": "PrintQueue", "type": "bit", "nullable": true},
    {"name": "DocumentId", "type": "nvarchar(50)", "nullable": true},
    {"name": "DocumentItemId", "type": "nvarchar(50)", "nullable": true},
    {"name": "offline_epc", "type": "nvarchar(50)", "nullable": true},
    {"name": "offline_reg_status", "type": "int", "nullable": true},
    {"name": "offline_regDate", "type": "nvarchar(10)", "nullable": true},
    {"name": "RegisterUser", "type": "nvarchar(50)", "nullable": true},
    {"name": "RegisterDate", "type": "nvarchar(50)", "nullable": true},
    {"name": "Manufacturer", "type": "nvarchar(50)", "nullable": true},
    {"name": "AntiFire", "type": "nvarchar(50)", "nullable": true},
    {"name": "DestinationCode", "type": "nvarchar(50)", "nullable": true},
    {"name": "SoftDeleteUser", "type": "nvarchar(50)", "nullable": true},
    {"name": "SoftDeleteDate", "type": "nvarchar(50)", "nullable": true},
    {"name": "fld_ProductGroup", "type": "nvarchar(128)", "nullable": true},
    {"name": "fld_ProductBrand", "type": "nvarchar(128)", "nullable": true},
    {"name": "ProductProperties", "type": "nvarchar(max)", "nullable": true},
    {"name": "fld_ProductSubGroup", "type": "nvarchar(128)", "nullable": true},
    {"name": "fld_ProductClass", "type": "nvarchar(128)", "nullable": true}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_Products",
        "type": "Many-to-One",
        "via": "tbl_Print.ProductCode → tbl_Products.ProductCode",
        "keyType": "Business Key (string)"
      },
      {
        "table": "tbl_Tags",
        "type": "Many-to-One",
        "via": "tbl_Print.ProductSerial → tbl_Tags.ProductSerial",
        "keyType": "Business Key (string)"
      },
      {
        "table": "tbl_DocumentHeader",
        "type": "Many-to-One",
        "via": "tbl_Print.DocumentId → tbl_DocumentHeader.fld_DocumentKey",
        "keyType": "Business Key (string)"
      },
      {
        "table": "tbl_Destination",
        "type": "Many-to-One",
        "via": "tbl_Print.DestinationCode → tbl_Destination.DestinationCode",
        "keyType": "Business Key (string)"
      },
      {
        "table": "tbl_User",
        "type": "Many-to-One",
        "via": "tbl_Print.SoftDeleteUser → tbl_User.Id",
        "keyType": "Primary Key"
      }
    ]
  },

  "businessMeaning": "Stores transaction history and logging for RFID tag printing and registration actions, acting as a denormalized historical ledger tracking what metadata was written to an EPC during print execution.",
  "role": "TRANSACTION FACT (Printing Log Layer)",
  "timeField": "PrintActionDateTime",
  "semanticNotes": [
    "This table acts as a flattened snapshot ledger for print queue events.",
    "SoftDelete column flags removed log items (e.g. where SoftDelete = 1 means deleted).",
    "Links print metrics back to core SKU data via ProductCode and physical items via ProductSerial."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_Province",
  "primaryKey": "fld_Id",

  "columns": [
    {"name": "fld_Id", "type": "int", "identity": true, "nullable": false},
    {"name": "fld_Name", "type": "nvarchar(50)", "nullable": false}
  ],

  "relationships": {
    "children": [
      "tbl_City"
    ]
  },

  "businessMeaning": "Defines geographical provinces or states used for mapping warehouse locations, customer addresses, or regional distribution tracking within the logistics lifecycle.",
  "role": "DIMENSION / REFERENCE (Geographical Hierarchy Level 1)",
  "semanticNotes": [
    "Acts as the master root for regional classification.",
    "Contains a one-to-many relationship with tbl_City via the child collection."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_UHFReaderLogHeader",
  "primaryKey": "fld_UHFReaderLogHeaderId",

  "columns": [
    {"name": "fld_UHFReaderLogHeaderId", "type": "int", "identity": true, "nullable": false},
    {"name": "fld_StationCode", "type": "nvarchar(128)", "nullable": true},
    {"name": "fld_ActionType", "type": "nvarchar(128)", "nullable": true},
    {"name": "fld_DocumentCode", "type": "nvarchar(450)", "nullable": true},
    {"name": "fld_TruckCrossId", "type": "bigint", "nullable": true},
    {"name": "fld_UHFReaderLogHeaderUserId", "type": "nvarchar(128)", "nullable": true},
    {"name": "fld_UHFReaderLogHeaderControlType", "type": "int", "nullable": true},
    {"name": "fld_CarProperties", "type": "nvarchar(max)", "nullable": true},
    {"name": "fld_HeaderUsedStatus", "type": "int", "nullable": true},
    {"name": "fld_MovementActionId", "type": "int", "nullable": true},
    {"name": "fld_HeaderCreateDateTime", "type": "datetime", "nullable": true}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_Station",
        "type": "Many-to-One",
        "via": "tbl_UHFReaderLogHeader.fld_StationCode → tbl_Station.fld_StationCode",
        "keyType": "Business Key (string)"
      },
      {
        "table": "tbl_DocumentHeader",
        "type": "Many-to-One",
        "via": "tbl_UHFReaderLogHeader.fld_DocumentCode → tbl_DocumentHeader.fld_DocumentKey",
        "keyType": "Business Key (string)"
      },
      {
        "table": "tbl_TruckCross",
        "type": "Many-to-One",
        "via": "tbl_UHFReaderLogHeader.fld_TruckCrossId → tbl_TruckCross.fld_TruckCrossId",
        "keyType": "Primary Key"
      },
      {
        "table": "tbl_User",
        "type": "Many-to-One",
        "via": "tbl_UHFReaderLogHeader.fld_UHFReaderLogHeaderUserId → tbl_User.Id",
        "keyType": "Primary Key"
      },
      {
        "table": "tbl_MovementActions",
        "type": "Many-to-One",
        "via": "tbl_UHFReaderLogHeader.fld_MovementActionId → tbl_MovementActions.MovementActionId",
        "keyType": "Primary Key"
      }
    ],
    "children": [
      "tbl_UHF_ReaderLog"
    ]
  },

  "businessMeaning": "Stores hardware-level transaction headers for bulk RFID scanning events captured automatically by physical UHF gates or reader stations.",
  "role": "TRANSACTION HEADER (Hardware Scan Event Layer)",
  "timeField": "fld_HeaderCreateDateTime",
  "semanticNotes": [
    "Serves as the bridge between raw physical hardware antenna reads and the validated warehouse actions in tbl_MovementActions.",
    "fld_CarProperties usually contains stringified JSON metadata regarding the scanned vehicle."
  ]
}

---------------------------------------------------------
{
  "tableName": "tbl_UHF_ReaderLog",
  "primaryKey": "id",

  "columns": [
    {"name": "id", "type": "int", "identity": true, "nullable": false},
    {"name": "fld_TagSerial", "type": "nvarchar(50)", "nullable": true},
    {"name": "fld_Reader_Gate", "type": "nvarchar(256)", "nullable": true},
    {"name": "fld_ReaderIp", "type": "nvarchar(15)", "nullable": true},
    {"name": "fld_TagRead_DateTime", "type": "nvarchar(50)", "nullable": true},
    {"name": "fld_TagSelectedFlag", "type": "tinyint", "nullable": true},
    {"name": "fld_InventoryId", "type": "int", "nullable": true},
    {"name": "fld_Reader_GateType", "type": "int", "nullable": true},
    {"name": "fld_Desc", "type": "nvarchar(256)", "nullable": true},
    {"name": "fld_ReaderDeviceType", "type": "int", "nullable": true},
    {"name": "ActionStatus", "type": "int", "nullable": true},
    {"name": "ActionDesc", "type": "nvarchar(250)", "nullable": true},
    {"name": "fld_DocumentId", "type": "nvarchar(128)", "nullable": true},
    {"name": "fld_WMUserId", "type": "nvarchar(50)", "nullable": true},
    {"name": "fld_InventoryPackage", "type": "int", "nullable": true},
    {"name": "MovementActionId", "type": "int", "nullable": true},
    {"name": "fld_TagRead_DateTimeMiladi", "type": "datetime", "nullable": true},
    {"name": "fld_SaveUserId", "type": "nvarchar(128)", "nullable": true},
    {"name": "fld_ProductSerial", "type": "nvarchar(50)", "nullable": true}
  ],

  "relationships": {
    "parents": [
      {
        "table": "tbl_UHFReaderLogHeader",
        "type": "Many-to-One",
        "via": "tbl_UHF_ReaderLog.fld_InventoryId → tbl_UHFReaderLogHeader.fld_UHFReaderLogHeaderId",
        "keyType": "Primary Key"
      },
      {
        "table": "tbl_Tags",
        "type": "Many-to-One",
        "via": "tbl_UHF_ReaderLog.fld_ProductSerial → tbl_Tags.ProductSerial",
        "keyType": "Business Key (string)"
      },
      {
        "table": "tbl_DocumentHeader",
        "type": "Many-to-One",
        "via": "tbl_UHF_ReaderLog.fld_DocumentId → tbl_DocumentHeader.fld_DocumentKey",
        "keyType": "Business Key (string)"
      },
      {
        "table": "tbl_MovementActions",
        "type": "Many-to-One",
        "via": "tbl_UHF_ReaderLog.MovementActionId → tbl_MovementActions.MovementActionId",
        "keyType": "Primary Key"
      }
    ]
  },

  "businessMeaning": "Stores raw, line-level RFID tag signals caught instantly by reader antennas, tracking every time an antenna detects an active unit during operations.",
  "role": "FACT TABLE (Raw Hardware Signal Stream Log)",
  "timeField": "fld_TagRead_DateTimeMiladi",
  "semanticNotes": [
    "This is a massive buffer ledger populated by active RFID middleware/services.",
    "fld_InventoryId maps items directly to their master parent batch scan context in tbl_UHFReaderLogHeader.",
    "Always filter using fld_TagRead_DateTimeMiladi for robust calendar dates rather than the Persian text field fld_TagRead_DateTime."
  ]
}