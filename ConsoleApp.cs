using CloudLiquid.ContentFactory;
using DotLiquid;
using DotLiquid.FileSystems;
using Microsoft.Extensions.Logging;
using Serilog;

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
//.LogInformation(System.IO.Directory.GetCurrentDirectory()+"/liquid/");
Template.FileSystem = new LocalFileSystem(System.IO.Directory.GetCurrentDirectory()+"/liquid/");
string contenttype;
microsoftLogger.LogInformation(args[0].Split(".")[2]);
var split= args[0].Split(".");
switch(split[2])
{
    case "json": contenttype="application/json";
    break;
    case "xml": contenttype="application/xml";
    break;
    default: contenttype="text/plain";
    break;
}
var contentReader = ContentFactory.GetContentReader(contenttype);
Hash parsedJSON = contentReader.ParseString(inputJSON);

var liquidInstance = new CloudLiquid.CloudLiquid(microsoftLogger);

liquidInstance.InitializeTagsAndFilters();

var output = liquidInstance.Run(parsedJSON,liquid);
//microsoftLogger.LogInformation(output);
split= args[2].Split(".");
microsoftLogger.LogInformation("Content Type is "+split[1]);
switch(split[1])
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

