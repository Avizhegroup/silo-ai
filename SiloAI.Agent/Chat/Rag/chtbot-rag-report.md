# Knowledge Base – Database Schema & Business Logic

## Available Tables

The following tables exist in the database. Use only these tables when building queries.

| Table | Description |
|---|---|
| `tbl_Tags` | Products currently in the warehouse (inventory items / RFID tags) |
| `tbl_Destination` | Warehouses and warehouse hierarchy |
| `tbl_DestinationType` | Types and categories of warehouses/destinations |
| `tbl_Zones` | Locations / storage zones inside a warehouse |
| `tbl_ProductBrand` | Product brands |
| `tbl_ProductGroup` | Product groups |
| `tbl_ProductSubGroup` | Product sub-groups |
| `tbl_ProductClass` | Product classes |
| `tbl_ProductStatus` | Product quality/status codes |
| `tbl_ProductPropertyA` | Product property dimension A |
| `tbl_ProductPropertyB` | Product property dimension B (linked to A) |
| `tbl_ProductPropertyC` | Product property dimension C |
| `tbl_ProductType` | Product types and categories |
| `tbl_MovementActions` | History of all movements, operations, and transactions (Header) |
| `tbl_TagsMovement` | Details of tags moved in a transaction (Line items) |
| `tbl_ActionTypes` | Types of operations (e.g., Entry, Exit, Sale) |
| `tbl_Station` | Gates or stations where RFID logs are recorded |
| `tbl_User` | System users and operators |

### Key Column Reference

**`tbl_Tags`** (main inventory table)
- `ProductSerial`, `TagEpc` – primary key
- `ProductCode`, `ProductName`, `ProductType`
- `ProductStatus` – quality/status title
- `ProductCount` – quantity
- `TagInDestinationId` – warehouse code (FK → `tbl_Destination.DestinationCode`)
- `TagZone` – zone code (FK → `tbl_Zones.ZoneCode`)
- `TagRegisterDateTime` – registration datetime
- `TagRegisterShamsiUnixDate` – Shamsi (Persian) date as Unix timestamp
- `TagStatus` – tag lifecycle status
- `Lock`, `Freeze`, `Deactivate` – boolean flags. (Only filter by `Deactivate = 0` if the user explicitly says "فعال", or `Deactivate = 1` for "غیرفعال". If not mentioned, do not filter).
- `fld_ProductPropertyAId`, `fld_ProductPropertyBId`, `fld_ProductPropertyCId`
- `fld_ProductGroup`, `fld_ProductBrand`, `fld_ProductSubGroup`, `fld_ProductClass`
- `TagRegisterUser`, `Username`, `fld_LastModifierUser`
- `ProductType` – Product type code (FK → `tbl_ProductType.ProductTypeCode`)

**`tbl_Destination`** (warehouses)
- `DestinationId`, `DestinationCode`, `DestinationTitle`
- `DestinationType`, `DestinationParentId` – hierarchy

**`tbl_Zones`** (locations)
- `ZoneCode`, `ZoneTitle`, `ZoneStoreCode` – warehouse code
- `ZoneCapacity`, `ZoneOccupiedCapacity`

**`tbl_MovementActions`** (Transaction Header)
- `MovementActionId` – Primary Key
- `MovementActionStore` – Source warehouse code (FK → `tbl_Destination.DestinationCode`)
- `MovementActionDestinationId` – Target warehouse code (FK → `tbl_Destination.DestinationCode`)
- `MovementActionTp` – Action type code (FK → `tbl_ActionTypes.fld_ActionTypeId`)
- `MovementActionUserId` – User ID (FK → `tbl_User.Id`)
- `MovementActionUHFLogGate` – Station/Gate code (FK → `tbl_Station.fld_StationCode`)
- `MovementActionDate`, `MovementActionTime`, `MovementActionDesc`

**`tbl_TagsMovement`** (Transaction Details)
- `TagsMovementId` – Primary Key
- `RMovementActionId` – FK to `tbl_MovementActions.MovementActionId`
- `ProductSerial`, `ProductCode`
- `ProductCount` – quantity moved

**`tbl_ActionTypes`, `tbl_Station`, `tbl_User`** (Lookups)
- `tbl_ActionTypes`: `fld_ActionTypeId`, `fld_ActionTypeTitle`
- `tbl_Station`: `fld_StationCode`, `fld_StationName`
- `tbl_User`: `Id`, `Name`, `Username`

**`tbl_Products`** (Master Product Data)
- `ProductCode` – Primary Key
- `ProductTitle`, `ProductTechnicalCode`, `ProductSize`

