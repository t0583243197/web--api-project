using System;

namespace WebApplication2.BLL
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
    }
}