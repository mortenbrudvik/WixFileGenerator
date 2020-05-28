using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace WixFileGeneratorWpf
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            FileTextInput.Drop += FileTextInput_Drop;
            FileTextInput.PreviewDragOver += FileTextInput_PreviewDragOver;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var inputText = FileTextInput.Text;
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


        private void FileTextInput_PreviewDragOver(object sender, DragEventArgs e) // TRICK: To allow other data objects then strings
        {
            e.Handled = true; 
        }

        private void FileTextInput_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (var file in files)
            {
                if(Directory.Exists(file))
                    continue;

                FileTextInput.Text += Environment.NewLine + Path.GetFileName(file);
            }
            FileTextInput.Text = Regex.Replace(FileTextInput.Text, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
        }
    }
}