**`tbl_DestinationType`** (Warehouse Types)
- `fld_DestinationTypeCode` – Primary Key
- `fld_DestinationTypeName` – Type Name/Title
*(Note: `tbl_Destination.DestinationType` is the FK to `tbl_DestinationType.fld_DestinationTypeCode`)*

### Foreign Key & Lookup Table Mapping (CRITICAL FOR FILTERING)
When a user wants to filter by a name or title (e.g., Warehouse, Zone, Product, Size, Line, Shift), you MUST use this mapping to JOIN the correct table and filter on the Title/Name column using `LIKE`.
**NEVER** compare a user's Persian text directly against a code/ID column in `tbl_Tags`.

| Entity Name (User asks for...) | FK in `tbl_Tags` | Target Table | Join Condition | Filter Column (Use LIKE) |
|---|---|---|---|---|
| Warehouse / Destination | `TagInDestinationId` | `tbl_Destination` | `DestinationCode` | `DestinationTitle` |
| Zone / Location | `TagZone` | `tbl_Zones` | `ZoneCode` | `ZoneTitle` |
| Master Product | `ProductCode` | `tbl_Products` | `ProductCode` | `ProductTitle` |
| Brand | `fld_ProductBrand` | `tbl_ProductBrand` | `fld_ProductBrandCode` | `fld_ProductBrandTitle` |
| Group | `fld_ProductGroup` | `tbl_ProductGroup` | `fld_ProductGroupCode` | `fld_ProductGroupTitle` |
| SubGroup | `fld_ProductSubGroup` | `tbl_ProductSubGroup` | `fld_ProductSubGroupCode` | `fld_ProductSubGroupTitle` |
| Class | `fld_ProductClass` | `tbl_ProductClass` | `fld_ProductClassCode` | `fld_ProductClassTitle` |
| Production Line (Property A) | `fld_ProductPropertyAId`| `tbl_ProductPropertyA`| `fld_ProductPropertyAId` | `fld_ProductPropertyATitle` |
| Work Shift (Property B) | `fld_ProductPropertyBId`| `tbl_ProductPropertyB`| `fld_ProductPropertyBId` | `fld_ProductPropertyBTitle` |
| Size (Property C) | `fld_ProductPropertyCId`| `tbl_ProductPropertyC`| `fld_ProductPropertyCId` | `fld_ProductPropertyCTitle` |
| Quality Status | `ProductStatus` | `tbl_ProductStatus` | `ProductStatusCode` | `ProductStatusTitle` |
| Product Type / Category | `ProductType` | `tbl_ProductType` | `ProductTypeCode` | `ProductTypeTitle` |
| Action Type / Operation | `MovementActionTp` | `tbl_ActionTypes` | `fld_ActionTypeId` | `fld_ActionTypeTitle` |
| Station / Gate | `MovementActionUHFLogGate` | `tbl_Station` | `fld_StationCode` | `fld_StationName` |
| User / Operator | `MovementActionUserId` | `tbl_User` | `Id` | `Name` |
| Warehouse Type (نوع انبار) | `DestinationType` | `tbl_DestinationType` | `fld_DestinationTypeCode` | `fld_DestinationTypeName` |

#### 1. ENTITY RESOLUTION — Where to Start the Query
Choose the correct starting table based on the user's intent:
- **Products (Master Data):**
  If asking about product definitions, technical specifications, packaging, product attributes, or master product information → Start with `tbl_Products`.

- **Inventory / Current Tags:**
  If asking about currently registered tags, current stock, or the current state/location of tagged items → Start with `tbl_Tags`.

- **Entry History (ورود):**
  If asking about goods entering the warehouse or entry history → Start with `tbl_TagsMovement` and relate the movement to `tbl_MovementActions` through `RMovementActionId`.

- **Exit / Return History (خروج / برگشت):**
  If asking about goods leaving the warehouse or returning to the warehouse → Start with `tbl_TagsMovement` and relate the movement to `tbl_MovementActions` through `HMovementActionId`.
  NEVER assume `HMovementActionId` alone means a physical exit. Always verify the actual operation using `MovementActionTp` and, when necessary, `MovementActionData`.

- **Operations / Movements (عملیات):**
  If asking about registered operations, users, gates, documents, operation dates, operation types, or movement records → Start with `tbl_MovementActions`.

- **RFID Logs:**
  If asking whether a tag was physically detected/read by an RFID reader, antenna, or gate → Start with `tbl_UHF_ReaderLog`.

- **Truck / Vehicle Crossings:**
  If asking about trucks, vehicle plates, drivers, shipments, or Truck Cross information → Start with `tbl_TruckCross`, or join it through `tbl_MovementActions.MovementActionTruckCrossId` when the question is operation-related.

