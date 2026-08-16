# SiloAI.UI

پنل مدیریت Blazor Server برای سرویس‌های هوش مصنوعی سیلو.

## قابلیت‌ها

- ورود مدیر با JWT
- مدیریت مشتریان
- مدیریت کلیدهای API و اتصال آن‌ها به مشتری
- نمایش داشبورد مدیریتی به زبان فارسی و راست‌به‌چپ
- کنترل اعتبار مشتری برای APIهای چت و OCR
- ثبت تاریخچه مکالمات AI در پایگاه داده

## ساختار راهکار

- `SiloAI.Domains`: موجودیت‌ها، `AiApiContext`، فکتوری Design-Time و migration اولیه
- `SiloAI.Api`: API مدیریتی و endpointهای سرویس AI
- `SiloAI.UI`: پنل مدیریتی Blazor Server

## مدل داده

### Customers

جدول `tbl_AiCustomers` شامل:

- `fld_Id`
- `fld_Name`
- `fld_RemainingCredit`
- `fld_CreatedAt`

### API Keys

جدول `tbl_AiApiKeys` اکنون علاوه بر مشخصات کلید، دارای `fld_CustomerId` نیز هست.

### Conversations

جدول `tbl_AiConversations` برای ذخیره‌ی پیام کاربر، پاسخ بات، کلید دستور، مصرف اعتبار، شناسه محلی گفتگو، مشتری و زمان ایجاد استفاده می‌شود.

## مهاجرت پایگاه داده

در `SiloAI.Api` به جای `EnsureCreated()` از `Database.Migrate()` استفاده می‌شود؛ بنابراین migration اولیه‌ی پروژه‌ی `SiloAI.Domains` در شروع برنامه اعمال خواهد شد.

## مسیرهای مهم API

### مدیریت مشتریان

- `GET /admin/customers`
- `GET /admin/customers/{id}`
- `POST /admin/customers`
- `PUT /admin/customers/{id}`
- `DELETE /admin/customers/{id}`
- `GET /admin/customers/{id}/api-keys`

### مدیریت کلیدهای API

- `GET /admin/api-keys`
- `POST /admin/api-keys`
- `DELETE /admin/api-keys/{id}`

### سرویس‌های AI

- `POST /api/ai/chat/new-session`
- `POST /api/ai/chat/send`
- `POST /api/ai/agent/ocr`

در صورت اتصال API Key به مشتری، شناسه‌ی مشتری به claim با نام `CustomerId` اضافه می‌شود و قبل از پاسخ‌گویی، اعتبار مشتری بررسی می‌شود.

## رابط کاربری

- صفحه خانه
- داشبورد
- صفحه مشتریان با امکان ویرایش، حذف و مدیریت کلیدهای API هر مشتری
- صفحه کلیدهای API
- صفحه ورود فارسی

## اجرا

```bash
cd SiloAI.Api
 dotnet run
```

```bash
cd SiloAI.UI
 dotnet run
```

## حساب پیش‌فرض مدیریت

- Username: `admin`
- Password: `Admin@123`
