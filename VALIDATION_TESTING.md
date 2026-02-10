# Testing Data Validation - Curl Examples

## 📋 דוגמאות בדיקה ל-Data Annotations

שימושו בקוד זה כדי לבדוק את הולידציה של ה-DTOs:

---

## 🎁 Gift DTO - בדיקות

### ✅ בדיקה תקנית (201 Created):
```bash
curl -X POST http://localhost:5000/api/gift \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "name": "מתנה יפה",
    "description": "תיאור מפורט של המתנה",
    "ticketPrice": 99.99,
    "category": "אלקטרוניקה",
    "donorName": "משה כהן"
  }'
```

### ❌ בדיקה 1 - שם ריק (400 Bad Request):
```bash
curl -X POST http://localhost:5000/api/gift \
  -H "Content-Type: application/json" \
  -d '{
    "name": "",
    "ticketPrice": 99.99,
    "category": "אלקטרוניקה",
    "donorName": "משה כהן"
  }'
```
**Response:**
```json
{
  "errors": {
    "name": ["שם המתנה הוא חובה"]
  }
}
```

### ❌ בדיקה 2 - שם קצר מדי (400 Bad Request):
```bash
curl -X POST http://localhost:5000/api/gift \
  -H "Content-Type: application/json" \
  -d '{
    "name": "א",
    "ticketPrice": 99.99,
    "category": "אלקטרוניקה",
    "donorName": "משה כהן"
  }'
```
**Response:**
```json
{
  "errors": {
    "name": ["שם המתנה חייב להיות בין 2 ל-100 תווים"]
  }
}
```

### ❌ בדיקה 3 - מחיר שלילי (400 Bad Request):
```bash
curl -X POST http://localhost:5000/api/gift \
  -H "Content-Type: application/json" \
  -d '{
    "name": "מתנה",
    "ticketPrice": -50,
    "category": "אלקטרוניקה",
    "donorName": "משה כהן"
  }'
```
**Response:**
```json
{
  "errors": {
    "ticketPrice": ["מחיר הכרטיס חייב להיות בין 0.01 ל-10000"]
  }
}
```

### ❌ בדיקה 4 - קטגוריה ריקה (400 Bad Request):
```bash
curl -X POST http://localhost:5000/api/gift \
  -H "Content-Type: application/json" \
  -d '{
    "name": "מתנה",
    "ticketPrice": 99.99,
    "category": "",
    "donorName": "משה כהן"
  }'
```
**Response:**
```json
{
  "errors": {
    "category": ["קטגוריה היא חובה"]
  }
}
```

---

## 👤 User DTO - בדיקות

### ✅ בדיקה תקנית (201 Created):
```bash
curl -X POST http://localhost:5000/api/user \
  -H "Content-Type: application/json" \
  -d '{
    "name": "דוד לוי",
    "email": "david@example.com",
    "phone": "0526374859",
    "password": "SecurePassword123",
    "role": "Customer"
  }'
```

### ❌ בדיקה 1 - דוא"ל לא תקני (400 Bad Request):
```bash
curl -X POST http://localhost:5000/api/user \
  -H "Content-Type: application/json" \
  -d '{
    "name": "דוד לוי",
    "email": "not-an-email",
    "phone": "0526374859",
    "password": "SecurePassword123"
  }'
```
**Response:**
```json
{
  "errors": {
    "email": ["דוא״ל אינו תקני"]
  }
}
```

### ❌ בדיקה 2 - טלפון לא תקני (400 Bad Request):
```bash
curl -X POST http://localhost:5000/api/user \
  -H "Content-Type: application/json" \
  -d '{
    "name": "דוד לוי",
    "email": "david@example.com",
    "phone": "123",
    "password": "SecurePassword123"
  }'
```
**Response:**
```json
{
  "errors": {
    "phone": ["הטלפון חייב להיות 9-10 ספרות"]
  }
}
```

### ❌ בדיקה 3 - סיסמה קצרה מדי (400 Bad Request):
```bash
curl -X POST http://localhost:5000/api/user \
  -H "Content-Type: application/json" \
  -d '{
    "name": "דוד לוי",
    "email": "david@example.com",
    "phone": "0526374859",
    "password": "123"
  }'
```
**Response:**
```json
{
  "errors": {
    "password": ["סיסמה חייבת להיות לפחות 6 תווים"]
  }
}
```

---

## 📝 Login DTO - בדיקות

### ✅ בדיקה תקנית (200 OK):
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "Password123"
  }'
```

### ❌ בדיקה 1 - דוא"ל חסר (400 Bad Request):
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "password": "Password123"
  }'
```
**Response:**
```json
{
  "errors": {
    "email": ["דוא״ל הוא חובה"]
  }
}
```

### ❌ בדיקה 2 - דוא"ל לא תקני (400 Bad Request):
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "invalid-email",
    "password": "Password123"
  }'
```
**Response:**
```json
{
  "errors": {
    "email": ["דוא״ל אינו תקני"]
  }
}
```

---

## 🛒 Order DTO - בדיקות

### ✅ בדיקה תקנית (201 Created):
```bash
curl -X POST http://localhost:5000/api/order \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 1,
    "totalAmount": 199.98,
    "orderItems": [
      {
        "giftId": 1,
        "quantity": 2
      },
      {
        "giftId": 3,
        "quantity": 1
      }
    ]
  }'
