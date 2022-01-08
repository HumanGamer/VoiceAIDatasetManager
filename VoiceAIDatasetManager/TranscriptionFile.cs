using System.Collections;

namespace VoiceAIDatasetManager;

public class TranscriptionFile : IEnumerable<KeyValuePair<string, string>>
{
    private readonly Dictionary<string, string> _entries;

    public string FilePath { get; set; }
    
    public TranscriptionFile()
    {
        _entries = new Dictionary<string, string>();
    }
    
    public void AddEntry(string file, string transcription)
    {
        _entries.Add(file, transcription);
    }
    
    public void RemoveEntry(string file)
    {
        _entries.Remove(file);
    }
    
    public string GetTranscription(string file)
    {
        return _entries[file];
    }

    public string GetFile(int index)
    {
        return _entries.ElementAt(index).Key;
    }
    
    public int GetCount()
    {
        return _entries.Count;
    }
    
    public void Clear()
    {
        _entries.Clear();
    }

    public void Read(string file)
    {
        FilePath = file;
        
        var lines = File.ReadAllLines(file);
        foreach (var line in lines)
        {
            var parts = line.Split('|');
            _entries.Add(parts[0], parts[1]);
        }
    }
    
    public void Write(string file)
    {
        if (!Directory.Exists(Path.GetDirectoryName(file)))
            Directory.CreateDirectory(Path.GetDirectoryName(file));

        var lines = new List<string>();
        foreach (var entry in _entries)
        {
            lines.Add($"{entry.Key}|{entry.Value}");
        }
        File.WriteAllLines(file, lines);
    }

    public bool VerifyPaths()
    {
        var basePath = Path.GetDirectoryName(FilePath);
        
        foreach (var entry in _entries)
        {
            if (!File.Exists(basePath + "/" + entry.Key))
                return false;
        }

        return true;
    }

    public bool ValidateTranscriptions()
    {
        foreach (var entry in _entries)
        {
            if (!Utils.IsValidTranscription(entry.Value))
                return false;
        }

        return true;
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return _entries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}