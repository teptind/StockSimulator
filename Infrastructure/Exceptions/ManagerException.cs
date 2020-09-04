using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsCounting.Infrastructure.Exceptions {
    internal class ManagerException : Exception {
        public ManagerException(string message) : base(message) {
        }
    }
}
