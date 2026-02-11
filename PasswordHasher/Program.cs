using System;

string[] passwords = { "Admin123!", "User123!" };

Console.WriteLine("Hashed Passwords:");
Console.WriteLine("=================\n");

foreach (var password in passwords)
{
    string hashed = BCrypt.Net.BCrypt.HashPassword(password);
    Console.WriteLine($"Password: {password}");
    Console.WriteLine($"Hashed:   {hashed}\n");
}
