using System;
using System.IO;
using System.Text;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace WixFileGeneratorWpf
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var inputText = TextInput.Text;
            var sourcePrefix = SourcePrefix.Text;

            try
            {
                var files = inputText.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                var stringBuilder = new StringBuilder();
                foreach (var file in files)
                {
                    var filename = Path.GetFileName(file);
                    var guid = Guid.NewGuid().ToString().ToLower();

                    stringBuilder.AppendLine($"<Component Id=\"{filename}\" Guid=\"{guid}\" >");
                    stringBuilder.AppendLine($"\t<File Id=\"{filename}\" Name=\"{filename}\" Source=\"{sourcePrefix}{filename}\" />");
                    stringBuilder.AppendLine("</Component>");
                }

                TextOutput.Text = stringBuilder.ToString();
            }
            catch (Exception exception)
            {
                this.ShowMessageAsync("An error occured when processing text.", exception.Message);
            }
        }
    }
}
