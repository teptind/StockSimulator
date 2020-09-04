using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsCounting.Infrastructure.Exceptions {
    internal class ValidationException : Exception {
        public ValidationException(string message) : base(message) {
        }
    }
}
