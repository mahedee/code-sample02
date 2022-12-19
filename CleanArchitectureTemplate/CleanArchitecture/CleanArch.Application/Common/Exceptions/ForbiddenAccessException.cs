namespace CleanArch.Application.Common.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base("You are not authorized") { }
    }
}
