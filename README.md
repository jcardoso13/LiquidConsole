# Console Application to run Liquid

The end goal of this project is to be used in WSL2 or a Linux Machine (Instructions are for Ubuntu/Debian)

## Prerequisites

You need to install .NET8, CloudLiquid.

## Installation

First, install the .NET deb package:

```bash
dotnet tool install --global dotnet-deb
dotnet deb install
```

## Build

To build the project, run:

```bash
dotnet build
dotnet deb
sudo apt install ./bin/Debug/net6.0/LiquidConsole.1.0.0.deb
```

This will output a deb file that you will install on your Linux machine.

## Usage

To run the application, use the following command:

```bash
LiquidConsole ./tests/data/input.json ./tests/data/example.liquid ./tests/data/output.json
```

Now you can use this program anywhere on your Linux machine.

## ConsoleApp.cs

The `ConsoleApp.cs` file is the main entry point of the application. It performs the following tasks:

1. **Logging Setup**: Initializes Serilog for logging and creates a Microsoft logger.
2. **Argument Validation**: Checks if the required arguments (input JSON file, Liquid template file, and output file path) are provided.
3. **File Reading**: Reads the input JSON and Liquid template files.
4. **Content Type Determination**: Determines the content type based on the file extension.
5. **Content Parsing**: Uses `ContentFactory` to parse the input JSON.
6. **Liquid Processing**: Processes the Liquid template with the parsed JSON data using `LiquidProcessor`.
7. **Output Writing**: Writes the processed output to the specified output file.

## GitHub Actions

This project includes a GitHub Actions workflow to automate testing and building the application. The workflow is defined in the `.github/workflows/test.yml` file.

### Workflow Steps

1. **Read Files**: Reads the liquid and JSON files and sets them as outputs.
2. **Call Build and Run**: Uses the `run.yml` workflow to process the files.
3. **Upload Output**: Uploads the generated `output.json` as an artifact.

To manually trigger the workflow, go to the Actions tab in your GitHub repository and select the `Test LiquidConsole` workflow.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is Open Source and developed by João Pedro Cardoso. It is licensed under the GNU General Public License. See the [LICENSE](LICENSE) file for details.

## Contact

For any questions or feedback, please contact [João Pedro Cardoso](mailto:jpcardoso@outlook.pt).