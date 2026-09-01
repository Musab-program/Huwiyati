# المرجع الأساسي لهيكلية Backend مشروع هويتي

## 1. الهدف من هذه الهيكلية

تم اعتماد هيكلية Backend لمشروع **هويتي** تحقق التوازن بين الاحترافية والبساطة.

الهدف ليس بناء نظام بسيط وعشوائي يعتمد فقط على `CRUD`، وفي نفس الوقت لا نريد الدخول منذ البداية في تعقيدات `Enterprise Architecture` الكبيرة التي تضيف طبقات وأنماطاً لا يحتاجها المشروع حالياً.

لذلك سيتم اعتماد:

- `Clean Architecture`
- `Layered Architecture`
- `Feature-Based Organization`
- `Simplified CQRS`
- `Dependency Injection`
- `ASP.NET Core Identity`
- `Entity Framework Core`

وسيتم تطبيق هذه المفاهيم بصورة عملية وبسيطة، مع إضافة الأنماط المتقدمة فقط عند ظهور حاجة حقيقية لها.

---

## 2. المفاهيم والتنظيم المعتمد

### أولاً: `Clean Architecture`

المشروع سيتم تقسيمه إلى طبقات واضحة، بحيث تكون مسؤولية كل طبقة محددة.

الطبقات الأساسية هي:

- `Domain`
- `Application`
- `Infrastructure`
- `API`

**القاعدة الأساسية:**

> «الطبقات الداخلية لا تعتمد على الطبقات الخارجية.»

أي أن `Domain` لا يعرف:

- `Infrastructure`
- `API`
- `SQL Server`
- `Entity Framework Core`

بينما الطبقات الخارجية يمكنها الاعتماد على الطبقات الداخلية.

---

### ثانياً: `Layered Architecture`

سيتم تقسيم المشروع إلى `Layers`، وكل `Layer` ستكون في مشروع مستقل:

- `Huwiyati.Domain`
- `Huwiyati.Application`
- `Huwiyati.Infrastructure`
- `Huwiyati.API`

كل طبقة لها مسؤولية محددة ولا تتدخل في مسؤوليات الطبقات الأخرى.

---

### ثالثاً: `Feature-Based Organization`

داخل طبقة `Application` لن يتم تقسيم المشروع بالكامل بهذه الطريقة:

- `Commands`
- `Queries`
- `Validators`
- `DTOs`

لأن هذا يؤدي إلى توزيع كل ما يتعلق بميزة واحدة في أماكن متفرقة.

بدلاً من ذلك، سيتم التنظيم حسب الـ `Features`.

**مثال:**

- `Authentication`
- `Passports`
- `Documents`
- `Requests`
- `Family`

ثم داخل كل `Feature` نضع فقط الملفات التي يحتاجها.

```text
Passports
│
├── Commands
├── Queries
├── DTOs
└── Validators
```

لكن ليس من الضروري أن يحتوي كل `Feature` على جميع هذه المجلدات.

**القاعدة:**

> «كل `Feature` يحتوي فقط على ما يحتاجه.»

---

### رابعاً: `Simplified CQRS`

سيتم استخدام نسخة مبسطة من `CQRS`.

المقصود هو الفصل بين:

#### `Commands`
العمليات التي تغير بيانات النظام.

مثال:
- `Register`
- `ActivateAccount`
- `CreatePassportRequest`
- `RenewPassport`
- `ApproveRequest`

#### `Queries`
العمليات التي تقرأ البيانات فقط.

مثال:
- `GetPassportById`
- `GetCitizenDocuments`
- `GetUserProfile`
- `GetRequests`

لن يتم استخدام `CQRS` المعقد الذي يحتاج قواعد بيانات منفصلة للقراءة والكتابة أو `Event Sourcing`. سيتم استخدام نفس قاعدة البيانات. الهدف فقط هو تنظيم العمليات بشكل واضح.

---

### خامساً: `Dependency Injection`

سيتم استخدام `Dependency Injection` لفصل الطبقات عن بعضها.

**مثال:**

طبقة `Application` تحتاج خدمة إرسال بريد: `IEmailService` (لكنها لا تعرف كيف يتم الإرسال).

طبقة `Infrastructure` توفر التنفيذ: `EmailService`.

ثم يتم ربطهما من خلال `Dependency Injection`.

---

## 3. الهيكلية العامة للمشروع

```text
Huwiyati.sln
│
├── Huwiyati.Domain
│
├── Huwiyati.Application
│
├── Huwiyati.Infrastructure
│
└── Huwiyati.API
```

---

## 4. `Huwiyati.Domain`

