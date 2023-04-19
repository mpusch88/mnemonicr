using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Controls;

namespace mnemonicr
{
    public partial class MainWindow : Window
	{
		private Runspace _rs;
		public static bool firstSearch = true;
		private static bool clearResults = true;

		public MainWindow()
		{
			InitializeComponent();
			_rs = RunspaceFactory.CreateRunspace();
			_rs.Open();
		}

		private List<string> RunScript(string script, string input, bool firstSearch)
		{
			List<string> output = new List<string>();

			using (PowerShell ps = PowerShell.Create())
			{
				ps.Runspace = _rs;
				ps.AddScript(script).AddParameter("InputString", input);
				ps.AddParameter("FirstSearch", firstSearch);

				Collection<PSObject> result = ps.Invoke();

				if (ps.Streams.Error.Count > 0)
				{
					// Handle errors if needed
					foreach (var error in ps.Streams.Error)
					{
						output.Add(error.ToString());
					}
				}

				if (ps.Streams.Information.Count > 0)
				{
					// Print debug information
					foreach (var info in ps.Streams.Information)
					{
						Debug.WriteLine(info.MessageData);
					}
				}

				foreach (PSObject obj in result)
				{
					output.Add(obj.ToString());
				}
			}

			return output;
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			List<string> inputList = new List<string>();
			List<string> mnemonics = new List<string>();

			if (!clearResults && ResultListBox_input.ItemsSource != null)
			{
				foreach (string item in ResultListBox_input.ItemsSource)
				{
					inputList.Add(item);
				}
				foreach (string item in ResultListBox_mnem.ItemsSource)
				{
					mnemonics.Add(item);
				}
			}

            string input = InputTextBox.Text.Trim();

			input = input.Replace("\r\n", ";").Replace(";;", ";");

			// Populate input listbox
			inputList.AddRange(new List<string>(input.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)));

			ResultListBox_input.ItemsSource = inputList;

			string script = File.ReadAllText("script.ps1");

			mnemonics.AddRange(RunScript(script, input, firstSearch));


            // test for:	The term 'Add-PSSnapin' is not recognized as a name of a cmdlet, function, script file, or executable program.
            //				Check the spelling of the name, or if a path was included, verify that the path is correct and try again.
			

            ResultListBox_mnem.ItemsSource = mnemonics;

			firstSearch = false;
		}

		private void CopyButton_Click(object sender, RoutedEventArgs e)
		{
			CopySelectedItems();
		}

		private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
			{
				CopySelectedItems();
			}
		}

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			CopyButton.IsEnabled = (ResultListBox_input.SelectedItems.Count > 0) || (ResultListBox_mnem.SelectedItems.Count > 0);
		}

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
			clearResults = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
			clearResults = false;
        }

        private void CopySelectedItems()
		{
			ListBox? focusedListBox = null;

			if (ResultListBox_input.IsFocused || ResultListBox_input.IsKeyboardFocusWithin)
			{
				focusedListBox = ResultListBox_input;
			}
			else if (ResultListBox_mnem.IsFocused || ResultListBox_mnem.IsKeyboardFocusWithin)
			{
				focusedListBox = ResultListBox_mnem;
			}

			if (focusedListBox != null && focusedListBox.SelectedItems.Count > 0)
			{
				StringBuilder copyTextBuilder = new StringBuilder();
				foreach (var item in focusedListBox.SelectedItems)
				{
					copyTextBuilder.AppendLine(item.ToString());
				}

				string copyText = copyTextBuilder.ToString().TrimEnd();
				Clipboard.SetText(copyText);
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			_rs.Dispose();
		}
    }
}
