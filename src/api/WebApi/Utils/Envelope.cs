﻿using System;

namespace WebApi.Utils
{
    public class Envelope<T>
    {
        public T Result { get; }
        public string ErrorMessage { get; }
        public DateTime TimeGenerated { get; }

        protected internal Envelope(T result, string errorMessage)
        {
            Result = result;
            ErrorMessage = errorMessage;
            TimeGenerated = DateTime.UtcNow;
        }

        public static Envelope<T> Error(string errorMessage)
        {
            return new Envelope<T>(default, errorMessage);
        }
    }

    public sealed class Envelope : Envelope<string>
    {
        private Envelope(string errorMessage)
            : base(null, errorMessage)
        {
        }

        public static Envelope<T> Ok<T>(T result)
        {
            return new Envelope<T>(result, null);
        }

        public static Envelope Ok()
        {
            return new Envelope(null);
        }

        public static new Envelope Error(string errorMessage)
        {
            return new Envelope(errorMessage);
        }
    }
}
