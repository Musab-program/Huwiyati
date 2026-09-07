# الخطة النهائية لتنفيذ Backend مشروع هويتي (Huwiyati)

هذا المستند يمثل خطة التنفيذ المعتمدة لـ Backend مشروع **هويتي** بناءً على ملف السيناريوهات وملف تصميم قاعدة البيانات.

---

## 🎯 المبادئ والقرارات الأساسية للخطة

- **`Authentication / Authorization`:** نؤجلها إلى وقتها المناسب عند الوصول لمرحلة الأمن (`Security`).
- **`ApplicationDbContext`:** ننشئه من البداية كـ `IdentityDbContext<ApplicationUser>`.
- **التطوير التدريجي (`Feature by Feature`):** بناء كل ميزة بالكامل قبل الانتقال للميزة التالية.
- **التغطية الفعلية للسيناريو:** أي شيء موجود في السيناريو موثق ومحدد أين سيُغطى.
- **تجنب التعقيد الزائد (`No Overengineering`):** عدم إضافة أي مكون غير موجود بالسيناريو أو قاعدة البيانات لمجرد أن المعمارية تسع ذلك.

---

## 🚀 مراحل تنفيذ المشروع

### المرحلة 0 — إنشاء المشروع
نبدأ من الصفر بالهيكلية التالية:

```text
Huwiyati/
│
├── Huwiyati.sln
│
├── Huwiyati.Domain
├── Huwiyati.Application
├── Huwiyati.Infrastructure
└── Huwiyati.API
```

**الخطوات:**
1. إنشاء الحل `Huwiyati.sln`.
2. إنشاء المشاريع الأربعة.
3. إضافة مراجع المشاريع (`Project References`).
4. التأكد من اتجاه الاعتمادية:

```text
Domain
   ↑
Application
   ↑
Infrastructure
   ↑
API
```

---

### المرحلة 1 — تأسيس `Domain`
تأسيس المكونات المشتركة لطبقة المجال:

```text
Huwiyati.Domain
│
├── Common
│   ├── BaseEntity.cs
│   └── AuditableEntity.cs
│
├── Entities
├── Enums
├── Exceptions
└── ValueObjects
```

*ملاحظة:* لن نضع الكيانات الـ 28 دفعة واحدة، بل يتم إنشاء الكيانات تدريجياً حسب كل `Feature`.

---

### المرحلة 2 — تأسيس قاعدة البيانات (`Database Foundation`)

إنشاء الأساس لـ `Infrastructure`:

```text
Huwiyati.Infrastructure
│
├── Persistence
│   ├── ApplicationDbContext.cs
│   ├── Configurations
│   └── Migrations
│
├── Identity
│   └── ApplicationUser.cs
│
└── DependencyInjection.cs
```

وربط `ApplicationDbContext` ليرث من `IdentityDbContext<ApplicationUser>` مباشرة، دون الدخول في تفاصيل `Login`, `JWT`, `Authorization`, `Roles`, `Policies` في هذه المرحلة.

---

### المرحلة 3 — أول `Feature`: إنشاء الحساب وتفعيله (`Must Have`)

تغطية متطلبات السيناريو:
1. إدخال الرقم الوطني.
2. تاريخ الميلاد.
3. الهاتف.
4. البريد الإلكتروني.
5. التحقق من بيانات الشخص.
6. `OTP`.
7. إنشاء كلمة المرور.
8. إنشاء الحساب بحالة `PendingActivation`.
9. مراجعة الموظف.
10. تفعيل الحساب.

**الكيانات المرتبطة:**
`Person`, `ApplicationUser`, `VerificationCode`, `Employee`, `OrganizationBranch`.

| الجزئية في السيناريو | هل نغطيها في هذه المرحلة؟ |
| :--- | :--- |
| إنشاء الحساب | ✅ نعم |
| ربط الحساب بـ `Person` | ✅ نعم |
| `OTP` | ✅ نعم |
| إنشاء كلمة المرور | ✅ نعم |
| `Pending Activation` | ✅ نعم |
| تفعيل الحساب من الموظف | ✅ نعم |
| تسجيل الدخول | ⏸️ نؤجله |
| `Authentication` الحقيقي | ⏸️ نؤجله |
| `Authorization` | ⏸️ نؤجله |