```

### ❌ בדיקה 1 - סכום שלילי (400 Bad Request):
```bash
curl -X POST http://localhost:5000/api/order \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 1,
    "totalAmount": -50,
    "orderItems": [{"giftId": 1, "quantity": 1}]
  }'
```
**Response:**
```json
{
  "errors": {
    "totalAmount": ["סכום כולל חייב להיות גדול מ-0"]
  }
}
```

### ❌ בדיקה 2 - ללא פריטים (400 Bad Request):
```bash
curl -X POST http://localhost:5000/api/order \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 1,
    "totalAmount": 50,
    "orderItems": []
  }'
```
**Response:**
```json
{
  "errors": {
    "orderItems": ["חייב להיות לפחות פרית אחת"]
  }
}
```

### ❌ בדיקה 3 - כמות גדולה מדי (400 Bad Request):
```bash
curl -X POST http://localhost:5000/api/order \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 1,
    "totalAmount": 50,
    "orderItems": [{"giftId": 1, "quantity": 200}]
  }'
```
**Response:**
```json
{
  "errors": {
    "orderItems[0].quantity": ["כמות חייבת להיות בין 1 ל-100"]
  }
}
```

---

## 🎯 Category DTO - בדיקות

### ✅ בדיקה תקנית (201 Created):
```bash
curl -X POST http://localhost:5000/api/category \
  -H "Content-Type: application/json" \
  -d '{
    "name": "אלקטרוניקה"
  }'
```

### ❌ בדיקה - שם ריק (400 Bad Request):
```bash
curl -X POST http://localhost:5000/api/category \
  -H "Content-Type: application/json" \
  -d '{
    "name": ""
  }'
```
**Response:**
```json
{
  "errors": {
    "name": ["שם הקטגוריה הוא חובה"]
  }
}
```

---

## 💾 Filter DTO - בדיקות

### ✅ בדיקה תקנית עם סינוןים שונים:
```bash
# סינון לפי שם מתנה
curl "http://localhost:5000/api/gift/search?giftName=מתנה&category=אלקטרוניקה"

# סינון עם מיון
curl "http://localhost:5000/api/gift/search?sortBy=price"

# סינון חלקי בלבד
curl "http://localhost:5000/api/gift/search?minPurchasers=10"
```

### ❌ בדיקה - סוג מיון לא תקני (400 Bad Request):
```bash
curl "http://localhost:5000/api/gift/search?sortBy=invalid"
```
**Response:**
```json
{
  "errors": {
    "sortBy": ["סוג המיון חייב להיות 'price' או 'popularity'"]
  }
}
```

---

## 🧪 Batch Tests - בדוקות מרובות

### Script לבדיקה של כל ה-scenarios:
```bash
#!/bin/bash

BASE_URL="http://localhost:5000/api"

echo "=== Test 1: Valid Gift ==="
curl -X POST $BASE_URL/gift \
  -H "Content-Type: application/json" \
  -d '{"name":"מתנה","ticketPrice":50,"category":"A","donorName":"X"}'
echo "\n"

echo "=== Test 2: Invalid Gift - Missing Name ==="
curl -X POST $BASE_URL/gift \
  -H "Content-Type: application/json" \
  -d '{"ticketPrice":50,"category":"A","donorName":"X"}'
echo "\n"

echo "=== Test 3: Invalid Gift - Bad Price ==="
curl -X POST $BASE_URL/gift \
  -H "Content-Type: application/json" \
  -d '{"name":"מתנה","ticketPrice":-10,"category":"A","donorName":"X"}'
echo "\n"

echo "=== Test 4: Valid User ==="
curl -X POST $BASE_URL/user \
  -H "Content-Type: application/json" \
  -d '{"name":"דוד","email":"david@test.com","phone":"0526374859","password":"Pass123"}'
echo "\n"

echo "=== Test 5: Invalid User - Bad Email ==="
curl -X POST $BASE_URL/user \
  -H "Content-Type: application/json" \
  -d '{"name":"דוד","email":"bad","phone":"0526374859","password":"Pass123"}'
echo "\n"
```

---

## 📊 Summary - הסכומ בטבלה

| DTO | שדה | Validation | Error Message |
|-----|-----|-----------|-----------------|
| GiftDTO | name | Required, 2-100 | שם המתנה הוא חובה / בין 2 ל-100 |
| GiftDTO | ticketPrice | Required, 0.01-10000 | מחיר הכרטיס בין 0.01 ל-10000 |
| UserDTO | email | Required, Email | דוא״ל אינו תקני |
| UserDTO | phone | Required, 9-10 ספרות | הטלפון חייב 9-10 ספרות |
| LoginDTO | email | Required, Email | דוא״ל אינו תקני |
| OrderDTO | totalAmount | Required, > 0 | סכום גדול מ-0 |
| OrderDTO | orderItems | MinLength(1) | לפחות פרית אחת |

---

## 🎯 טיפס חשובים

1. **תמיד תקבל 400 Bad Request** כשיש שגיאת ולידציה
2. **Response יכיל את כל השגיאות** בשדה `errors`
3. **Error Messages בעברית** - קל להבין
4. **الولידציה מתבצעת אוטומטי** - לא צריך קוד מיוחד בקונטרולר

---

**סטטוס:** ✅ מוכן לשימוש