- **Handheld Inventory (انبارگردانی دستی):**
  If asking about manual inventory counting performed using handheld devices → Start with `tbl_InventoryTags`.

- **Physical Counting (شمارش فیزیکی):**
  This is a separate business concept from handheld inventory counting. Do NOT automatically treat "شمارش فیزیکی" as "انبارگردانی". Physical counting may involve other reader devices and Excel-based processes.

- **Placement Missions (جانمایی):**
  If asking about product/tag placement or movement between zones → Start with `tbl_ProductPlacementMissions`.

#### 2. THE GOLDEN MOVEMENT CHAIN — History Queries
For movement and history queries, understand and follow this relationship chain:
`tbl_Tags`
→ `ProductSerial`
→ `tbl_TagsMovement`
→ `RMovementActionId / HMovementActionId`
→ `tbl_MovementActions`
→ `MovementActionTp`
→ `tbl_ActionTypes`

Important:
- `RMovementActionId` and `HMovementActionId` represent different movement-history paths in the system.
- `RMovementActionId` is used by the entry/registration movement path in the existing system logic.
- `HMovementActionId` is used by the exit/return movement path in the existing system logic.
- `HMovementActionId` alone does NOT prove that the operation was a physical exit.
- To determine the exact business meaning of an operation, inspect:

  * `tbl_MovementActions.MovementActionTp`
  * `tbl_ActionTypes.fld_ActionTypeTitle`
  * `tbl_MovementActions.MovementActionData` when necessary.

#### 3. LOCATION LOGIC — Destination vs. DestinationType
Never confuse `tbl_Destination` with `tbl_DestinationType`.
- `tbl_Destination` = Actual physical location / warehouse / destination.
- `tbl_DestinationType` = Category or type of the physical location.

Conceptually:
`tbl_Destination.DestinationType` → `tbl_DestinationType`

The operation table contains location references:
- `tbl_MovementActions.MovementActionStore`
  → References a `DestinationCode` in `tbl_Destination` and represents the Store/location associated with the operation.

- `tbl_MovementActions.MovementActionDestinationId`
  → References a `DestinationCode` in `tbl_Destination` and represents the destination/target associated with the operation.

Do NOT automatically assume `MovementActionStore` is always the physical source (مبدأ) in every business scenario. Determine the actual source/target semantics from the specific `ActionType` and the operation's business logic.

For determining the general movement type:
- `tbl_ActionTypes.fld_ActionTypeFromDestinationType` → Source location type.

- `tbl_ActionTypes.fld_ActionTypeToTypeDestinationType` → Target location type.

These values relate conceptually to `tbl_DestinationType`.

Therefore, to understand a movement such as:
`نوع مکان مبدأ → نوع عملیات → نوع مکان مقصد`
you may need to combine:
`tbl_ActionTypes`
+
`tbl_DestinationType`
+
`tbl_Destination`
+
`tbl_MovementActions`

#### 4. DYNAMIC DATA HANDLING — JSON
Dynamic JSON fields MUST NOT be queried as ordinary relational columns.
Use `JSON_VALUE` to extract individual JSON properties.
- **Tag / Item Dynamic Properties:**
`JSON_VALUE(t.ProductProperties, N'$."FieldName"')`
Source: `tbl_Tags.ProductProperties`

- **Operation Dynamic Properties:**
`JSON_VALUE(ma.MovementActionData, N'$."FieldName"')`
Source: `tbl_MovementActions.MovementActionData`

- **Master Product Technical Properties:**
`JSON_VALUE(p.ProductTechnicalData, N'$."FieldName"')`
Source: `tbl_Products.ProductTechnicalData`

Always determine which JSON field contains the requested business property before writing the query.

#### 5. AGGREGATION RULES — CRITICAL
Do NOT confuse the number of unique tagged items with the total quantity.
If the user asks:
- "How many unique items/tags/serials?"
- "تعداد کالا"
- "تعداد سریال"
- "چند تگ داریم؟"
Use:
`COUNT(DISTINCT ProductSerial)`
Typically:
`COUNT(DISTINCT tm.ProductSerial)`
This returns the number of unique serials/items.

If the user asks:
- "What is the total quantity?"
- "مجموع موجودی"
- "مجموع تعداد کالا"
- "چه مقدار کالا داریم؟"
Use:
`SUM(ProductCount)`
Typically:
`SUM(t.ProductCount)`
This returns the sum of the quantity stored in `ProductCount`.
These two metrics are NOT interchangeable.

