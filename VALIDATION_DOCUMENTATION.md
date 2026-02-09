# Data Validation - Data Annotations in DTOs

## 📋 סיכום

עדכנו את כל ה-DTOs בתיקיית `Models/DTO` להוסיף Data Annotations לולידציה. זה מאפשר טיפול אוטומטי בבדיקת נתונים בצד השרת.

---

## 🎯 מה התווספ?

### 1. Attributes שנוספו:

#### [Required]
מציין שהשדה הוא חובה לא ניתן להיות null או ריק

```csharp
[Required(ErrorMessage = "שם המתנה הוא חובה")]
public string Name { get; set; }
```

#### [EmailAddress]
בדוק שהערך הוא כתובת דוא"ל תקנית

```csharp
[EmailAddress(ErrorMessage = "דוא״ל אינו תקני")]
public string Email { get; set; }
```

#### [StringLength]
בדוק אורך של טקסט עם min ו-max

```csharp
[StringLength(100, MinimumLength = 2, ErrorMessage = "חייב להיות בין 2 ל-100 תווים")]
public string Name { get; set; }
```

#### [Range]
בדוק ערך מספרי בטווח מסוים

```csharp
[Range(0.01, 10000, ErrorMessage = "חייב להיות בין 0.01 ל-10000")]
public decimal TicketPrice { get; set; }
```

#### [Phone]
בדוק שהערך הוא מספר טלפון תקני

```csharp
[Phone(ErrorMessage = "מספר הטלפון אינו תקני")]
public string Phone { get; set; }
```

#### [RegularExpression]
בדוק התאמה לbiblical regular expression

```csharp
[RegularExpression(@"^[0-9]{9,10}$", ErrorMessage = "טלפון חייב להיות 9-10 ספרות")]
public string Phone { get; set; }
```

#### [MinLength]
בדוק מינימום אלמנטים ברשימה

```csharp
[MinLength(1, ErrorMessage = "חייב להיות לפחות פרית אחת")]
public List<OrderItemDTO> OrderItems { get; set; }
```

---

## 📁 DTOs שעודכנו

### 1. UserDTO.cs
```csharp
[Required] Name            → שם חובה, 2-50 תווים
[Required, Email] Email     → דוא"ל חובה, תקני
[Required, Phone] Phone     → טלפון חובה, 9-10 ספרות
[Required] Password         → סיסמה חובה, 6-100 תווים
[StringLength(20)] Role     → תפקיד אופציונלי, עד 20 תווים
```

### 2. GiftDTO.cs
```csharp
[Range(1, max)] Id          → ID חובה, חיובי
[Required] Name             → שם חובה, 2-100 תווים
[StringLength(500)] Description → תיאור אופציונלי, עד 500 תווים
[Required, Range(0.01, 10000)] TicketPrice → מחיר חובה, 0.01-10000
[Required] Category         → קטגוריה חובה, 1-50 תווים
[Required] DonorName        → שם תורם חובה, 2-100 תווים
```

### 3. LoginDTO.cs
```csharp
[Required, Email] Email     → דוא"ל חובה, תקני
[Required] Password         → סיסמה חובה, 6-100 תווים
```

### 4. CategoryDTO.cs
```csharp
[Range(1, max)] Id          → ID חובה, חיובי
[Required] Name             → שם חובה, 2-50 תווים
```

### 5. DonorDTO.cs
```csharp
[Range(1, max)] Id          → ID חובה, חיובי
[Required] Name             → שם חובה, 2-100 תווים
[Required, Email] Email     → דוא"ל חובה, תקני
[StringLength(200)] Address → כתובת אופציונלית, עד 200 תווים
Gifts                       → רשימת מתנות (DTO)
```

