using System.Diagnostics;
using CloudLiquid.ContentFactory;
using CloudLiquid.Core;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Encodings.Web;

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

var stopwatch = new Stopwatch();

// Parsing files
stopwatch.Start();
microsoftLogger.LogInformation("Input File is " + args[0]);
string inputJSON = File.ReadAllText(args[0]);
microsoftLogger.LogInformation("Liquid File is " + args[1]);
string liquid = File.ReadAllText(args[1]);
microsoftLogger.LogInformation("Output File is " + args[2]);
stopwatch.Stop();
microsoftLogger.LogInformation($"Parsing files took {stopwatch.ElapsedMilliseconds} ms");

// Initializing Liquid Processor
stopwatch.Restart();
string contenttype;
microsoftLogger.LogInformation(args[0].Split(".")[2]);
var split = args[0].Split(".");
switch (split[2])
{
    case "json": contenttype = "application/json"; break;
    case "xml": contenttype = "application/xml"; break;
    case "csv": contenttype = "text/csv"; break;
    default: contenttype = "text/plain"; break;
}
var contentReader = ContentFactory.GetContentReader(contenttype);
Dictionary<string,object> parsedJSON = contentReader.ParseString(inputJSON);

var liquidInstance = new LiquidProcessor(microsoftLogger, null,true);
liquidInstance.InitializeTagsAndFilters();
stopwatch.Stop();
microsoftLogger.LogInformation($"Initializing Liquid Processor took {stopwatch.ElapsedMilliseconds} ms");

// Running Liquid Processor
stopwatch.Restart();
var result = liquidInstance.Run(parsedJSON, liquid);
stopwatch.Stop();
microsoftLogger.LogInformation($"Running Liquid Processor took {stopwatch.ElapsedMilliseconds} ms");

// Writing output
stopwatch.Restart();
split = args[2].Split(".");
microsoftLogger.LogInformation("Content Type is " + split.Last());
switch (split.Last())
{
    case "json": 
    contenttype = "application/json"; 
    var jsonElement = JsonSerializer.Deserialize<Dictionary<string, object>>(result.Output, new JsonSerializerOptions
            {
                Converters = {new DictionaryStringObjectJsonConverter()},
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                AllowTrailingCommas = true // Allow trailing commas
            });
    result.Output = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true, AllowTrailingCommas = true});
    break;
    case "xml": contenttype = "application/xml"; break;
    case "csv": contenttype = "text/csv"; break;
    default: contenttype = "text/plain"; break;
}
var writer = ContentFactory.GetContentWriter(contenttype);

if (result.Success)
{
    var content = writer.CreateResponse(result.Output);
    File.WriteAllText(args[2], await content.ReadAsStringAsync());
    microsoftLogger.LogInformation($"Data processed successfully. Output written to {args[2]}");
}
else
{
    microsoftLogger.LogError($"Error while processing data.\nAction: {result.ErrorAction}\nMessage:{result.ErrorMessage}");
}
stopwatch.Stop();
microsoftLogger.LogInformation($"Writing output took {stopwatch.ElapsedMilliseconds} ms");