هذه هي الطبقة الأساسية وقلب النظام. تحتوي على المفاهيم وقواعد العمل الأساسية الخاصة بالنظام.

لا تعتمد على:
- `Entity Framework Core`
- `SQL Server`
- `ASP.NET Core`
- `API`
- `Infrastructure`

### الهيكلية:

```text
Huwiyati.Domain
│
├── Common
│   ├── BaseEntity.cs
│   └── AuditableEntity.cs
│
├── Entities
│   ├── CivilRegistry
│   ├── Documents
│   ├── Requests
│   └── Family
│
├── Enums
│
├── Exceptions
│
└── ValueObjects
    └── عند الحاجة فقط
```

### `Common`
يحتوي على الأشياء المشتركة بين الـ `Entities`.

- **`BaseEntity`**: يحتوي على الخصائص الأساسية المشتركة، مثل `Id`.
```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
}
```

- **`AuditableEntity`**: يستخدم للـ `Entities` التي تحتاج إلى تتبع معلومات الإنشاء والتعديل مثل: `CreatedAt`, `UpdatedAt`, `CreatedBy`, `UpdatedBy`. ولا يتم إضافة خصائص مثل `DeletedAt` أو `DeletedBy` إلا إذا قررنا فعلاً استخدام `Soft Delete`.

---

### `Entities`
تمثل الكيانات الأساسية للنظام. سيتم تقسيمها حسب المجال لتجنب وجود عدد كبير من الملفات في مجلد واحد.

```text
Entities
│
├── CivilRegistry
│   ├── Person.cs
│   ├── BirthRecord.cs
│   ├── Marriage.cs
│   └── DeathRecord.cs
│
├── Documents
│   ├── Passport.cs
│   ├── NationalId.cs
│   └── FamilyCard.cs
│
├── Requests
│   └── ServiceRequest.cs
│
└── Family
    ├── Family.cs
    └── FamilyMember.cs
```

هذه مجرد مجلدات تنظيمية حسب المجال.

---

### `Enums`
تحتوي على القيم الثابتة التي تمثل حالات أو أنواعاً محددة مثل: `RequestStatus`, `DocumentStatus`, `Gender`, `MaritalStatus`.

---

### `Exceptions`
تحتوي على الاستثناءات الخاصة بقواعد المجال عند الحاجة.

---

### `ValueObjects`
لن يتم استخدام `Value Objects` بشكل مبالغ فيه.

لن يتم تحويل: `NationalNumber`, `PhoneNumber`, `Email` تلقائياً إلى `Value Objects`. في البداية يمكن أن تكون `Properties` عادية مع `Validation` مناسب.

سيتم إنشاء `Value Object` فقط عندما يوجد مفهوم له قواعد وسلوك خاص ويحتاج إلى حماية منطقية إضافية.

---

## 5. `Huwiyati.Application`

هذه الطبقة تحتوي على `Use Cases` الخاصة بالنظام. أي العمليات التي ينفذها المستخدم أو النظام مثل: `Register`, `Login`, `ActivateAccount`, `RequestPassport`, `RenewPassport`, `GetPassport`, `SubmitRequest`, `ApproveRequest`.

### الهيكلية:

```text
Huwiyati.Application
│
├── Common
│   ├── Interfaces
│   └── Exceptions
│
├── Authentication
│
├── AccountActivation
│
├── Passports
│
├── Documents
│
├── Requests
│
├── Family
│
└── DependencyInjection.cs
```

---

### `Common` داخل `Application`
يحتوي على الأشياء المشتركة بين `Features` المختلفة، ولا علاقة له بـ `Common` الموجود داخل `Domain`.

- **`Interfaces`**: تحتوي على العقود التي تحتاجها طبقة `Application` مثل: `IApplicationDbContext`, `ICurrentUserService`, `IEmailService`, `ITokenService`, `IFileStorageService`.
```text
Application (تحدد ما تحتاجه)
        ↓
    Interface
        ↓
Infrastructure (تنفذ الحاجة)
```

- **`Exceptions`**: تحتوي على الاستثناءات الخاصة بحالات التطبيق مثل: `NotFoundException`, `ForbiddenAccessException`, `ValidationException`.

---

### `Features`
سيتم تنظيم العمليات حسب الميزة أو المجال.

مثال: `Passports` قد يحتوي على:

```text
Passports
│
├── Commands
│   ├── RequestPassport
│   └── RenewPassport
│
├── Queries
│   ├── GetPassportById
│   └── GetCitizenPassport
│
├── DTOs
│
└── Validators
```

