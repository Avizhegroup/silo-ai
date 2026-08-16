# Introduction to کنترل و خروج کالا از درب حراست and ثبت جابجایی مستقیم کالا Pages

## purpose
The system contains two operational pages:
- کنترل و خروج کالا از درب حراست
- ثبت جابجایی مستقیم کالا
The purpose of both pages is to record the movement of identified serials from the source warehouse to the destination warehouse.

## Top Form Information
At the top of both forms, the following information is automatically recorded by the system and cannot be edited:
- کد عملیات
- تاریخ
- ساعت
- کاربر ثبت ‌کننده
The user must specify:
- انبار مبدأ
- انبار مقصد
- لوکیشن جایگذاری

### Location Selection
To select a location:
1.Click the button next to the location field.
2.Open the location search form.
In this form:
The warehouse is automatically set based on the selected destination warehouse.
Occupancy percentage can be selected:
- Less than 20%
- 20–40%
- 40–60%
- 60–80%
- More than 80%
Searching by location name is also possible.
After clicking Search, a list of locations is displayed, including:
- کد لوکیشن
- نام لوکیشن
- ظرفیت اشغال‌شده
- ظرفیت آزاد
- انبار مربوطه
Then, one of the displayed locations must be selected.

## Serial Selection

### کنترل و خروج کالا از درب حراست Page
Serials are identified via an RFID handheld device.
Steps:
1.Click the blue icon
2.Select the detection station
**Optionally, enter the operation code for more precise search**
The station list displays:
- کد
- تعداد
- تاریخ و ساعت
- وضعیت
If the station status is ثبت شده, no additional movement registration is required. Otherwise, the list of identified serials is displayed.

### ثبت جابجایی مستقیم کالا Page
Serials can be selected in two ways:
Method 1: Manual Serial Entry
- Enter the serial
- Click the + icon
Method 2: Advanced Product Search
- Use the button next to + to open the Product Search form.
Search can be performed by:
- بازه سریال
- کد کالا
- کد فنی
- نوع کالا
- خط تولید
- شیفت
- تاریخ
- گروه
- سایز
- درجه کیفیت
- وضعیت فریز
- برند
The کدفنی field includes a “همانند” option:
- When enabled, entering part of the technical code shows serials with similar codes.
- Otherwise, the full technical code must be entered.
After clicking Search, a list of serials along with all related information is displayed.

## Tables After Serial Selection
After selecting serials (via handheld or manually), two tables are displayed:

### سریال‌های انتخاب‌ شده Table
Displays the following information:
- سریال
- کد کالا
- نام کالا
- کد فنی
- مقدار
- وضعیت
Serial status can be:
- Valid → Available for the operation
- Invalid → Does not meet required conditions
- فریز → Freeze in the system
- مردود بازرسی → Failed quality control
- عدم موجودی → Serial not available in the selected source warehouse
Example: Source warehouse is انبار قرنطینه, but the serial is registered in انبار محصول.
- رجیستر نشده → Serial not registered in the system
**If there is an error:**
- The row is displayed in red
- The left-side warning icon turns red
- Serials can be removed using the delete icon

### تجمعی بر کد کالا Table
- Selected serials are aggregated by product code
- The total quantity and number of serials for each product code are displayed

## سند Section
At the bottom of the page, the Document section is available.
By entering the document number and clicking Search, the following information is displayed:
- کد محصول
- نام محصول
- جمع مقدار
- مقدار قابل استفاده
- وضعیت کالا

## Document vs. تجمعی Table Comparison
The system automatically compares document data with the aggregated table:
- If a product exists in the document but not in the aggregated table:
“مغایرت عدم شناسایی کالا” message is displayed in yellow
- If a product exists in both tables:
If the quantities match → normal status (no warning)
If the quantities differ → “مقدار مغایرت” message with the difference, displayed in yellow
- If a product exists in the aggregated table but not in the document:
“مغایرت در کد کالا” message is displayed in yellow

## Notes and Dynamic Fields
In the document section:
- A Notes box is available for additional comments
- Dynamic fields are shown based on the operation type and انبار مبدا و مقصد 
- Fields are automatically populated from document information
To add new fields:
- Go to تعریف فیلدهای اطلاعاتی form
- Select type اطلاعات داینامیک هدر سند
Example: If the source warehouse is Product Warehouse and the destination is Sold Warehouse, fields such as the following can be registered:
- کد حواله
- نام راننده
- نوع ماشین
- مقصد ارسال
- کد مجوز فروش

## Final Conditions for Movement Registration
Movement registration can only proceed if:
- The operation has not been previously registered
- Source and destination warehouses are selected
- At least one valid serial is selected
- No serial has an error
- If mandatory, document number is entered
- If mandatory, truck crossing information is entered
- No discrepancies exist between serials and document
- All required dynamic fields are completed
- After resolving all errors, registration can be completed.

## Print
Clicking Print allows the form to be printed.