---

### المرحلة 4 — عرض وثائق المواطن (`Must Have`)

عرض الوثائق الرسمية (البطاقة الشخصية، جواز السفر، البطاقة العائلية، رخصة القيادة، حالة الوثيقة، `QR Code`).

**الهيكلية:**
```text
Documents
├── Queries
└── DTOs
```

**الكيانات:** `Person`, `NationalIdCard`, `Passport`, `Family`, `DrivingLicense`.

---

### المرحلة 5 — نظام الطلبات (`Must Have`)

تتبع الحالات: `Pending` ← `UnderReview` ← `Approved` / `Rejected` ← `Issued / Completed`.

**الكيانات:** `ServiceRequest`, `ServiceType`, `RequestStatusHistory`.

نظام طلبات موحد وتستفيد منه باقي الـ `Features`.

---

### المرحلة 6 — تجديد الوثائق (`Must Have`)

إشعار تجديد، اختيار الفرع، مراجعة وتحديث البيانات، رفع الصور، إرسال الطلب والمراجعة والتنفيذ.

**الكيانات:** `ServiceRequest`, `OrganizationBranch`, والوثيقة المراد تجديدها.

سيتم استخدام `Background Jobs` هنا لإرسال الإشعارات قبل 90 يوماً من انتهاء الوثيقة.

---

### المرحلة 7 — إدارة السيارات (`Could Have`)

عرض السيارات والمواصفات والترخيص والتجديد.

**الكيانات:** `Vehicle`, `VehicleOwnership`, `VehicleLicense`.

---

### المرحلة 8 — رخصة القيادة (`Could Have`)

عرض الرخصة، إصدار أول مرة، تجديد، ومتابعة الطلب.

**الكيانات:** `DrivingLicense`, `ServiceRequest`.

---

### المرحلة 9 — جواز السفر (`Must Have`)

تطوير الـ `Feature` بشكل كامل:
```text
Passport
   ↓
Passport Queries
Passport Commands
DTOs
Validators
   ↓
EF Configuration
   ↓
API Controller
```

**الكيانات:** `Passport`, `ServiceRequest`, `TravelRecord`, `OrganizationBranch`.

---

### المرحلة 10 — البطاقة العائلية (`Must Have`)

إدارة رب الأسرة، الزوجة، الأبناء، العلاقات، الوفاة، الطلاق، وصلاحيات الأفراد.

**الكيانات:** `Family`, `FamilyMember`, `Person`, `MarriageContract`, `BirthCertificate`.

---

### المرحلة 11 — تسجيل المواليد (`Should Have`)

مستشفى ← تسجيل ولادة ← أحوال مدنية ← مراجعة واعتماد ← إنشاء رقم وطني ← ربط المولود بالأسرة.

**الكيانات:** `BirthCertificate`, `Person`, `Family`, `FamilyMember`.

---

### المرحلة 12 — توثيق الزواج (`Should Have`)

عقد زواج ← مراجعة واعتماد ← إنشاء سجل أسرة ← ربط الزوجين.

**الكيانات:** `MarriageContract`, `Person`, `Family`, `FamilyMember`.

---

### المرحلة 13 — شهادة الوفاة (`Should Have`)

مستشفى ← تسجيل وفاة ← أحوال مدنية ← اعتماد ← `PersonStatus = Deceased` ← تحديث الأسرة.

**الكيانات:** `DeathCertificate`, `Person`, `Family`, `FamilyMember`.

---

### المرحلة 14 — المخالفات المرورية (`Could Have`)

عرض المخالفة وحالة السداد فقط. (لا يوجد معالجة دفع داخل النظام `No Payment Processing`).

**الكيانات:** `TrafficViolation`, `Vehicle`, `Person`.

---

### المرحلة 15 — السجل الطبي (`Should Have`)

عرض السجل المنشأة، التشخيص، العمليات، والأمراض المزمنة، والتحكم بالرؤية عبر `IsVisibleToPerson`.

