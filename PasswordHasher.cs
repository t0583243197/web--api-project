using System;

// סקריפט עזר להצפנת סיסמאות
// הרץ את הקוד הזה כדי לקבל סיסמאות מוצפנות

class PasswordHasher
{
    static void Main()
    {
        string[] passwords = { "Admin123!", "User123!" };
        
        Console.WriteLine("Hashed Passwords:");
        Console.WriteLine("=================");
        
        foreach (var password in passwords)
        {
            string hashed = BCrypt.Net.BCrypt.HashPassword(password);
            Console.WriteLine($"Password: {password}");
            Console.WriteLine($"Hashed:   {hashed}");
            Console.WriteLine();
        }
        
        // בדיקה שההצפנה עובדת
        Console.WriteLine("Verification Test:");
        Console.WriteLine("==================");
        string testPassword = "User123!";
        string testHash = BCrypt.Net.BCrypt.HashPassword(testPassword);
        bool isValid = BCrypt.Net.BCrypt.Verify(testPassword, testHash);
        Console.WriteLine($"Test Password: {testPassword}");
        Console.WriteLine($"Verification Result: {isValid}");
    }
}
