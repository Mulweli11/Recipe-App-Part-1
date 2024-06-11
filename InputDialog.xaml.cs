using System.Windows;

namespace RecipeApp
{
    public partial class InputDialog : Window
    {
        public string ResponseText { get; private set; }

        public InputDialog(string prompt)
        {
            InitializeComponent();
            PromptTextBlock.Text = prompt;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ResponseText = ResponseTextBox.Text;
            DialogResult = true;
        }

        private void ResponseTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
