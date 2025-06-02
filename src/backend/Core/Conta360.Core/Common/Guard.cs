namespace Conta360.Core.Common
{
    public static class Guard
    {
        public static T AgainstNull<T>(T argument, string parameterName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            return argument;
        }

        public static string AgainstNullOrEmpty(string argument, string parameterName)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException("Argument cannot be null or empty.", parameterName);
            }
            return argument;
        }
        // Add more guard clauses
    }
}