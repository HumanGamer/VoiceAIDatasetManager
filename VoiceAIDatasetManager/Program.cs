using VoiceAIDatasetManager;

Console.WriteLine("=========================");
Console.WriteLine(" VoiceAI Dataset Manager ");
Console.WriteLine("=========================");

if (args.Length < 2)
{
    Console.WriteLine("Usage: VoiceAIDatasetManager.exe <dataset_path> <output_path>");
    return;
}

var dataset = new Dataset();

Console.WriteLine(":: Loading dataset...");
dataset.Load(args[0]);

Console.WriteLine(":: Saving Master List...");
dataset.Save();

Console.WriteLine(":: Converting to Usable Format...");
try
{
    dataset.ConvertToUsableDataset(args[1]);
} catch (DatasetException e)
{
    Console.WriteLine("Error: " + e.Message);
}