**الكيانات:** `MedicalRecord`, `MedicalDiagnosis`, `MedicalOperation`, `ChronicDisease`.

---

### المرحلة 16 — الأجهزة الموثوقة (`Could Have`)

تسجيل الجهاز، `OTP`, إشعارات الموافقة، وإدارة الأجهزة الموثوقة (تُنفذ بعد مرحلة `Security`).

**الكيانات:** `UserDevice`, `VerificationCode`.

---

### المرحلة 17 — الإشعارات والتقويم (`Must Have`)

إشعارات انتهاء الوثائق، تحديث الطلبات، والمواعيد مع عرض التاريخ الهجري والميلادي.

**الكيانات:** `Notification`.

---

### المرحلة 18 — الأمن والصلاحيات (`Security`)

تفعيل `Authentication`, `Authorization`, `Identity`, `JWT`, `Roles`, `Policies` وإدارة أدوار المستخدمين (`SuperAdmin`, `Admin`, `Employee`, `Hospital`, `Citizen`).

---

### المرحلة 19 — الخدمات المتقدمة والمشتركة

- `File Storage Service`
- `QR Code Service`
- `Background Jobs`
- `Domain Events`
- `EF Core Interceptors`
- `Pagination / Filtering / Searching`
- `API Versioning`
- `Result Pattern`

---

### المرحلة 20 — الاختبارات والمراجعة النهائية

تغطية حالات `Valid`, `Invalid`, `Not Found`, `Duplicate Data`, `Unauthorized`, `Forbidden` وإجراء:
- `Unit Tests`
- `Integration Tests`
- `API Testing & Swagger`
- `Security & Performance Review`

---

## 📊 جدول التغطية والأولويات للخدمات

| الـ Feature | الأولوية | الكيانات في قاعدة البيانات | حالة التنفيذ |
| :--- | :--- | :--- | :--- |
| إنشاء الحساب وتفعيله | 🔴 `Must Have` | `Person`, `ApplicationUser`, `VerificationCode` | مرحلة أولية |
| عرض الوثائق | 🔴 `Must Have` | الوثائق الأربع الرسمية | مرحلة أولية |
| تتبع الطلبات | 🔴 `Must Have` | `ServiceRequest`, `RequestStatusHistory`, `ServiceType` | مرحلة أولية |
| تجديد الوثائق | 🔴 `Must Have` | `Requests` + `Documents` + `Branch` | مرحلة أولية |
| جواز السفر | 🔴 `Must Have` | `Passport`, `TravelRecord` | مرحلة أولية |
| البطاقة العائلية | 🔴 `Must Have` | `Family`, `FamilyMember` | مرحلة أولية |
| الإشعارات | 🔴 `Must Have` | `Notification` | تنفيذي تدريجي |
| تسجيل المواليد | 🟡 `Should Have` | `BirthCertificate` | لاحقاً |
| توثيق الزواج | 🟡 `Should Have` | `MarriageContract` | لاحقاً |
| شهادة الوفاة | 🟡 `Should Have` | `DeathCertificate` | لاحقاً |
| السجل الطبي | 🟡 `Should Have` | `MedicalRecord` والتفاصيل | لاحقاً |
| إدارة السيارات | 🟢 `Could Have` | `Vehicle`, `VehicleOwnership`, `VehicleLicense` | لاحقاً |
| المخالفات المرورية | 🟢 `Could Have` | `TrafficViolation` | لاحقاً |
| رخصة القيادة | 🟢 `Could Have` | `DrivingLicense` | لاحقاً |
| الأجهزة الموثوقة | 🟢 `Could Have` | `UserDevice`, `VerificationCode` | بعد مرحلة Security |

---

## 🔄 مسار تنفيذ المشروعات (`Execution Flow`)

```text
Foundation
     ↓
Database Foundation
     ↓
Feature 1 كاملة
     ↓
Feature 2 كاملة
     ↓
Feature 3 كاملة
     ↓
Security (عند وقتها)
     ↓
الميزات المعتمدة عليها
     ↓
Cross-cutting
     ↓
Testing
```
