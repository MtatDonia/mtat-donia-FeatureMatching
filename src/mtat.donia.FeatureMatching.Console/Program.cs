// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System.Text.Json;
using mtat.donia.FeatureMatching.sln;

partial class Program
{
    static async Task Main(string[] args)
    {
        string objectImagePath = args[0];
        string scenesDirectoryPath = args[1];

        byte[] objectImageData = await File.ReadAllBytesAsync(objectImagePath);
        var imageScenesData = Directory.GetFiles(scenesDirectoryPath).Select(File.ReadAllBytes).ToList();

        var detectObjectInScenesResults = await new ObjectDetection().DetectObjectInScenesAsync(objectImageData, imageScenesData);

        foreach (var objectDetectionResult in detectObjectInScenesResults)
        {
            System.Console.WriteLine($"Points: {JsonSerializer.Serialize(objectDetectionResult.Points)}");
        }
    }
}