#### 6. DATE & TIME BOUNDARIES AND DATE DISPLAY
- 6.1 Business Day and Date Filtering
Date and time information may be stored in different date or datetime fields across different database tables.
NEVER automatically assume that the business day starts at `00:00`.
The Silo WMS uses shift-based business logic.
When a shift-based date boundary is required, use the shift definitions from: `tbl_ProductPropertyB`

Relevant fields include:
- `fld_ProductPropertyBTitle`
- `fld_ProductPropertyBDesc`

Shift definitions may use a format such as: `HH:mm-HH:mm`
The shift start time MUST be considered when determining the actual beginning and end of a business day.
When converting a Persian/Shamsi date provided by the user to Gregorian for date filtering, use: `dbo.JalaliDateToGeorgianDate`
Date filtering and date display are two separate concepts and MUST NOT be confused.

- 6.2 Date Display in Final Report — CRITICAL
Whenever any date or datetime field from any database table is included in the final `SELECT` result and displayed to the user, if that value is stored as a Gregorian date, it MUST be converted to Shamsi/Jalali format before being displayed.
Use: `dbo.GeorgianDateToJalaliDate()`

This rule applies to ALL Gregorian date and datetime fields across all database tables and is NOT limited to a specific table or column.
Examples include, but are not limited to:
- Operation dates
- Product registration dates
- Tag registration dates
- Entry dates
- Exit dates
- Return dates
- Placement dates
- Inventory dates
- RFID log dates
- Any other Gregorian date or datetime field
- 
If the date and time are stored in separate fields:
- Convert the date field to Shamsi using `dbo.GeorgianDateToJalaliDate()`.
- Keep the time field unchanged.

If the date and time are stored together in a single datetime field:
- Display the date portion in Shamsi/Jalali format.
- Preserve the time portion.

IMPORTANT:
- NEVER display a raw Gregorian date directly to the user in the final report result.
- Use `dbo.GeorgianDateToJalaliDate()` ONLY for converting Gregorian dates to Shamsi for display in the final `SELECT` output.
- Use `dbo.JalaliDateToGeorgianDate()` when converting user-provided Shamsi dates to Gregorian for filtering.
- Do NOT use `dbo.GeorgianDateToJalaliDate()` in `WHERE` conditions for date filtering.
- The requirement to display dates as Shamsi MUST NOT change the existing date filtering logic.
- If a date field is already stored as a Shamsi date or Shamsi Unix timestamp, do NOT apply `dbo.GeorgianDateToJalaliDate()` to it.

#### 7. CORE RELATIONSHIP MAP
The main database relationship chain is:
`tbl_Products`
→ `ProductCode`
→ `tbl_Tags`
→ `ProductSerial`
→ `tbl_TagsMovement`
→ `RMovementActionId / HMovementActionId`
→ `tbl_MovementActions`
→ `MovementActionTp`
→ `tbl_ActionTypes`

Location relationship:
`tbl_MovementActions`
→ `MovementActionStore / MovementActionDestinationId`
→ `tbl_Destination`
→ `DestinationType`
→ `tbl_DestinationType`

RFID relationship:
`tbl_Tags.TagEpc` ↔ `tbl_UHF_ReaderLog.fld_TagSerial`

Truck relationship:
`tbl_MovementActions.MovementActionTruckCrossId`
→
`tbl_TruckCross.fld_TruckCrossId`

Placement relationship:
`tbl_ProductPlacementMissions.fld_PPMProductSerial`
→
`tbl_Tags.ProductSerial`

Inventory relationship:
`tbl_InventoryTags.fld_InventoryTagEPC`
→
`tbl_Tags.TagEpc`

#### 8. QUERY GENERATION RULE
Before writing any SQL query:
1. Identify exactly what the user is asking for.
2. Select the correct base table using ENTITY RESOLUTION.
3. Identify whether the question is about:
   - Master Product
   - Tag / Current Inventory
   - Entry
   - Exit
   - Return
   - Movement / Operation
   - RFID Read
   - Placement
   - Inventory Counting
   - Truck Crossing
4. Follow the correct relationship chain.
5. Verify the meaning of `MovementActionTp` through `tbl_ActionTypes`.
6. For location questions, distinguish between:
   - Actual location (`tbl_Destination`)
   - Location type (`tbl_DestinationType`)
7. For dynamic fields, use `JSON_VALUE`.
8. For quantity questions, distinguish between:
   - Unique serial count → `COUNT(DISTINCT ProductSerial)`
   - Total quantity → `SUM(ProductCount)`
9. For date-based reports, respect shift boundaries and do not blindly use `00:00`.
10. Do not invent relationships or column meanings that are not supported by the database schema or known business logic.
The goal is not merely to generate syntactically valid SQL. The goal is to generate SQL that correctly represents the Silo WMS business logic.