### 6. OrderDTO.cs + OrderItemDTO.cs
```csharp
OrderDTO:
[Required, Range(1, max)] UserId        → ID משתמש חובה, חיובי
[Required, Range(0.01, max)] TotalAmount → סכום חובה, > 0
[Required, MinLength(1)] OrderItems      → לפחות פרית אחת

OrderItemDTO:
[Required, Range(1, max)] GiftId        → ID מתנה חובה, חיובי
[Required, Range(1, 100)] Quantity      → כמות חובה, 1-100
```

### 7. TicketDTO.cs
```csharp
[Range(1, max)] Id                      → ID חובה, חיובי
[Required, Range(1, max)] GiftId        → ID מתנה חובה, חיובי
[Required, Range(1, max)] UserId        → ID משתמש חובה, חיובי
PurchaseDate                            → תאריך קנייה
IsUsed                                  → האם כבר שומש
```

### 8. WinnerDTO.cs
```csharp
[Required] GiftName         → שם מתנה חובה, 2-100 תווים
[Required] WinnerName       → שם זוכה חובה, 2-100 תווים
[Required, Email] WinnerEmail → דוא"ל חובה, תקני
```

### 9. GiftFilterDto.cs
```csharp
[StringLength(100)] GiftName             → חיפוש לפי שם מתנה
[StringLength(100)] DonorName            → חיפוש לפי שם תורם
[Range(0, 10000)] MinPurchasers          → מינימום רוכשים
[StringLength(50)] Category              → סינון לפי קטגוריה
[StringLength(20), Regex] SortBy         → מיון (price/popularity)
```

### 10. PurchaserDetailsDto.cs
```csharp
[Required] CustomerName     → שם רוכש חובה, 2-100 תווים
[Required, Email] Email     → דוא"ל חובה, תקני
[Required, Range(1, 10000)] TicketsCount → כרטיסים, 1-10000
```

### 11. SalesSummaryDto.cs + GiftSalesDto.cs
```csharp
SalesSummaryDto:
[Required, Range(0, max)] TotalRevenue       → סך הכנסות >= 0
[Required, Range(0, max)] TotalTicketsSold   → כרטיסים >= 0
SalesPerGift                                 → רשימת מכירות

GiftSalesDto:
[Required] GiftName                         → שם מתנה חובה
[Required, Range(0, max)] PurchaseCount    → רוכשים >= 0
[Required, Range(0, max)] RevenueFromGift  → הכנסה >= 0
```

### 12. GiftWinnerDto.cs
```csharp
[Required, Range(1, max)] GiftId        → ID מתנה חובה, חיובי
[Required] GiftName                     → שם מתנה חובה, 2-100 תווים
[StringLength(100, Min=2)] WinnerName   → שם זוכה אופציונלי, 2-100 תווים
```

---

## 🚀 איך זה עובד?

### בקונטרולר (Model Binding)
```csharp
[HttpPost("create")]
public IActionResult CreateGift([FromBody] GiftDTO dto)
{
    // ASP.NET Core מבצע אוטומטי ולידציה
    if (!ModelState.IsValid)
    {
        // לא תקף - החזר שגיאה
        return BadRequest(ModelState);
    }
    
    // תקף - המשך עם ההשק
    return Ok(await _service.CreateAsync(dto));
}
```

### בקלינט (Angular)
```typescript
// בואו השאר ValidationErrors מעל כל שדה
<div *ngIf="form.get('name')?.hasError('required')">
  שם המתנה הוא חובה
</div>

<div *ngIf="form.get('name')?.hasError('minlength')">
  שם המתנה חייב להיות לפחות 2 תווים
</div>
```

---

## ✅ דוגמהות בדיקה

### בקשה תקנית:
```json
{
  "name": "מתנה יפה",
  "description": "תיאור ארוך",
  "ticketPrice": 50.00,
  "category": "אלקטרוניקה",
  "donorName": "משה כהן"
}
```
✅ תקבל 200 OK

