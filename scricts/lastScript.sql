USE [RaffleDB];
GO

-- 1. ניקוי יסודי
DELETE FROM Winners; DELETE FROM OrderTicket; DELETE FROM Orders;
DELETE FROM Gifts; DELETE FROM Categories; DELETE FROM Donors; DELETE FROM Users;

-- 2. משתמשים ותורמים
INSERT INTO Users (Name, Email, Password, Role, IsDeleted) VALUES
(N'אדמין המערכת', 'admin@example.com', '123456', N'Admin', 0),
(N'משתמשת לדוגמה', 'user@example.com', '123456', N'Customer', 0);

INSERT INTO Donors (Name, Email, Address, IsDeleted) VALUES
(N'טק-סולושנס', 'tech@company.com', N'קרית המדע 5', 0),
(N'קרן החיוך', 'smile@foundation.org', N'רחוב האושר 10', 0);

INSERT INTO Categories (Name, IsDeleted) VALUES (N'אלקטרוניקה', 0), (N'פנאי ונופש', 0);

-- 3. שליפת ID
DECLARE @Cat1 INT = (SELECT TOP 1 Id FROM Categories WHERE Name = N'אלקטרוניקה');
DECLARE @Cat2 INT = (SELECT TOP 1 Id FROM Categories WHERE Name = N'פנאי ונופש');
DECLARE @D1 INT = (SELECT TOP 1 Id FROM Donors WHERE Name = N'טק-סולושנס');
DECLARE @D2 INT = (SELECT TOP 1 Id FROM Donors WHERE Name = N'קרן החיוך');

-- 4. הכנסת המתנות - מותאם לתמונות שלך
INSERT INTO Gifts (Name, Description, ImageUrl, TicketPrice, CategoryId, DonorId, IsDeleted) VALUES
-- האלקטרוניקה
(N'הפתעה!', N'אף אחד לא יודע מה זה, גם לא המתכנתת שכתבה את הקוד.', 'assets/surprise.jpg', 250, @Cat1, @D1, 0),
(N'מחשב נייד חזק', N'מגיע עם מאוורר כל כך חזק שהוא כמעט ממריא מהשולחן.', 'assets/tech.jpg', 150, @Cat1, @D1, 0),
(N'מצלמת 4K', N'כדי שתוכלי לראות את הבאגים בחדות מקסימלית.', 'assets/camera.jpg', 80, @Cat1, @D1, 0),
(N'קופת חיסכון', N'כי בסוף התואר כולנו נצטרך להתחיל לחסוך.', 'assets/cash.jpg', 999, @Cat1, @D1, 0),

-- פנאי ונופש
(N'מטבח מפואר', N'כדי שיהיה לך איפה להכין קפה בזמן שהקוד רץ.', 'assets/kitchen.jpg', 120, @Cat2, @D2, 0),
(N'חופשה חלומית', N'הזמן היחיד שבו מותר לך לכבות את המחשב בלי רגשות אשם.', 'assets/alps.jpg', 500, @Cat2, @D2, 0),
(N'ספה מפנקת', N'המקום האידיאלי לבהות במסך ולתהות למה ה-CSS לא עובד.', 'assets/Sofas.jpg', 120, @Cat2, @D2, 0),
(N'בוקר בבית קפה', N'ארוחה שמגיעה בדיוק כשאת מוכנה (Await Breakfast).', 'assets/breakfest.jpg', 45, @Cat2, @D2, 0),
(N'רכב חשמלי', N'בניגוד לקוד שלך - הוא באמת יודע לאן הוא נוסע.', 'assets/car.jpg', 2000, @Cat2, @D2, 0),
(N'דובי Debugging', N'מסבירים לו את הבעיה והוא עוזר להגיע לציון 100.', 'assets/bears.jpg', 30, @Cat2, @D2, 0),
(N'טיסה ל-Success', N'הדרך המהירה ביותר להטיס את הפרויקט הזה לסיום.', 'assets/flight.jpg', 20, @Cat2, @D1, 0),
(N'ערכת איפור', N'מתאימה את עצמה לכל רמת עייפות של סטודנטית.', 'assets/makeup.jpg', 60, @Cat2, @D2, 0);

PRINT 'Database is ready with all the fixes. Good luck!';