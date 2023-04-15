using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace mnemonicr
{
	public partial class MainWindow : Window
	{

		public static int firstSearch = -1;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			string input = InputTextBox.Text.Trim();

			// add semicolon to the end of each line in input if it doesn't already have one
			input = input.Replace("\r\n", ";").Replace(";;", ";");

			Debug.WriteLine(input);

			// Populate input listbox
			List<string> inputList = new List<string>(input.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));

			ResultListBox_input.ItemsSource = inputList;

			string Arguments = "";

			if (firstSearch < 0)
			{
				Arguments = $"-ExecutionPolicy Bypass -File script.ps1 -InputString \"{input}\" -FirstSearch";
				firstSearch = 0;
			}
			else
			{
				Arguments = $"-ExecutionPolicy Bypass -File script.ps1 -InputString \"{input}\"";
			}

			// TODO: add second process call to set up PS on the first search.
			// also, test whether each process needs to be set up

			// Setup PowerShell script
			Process process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "powershell.exe",
					Arguments = Arguments,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				}
			};

			// Run PowerShell script
			process.Start();
			string output = process.StandardOutput.ReadToEnd();
			process.WaitForExit();

			// Clean up output mnemonics
			List<string> mnemonics = new List<string>(output.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
			ResultListBox_mnem.ItemsSource = mnemonics;
		}

		private void CopyButton_Click(object sender, RoutedEventArgs e)
		{
			if (ResultListBox_mnem.SelectedItems.Count > 0)
			{
				StringBuilder copyTextBuilder = new StringBuilder();
				foreach (var item in ResultListBox_mnem.SelectedItems)
				{
					copyTextBuilder.AppendLine(item.ToString());
				}

				string copyText = copyTextBuilder.ToString();
				Clipboard.SetText(copyText);
			}
		}
	}
}
