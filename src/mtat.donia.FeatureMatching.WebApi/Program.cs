// See https://aka.ms/new-console-template for more information

using mtat.donia.FeatureMatching.sln;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/FeatureMatching", async ([FromForm] IFormFileCollection files) =>
{
    if (files.Count != 2)
    {
        return Results.BadRequest("Exactly two files are required.");
    }

    using var objectSourceStream = files[0].OpenReadStream();
    using var objectMemoryStream = new MemoryStream();
    await objectSourceStream.CopyToAsync(objectMemoryStream);
    var imageObjectData = objectMemoryStream.ToArray();

    var imageSceneDataList = new List<byte[]>();

    for (int i = 1; i < files.Count; i++)
    {
        using var sceneSourceStream = files[i].OpenReadStream();
        using var sceneMemoryStream = new MemoryStream();
        await sceneSourceStream.CopyToAsync(sceneMemoryStream);
        var imageSceneData = sceneMemoryStream.ToArray();
        imageSceneDataList.Add(imageSceneData);
    }

    var detectObjectInScenesResults = await new ObjectDetection().DetectObjectInScenesAsync(imageObjectData, imageSceneDataList);
    return Results.File(detectObjectInScenesResults[0].ImageData, "image/png");
}).DisableAntiforgery();

app.Run();