- **`Commands`**: تمثل العمليات التي تغير بيانات النظام (`Create`, `Update`, `Delete`, `Register`, `Renew`, `Approve`, `Reject`, `Activate`).
- **`Queries`**: تمثل العمليات التي تقرأ البيانات فقط (`GetById`, `GetAll`, `GetProfile`, `GetDocuments`, `GetRequests`).
- **`DTOs`**: اختصار `Data Transfer Object` وتستخدم لنقل البيانات بين الطبقات أو لإرجاع البيانات إلى الـ `API`. لا يتم إرسال الـ `Entities` مباشرة إلى المستخدم (مثال: تحويل `Passport` الكيان إلى `PassportDto`).
- **`Validators`**: تستخدم للتحقق من صحة البيانات قبل تنفيذ العملية (مثل التحقق من الرقم الوطني أو البريد أو وجود البيانات المطلوبة).

---

## 6. `Huwiyati.Infrastructure`

هذه الطبقة تحتوي على التنفيذ الفعلي للأشياء التقنية مثل:
- قاعدة البيانات.
- `Entity Framework Core`
- `ASP.NET Core Identity`
- الخدمات الخارجية.
- إرسال البريد.
- تخزين الملفات.
- إنشاء `QR Code`.

### الهيكلية:

```text
Huwiyati.Infrastructure
│
├── Persistence
│   ├── ApplicationDbContext.cs
│   ├── Configurations
│   ├── Migrations
│   └── Interceptors
│
├── Identity
│   ├── ApplicationUser.cs
│   └── TokenService.cs
│
├── Services
│   ├── EmailService.cs
│   ├── FileStorageService.cs
│   └── QRCodeService.cs
│
└── DependencyInjection.cs
```

*ملاحظة: بعض هذه الأشياء سيتم إضافتها لاحقاً عند الحاجة، مثل `Interceptors` و `File Storage` و `QRCode`.*

---

### `Persistence`
المقصود بها: «حفظ واسترجاع البيانات»، وتحتوي على كل ما يتعلق بقاعدة البيانات و `Entity Framework Core`.

- **`ApplicationDbContext`**: هو الجسر بين الـ `Entities` وقاعدة البيانات، ويحتوي على `DbSets`, `Relationships`, `Database Configuration`. وبما أن المشروع يستخدم `ASP.NET Core Identity` فسيكون مبنياً على `IdentityDbContext<ApplicationUser>`.
- **`Configurations`**: تحتوي على إعدادات `Entity Framework` الخاصة بكل `Entity` (مثل: `PersonConfiguration`, `PassportConfiguration`, `FamilyConfiguration`). وتحدد أسماء الجداول والحقول المطلوبة والطول الأقصى والعلاقات والفهارس.
- **`Migrations`**: تحتوي على ملفات التغييرات الخاصة ببنية قاعدة البيانات.
- **`Interceptors`**: يمكن استخدامها لاحقاً لتنفيذ عمليات تلقائية أثناء `SaveChanges` مثل تعبئة `CreatedAt` و `UpdatedAt` تلقائياً.

---

### `Identity`
تحتوي على التنفيذ الفعلي الخاص بـ `ASP.NET Core Identity` مثل: `ApplicationUser`, `ApplicationRole`, `TokenService`.

*ملاحظة مهمة: عمليات مثل `Register` و `Login` مكانها في `Application/Authentication` لأنها `Use Cases`. أما التنفيذ الفعلي باستخدام `UserManager` و `SignInManager` و `JWT` فيكون في `Infrastructure`.*

---

### `Services`
تحتوي على الخدمات التقنية أو الخدمات المرتبطة بتقنيات خارجية مثل: `EmailService`, `TokenService`, `FileStorageService`, `QRCodeService`.

لا يتم إنشاء `PersonService` أو `PassportService` أو `RequestService` تلقائياً لكل `Entity`؛ لأن العمليات الأساسية سيتم تنظيمها حسب `Use Cases` داخل `Features`.

---

### `DependencyInjection.cs`
يستخدم لتسجيل الخدمات الخاصة بـ `Infrastructure`.

مثال:
```csharp
services.AddScoped<IEmailService, EmailService>();
```
بدل وضع جميع التسجيلات داخل `Program.cs`.

---

## 7. `Huwiyati.API`

هذه هي نقطة دخول النظام من الخارج. مسؤوليتها التعامل مع: `HTTP Request`, `HTTP Response`, `Authentication`, `Authorization`, `Controllers`, `Middleware`.

### الهيكلية:

```text
Huwiyati.API
│
├── Controllers
│
├── Middleware
│
├── Extensions
│
├── Program.cs
│
└── appsettings.json
```

