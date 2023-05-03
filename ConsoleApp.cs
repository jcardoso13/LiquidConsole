using Serilog;
using Microsoft.Extensions.Logging;
using DotLiquid;
using CloudLiquid;
using TransformData.ContentFactory;

var serilogLogger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Verbose()
    .WriteTo.Console() // Serilog.Sinks.Debug
    .CreateLogger();

var loggerFactory = new LoggerFactory()
    .AddSerilog(serilogLogger);



var microsoftLogger = loggerFactory.CreateLogger("Logger");
microsoftLogger.LogInformation("Number of Arguments is " + args.Length);
if (args.Length < 3)
{
    microsoftLogger.LogError("This App needs 3 inputs, JSON File, Liquid File and where the Output file should be stored");
    return;
}
microsoftLogger.LogInformation("Input File is " + args[0]);
string inputJSON = File.ReadAllText(args[0]);
microsoftLogger.LogInformation("Liquid File is " + args[1]);
string liquid = File.ReadAllText(args[1]);
microsoftLogger.LogInformation("Output File is " + args[2]);
AzureCloudLiquid.log = microsoftLogger;
var contentReader = ContentFactory.GetContentReader("application/json");
Hash parsedJSON = contentReader.ParseString(inputJSON);
var output = AzureCloudLiquid.Run(parsedJSON,liquid);
//microsoftLogger.LogInformation(output);
var split= args[2].Split(".");
string contenttype;
microsoftLogger.LogInformation("Content Type is "+split[2]);
switch(split[2])
{
    case "json": contenttype="application/json";
    break;
    case "xml": contenttype="application/xml";
    break;
    default: contenttype="text/plain";
    break;
}
var writer = ContentFactory.GetContentWriter(contenttype);
var content = writer.CreateResponse(output);
File.WriteAllText(args[2], await content.ReadAsStringAsync());

