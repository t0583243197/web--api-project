-- ========================================
-- סקריפט מלא ליצירת מסד נתונים והוספת נתונים
-- ========================================

-- יצירת מסד נתונים חדש (אם לא קיים)
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'RaffleDB')
BEGIN
    CREATE DATABASE RaffleDB;
END
GO

USE RaffleDB;
GO

-- מחיקת טבלאות קיימות (אם קיימות) - בסדר הפוך בגלל Foreign Keys
IF OBJECT_ID('Winners', 'U') IS NOT NULL DROP TABLE Winners;
IF OBJECT_ID('Tickets', 'U') IS NOT NULL DROP TABLE Tickets;
IF OBJECT_ID('OrderTicket', 'U') IS NOT NULL DROP TABLE OrderTicket;
IF OBJECT_ID('Orders', 'U') IS NOT NULL DROP TABLE Orders;
IF OBJECT_ID('Gifts', 'U') IS NOT NULL DROP TABLE Gifts;
IF OBJECT_ID('Categories', 'U') IS NOT NULL DROP TABLE Categories;
IF OBJECT_ID('Donors', 'U') IS NOT NULL DROP TABLE Donors;
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;
GO

-- ========================================
-- יצירת טבלאות
-- ========================================

-- טבלת משתמשים
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    Password NVARCHAR(200) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
);

-- טבלת תורמים
CREATE TABLE Donors (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    Address NVARCHAR(300) NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
);

-- טבלת קטגוריות
CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
);

-- טבלת מתנות
CREATE TABLE Gifts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NULL,
    ImageUrl NVARCHAR(2000) NULL,
    TicketPrice DECIMAL(18,2) NOT NULL,
    CategoryId INT NOT NULL,
    DonorId INT NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Gifts_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    CONSTRAINT FK_Gifts_Donors FOREIGN KEY (DonorId) REFERENCES Donors(Id)
);

-- טבלת הזמנות
CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    IsDraft BIT NOT NULL,
    OrderDate DATETIME2 NOT NULL,
    TotalAmount FLOAT NOT NULL,
    CONSTRAINT FK_Orders_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- טבלת פריטי הזמנה
CREATE TABLE OrderTicket (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    GiftId INT NOT NULL,
    Quantity INT NOT NULL,
    CONSTRAINT FK_OrderTicket_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    CONSTRAINT FK_OrderTicket_Gifts FOREIGN KEY (GiftId) REFERENCES Gifts(Id)
);

-- טבלת כרטיסים
CREATE TABLE Tickets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    GiftId INT NOT NULL,
    UserId INT NOT NULL,
    PurchaseDate DATETIME2 NOT NULL,
    IsUsed BIT NOT NULL
);

