# Production Report Builder – Page Overview

## Purpose
The Production Report Builder page is designed to help users create flexible and reusable production reports without needing technical knowledge.
Using this page, users can filter production data, choose the required columns, analyze results in tables and charts, and save report formats for repeated use.

## Access Path
**تولید menu → گزارش ساز تولید**

## Page Structure
This page consists of three main sections:
- Filters
- Columns
- Search & Report Format Management

## Step 1: Filters Section
In this section, you define which production records will be included in the report.

### How filters work
- Select a filter type
- Choose values from the list or manually enter a value (depending on the filter)
- Click the ➕ (Add) button to apply the filter
- After adding a filter, you can move on to the next one
- ⚠️ A filter is not applied until the ➕ button is clicked.

### Available filters
You can filter production data by:
- کد کالا
- درجه کیفیت
- کاربر
- شیفت
- سایز کالا
- کد فنی
- سریال
- تاریخ
- برند کالا
- گروه کالا
- خط تولید
- دستگاه رجیستر
- وضعیت بازرسی
- انبار
- زیرگروه کالا

### Filter input types
- For some filters (such as Product Code or Quality Grade), a selectable list of values is displayed.
- For others (such as شماره سریال or کد فنی), values must be entered manually.

### Managing filters
- To remove a filter, right-click on the applied filter and select Remove.

**Important note**
✅ Selecting at least one Data Column is mandatory to run the report.

## Step 2: Columns Section
This section defines what information will be displayed in the report results.
You can select different types of columns depending on the level of detail or aggregation you need.
**Column types**
**Data Columns**
These show raw, detailed information for each record, such as:
- کد عنوان
- عنوان سالن
- تاریخ و ساعت عملیات شمسی و میلادی
- کد شیفت
- عنوان شیفت
- کد کالا، عنوان کالا
- کد و عنوان درجه کیفیت 
- کد و عنوان برند کالا
- کد سالن 
- دستگاه رجیستر
- کد و عنوان گروه کالا
- کد و عنوان نوع کالا
- سریال
- کد فنی
- کد و عنوان سایز
- شماره سند
- وضعیت بازرسی
- کاربر ثبت
- تاریخ شمسی و میلادی
- کد و عنوان انبار
- کد و عنوان زیر گروه کالا
**محاسباتی Columns**
Used for calculations and aggregations:
- بیشترین مقدار
- کمترین مقدار
- مقدار میانگین
- تعداد
- جمع مقدار
- جمع مقدار واحد دوم
- درصد
- اولین تاریخ
- دومین تاریخ
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

## Step 3: Search & Report Format Management
Running the report
After selecting the desired filters and columns:
- Click the Search button
- The report results will be displayed in a table below

### Saving a report format
- Next to the Search button, there is an option to Save Report Format
- Enter a title for the report format
- Click Save
- The saved format will appear in the formats list
- **Note**:The report table does not have a print option. 
You can export the data to Excel. Printing is available only for charts.

### Managing saved report formats
For each saved report format, three options are available:
1.✅(View) – Load and run the saved report
2.Delete – Remove the saved format
3.Add to Menu – Add the report as a menu item in the application

### Adding a report to the menu
After selecting Add to Menu:
- Define the report address and display title
- In the Users section, grant access to specific users
- In the Menu section, choose where the report should appear in the application menu
- Click Save
Once saved, the report will appear in the menu for authorized users, and there will be no need to reselect filters and columns each time.

## Step 4:Charts and Visualization
After running the report and viewing the table results:
- A button is available to display Bar Charts and Pie Charts
- To enable charts, selecting at least one Calculated Column is mandatory
Example
If you select:
- A Data Column such as Product Title
- A محاسباتی Column such as Count
The chart will display how many items exist for each product title, with values shown directly on the chart.
These charts can also be printed if needed.

## Summary
- The Production Report Builder allows users to:
- Filter production data flexibly
- Display detailed or summarized information
- Save and reuse report formats
- Add reports directly to the application menu
- Analyze data visually using charts
- This ensures faster reporting, consistency, and ease of use for daily production analysis.