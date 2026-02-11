# הצפנת סיסמאות - מדריך

## מה השתנה?

### 1. הוספת ספריית BCrypt
- נוספה חבילת `BCrypt.Net-Next` לפרויקט
- זו ספרייה מובילה להצפנת סיסמאות ב-.NET

### 2. עדכון UserServiceBLL.cs
**בהרשמה (AddUser):**
```csharp
userDto.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
```
הסיסמה מוצפנת לפני השמירה במסד הנתונים.

**בהתחברות (ValidateUser):**
```csharp
if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
```
השוואת הסיסמה המוצפנת במסד הנתונים לסיסמה שהמשתמש הזין.

### 3. עדכון סקריפט SQL
הסיסמאות בסקריפט `insert_sample_data.sql` עודכנו לסיסמאות מוצפנות.

## פרטי התחברות

**אדמין:**
- Email: admin@example.com
- Password: Admin123!

**משתמשים רגילים:**
- Email: yossi@example.com / sara@example.com / david@example.com / rachel@example.com
- Password: User123!

## איך זה עובד?

1. **הרשמה:** כשמשתמש נרשם, הסיסמה שלו מוצפנת באמצעות BCrypt לפני השמירה
2. **התחברות:** כשמשתמש מתחבר, BCrypt משווה את הסיסמה שהוזנה לסיסמה המוצפנת
3. **אבטחה:** אי אפשר לפענח את הסיסמה המוצפנת חזרה לטקסט פשוט

## הערות חשובות

- משתמשים קיימים במסד הנתונים עם סיסמאות לא מוצפנות לא יוכלו להתחבר
- צריך להריץ מחדש את סקריפט ה-SQL או לעדכן את הסיסמאות ידנית
- כל הרשמה חדשה תשמור סיסמה מוצפנת אוטומטית