### בקשה לא תקנית - שם ריק:
```json
{
  "name": "",
  "ticketPrice": 50.00,
  "category": "אלקטרוניקה",
  "donorName": "משה כהן"
}
```
❌ תקבל 400 Bad Request
```json
{
  "errors": {
    "name": ["שם המתנה הוא חובה"]
  }
}
```

### בקשה לא תקנית - מחיר שגוי:
```json
{
  "name": "מתנה",
  "ticketPrice": -50,
  "category": "אלקטרוניקה",
  "donorName": "משה כהן"
}
```
❌ תקבל 400 Bad Request
```json
{
  "errors": {
    "ticketPrice": ["מחיר הכרטיס חייב להיות בין 0.01 ל-10000"]
  }
}
```

---

## 🔧 איך להוסיף ולידציה למשדה חדש?

### שלב 1: בחר את ה-Attribute
```csharp
// לטקסט
[Required] → חובה
[StringLength(50)] → אורך מקסימום
[MinLength(2)] → אורך מינימום
[RegularExpression(@"pattern")] → pattern מותאם

// למספרים
[Range(0, 100)] → טווח
[Range(1, int.MaxValue)] → רק חיובי

// למייל
[EmailAddress] → תקני בתור דוא"ל

// לטלפון
[Phone] → מספר טלפון תקני

// לרשימות
[MinLength(1)] → לפחות אלמנט אחד
```

### שלב 2: הוסף לשדה
```csharp
[Required(ErrorMessage = "שדה חובה")]
[StringLength(100, MinimumLength = 5)]
public string MyField { get; set; }
```

### שלב 3: בדוק בקונטרולר
```csharp
if (!ModelState.IsValid)
{
    return BadRequest(ModelState);
}
```

---

## 🎨 סוגי Error Messages

| סוג | דוגמה |
|-----|--------|
| Required | "שדה זה הוא חובה" |
| Length | "חייב להיות בין 2 ל-100 תווים" |
| Email | "דוא״ל אינו תקני" |
| Range | "חייב להיות בין 0 ל-100" |
| Regex | "פורמט אינו תקני" |

---

## 🔒 Security Benefits

✅ **אם לא היה ולידציה:**
- בקשה עם name = "" היתה עוברת לשרת
- הייתה יוצרת מתנה עם שם ריק
- היתה בעיה בדטה בייס

✅ **עם ולידציה:**
- בקשה נדחית לפני שמגעת לשרת
- Client מקבל הודעת שגיאה ברורה
- Error Message בעברית שקל להבין

---

## 📊 טבלת ה-Attributes בשימוש

| DTO | Attributes שנוספו |
|-----|------------------|
| UserDTO | 6 - Required, StringLength, EmailAddress, Phone, RegularExpression |
| GiftDTO | 5 - Required, StringLength, Range, EmailAddress |
| LoginDTO | 3 - Required, EmailAddress, StringLength |
| CategoryDTO | 3 - Required, StringLength, Range |
| DonorDTO | 4 - Required, EmailAddress, StringLength, Range |
| OrderDTO | 4 - Required, Range, MinLength |
| OrderItemDTO | 3 - Required, Range |
| TicketDTO | 3 - Range, Required |
| WinnerDTO | 4 - Required, StringLength, EmailAddress |
| GiftFilterDto | 5 - StringLength, Range, RegularExpression |
| PurchaserDetailsDto | 4 - Required, StringLength, EmailAddress, Range |
| SalesSummaryDto | 2 - Required, Range |
| GiftSalesDto | 3 - Required, StringLength, Range |
| GiftWinnerDto | 4 - Required, StringLength, Range |

---

## 🎯 תוצאה

✅ כל ה-DTOs מעכשיו:
- מאמתים את הנתונים בצד השרת
- מחזירים error messages ברורים בעברית
- מנעים אחסון של נתונים לא תקינים
- משפרים את ה-UX בדף הקליינט (Angular)

**סטטוס:** ✅ מוכן לשימוש בפרודקשן
