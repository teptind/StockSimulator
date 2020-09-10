using ProductsCounting.ViewModel;
using System;
using System.Windows;
using ProductsCounting.Infrastructure.Exceptions;

namespace ProductsCounting.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _mainWindowViewModel = new MainWindowViewModel();
            DataContext = new
            {
                viewModelInfo = _mainWindowViewModel,
                productManagerInfo = _mainWindowViewModel.ProductManager,
                queriesInfo = _mainWindowViewModel.ProductManager.QueryService
            };
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            TextBoxProductName.Text = "";
            TextBoxAmount.Text = "";
        }

        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ComboBoxAction.Text == "ADD")
                {
                    _mainWindowViewModel.AddProduct(TextBoxProductName.Text, TextBoxAmount.Text);
                }
                else if (ComboBoxAction.Text == "DELETE")
                {
                    _mainWindowViewModel.DeleteProduct(TextBoxProductName.Text, TextBoxAmount.Text);
                }
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(this, ex.Message, "Operation denied");
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ButtonUpdate.IsEnabled = false;
                _mainWindowViewModel.GetStockInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Operation denied");
            }
            finally
            {
                ButtonUpdate.IsEnabled = true;
            }
        }
    }
}