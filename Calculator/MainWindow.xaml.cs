using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public interface IOperation
        {
            decimal DoOperation(decimal val1, decimal val2);
        }

        public class Sum : IOperation
        {
            public decimal DoOperation(decimal val1, decimal val2) 
            { 
                return val1 + val2; 
            }
        }

        public class Subtraction : IOperation
        {
            public decimal DoOperation(decimal val1, decimal val2)
            {
                return val1 - val2;
            }
        }

        public class Division : IOperation
        {
            public decimal DoOperation(decimal val1, decimal val2)
            {
                if (val2 == 0)
                {
                    return 1;
                }
                return val1 / val2;
            }
        }
        public class Multiplication : IOperation
        {
            public decimal DoOperation(decimal val1, decimal val2)
            {
                return val1 * val2;
            }
        }

        string DecimalSeparator = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;

        IOperation CurrentOperation;
        decimal FirstValue { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            btnPoint.Content = DecimalSeparator;
            btnSum.Tag = new Sum();
            btnSubtraction.Tag = new Subtraction();
            btnDivision.Tag = new Division();
            btnMultiplication.Tag = new Multiplication();
        }

        private void typicalButtonClick(object sender, RoutedEventArgs e)
        {
            SendToInput(((Button)sender).Content.ToString());
        }

        private void btnPoint_Click(object sender, RoutedEventArgs e)
        {
            if (txtInput.Text.Contains(this.DecimalSeparator))
                return;

            typicalButtonClick(sender, e);
        }


        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (txtInput.Text == "0")
                return;

            txtInput.Text = txtInput.Text.Substring(0, txtInput.Text.Length - 1);
            if (txtInput.Text == "")
                txtInput.Text = "0";
            txtPrevious.Text = txtInput.Text;
        }
        private void btnClearEntry_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Text = "0";
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            FirstValue = 0;
            CurrentOperation = null;
            txtInput.Text = "0";
            txtPrevious.Text = "0";
        }

        private void operationButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentOperation == null)
                FirstValue = Convert.ToDecimal(txtInput.Text);

            CurrentOperation = (IOperation)((Button)sender).Tag;
            txtInput.Text = "";
            txtPrevious.Text = $"{txtPrevious.Text}{((Button)sender).Content}";
        }

        private void btnEquals_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentOperation == null)
                return;

            if (txtInput.Text == "")
                return;

            decimal val2 = Convert.ToDecimal(txtInput.Text);
            txtInput.Text = CurrentOperation.DoOperation(FirstValue, val2).ToString();
            CurrentOperation = null;
        }

        private void SendToInput(string content)
        {
            if (txtInput.Text == "0")
                txtInput.Text = "";
            if (txtPrevious.Text == "0")
                txtPrevious.Text = "";

            txtInput.Text = $"{txtInput.Text}{content}";
            txtPrevious.Text = $"{txtPrevious.Text}{content}";
        }

        private void DoClick(Button btn)
        {
            btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            switch (e.Text)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    SendToInput(e.Text);
                    break;

                case "*":
                    DoClick(btnMultiplication);
                    break;

                case "-":
                    DoClick(btnSubtraction);
                    break;

                case "+":
                   DoClick(btnSum);
                    break;

                case "/":
                    DoClick(btnDivision);
                    break;

                case "=":
                    DoClick(btnEquals);
                    break;
                default:
                    break;

            }

            
        }
    }

    
}
