using System;
using ProductsCounting.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ProductsCounting.Infrastructure.Exceptions;

namespace ProductsCounting.ViewModel
{
    public class MainWindowViewModel
    {
        public readonly ProductManager ProductManager;

        public MainWindowViewModel()
        {
            ProductManager = new ProductManager();
        }

        private static void ValidateName(string name)
        {
            var rg = new Regex(@"^[a-zA-Z0-9]+");
            if (name == null || !rg.IsMatch(name))
            {
                throw new ValidationException("The name must consist of latin letters and digits only");
            }
        }

        private static int ParseNumber(string number)
        {
            if (int.TryParse(number, out var parsedNumber))
            {
                if (parsedNumber <= 0)
                {
                    throw new ValidationException("The amount must be positive");
                }
            }
            else
            {
                throw new ValidationException("The amount must be an integer");
            }

            return parsedNumber;
        }

        public void AddProduct(string name, string number)
        {
            ValidateName(name);
            ProductManager.AddProduct(name, ParseNumber(number));
        }

        public void DeleteProduct(string name, string number)
        {
            ValidateName(name);
            ProductManager.DeleteProduct(name, ParseNumber(number));
        }
    }
}