- **`Controllers`**: وظيفتها استقبال الطلب ← إرساله إلى `Application` ← إرجاع النتيجة. لا يتم وضع `Business Logic` داخل `Controllers`.
- **`Middleware`**: تحتوي على العمليات العامة الخاصة بالـ `HTTP Pipeline` مثل `Global Exception Handling`, `Logging`, `Error Handling`.
- **`Extensions`**: تستخدم لتنظيم إعدادات المشروع بدل تكديس كل شيء داخل `Program.cs`.

---

## 8. أنواع المشاريع عند الإنشاء

تم إنشاء `Solution` باسم `Huwiyati` وتحتوي على المشاريع التالية:

| المشروع | نوع المشروع |
| :--- | :--- |
| `Huwiyati.Domain` | `Class Library` |
| `Huwiyati.Application` | `Class Library` |
| `Huwiyati.Infrastructure` | `Class Library` |
| `Huwiyati.API` | `ASP.NET Core Web API` |

---

## 9. `Project References`

العلاقات والمراجع بين المشاريع هي كالتالي:

- `Huwiyati.Domain`: لا يعتمد على أي مشروع.
- `Huwiyati.Application` ← يعتمد على `Huwiyati.Domain`.
- `Huwiyati.Infrastructure` ← يعتمد على `Huwiyati.Application` و `Huwiyati.Domain`.
- `Huwiyati.API` ← يعتمد على `Huwiyati.Application` و `Huwiyati.Infrastructure`.

```text
                 Huwiyati.API
                 /           \
                ▼             ▼
     Huwiyati.Application   Huwiyati.Infrastructure
                │             │
                └──────┬──────┘
                       ▼
                 Huwiyati.Domain
```

---

## 10. تدفق العملية داخل النظام (`Workflow`)

مثال: المواطن يقدم طلب تجديد جواز.

```text
Flutter
   ↓
HTTP Request
   ↓
API Controller
   ↓
Application Feature
   ↓
Command
   ↓
Validation
   ↓
Command Handler
   ↓
Domain Entities
   ↓
IApplicationDbContext
   ↓
Infrastructure
   ↓
ApplicationDbContext
   ↓
Entity Framework Core
   ↓
SQL Server
   ↓
DTO / Response
   ↓
API
   ↓
Flutter
```

---

## 11. `Repository Pattern`

لن يتم إنشاء `IBaseRepository` أو `BaseRepository` لكل الـ `Entities` بشكل تلقائي.

ولن يتم إنشاء `PersonRepository` أو `PassportRepository` أو `FamilyRepository` فقط لأن لدينا `Entities`.

في البداية سيتم استخدام `IApplicationDbContext` للتعامل مع البيانات. وسيتم إنشاء `Repository` خاص فقط عندما توجد عمليات وصول للبيانات معقدة أو متكررة وتحتاج إلى عزل خاص.

---

## 12. القواعد الأساسية للمشروع

1. **القاعدة الأولى:** لا نضيف `Pattern` لمجرد أنه مشهور. نسأل أولاً: *«هل لدينا مشكلة حقيقية يحتاج هذا الـ Pattern إلى حلها؟»* إذا نعم نستخدمه، وإذا لا لا نضيفه.
2. **القاعدة الثانية:** لا ننشئ `Repository` لكل `Entity` تلقائياً.
3. **القاعدة الثالثة:** لا ننشئ `Service` لكل `Entity` تلقائياً.
4. **القاعدة الرابعة:** يتم تنظيم `Application` حسب `Features`.
5. **القاعدة الخامسة:** كل `Feature` يحتوي فقط على الملفات والمجلدات التي يحتاجها.
6. **القاعدة السادسة:** لا نستخدم `Value Objects` إلا عند وجود حاجة حقيقية.
7. **القاعدة السابعة:** لا نضيف أنظمة متقدمة قبل الحاجة إليها.
8. **القاعدة الثامنة:** الهدف هو بناء مشروع `Professional`, `Clean`, `Scalable`, `Maintainable` بدون `Overengineering`.

---

## 14. ملخص القرار النهائي

المعمارية المعتمدة للمشروع هي:
- `Clean Architecture`
- `Layered Architecture`
- `Feature-Based Organization`
- `Simplified CQRS`
- `Dependency Injection`
- `ASP.NET Core Identity`
- `Entity Framework Core`

وسيتم إتباع قاعدة أساسية طوال المشروع:

> «نبدأ بالحلول البسيطة والاحترافية، ولا نضيف أي `Pattern` أو تقنية أو طبقة إضافية إلا عندما توجد حاجة حقيقية لها.»

الهدف النهائي هو بناء `Backend` نظيف وقابل للتوسع والصيانة، بمستوى احترافي مناسب لمشروع كبير، دون الوقوع في التعقيد الزائد أو الـ `Overengineering`.
