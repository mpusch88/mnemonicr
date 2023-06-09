# Mnemonicr (WIP)

Mnemonicr is a simple program that allows the user to search for user accounts in active directory and retrieve their corresponding mnemonics. It provides a simple interface for the user to enter a list of names and/or emails, and searches for matches in the active directory using a PowerShell script. Mnemonicr then returns a formatted list of the mnemonics for the matched user accounts.

## How to Use

### Prerequisites

* Windows operating system with PowerShell installed.
* Active Directory access.

### Building the Project

1. Clone the repository or download the project files.
2. Open `mnemonicr.sln` in Visual Studio.
3. Build the project.

### Running the Program

1. Launch the mnemonicr application.
2. Enter a list of names or emails separated by semicolons into the input text box.
3. Click the `Search` button to search for user accounts.
4. The mnemonics for the matched user accounts will be displayed in the results box.
5. To copy the mnemonics, select the mnemonics and click the `Copy` button.

## Context

This project uses a modified version of  <https://github.com/mpoon/gpt-repository-loader> to maintain up to date code context for use with ChatGPT / GPT4.

Example command:

```bash
python gpt_repository_loader.py ../mnemonicr -p ../mnemonicr/.preamble -o ../mnemonicr/context.txt -t 4000 -m 10
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
