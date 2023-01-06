namespace CleanArch.Application.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : base("Invalid Request")
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
