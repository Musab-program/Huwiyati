# توثيق المراحل المكتملة (Completed Phases Documentation)

يوثق هذا المستند كافة المراحل التقنية والمعمارية التي تم تنفيذها بنجاح في مشروع "هويتي" (`Huwiyati Backend`) من المرحلة الأولى حتى المرحلة السابعة.

---

## 🏗️ المرحلة الأولى: إعداد البنية التحتية والمشروع (Clean Architecture)

- **الهدف**: بناء الهيكل التنظيمي للمشروع بناءً على نمط الفصل النظيف `Clean Architecture`.
- **المشاريع المُنشأة**:
  1. `Huwiyati.Domain`: يضم الكيانات (`Entities`)، الـ `Enums`، والـ `Constants` الأساسية، وبدون أي اعتمادية على مكتبات خارجية.
  2. `Huwiyati.Application`: يضم منطق العمل (`CQRS` / `Use Cases`) والـ `DTOs` والـ `Validators` والـ `Interfaces`.
  3. `Huwiyati.Infrastructure`: يضم الاتصال بقاعدة البيانات (`EF Core` و `DbContext`) وتطبيقات الخدمات الخارجية و `ASP.NET Core Identity`.
  4. `Huwiyati.API`: طبقة العرض والـ `Controllers` وإعدادات `Swagger` والـ `Middleware`.

---

## 🆔 المرحلة الثانية: نمذجة السجل المدني والمستخدمين (Civil Registry & Identity Modeling)

- **الهدف**: ربط المستخدمين بالسجل المدني الحقيقي لتفادي الحسابات الوهمية.
- **الكيانات والروابط**:
  - `Person`: الكيان الخاص بالسجل المدني (الاسم الرباعي، الرقم الوطني، تاريخ الميلاد، مكان الميلاد، الجنس، الحالة الاجتماعية، المحافظة، المديرية).
  - `ApplicationUser`: الكيان الخاص بنظام الهوية و `Identity` وله علاقة 1-إلى-1 أو 1-إلى-كثير مع `Person` عبر `PersonId`.

---

## 📝 المرحلة الثالثة: تسجيل حساب المواطن (Citizen Registration)

- **الهدف**: تمكين المواطن من إنشاء حساب مرتبط برقمه الوطني المسجل بالسجل المدني.
- **المنطق التقني**:
  - `RegisterUserCommand`: يستقبل الرقم الوطني، البريد الإلكتروني، رقم الهاتف، وكلمة المرور.
  - `RegisterHandler`: يفحص وجود المواطن في السجل المدني `Persons` ويتأكد من عدم وجود حساب سابق برقم الهوية أو البريد.
  - إسناد دور `Citizen` كدور افتراضي تلقائياً للحساب الجديد عبر `AddToRoleAsync(user, AppRoles.Citizen)`.

---

## 🔑 المرحلة الرابعة: التفعيل عبر OTP وتسجيل الدخول وتوليد JWT

- **الهدف**: حماية الحسابات وإصدار التوكن المشفر للمستخدمين المفعّلين.
- **المكونات**:
  - `VerificationCode`: جدول تخزين رموز التحقق المؤقتة بحالة استخدام `IsUsed` وزمن انتهاء `ExpirationTime`.
  - `VerifyOTPCommand`: التحقق من كود الـ OTP وتنشيط حالة الحساب إلى `Active`.
  - `TokenService`: خدمة توليد `JWT Token` تحتوي على `UserId`, `NationalNumber`, `FullName`, `AccountStatus`, و `Role Claims`.

---

## 🔄 المرحلة الخامسة: استعادة كلمة المرور (Forgot Password Flow)

- **الهدف**: تمكين المستخدمين من استرجاع كلمة المرور عبر الـ OTP في حال نسيانها.
- **الدورة المكتملة**:
  1. `ForgotPasswordCommand`: يستقبل الرقم الوطني وينشئ كود OTP لاستعادة الحساب.
  2. `VerifyResetCommand`: يتأكد من صحة كود الاستعادة دون استهلاكه نهائياً.
  3. `ResetPasswordCommand`: يغير كلمة المرور بعد التحقق النهائي من الكود.

---

## 📱 المرحلة السادسة: إدارة الأجهزة الموثوقة (Trusted / Untrusted Devices)

- **الهدف**: منع الدخول غير المصرح به من أجهزة جديدة دون توثيق OTP.
- **الهيكلية والمنطق**:
  - كيان `UserDevice`: يضم `DeviceId`, `UserId`, `DeviceIdentifier`, `DeviceName`, `OperatingSystem`, `IsTrusted`, `LastLogin`, و `CreatedAt`.
  - فحص الجهاز الإجباري في `LoginHandler`:
    - **إذا كان الجهاز موثوقاً (`IsTrusted == true`)**: دخول مباشر وإصدار `JWT Token`.
    - **إذا كان الجهاز جديداً أو غير موثوق**: تسجيل الجهاز كـ `IsTrusted = false` وإنشاء رمز OTP وإرجاع `RequiresDeviceVerification = true`.
  - معالجة التوثيق `VerifyDeviceHandler`: فحص رمز الـ OTP، تحويل الجهاز إلى `IsTrusted = true` وإصدار الـ `JWT Token`.

---

## 🛡️ المرحلة السابعة: الصلاحيات والأدوار (Authorization & Roles)

- **الهدف**: حماية الـ Endpoints وتقسيم مستخدمي النظام حسب الأدوار المعرفّة.
- **الأدوار المعتمدة (`AppRoles`)**:
  - `Citizen`: المواطن العادي.
  - `Employee`: الموظف الحكومي/موظف الخدمة.
  - `Admin`: المدير.
  - `SuperAdmin`: المدير العام للنظام.
- **التهيئة والتهجير**:
  - `DbInitializer.SeedAsync`: زرع الأدوار الأربعة تلقائياً وإسناد دور `Citizen` لأي حساب قديم لا يمتلك أدواراً.
  - ضبط `RoleClaimType = ClaimTypes.Role` في `Program.cs`.
  - حماية التوابع بالوسم `[Authorize(Roles = "...")]`.
