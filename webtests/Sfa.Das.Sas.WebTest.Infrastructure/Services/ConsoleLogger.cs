namespace Sfa.Das.Sas.WebTest.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;

    public class ConsoleLogger : ILog
    {
        public void Trace(object message)
        {
            throw new NotImplementedException();
        }

        public void Debug(object message)
        {
            throw new NotImplementedException();
        }

        public void Debug(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message, Dictionary<string, object> properties)
        {
            throw new NotImplementedException();
        }

        public void Info(string message, Dictionary<string, object> properties)
        {
            throw new NotImplementedException();
        }

        public void Info(object message)
        {
            Console.WriteLine($"-> {message}");
        }

        public void Info(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Warn(object message)
        {
            throw new NotImplementedException();
        }

        public void Warn(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error(object message)
        {
            Console.WriteLine($"{message}");
        }

        public void Error(object message, Exception exception)
        {
            Console.WriteLine($"{message} {exception.ToNiceString()}");
        }

        public void Fatal(object message)
        {
            throw new NotImplementedException();
        }

        public void Fatal(object message, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}