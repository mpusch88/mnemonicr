using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace mnemonicr
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string input = InputTextBox.Text.Trim();

            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-ExecutionPolicy Bypass -File script.ps1 -InputString \"{input}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            List<string> mnemonics = new List<string>(output.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));

            ResultListBox.ItemsSource = mnemonics;
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResultListBox.SelectedItems.Count > 0)
            {
                string copyText = string.Join(Environment.NewLine, ResultListBox.SelectedItems);
                Clipboard.SetText(copyText);
            }
        }
    }
}
