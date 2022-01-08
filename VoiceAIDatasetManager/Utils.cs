namespace VoiceAIDatasetManager;

public static class Utils
{
    public static bool IsValidTranscription(string transcription)
    {
        if (string.IsNullOrEmpty(transcription))
            return false;

        if (!transcription.Trim().EndsWith("."))
            return false;
        
        return true;
    }
}