using System;
using System.Collections.Generic;

namespace UniSharp.Tools.Validations
{
    /// <summary>
    /// An exception type specifically for handling validation errors, storing the collection of error messages resulting from validation failures.
    /// </summary>
    public class ValidationException : Exception
    {
        public IEnumerable<string> Errors { get; private set; }

        public ValidationException(IEnumerable<string> errors) : base("Validation failed")
        {
            Errors = errors;
        }
    }
}
