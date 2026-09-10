# توثيق اكتمال المرحلة الأولى وتأسيس قاعدة البيانات (Phase 1 Completion Log)

تغطي هذه الوثيقة التقرير التنفيذي الكامل لجميع الخطوات التي تم تنفيذهما وإكمالها بنجاح من **الخطوة 9 إلى الخطوة 17** لتأسيس قاعدة البيانات والـ `Identity` في مشروع **هويتي** (Backend)، متبوعةً بالقرارات الفنية والإجابات الشاملة.

---

## 📍 أولاً: الخطة والمهام المنجزة تنفيذاً وتطبيقاً (Steps 9 to 17 Executed)

تم إكمال البنية التحتية والربط الفعلي مع قاعدة البيانات `SQL Server` وتجربتها بنجاح عبر الخطوات التالية:

### 1. إعداد نص الاتصال بقاعدة البيانات (`Step 9: Connection String`)
- تم إعداد نص الاتصال المباشر بقاعدة البيانات في ملف `appsettings.json` في مشروع `Huwiyati.API`:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=HuwiyatiDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
  ```

### 2. تسجيل الخدمات في حاقن التبعيات (`Step 10: Dependency Injection`)
- تم ربط `ApplicationDbContext` بمزود `SQL Server` ومحرّك حماية البيانات `AddDataProtection()` داخل دالة `AddInfrastructureServices` في مشروع `Huwiyati.Infrastructure\DependencyInjection.cs`.
- تم استدعاء `builder.Services.AddInfrastructureServices(builder.Configuration)` في `Huwiyati.API\Program.cs`.

### 3. تسجيل وإعداد خدمات الهوية (`Steps 11 & 12: ASP.NET Core Identity`)
- تم تسجيل `AddIdentityCore<ApplicationUser>` وتحديد خيارات الأمان الأساسية:
  - **كلمة المرور:** الحد الأدنى 8 خانات، تشترط أرقام وحروف كبيرة وصغيرة (`RequiredLength = 8`, `RequireDigit`, `RequireUppercase`, `RequireLowercase`).
  - **المستخدم:** بريد إلكتروني فريد لكل مواطن (`RequireUniqueEmail = true`).
  - **الحماية والقفل:** قفل الحساب 15 دقيقة بعد 5 محاولات فاشلة متتالية (`DefaultLockoutTimeSpan = 15m`, `MaxFailedAccessAttempts = 5`).
  - **الأدوار والتوكنز:** ربط الأدوار عبر `.AddRoles<IdentityRole<Guid>>()` وتفعيل المزودات المدمجة عبر `.AddDefaultTokenProviders()`.

### 4. إنشاء الترحيل الأول (`Step 13: Initial Migration`)
- تم توليد ملفات الترحيل الأول بنجاح داخل المجلد المخصص `Huwiyati.Infrastructure\Persistence\Migrations` عبر الأمر:
  ```powershell
  Add-Migration InitialCreate -Project Huwiyati.Infrastructure -StartupProject Huwiyati.API -OutputDir Persistence/Migrations
  ```

### 5. إنشاء وبناء قاعدة البيانات الفعلية (`Step 14: Database Creation`)
- تم تطبيق الترحيلات وتأثيث قاعدة البيانات `HuwiyatiDb` في `SQL Server` بنجاح عبر الأمر:
  ```powershell
  Update-Database -Project Huwiyati.Infrastructure -StartupProject Huwiyati.API
  ```

### 6. فحص الجداول والعلاقات والـ `Indexes` (`Steps 15 & 16: Verification & SSMS Inspection`)
- تم فحص قاعدة البيانات في `SSMS` والتحقق من صحة الجداول:
  - جدول **`Persons`**: كل الحقول القياسية `NOT NULL` بينما `PhotoUrl` اختياري (`NULL`). وتأكيد وجود الفهرس الفريد **`IX_Persons_NationalNumber`**.
  - جدول **`ApplicationUsers`**: تم تخصيص اسم الجدول صراحةً، وتأكيد وجود المفتاح الأجنبي **`FK_ApplicationUsers_Persons_PersonId`** والفهارس الفريدة.

### 7. اختبار الاتصال الفعلي من الـ `API` لقاعدة البيانات (`Step 17: Database Health Test`)
- تم إنشاء `TestController.cs` في `Huwiyati.API\Controllers\TestController.cs` يحتوي على نقطة نهاية `GET /api/test/db-check`.
- تم اختبار الاتصال والاستعلام الفعلي عبر `_context.Database.CanConnectAsync()` وإرجاع استجابة `JSON` بنجاح بنسبة 100%.

---

## ❓ ثانياً: الأسئلة والإجابات والقرارات الفنية (Q&A & Architectural Rationale)

### س1: هل السماح بـ `NULL` لـ `PasswordHash` و `PhoneNumber` في جدول `ApplicationUsers` هو المعيار العالمي الممتاز (`Best Practice`)؟
- **الإجابة:** **نعم، 100% هذا هو المعيار المعتمد والمطبق رسمياً من قِبل فريق مهندسي مايكروسوفت في `ASP.NET Core Identity`.**
- **السبب الفني:** نظام `Identity` مصمم عالمياً ليدعم طرق مصادقة متعددة (مثل الدخول عبر `Google OAuth`, `Microsoft Entra`, أو `OTP` الهاتف)؛ وفي هذه الحالات لا يوجد كلمة مرور فتكون `PasswordHash = NULL`.
- **التكفيل التطبيقي:** التطبيق يمنع إدخال حسابات فارغة على مستوى الـ `API` عبر خيارات `AddIdentityCore` والـ `Validation` قبل أن يصل الطلب لقاعدة البيانات.

---

### س2: ما هو سيناريو الربط بين `AddDefaultTokenProviders` و `AddDataProtection`؟
- **السيناريو الفني:**
  1. عند استعادة كلمة المرور، يستدعي النظام `DataProtectorTokenProvider` لتوليد توكن تشفيري موثوق.
  2. فئة التوكن لا تقوم بالتشفير بنفسها، بل تطلب محرك التشفير المركزي `IDataProtectionProvider`.
  3. تسجيل `services.AddDataProtection();` يزود حاقن التبعيات بمحرك التشفير المطلوب لمنع خطأ `Unable to resolve IDataProtectionProvider`.

---

### س3: ما هو دور السطر `<FrameworkReference Include="Microsoft.AspNetCore.App" />` في `Infrastructure.csproj`؟
- **الإجابة:** مشروع `Infrastructure` يظل 100% مكتبة فئات (`Class Library` من نوع `Microsoft.NET.Sdk`).
- إضافة `FrameworkReference` تمنح مكتبة الفئات إمكانية استخدام امتدادات `ASP.NET Core Web Identity` المتقدمة دون تحويل المشروع إلى تطبيق ويب ودون إضافة ملفات `Controllers` غريبة إليه، وهي الممارسة الموصى بها في المعمارية النظيفة.

---

### س4: كيف نحدد المجلد المخصص للـ `Migrations` بدقة؟
- **في CLI:** باستخدام خيار `--output-dir Persistence/Migrations`.
- **في PMC (Visual Studio):** باستخدام خيار `-OutputDir Persistence/Migrations`.
- يضمن هذا وضع الترحيلات مباشرة في `Huwiyati.Infrastructure\Persistence\Migrations`.

---

### س5: كيف نتحقق من الـ `Indexes` والعلاقات في `SSMS`؟
- في `SSMS` تحت `HuwiyatiDb -> Tables -> dbo.Persons -> Indexes` نجد `IX_Persons_NationalNumber`.
- وتحت `dbo.ApplicationUsers -> Keys` نجد المفتاح الأجنبي `FK_ApplicationUsers_Persons_PersonId`.
