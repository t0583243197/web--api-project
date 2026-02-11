    -- סקריפט להוספת נתוני דוגמה למסד הנתונים
-- הרץ את הסקריפט הזה על מסד הנתונים שלך

-- ניקוי נתונים קיימים (אופציונלי - הסר את ההערה אם רוצה למחוק נתונים קיימים)
-- DELETE FROM Winners;
-- DELETE FROM Tickets;
-- DELETE FROM OrderTickets;
-- DELETE FROM Orders;
-- DELETE FROM Gifts;
-- DELETE FROM Categories;
-- DELETE FROM Donors;
-- DELETE FROM Users;

-- הוספת משתמשים (סיסמאות מוצפנות עם BCrypt)
INSERT INTO Users (Name, Email, Password, Role, IsDeleted) VALUES
('אדמין ראשי', 'admin@example.com', '$2a$11$Sx0v1IT3PH8/RKPsA0e2be.Sk7Nb/qCtRC7aPF4qHS2Q.g9EQXu1K', 0, 0),
('יוסי כהן', 'yossi@example.com', '$2a$11$trWvONBCK2Jm2NDK4Npgge8OvgnU/FOHQiIuLNm6IQrYdsv6cWTHu', 4, 0),
('שרה לוי', 'sara@example.com', '$2a$11$trWvONBCK2Jm2NDK4Npgge8OvgnU/FOHQiIuLNm6IQrYdsv6cWTHu', 4, 0),
('דוד מזרחי', 'david@example.com', '$2a$11$trWvONBCK2Jm2NDK4Npgge8OvgnU/FOHQiIuLNm6IQrYdsv6cWTHu', 4, 0),
('רחל אברהם', 'rachel@example.com', '$2a$11$trWvONBCK2Jm2NDK4Npgge8OvgnU/FOHQiIuLNm6IQrYdsv6cWTHu', 4, 0);

-- הסיסמאות המקוריות (לפני הצפנה):
-- אדמין: Admin123!
-- משתמשים רגילים: User123!

-- הוספת תורמים
INSERT INTO Donors (Name, Email, Address, IsDeleted) VALUES
('חברת טכנולוגיה בע"מ', 'tech@company.com', 'רחוב הרצל 10, תל אביב', 0),
('קרן הילדים', 'kids@foundation.org', 'שדרות בן גוריון 25, ירושלים', 0),
('חנות האלקטרוניקה', 'electronics@shop.com', 'רחוב דיזנגוף 50, תל אביב', 0),
('מתנות ושי', 'gifts@store.com', 'רחוב הנביאים 15, חיפה', 0);

-- הוספת קטגוריות
INSERT INTO Categories (Name, IsDeleted) VALUES
('אלקטרוניקה', 0),
('ספורט ופנאי', 0),
('בית ומשפחה', 0),
('תרבות ובידור', 0),
('נסיעות וטיולים', 0);

-- הוספת מתנות
INSERT INTO Gifts (Name, Description, ImageUrl, TicketPrice, CategoryId, DonorId, IsDeleted) VALUES
('אייפון 15 Pro', 'סמארטפון מתקדם עם מצלמה איכותית', 'https://example.com/iphone.jpg', 50.00, 1, 1, 0),
('מחשב נייד Dell', 'מחשב נייד לעבודה ולימודים', 'https://example.com/laptop.jpg', 40.00, 1, 1, 0),
('אופניים חשמליים', 'אופניים חשמליים לנסיעות בעיר', 'https://example.com/bike.jpg', 30.00, 2, 2, 0),
('שעון חכם Apple Watch', 'שעון חכם עם מעקב כושר', 'https://example.com/watch.jpg', 25.00, 1, 3, 0),
('רובוט שואב אבק', 'רובוט שואב אבק חכם', 'https://example.com/robot.jpg', 20.00, 3, 4, 0),
('כרטיסים להופעה', 'כרטיסים להופעה של אמן מוביל', 'https://example.com/concert.jpg', 15.00, 4, 2, 0),
('חבילת נופש לאילת', 'חבילת נופש ל-2 לילות באילת', 'https://example.com/vacation.jpg', 35.00, 5, 2, 0),
('קונסולת PlayStation 5', 'קונסולת משחקים מתקדמת', 'https://example.com/ps5.jpg', 45.00, 1, 3, 0);

-- הוספת הזמנות
INSERT INTO Orders (UserId, IsDraft, OrderDate, TotalAmount) VALUES
(2, 0, '2024-01-15', 150.00),
(3, 0, '2024-01-16', 200.00),
(4, 0, '2024-01-17', 100.00),
(5, 1, '2024-01-18', 75.00);

-- הוספת פריטי הזמנה (כרטיסים בהזמנה)
INSERT INTO OrderTickets (OrderId, GiftId, Quantity) VALUES
(1, 1, 2),
(1, 3, 1),
(2, 2, 3),
(2, 4, 2),
(3, 5, 4),
(4, 6, 5);

-- הוספת כרטיסים
INSERT INTO Tickets (GiftId, UserId, PurchaseDate, IsUsed) VALUES
(1, 2, '2024-01-15', 0),
(1, 2, '2024-01-15', 0),
(3, 2, '2024-01-15', 0),
(2, 3, '2024-01-16', 0),
(2, 3, '2024-01-16', 0),
(2, 3, '2024-01-16', 0),
(4, 3, '2024-01-16', 0),
(4, 3, '2024-01-16', 0),
(5, 4, '2024-01-17', 0),
(5, 4, '2024-01-17', 0),
(5, 4, '2024-01-17', 0),
(5, 4, '2024-01-17', 0);

-- הוספת זוכים (אופציונלי)
INSERT INTO Winners (GiftId, UserId) VALUES
(7, 2),
(8, 3);

-- בדיקת הנתונים שהוספנו
SELECT 'Users' AS TableName, COUNT(*) AS RecordCount FROM Users
UNION ALL
SELECT 'Donors', COUNT(*) FROM Donors
UNION ALL
SELECT 'Categories', COUNT(*) FROM Categories
UNION ALL
SELECT 'Gifts', COUNT(*) FROM Gifts
UNION ALL
SELECT 'Orders', COUNT(*) FROM Orders
UNION ALL
SELECT 'OrderTickets', COUNT(*) FROM OrderTickets
UNION ALL
SELECT 'Tickets', COUNT(*) FROM Tickets
UNION ALL
SELECT 'Winners', COUNT(*) FROM Winners;
