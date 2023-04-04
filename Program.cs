namespace mnemonicr
{
    using System;
    using System.Diagnostics;

    internal class Program
    {
        static void Main(string[] args)
        {
            // Prompt the user to enter a list of names
            Console.Write("Enter a list of names (separated by commas): ");
            string input = Console.ReadLine();
            string[] names = input.Split(',');

            // Run the PowerShell script and capture the output
            Process process = new Process();
            process.StartInfo.FileName = "powershell.exe";
            process.StartInfo.Arguments = $"-ExecutionPolicy Bypass -File [script_path] -Names \"{string.Join(",", names)}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            // Display the formatted output
            Console.WriteLine(output);

            // Wait for user input before exiting
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
