# توثيق المرحلة الأولى: التأسيس والبنية التحتية الأساسية (Plan 1 - Foundation Log)

تغطي هذه الوثيقة كل ما تم التخطيط له وتنفيذه بالفعل في **المرحلة الأولى (Foundation)**، متبوعةً بالقرارات والأسئلة الفنية التي تم حسمها وتوضيحها.

---

## 📍 أولاً: الخطة وما تم تنفيذه وإنجازه بالفعل في المرحلة الأولى

في هذه المرحلة، تم تأسيس النواة الأساسية لـ Backend مشروع **هويتي** بنجاح وتوفير المكونات التالية:

### 1. تأسيس مشاريع الحل (`Solution Projects`)
- حل C# باسم `Huwiyati.sln`.
- المشاريع الأربعة بنمط المعمارية النظيفة:
  - `Huwiyati.Domain` (`Class Library`)
  - `Huwiyati.Application` (`Class Library`)
  - `Huwiyati.Infrastructure` (`Class Library`)
  - `Huwiyati.API` (`ASP.NET Core Web API`)

### 2. الكيانات والمكونات المشتركة في الـ `Domain`
- **`BaseEntity.cs`** (`Huwiyati.Domain\Common`): الكيان الأساسي يحتوي على المعرف الفريد `Guid Id = Guid.CreateVersion7()`.
- **`AuditableEntity.cs`** (`Huwiyati.Domain\Common`): يرث من `BaseEntity` ويحتوي على خصائص التتبع الزمني (`CreatedAt`, `CreatedBy`, `LastModifiedAt`, `LastModifiedBy`).
- **`Person.cs`** (`Huwiyati.Domain\Entities\CivilRegistry`): كيان المواطن يرث من `AuditableEntity` ويغطي كافة بيانات الأحوال المدنية.

### 3. الأنواع الثابتة (`Enums`)
تم إنشاء الـ `Enums` التالية داخل `Huwiyati.Domain\Enums`:
- **`Gender.cs`**: (`Male`, `Female`).
- **`MaritalStatus.cs`**: (`Single`, `Married`, `Divorced`, `Widowed`).
- **`PersonStatus.cs`**: (`Active`, `Suspended`, `Deceased`).
- **`AccountStatus.cs`**: (`PendingActivation`, `Active`, `Suspended`, `Deactivated`).

### 4. بنية الهوية والبيانات في `Infrastructure`
- **`ApplicationUser.cs`** (`Huwiyati.Infrastructure\Identity`): يرث من `IdentityUser<Guid>` ويحتوي على الخصائص الإضافية (`PersonId`, `Status`, `CreatedAt`, `ActivatedAt`, وحقل التنقل `Person`).
- **`ApplicationDbContext.cs`** (`Huwiyati.Infrastructure\Persistence`): يرث من `IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>` ويقوم بتطبيق التهيئة التلقائية للـ `Configurations`.
- **`PersonConfiguration.cs`** (`Huwiyati.Infrastructure\Persistence\Configurations`): يحدد قيود جداول المواطن في `SQL Server` وتحويل الـ `Enums` لنصوص.
- **`ApplicationUserConfiguration.cs`** (`Huwiyati.Infrastructure\Persistence\Configurations`): يحدد تسمية جدول المستخدمين كـ `"ApplicationUsers"` وتعيين علاقة (1 إلى 0..1) مع `Person`.

### 5. تهيئة الـ `API`
- **`Program.cs`** (`Huwiyati.API`): تم تفعيل محول الـ `Enums` لتتحول تلقائياً إلى نصوص `JSON` عبر `JsonStringEnumConverter`.

---

## ❓ ثانياً: الأسئلة والإجابات والقرارات الفنية (Q&A & Architectural Rationale)

