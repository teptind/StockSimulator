using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ProductsCounting.Infrastructure.Exceptions;

namespace ProductsCounting.Service {
    internal class Tools {
        public static void ValidateProductName(string name) {
            var rg = new Regex(@"^[a-zA-Z0-9]+");
            if (name == null || !rg.IsMatch(name))
                throw new ValidationException("The name must consist of latin letters and digits only");
        }

        public static int ParsePositiveNumber(string number) {
            if (!int.TryParse(number, out var parsedNumber))
                throw new ValidationException("The amount must be an integer");

            if (parsedNumber <= 0)
                throw new ValidationException("The amount must be positive");

            return parsedNumber;
        }
    }
}
