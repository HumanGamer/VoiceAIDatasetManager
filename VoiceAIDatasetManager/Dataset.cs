namespace VoiceAIDatasetManager;

public class Dataset
{
    public string DatasetPath { get; private set; }
    
    public List<TranscriptionFile> TranscriptionFiles { get; }
    
    public Dataset()
    {
        TranscriptionFiles = new List<TranscriptionFile>();
    }

    public void Load(string path)
    {
        DatasetPath = path;
        
        TranscriptionFiles.Clear();

        string[] files;
        
        if (!File.Exists(path + "/masterlist.txt"))
        {
            files = Directory.GetFiles(path, "list.txt", SearchOption.AllDirectories);
        }
        else
        {
            var lines = File.ReadAllLines(path + "/masterlist.txt");
            var f = new List<string>();
            foreach (var line in lines)
            {
                f.Add(path + "/" + line.Trim());
            }

            files = f.ToArray();
        }
        
        foreach (var file in files)
        {
            var transcriptionFile = new TranscriptionFile();
            transcriptionFile.Read(file);
            TranscriptionFiles.Add(transcriptionFile);
        }
    }

    public bool VerifyPaths()
    {
        foreach (var transcriptionFile in TranscriptionFiles)
        {
            if (!transcriptionFile.VerifyPaths())
                return false;
        }
        
        return true;
    }

    public bool ValidateTranscriptions()
    {
        foreach (var listFile in TranscriptionFiles)
        {
            if (!listFile.ValidateTranscriptions())
                return false;
        }
        
        return true;
    }
    
    public void Save()
    {
        List<string> paths = new List<string>();
        
        foreach (var listFile in TranscriptionFiles)
        {
            listFile.Write(listFile.FilePath);
            paths.Add(listFile.FilePath);
        }
        
        var lines = new List<string>();
        foreach (var file in paths)
        {
            lines.Add(Path.GetRelativePath(DatasetPath, file));
        }
        File.WriteAllLines(DatasetPath + "/masterlist.txt", lines);
    }

    public void ConvertToUsableDataset(string outputPath)
    {
        if (!VerifyPaths())
            throw new DatasetException("Missing wav files");
        
        //if (!ValidateTranscriptions())
        //    throw new DatasetException("Invalid transcriptions");
        
        TranscriptionFile file = new TranscriptionFile();
        file.FilePath = outputPath + "/list.txt";

        Dictionary<string, string> pathMap = new Dictionary<string, string>();
        int i = 1;

        foreach (var transcriptionFile in TranscriptionFiles)
        {
            foreach (var transcription in transcriptionFile)
            {
                string outPath = "wavs/" + i + ".wav";
                file.AddEntry(outPath, transcription.Value);
                pathMap.Add(Path.GetDirectoryName(transcriptionFile.FilePath) + "/" + transcription.Key, outputPath + "/" + outPath);
                i++;
            }
        }

        foreach (var paths in pathMap)
        {
            if (!Directory.Exists(Path.GetDirectoryName(paths.Value)))
                Directory.CreateDirectory(Path.GetDirectoryName(paths.Value));
            
            //Console.WriteLine(paths.Key + " -> " + paths.Value);
            File.Copy(paths.Key, paths.Value, true);
        }
        
        file.Write(file.FilePath);
    }
}