### س1: ما هو نوع الـ `Guid`؟ وكيف يُخزن في قاعدة البيانات؟ وما هي أفضل ممارسة له؟
- **الإجابة المختصرة:** الـ `Guid` هو هيكل بيانات باينري بحجم 16 بايت (128-bit).
- **التخزين في `SQL Server`:** يُخزن في عمود من نوع `UNIQUEIDENTIFIER` مخصص بكفاءة عالية جداً.
- **أفضل ممارسة (`Best Practice`):** استخدام `Guid.CreateVersion7()` التابع لـ `.NET 9` (المعروف بـ `UUIDv7`)؛ لأنه يولد معرفات مرتبة زمنياً، مما يمنع تجزئة الفهارس (`Index Fragmentation`) في `SQL Server` ويعطي أفضل أداء ممكن مع الحفاظ على الأمان والعشوائية.

---

### س2: من الذي يُخزن في قاعدة البيانات `IdentityUser` أم `ApplicationUser`؟ ولماذا نغير اسم الجدول إلى `ApplicationUsers`؟
- **الإجابة:** `ApplicationUser` هو الذي يُخزن في قاعدة البيانات؛ لأنه يرث كل خصائص `IdentityUser<Guid>` بالإضافة لخصائصنا الخاصة.
- **تغيير اسم الجدول:** افتراضياً تُسمي مايكروسوفت الجدول `"AspNetUsers"`. لتغيير اسمه ليكون احترافياً ومطابقاً لاسم الكلاس كـ `"ApplicationUsers"`، نستخدم `.ToTable("ApplicationUsers")` داخل `ApplicationUserConfiguration.cs`.
- **عدم الحاجة لـ `DbSet<ApplicationUser>` صريحة:** لأن `IdentityDbContext` يحتوي داخلياً على `Users` مجهزة، لكن إضافة الكلاس والتهيئة عبر `Configuration` تمنحنا التحكم الكامل باسم الجدول والعلاقات.

---

### س3: لماذا نستخدم `.HasConversion<string>()` في الـ `Configuration` طالما أضفنا التحويل في `Program.cs`؟
- **`Program.cs` (`JsonStringEnumConverter`):** يحول الـ `Enums` بين نصوص و C# في طلبات الـ `HTTP API` المعروضة لـ `Flutter`.
- **`EF Configuration` (`.HasConversion<string>()`):** يحول الـ `Enums` بين C# وأعمدة جداول `SQL Server` (`NVARCHAR`).
- **النتيجة:** كلاهما يحمي الطبقة الخاصة به (الأولى لـ API والثانية لـ Database).

---

### س4: هل القيود مثل `.IsRequired()` و `.HasMaxLength()` تعتبر `Validation`؟
- **الإجابة:** لا، هذه قيود بنية جدول قاعدة البيانات (`Database Schema Constraints / DDL`).
- الـ `Validation` الحقيقي للطلبات يتم في طبقة الـ `Application` عبر `FluentValidation` قبل إرسال البيانات لدعم الأمان المتعدد.

---

### س5: كيف تم ضبط علاقة (1 إلى 0..1) بين `ApplicationUser` و `Person`؟
- في المعمارية النظيفة، كلاس `Person` في الـ `Domain` لا يعرف `ApplicationUser`.
- لذلك أنشأنا خاصية التنقل من طرف واحد داخل `ApplicationUser` وضبطناها في `ApplicationUserConfiguration` عبر:
  ```csharp
  builder.HasOne(u => u.Person)
         .WithOne()
         .HasForeignKey<ApplicationUser>(u => u.PersonId)
         .IsRequired(false);
  ```

---

### س6: لماذا الوراثة `IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>`؟
- لأن `IdentityDbContext` الافتراضي يتوقع `string`. وبما أننا استخدمنا `Guid` في `ApplicationUser : IdentityUser<Guid>`، يجب إعلام `EF Core` بالنوع الثلاثي للمستخدم والـ Role والمفتاح لمنع خطأ `Type Mismatch`.
