# گزارش تردد Page Introduction

## purpose
The گزارش تردد page is designed to display and monitor all traffic records registered within the facility.
These records are retrieved directly from the Traffic **ثبت تردد** page and allow users to view detailed information about each vehicle’s entry and exit,
including driver information, vehicle details, and cargo information.

## Page Location
To access this page, navigate through the following menu path:
گزارش تردد → بخش حراست → منو

## Search Filters
Users can generate the desired report by completing one or more of the following fields:
- کد ملی
- نام راننده
- بازه زمانی
- علت مراجعه
- پلاک
- وضعیت:The وضعیت field is a dropdown list that includes the following options:
◦ پذیرش شده
◦ وارد شده
◦ خارج شده
◦ ابطال شده
- باربری
- نوع عملیات:The Operation Type field is a dropdown list that includes options such as:
◦ ارسال کالا
◦ جابجایی کالا بین انبار
◦ ورود کالاهای تعمیراتی
◦ تحویل کالا خدمات
◦ کالای برگشتی
- مقصد عملیات
- فرستنده / گیرنده
- عنوان کالا
After entering the required information, click the Search button to display the results in a table.


## Search Execution Rules
The گزارش تردد page does NOT use a dynamic filter builder.
There is:
- NO “Add Filter” button
- NO plus (+) icon for adding conditions
- NO advanced filtering panel
- NO report template saving
All search fields are fixed and always visible on the page.
To search:
1.Enter a value directly inside any of the available fields.
2.Click the Search button.
3.The system immediately displays matching records in the results table.
Filters are applied automatically based on the values entered.
If only one field (for example License Plate) is filled, the system returns all records matching that value.


## Results Table Structure
The results table consists of three main sections:
- لیست اطلاعات پذیرش
- لیست اطلاعات ورود
- لیست اطلاعات خروج
Each section displays all fields that were completed in the ثبت تردد page.


### اطلاعات پذیرش Section
This section includes all data recorded in the Acceptance tab of the ثبت تردد page, such as:
- کد
- تاریخ و ساعت پذیرش
- نام راننده
- کد ملی
- شماره تلفن
- وضعیت
- اسناد راننده
- پلاک
- نوع ماشین
- اسناد ماشین
- کاربر ابطال
- تاریخ ابطال
- علت مراجعه
- شرح مراجعه
- نوبت پذیرش
- نوع عملیات
- باربری
- مقصد عملیات
- فرستنده / گیرنده
All information entered during the acceptance stage is fully displayed in this section.


### اطلاعات ورود Section
This section includes all data recorded in the Entry tab of the ثبت تردد page, such as:
- تاریخ و ساعت ورود
- کاربر ثبت ورود
- وزن ورود
- اسناد ورود
- شخص مراجعه‌کننده
All entry-related fields are displayed here.


### اطلاعات خروج Section
This section includes all data recorded in the Exit tab of the ثبت تردد page, such as:
- تاریخ و ساعت خروج
- کاربر ثبت خروج
- وزن خروج
- سایر فیلدهای مربوط به خروج


## Viewing and Downloading Documents
In document-related fields (including Driver Documents, Vehicle Documents, Acceptance Documents, Entry Documents, and Exit Documents), uploaded image files can be viewed.
By clicking on a document field and selecting the appropriate option, users can download and view the file.

## Row-Level Actions
In the last column of each record in the گزارش تردد table,there are three action icons:
- Vehicle Icon
Clicking this icon opens a dialog displaying cargo details, including:
◦ شماره سند
◦ کد محصول
◦ نام محصول
◦ سریال
◦ مقدار
- Print Icon
Used to print only that specific record.
- Edit Icon
Clicking this icon redirects the user to ثبت تردد page. From there, the user can:
- Edit the information of the selected record by updating fields and then clicking Save.
- Delete the record by clicking the Delete button on ثبت تردد page.
- 
## Print Entire Report
A Print button is also available above the table, allowing users to print the complete list of displayed results.