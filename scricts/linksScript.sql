USE [RaffleDB];
GO

-- 1. ניקוי יסודי של הטבלאות
DELETE FROM Winners; DELETE FROM OrderTicket; DELETE FROM Orders;
DELETE FROM Gifts; DELETE FROM Categories; DELETE FROM Donors; DELETE FROM Users;

-- 2. הכנסת משתמשים ותורמים
INSERT INTO Users (Name, Email, Password, Role, IsDeleted) VALUES
(N'אדמין המערכת', 'admin@example.com', '123456', N'Admin', 0),
(N'משתמשת לדוגמה', 'user@example.com', '123456', N'Customer', 0);

INSERT INTO Donors (Name, Email, Address, IsDeleted) VALUES
(N'טק-סולושנס', 'tech@company.com', N'קרית המדע 5', 0),
(N'קרן החיוך', 'smile@foundation.org', N'רחוב האושר 10', 0);

INSERT INTO Categories (Name, IsDeleted) VALUES (N'אלקטרוניקה', 0), (N'פנאי ונופש', 0);

-- 3. שליפת מזהים (IDs) למשתנים
DECLARE @Cat1 INT = (SELECT TOP 1 Id FROM Categories WHERE Name = N'אלקטרוניקה');
DECLARE @Cat2 INT = (SELECT TOP 1 Id FROM Categories WHERE Name = N'פנאי ונופש');
DECLARE @D1 INT = (SELECT TOP 1 Id FROM Donors WHERE Name = N'טק-סולושנס');
DECLARE @D2 INT = (SELECT TOP 1 Id FROM Donors WHERE Name = N'קרן החיוך');

-- 4. הכנסת המתנות עם תמונות מעודכנות
INSERT INTO Gifts (Name, Description, ImageUrl, TicketPrice, CategoryId, DonorId, IsDeleted) VALUES
-- קטגוריית אלקטרוניקה
(N'הפתעה!', N'אף אחד לא יודע מה זה, גם לא המתכנתת שכתבה את הקוד.', 'https://images.pexels.com/photos/264787/pexels-photo-264787.jpeg?auto=compress&cs=tinysrgb&w=400', 250, @Cat1, @D1, 0),
(N'מחשב נייד חזק', N'מגיע עם מאוורר כל כך חזק שהוא כמעט ממריא מהשולחן.', 'https://images.unsplash.com/photo-1496181133206-80ce9b88a853?q=80&w=400', 150, @Cat1, @D1, 0),
(N'מצלמת 4K', N'כדי שתוכלי לראות את הבאגים בחדות מקסימלית.', 'https://images.unsplash.com/photo-1516035069371-29a1b244cc32?q=80&w=400', 80, @Cat1, @D1, 0),
-- כספת שווה
(N'כספת מזומנים מטורפת', N'כספת פלדה אמיתית עמוסה בחבילות של שטרות. כל מה שצריך כדי להתחיל את החיים בסטייל.', 'https://images.pexels.com/photos/4386431/pexels-photo-4386431.jpeg?auto=compress&cs=tinysrgb&w=400', 999, @Cat1, @D1, 0),

-- קטגוריית פנאי ונופש
(N'מטבח מפואר', N'כדי שיהיה לך איפה להכין קפה בזמן שהקוד רץ.', 'https://images.pexels.com/photos/1080721/pexels-photo-1080721.jpeg?auto=compress&cs=tinysrgb&w=400', 120, @Cat2, @D2, 0),
(N'חופשה חלומית', N'הזמן היחיד שבו מותר לך לכבות את המחשב בלי רגשות אשם.', 'https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?q=80&w=400', 500, @Cat2, @D2, 0),
(N'ספה מפנקת', N'המקום האידיאלי לבהות במסך ולתהות למה ה-CSS לא עובד.', 'https://images.unsplash.com/photo-1555041469-a586c61ea9bc?q=80&w=400', 120, @Cat2, @D2, 0),
(N'בוקר בבית קפה', N'ארוחה שמגיעה בדיוק כשאת מוכנה (Await Breakfast).', 'https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?q=80&w=400', 45, @Cat2, @D2, 0),
(N'רכב חשמלי', N'בניגוד לקוד שלך - הוא באמת יודע לאן הוא נוסע.', 'https://images.unsplash.com/photo-1593941707882-a5bba14938c7?q=80&w=400', 2000, @Cat2, @D2, 0),
-- במקום הדובי: סט גיימינג
(N'סט פיתוח וגיימינג', N'מקלדת מכנית מוארת ועכבר ארגונומי שיגרמו לקוד להיכתב מעצמו.', 'https://images.unsplash.com/photo-1542751371-adc38448a05e?q=80&w=400', 60, @Cat2, @D2, 0),
(N'טיסה ל-Success', N'הדרך המהירה ביותר להטיס את הפרויקט הזה לסיום.', 'https://images.pexels.com/photos/46148/aircraft-jet-landing-cloud-46148.jpeg?auto=compress&cs=tinysrgb&w=400', 20, @Cat2, @D1, 0),
(N'ערכת איפור', N'מתאימה את עצמה לכל רמת עייפות של סטודנטית.', 'https://images.unsplash.com/photo-1522335789203-aabd1fc54bc9?q=80&w=400', 60, @Cat2, @D2, 0);

PRINT 'Database is ready with the new Gaming Set and the Cash Vault!';