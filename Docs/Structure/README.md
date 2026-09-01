# توثيق هيكلية مشروع هويتي (Huwiyati)

مرحباً بك في توثيق الهيكلية والمعمارية البرمجية الخاصة بـ Backend مشروع **هويتي**. يحتوي هذا المجلد على مرجع المعمارية الكامل وتفاصيل التنظيم للمشروع.

---

## 📁 هيكلية شجرة المجلدات والملفات (`Project Structure`)

```text
Huwiyati.sln
│
├── Huwiyati.Domain
│   ├── Common
│   │   ├── BaseEntity.cs
│   │   └── AuditableEntity.cs
│   ├── Entities
│   │   ├── CivilRegistry
│   │   ├── Documents
│   │   ├── Requests
│   │   └── Family
│   ├── Enums
│   ├── Exceptions
│   └── ValueObjects
│
├── Huwiyati.Application
│   ├── Common
│   │   ├── Interfaces
│   │   └── Exceptions
│   ├── Authentication
│   ├── AccountActivation
│   ├── Documents
│   ├── Passports
│   ├── Requests
│   └── Family
│
├── Huwiyati.Infrastructure
│   ├── Persistence
│   │   ├── ApplicationDbContext.cs
│   │   ├── Configurations
│   │   ├── Migrations
│   │   └── Interceptors
│   ├── Identity
│   │   ├── ApplicationUser.cs
│   │   └── TokenService.cs
│   ├── Services
│   │   ├── EmailService.cs
│   │   ├── FileStorageService.cs
│   │   └── QRCodeService.cs
│   └── DependencyInjection.cs
│
└── Huwiyati.API
    ├── Controllers
    ├── Middleware
    ├── Extensions
    ├── Program.cs
    └── appsettings.json
```

---

## 📚 وثائق المعمارية التفصيلية

المستندات الخاصة بشرح المعمارية والأشياء المؤجلة مقسمة في الملفات التالية:

1. 📖 **[شرح المعمارية والهيكلية الأساسية](./Architecture_Explanation.md)**
   - يحتوي على الأهداف، مفاهيم `Clean Architecture` و `Layered Architecture` و `Feature-Based Organization` و `Simplified CQRS` و `Dependency Injection`.
   - شرح تفصيلي لطبقات المشروع الأربع، العلاقات وقواعد المراجع بين المشاريع (`Project References`)، وتدفق الطلبات (`Workflow`).
   - قواعد التعامل مع `Repository Pattern` والقواعد الثمانية الأساسية للمشروع.

2. 🚀 **[الأشياء المؤجلة للمراحل القادمة وأولوياتها](./Deferred_Features.md)**
   - يتضمن التقنيات والأنماط المخطط إضافتها لاحقاً حسب الحاجة الحقيقية.
   - تصنيف الميزات المؤجلة حسب الأهمية:
     - 🔴 **أهمية عالية جداً:** `Background Jobs`, `File Storage Service`, `QR Code Service`, `Unit & Integration Tests`.
     - 🟠 **أهمية عالية:** `Domain Events`, `Interceptors`, `Result Pattern`.
     - 🟡 **أهمية متوسطة:** `Value Objects`, `Repository` خاص, `Mapping Libraries`, `Pipeline Behaviors`, `Specifications Pattern`.
     - 🟢 **أهمية مستقبلية:** `Modular Monolith`, `Background Job Framework`.
