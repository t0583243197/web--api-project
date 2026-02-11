# מדריך התקנה למחשב חדש

## שלב 1: הכנת מסד הנתונים

### אופציה א': שימוש ב-LocalDB (מומלץ לפיתוח)
1. פתח את הקובץ `setup_complete_database.sql`
2. שנה את שם מסד הנתונים מ-`RaffleDB` ל-`MyStoreDatabase` (או השאר כמו שהוא)
3. הרץ את הסקריפט ב-SQL Server Management Studio או Visual Studio

### אופציה ב': שימוש ב-SQL Server מלא
1. פתח SQL Server Management Studio
2. התחבר לשרת
3. פתח את הקובץ `setup_complete_database.sql`
4. הרץ את הסקריפט (F5)

## שלב 2: עדכון Connection String

ערוך את הקובץ `appsettings.json` בפרויקט:

### עבור LocalDB:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=RaffleDB;Integrated Security=True;TrustServerCertificate=True"
}
```

### עבור SQL Server:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=RaffleDB;Integrated Security=True;TrustServerCertificate=True"
}
```

### עבור SQL Server עם אימות משתמש:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=RaffleDB;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True"
}
```

## שלב 3: התקנת תלויות

פתח טרמינל בתיקיית הפרויקט והרץ:
```bash
cd WebApplication2
dotnet restore
```

## שלב 4: הרצת הפרויקט

```bash
dotnet run
```

## פרטי התחברות

**אדמין:**
- Email: admin@example.com
- Password: Admin123!

**משתמשים רגילים:**
- Email: yossi@example.com (או sara/david/rachel)
- Password: User123!

## בדיקת התקנה

1. פתח דפדפן וגש ל: `https://localhost:5001/swagger`
2. נסה endpoint של Login עם פרטי האדמין
3. אם קיבלת Token - ההתקנה הצליחה!

## פתרון בעיות

### שגיאת חיבור למסד נתונים:
- ודא ש-SQL Server פועל
- בדוק את ה-Connection String
- ודא שמסד הנתונים נוצר בהצלחה

### שגיאת הרשאות:
- הרץ Visual Studio/VS Code כמנהל
- ודא שיש לך הרשאות ליצור מסד נתונים

### הסיסמאות לא עובדות:
- ודא שהסקריפט SQL רץ בהצלחה
- בדוק שהסיסמאות המוצפנות נשמרו נכון
