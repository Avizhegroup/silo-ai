# Exit Goods Operations Report Builder – Page Overview

## Access path
**گزارشات انبار menu → گزارش ساز عملیات های خروج کالا**
This page is designed to create custom reports for exit goods operations.
It allows users to define filters and columns, generate reports, and save report formats for later use.

## Page Structure
This page consists of three main sections:
- Filters
- Columns
- Search & Report Format Management

## Filters Section
In this section, you define which records will be included in the report.
How filters work:
- Select a filter type
- Choose one or more values from the list
- Click the ➕ (Add) button to apply the filter
- After adding a filter, you can proceed to the next one
**A filter is not applied until the ➕ button is clicked.**

### Available filtersYou can filter exit goods operations by:
- نوع عملیات 
- انبار
- تاریخ
- سایز کالا
- کد کالا
- کد فنی
- کد عملیات
- کد عملیات گیت
- ایستگاه شناسایی
- برند کالا
- گروه کالا
- نوع کالا
- کد سند
- نوع ماشین
- پلاک ماشین
- خط تولید
- کاربر ثبت خروج

### Example
In the انبار filter, you may see options such as:
- انبار فروش‌رفته
- انبار بارانداز
- انبار محصول
- انبار قرنطینه تولید
You can select one or more warehouses as needed.
In the Operation Type filter, you can choose operations such as:
- ارسال کالا از تولید به انبار محصول
- برگشت کالا از فروش
- برگشت کالا به انبار محصول
- ارسال کالا برای بارگیری
-انبار فروش‌رفته
and similar operations.

### Managing filters
To remove a filter:
- Right-click on the applied filter
- Select Remove

### Important note
✅ Selecting at least one operation type
✅ Selecting at least one data column
is mandatory to run the report.

## Columns Section
This section defines what information will be displayed in the report output.
**Column types**
**Data Columns**
These show raw, detailed information for each record, such as:
- کد سند عملیات
- کد عملیات
- تاریخ و ساعت عملیات شمسی و میلادی
- ایستگاه شناسایی
- سریال کالا
- کد کالا، کد فنی، عنوان کالا
- کد و عنوان درجه کیفیت 
- نوع کالا، سایز، برند، گروه، زیرگروه، طبقه
- سالن
- کاربر رجیستر
- تاریخ و ساعت تولید
- کاربر ثبت خروج
**محاسباتی Columns**
Used for calculations and aggregations:
- بیشترین مقدار
- کمترین مقدار
- مقدار میانگین
- تعداد
- جمع مقدار
- جمع مقدار واحد دوم
- درصد
- تعداد عملیات
**Pivot Columns**
Pivot columns split the data into separate columns based on a selected attribute (such as برند کالا or گروه کالا).
Each value of the selected Pivot attribute is displayed as its own column,
allowing users to view and compare data across different categories within a single table, without requiring the selection of a calculated column.
- سایز
- برند
- گروه کالا
- نوع کالا
- درجه کیفیت
- زیرگروه کالا
- طبقه کالا
- سالن
- شیفت
**Data Mining Elements**

## Search & Report Format Management
Running the report
After selecting the required filters and columns:
- Click the Search button
- The report results will be displayed
- Note:
This report does not support direct printing.
To print the report, please use the دریافت اکسل option.

### Saving a report format
- Next to the Search button, there is an option to Save Report Format
- Assign a عنوان to the report format
- Click Save to store it
- The saved format will appear in the formats list

### Managing saved report formats
For each saved format, three options are available:
1.View – Run and display the saved report
2.Delete – Remove the report format
3.Add to Menu – Add the report to the application menu

### Adding a report to the menu
After selecting Add to Menu:
- Define the report address and display title
- In the Users section, grant access to specific users
- In the Menu section, choose where the report should appear in the application menu
- Click Save to finalize
- The report will then be visible in the menu for authorized users