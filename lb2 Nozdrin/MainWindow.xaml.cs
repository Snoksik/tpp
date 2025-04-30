using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lb2_Nozdrin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Обработчик для кнопок с цифрами и операциями
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                // Добавляем текст кнопки к дисплею
                Display.Text += btn.Content.ToString();
            }
        }

        // Очистка дисплея
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = string.Empty;
        }

        // Удаление последнего символа 
        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Display.Text))
            {
                Display.Text = Display.Text.Substring(0, Display.Text.Length - 1);
            }
        }

        // Вычисление выражения
        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string expression = Display.Text;

                if (string.IsNullOrWhiteSpace(expression))
                    return;

                if (expression.Contains("/0"))
                {
                    MessageBox.Show("Деление на ноль невозможно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Используем DataTable.Compute для простых арифметических операций
                var result = new DataTable().Compute(expression, null);

                // Добавляем в историю
                HistoryList.Items.Insert(0, $"{expression} = {result}");

                // Отображаем результат
                Display.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка в выражении", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}