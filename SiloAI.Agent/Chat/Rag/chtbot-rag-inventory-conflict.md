# Silo AI Knowledge(Rag) - Inventory Reconciliation Discrepancy Report

## System Navigation Path
گزارش مغایرت‌های انبارگردانی ➔ انبارگردانی ➔ عملیات‌های انبار ➔ منو

## Purpose
This report is designed to display and analyze inventory discrepancies based on:
- کد عملیات
- بازه زمانی
- انبار
The primary reference for all comparisons in this report is the RFID inventory recorded in the system.
All other data sources are compared against this baseline.

## Inventory Counting Methods and Discrepancy Detection
The system supports three methods for detecting discrepancies:

###  بارگذاری فایل اکسل حسابداری
In this method:
- The inventory اکسل file is extracted from the company’s accounting software.
- The file is uploaded through the upload option in the report page.
- The uploaded data is stored in the system and compared against the RFID inventory.
Important:
- When an اکسل file is uploaded for the first time, its data is saved in the system. For future report generations,
the system retrieves the stored data for comparison.
- If a discrepancy is detected between the RFID inventory and the accounting data,
the corresponding record will be highlighted in yellow/orange in the report table.


### انبارگردانی با استفاده از دستگاه هندهلد
In this method:
- At a specific date and time, the operator performs the stocktaking process using a handheld device.
- The handheld device scans the items available in the warehouse.
- The system records the scanned data as the موجودی انبارگردانی.
- This موجودی انبارگردانی is then compared with the RFID Inventory registered in the system for the same operation.
- Any discrepancies between the موجودی انبارگردانی (resulting from the handheld scan) and the RFID Inventory are calculated and displayed.
**Note**:In this process,the موجودی انبارگردانی is generated exclusively through the handheld device.
- انبارگردانی (انبارگردانی با استفاده از دستگاه هندهلد) is a separate and independent process from شمارش فیزیکی using other Reader devices.
These two methods must never be described interchangeably


### شمارش فیزیکی Using Other Reader Devices
In this method:
- Other Reader devices (not handheld) perform item counting.
- The counting results are exported as an Excel file.
- The Excel file is uploaded using the “بارگذاری اکسل شمارش فیزیکی” option.
The system compares the uploaded شمارش فیزیکی from these devices with the RFID inventory registered in the system for that operation.

## Report Page Structure
At the top of the page, there are three main filter sections:
- فیلترهای اطلاعات عملیات
- فیلترهای اطلاعات کالا
- فیلتر کالاهای وارد و خارج شده

### فیلترهای اطلاعات عملیات
This section includes the following fields: 
- کد عملیات
- کاربر
- بازه زمانی برای تاریخ
- جانمایی
- توضیحات
- انبار
⚠️ کد عملیات, Date Range, and انبار are mandatory fields.
If any of these are not provided, the system will display an error.

### فیلترهای اطلاعات کالا
If within a specific operation you only need to view discrepancies for particular products, this section can be used.
Available filter fields:
◦ کد کالا
◦ کد فنی
◦ سایز کالا
◦ نوع کالا
◦ درجه کیفیت
- Product Code Search
There are two ways to enter a Product Code:
Manual entry
Using the search button next to the field
By clicking the search button, a Product Search form opens.
You can search by entering at least one of the following:
- نام محصول
- کد محصول
- کد فنی
- سایز کالا
- درجه کیفیت
A list of products will be displayed, and the desired product can be selected.
- Show Only Discrepancies Option
This section includes an option called “فقط نمایش مغایرت‌ها”.
When enabled, shows only products with discrepancies in the results table. Transactions are not affected.

### فیلتر کالاهای وارد و خارج شده
This section includes an option to consider inbound and outbound transactions.
When this option is enabled:
- All system-registered inbound and outbound transactions
- Within the inventory counting period (e.g., one week)
- For the selected warehouse
are included in the calculations.
The final discrepancy results will reflect these movements.


## Results Table
After entering the required information and clicking “Search,” the results table is displayed.
The table includes the following columns:
- کد کالا
- نام محصول
- کد فنی
- لوکیشن‌ها (Product Location)
- موجودی RFID (Quantity based on registered RFID tags)
- موجودی RFID برحسب متراژ (Based on Length/Meterage)
- موجودی حسابداری (From uploaded فایل اکسل حسابداری)
- شمارش فیزیکی (From بارگذاری اکسل شمارش فیزیکی)
- تعداد شناسایی‌شده (Correctly identified by handheld device and matching system data)
- مقدار شناسایی‌شده
- تعداد مغایرت RFID (Difference between RFID and handheld data)
- مغایرت حسابداری (Difference between RFID and accounting file)
- مغایرت شمارش فیزیکی (Difference between RFID and شمارش فیزیکی file)
- مغایرت کسری
- مغایرت اضافی

### مغایرت کسری
Definition:
Items that, according to RFID inventory, should exist in the warehouse but were not identified during handheld inventory counting.
By clicking the shortage discrepancy number:
- A new دیالوگ opens.
- A list of affected serial numbers is displayed.
- Records are highlighted in red.
Serial Details Table Fields:
◦ سریال
◦ کد کالا
◦ عنوان کالا
◦ کد فنی
◦ مقدار
◦ سند تولید
◦ تاریخ تولید
◦ انبار
◦ لوکیشن
Available Actions in Shortage دیالوگ:
- Filter serials by date range or specific serial number
- At the bottom of the shortage discrepancy دیالوگ, there is a **انبار مقصد** section.
If we know that a specific serial is located in another warehouse, we can select the checkbox next to that serial,
choose the new destination warehouse, and register it to resolve the discrepancy.
For example, if a serial is recorded in the product warehouse and shows a discrepancy,
but we know it is in the Sold Warehouse, changing the انبار مقصد to the Sold Warehouse will resolve the discrepancy.
- “سوابق” option for each serial, which navigates to the Shipment History Report page to view the complete serial history

### مغایرت اضافی
Definition:
Items that, according to RFID inventory, should not exist in the selected warehouse but are physically present.
The process is identical to Shortage Discrepancy:
- Clicking the surplus number opens the serial discrepancy دیالوگ.
- Warehouse correction can be registered.
- The discrepancy can be resolved.

## Row Color Indicators
- Red: Has مغایرت کسری  or مغایرت اضافی between RFID and انبارگردانی
- Orange,Yellow: Has مغایرت حسابداری
- White: No discrepancies

## Summary Section (Bottom of Page)
At the bottom of the report, a summary is displayed in both table and chart formats, including:
- تعداد کل کالاهای یک عملیات انبارگردانی
- تعداد کل موجودی های RFID
- کل موجودی های حسابداری
- کل موجودی های شمارش فیزیکی
- کل مغایرت ها  مربوط به شناسایی توسط هندهلد 
- کل مغایرت های حسابداری
- کل مغایرت های شمارش فیزیکی
This section provides a managerial overview of the overall inventory counting status.