-- טבלת זוכים
CREATE TABLE Winners (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    GiftId INT NOT NULL,
    UserId INT NOT NULL,
    CONSTRAINT FK_Winners_Gifts FOREIGN KEY (GiftId) REFERENCES Gifts(Id),
    CONSTRAINT FK_Winners_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- יצירת אינדקסים
CREATE INDEX IX_Gifts_CategoryId ON Gifts(CategoryId);
CREATE INDEX IX_Gifts_DonorId ON Gifts(DonorId);
CREATE INDEX IX_Orders_UserId ON Orders(UserId);
CREATE INDEX IX_OrderTicket_OrderId ON OrderTicket(OrderId);
CREATE INDEX IX_OrderTicket_GiftId ON OrderTicket(GiftId);
CREATE INDEX IX_Winners_GiftId ON Winners(GiftId);
CREATE INDEX IX_Winners_UserId ON Winners(UserId);
GO

-- ========================================
-- הוספת נתונים
-- ========================================

-- הוספת משתמשים (סיסמאות מוצפנות עם BCrypt)
INSERT INTO Users (Name, Email, Password, Role, IsDeleted) VALUES
(N'אדמין ראשי', 'admin@example.com', '$2a$11$Sx0v1IT3PH8/RKPsA0e2be.Sk7Nb/qCtRC7aPF4qHS2Q.g9EQXu1K', N'Admin', 0),
(N'יוסי כהן', 'yossi@example.com', '$2a$11$trWvONBCK2Jm2NDK4Npgge8OvgnU/FOHQiIuLNm6IQrYdsv6cWTHu', N'Customer', 0),
(N'שרה לוי', 'sara@example.com', '$2a$11$trWvONBCK2Jm2NDK4Npgge8OvgnU/FOHQiIuLNm6IQrYdsv6cWTHu', N'Customer', 0),
(N'דוד מזרחי', 'david@example.com', '$2a$11$trWvONBCK2Jm2NDK4Npgge8OvgnU/FOHQiIuLNm6IQrYdsv6cWTHu', N'Customer', 0),
(N'רחל אברהם', 'rachel@example.com', '$2a$11$trWvONBCK2Jm2NDK4Npgge8OvgnU/FOHQiIuLNm6IQrYdsv6cWTHu', N'Customer', 0);

-- הסיסמאות המקוריות (לפני הצפנה):
-- אדמין: Admin123!
-- משתמשים רגילים: User123!

-- הוספת תורמים
INSERT INTO Donors (Name, Email, Address, IsDeleted) VALUES
(N'חברת טכנולוגיה בע"מ', 'tech@company.com', N'רחוב הרצל 10, תל אביב', 0),
(N'קרן הילדים', 'kids@foundation.org', N'שדרות בן גוריון 25, ירושלים', 0),
(N'חנות האלקטרוניקה', 'electronics@shop.com', N'רחוב דיזנגוף 50, תל אביב', 0),
(N'מתנות ושי', 'gifts@store.com', N'רחוב הנביאים 15, חיפה', 0);

-- הוספת קטגוריות
INSERT INTO Categories (Name, IsDeleted) VALUES
(N'אלקטרוניקה', 0),
(N'ספורט ופנאי', 0),
(N'בית ומשפחה', 0),
(N'תרבות ובידור', 0),
(N'נסיעות וטיולים', 0);

-- הוספת מתנות
INSERT INTO Gifts (Name, Description, ImageUrl, TicketPrice, CategoryId, DonorId, IsDeleted) VALUES
(N'אייפון 15 Pro', N'סמארטפון מתקדם עם מצלמה איכותית', 'https://example.com/iphone.jpg', 50.00, 1, 1, 0),
(N'מחשב נייד Dell', N'מחשב נייד לעבודה ולימודים', 'https://example.com/laptop.jpg', 40.00, 1, 1, 0),
(N'אופניים חשמליים', N'אופניים חשמליים לנסיעות בעיר', 'https://example.com/bike.jpg', 30.00, 2, 2, 0),
(N'שעון חכם Apple Watch', N'שעון חכם עם מעקב כושר', 'https://example.com/watch.jpg', 25.00, 1, 3, 0),
(N'רובוט שואב אבק', N'רובוט שואב אבק חכם', 'https://example.com/robot.jpg', 20.00, 3, 4, 0),
(N'כרטיסים להופעה', N'כרטיסים להופעה של אמן מוביל', 'https://example.com/concert.jpg', 15.00, 4, 2, 0),
(N'חבילת נופש לאילת', N'חבילת נופש ל-2 לילות באילת', 'https://example.com/vacation.jpg', 35.00, 5, 2, 0),
(N'קונסולת PlayStation 5', N'קונסולת משחקים מתקדמת', 'https://example.com/ps5.jpg', 45.00, 1, 3, 0);

-- הוספת הזמנות
INSERT INTO Orders (UserId, IsDraft, OrderDate, TotalAmount) VALUES
(2, 0, '2024-01-15', 150.00),
(3, 0, '2024-01-16', 200.00),
(4, 0, '2024-01-17', 100.00),
(5, 1, '2024-01-18', 75.00);

-- הוספת פריטי הזמנה (כרטיסים בהזמנה)
INSERT INTO OrderTicket (OrderId, GiftId, Quantity) VALUES
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

GO

-- ========================================
-- בדיקת הנתונים שהוספנו
-- ========================================
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
SELECT 'OrderTicket', COUNT(*) FROM OrderTicket
UNION ALL
SELECT 'Tickets', COUNT(*) FROM Tickets
UNION ALL
SELECT 'Winners', COUNT(*) FROM Winners;

PRINT 'Database setup completed successfully!';
PRINT 'Database name: RaffleDB';
PRINT 'Admin login: admin@example.com / Admin123!';
PRINT 'User login: yossi@example.com / User123!';
--   Connection Stringלשנות בפרויקט ב
-- "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=RaffleDB;Integrated Security=True;TrustServerCertificate=True"