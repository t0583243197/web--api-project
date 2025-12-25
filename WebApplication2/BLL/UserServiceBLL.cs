using WebApplication2.DAL; // Importing DAL
using WebApplication2.Models.DTO; // Importing User DTOs
using System; // Importing for Exception handling
using System.Linq; // Importing FirstOrDefault and LINQ
using System.Text.RegularExpressions; // Required for Regex validation

namespace WebApplication2.BLL // BLL namespace
{
    public class UserServiceBLL : IUserBll // Implementing the BLL interface
    {
        private readonly IUserDal _userDal; // Dependency on DAL

        public UserServiceBLL(IUserDal userDal) // Constructor receiving DAL
        {
            _userDal = userDal; // Storing DAL in field
        }

        public void AddUser(UserDto userDto) // User registration method
        {
            if (string.IsNullOrEmpty(userDto.Role)) // Check if role is not defined
            {
                userDto.Role = "Customer"; // Default role
            }

            // Validate email format
            if (!IsValidEmail(userDto.Email))
            {
                throw new ArgumentException("Invalid email format.");
            }

            // Validate password strength
            if (!IsValidPassword(userDto.Password))
            {
                throw new ArgumentException("Password must be at least 6 characters long and contain a number.");
            }

            // Validate name contains only letters
            if (!IsValidName(userDto.Name))
            {
                throw new ArgumentException("Name must contain only letters.");
            }

            // Additional check to ensure email is unique
            var existingUser = _userDal.GetAll().FirstOrDefault(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("Email is already registered.");
            }

            _userDal.Add(userDto); // Save via DAL
        }

        public UserDto ValidateUser(string email, string password) // User validation for Login
        {
            var allUsers = _userDal.GetAll(); // Fetch all users
            return allUsers.FirstOrDefault(u => u.Email == email && u.Password == password); // Return match or null
        }

        private bool IsValidEmail(string email)
        {
            // Simple regex for email validation
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool IsValidPassword(string password)
        {
            // Validates password to be at least 6 characters long and include a digit
            return password.Length >= 6 && password.Any(char.IsDigit);
        }

        private bool IsValidName(string name)
        {
            // Check if name contains only letters
            return !string.IsNullOrEmpty(name) && name.All(char.IsLetter);
        }
    }
}