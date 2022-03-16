using Godot;

public class Audio : AudioStreamPlayer2D
{
    public float CurrentTime = -1f;

    public void SetAudio(string audioPath)
    {
        File file = new File();
        Error error = file.Open(audioPath, File.ModeFlags.Read);
        if (error != Error.Ok)
            throw new System.Exception($"cannot open {audioPath}");
        
        switch (System.IO.Path.GetExtension(audioPath))
        {
            case ".mp3":
                Stream = LoadAudioMP3(file);
                break;
            case ".ogg":
                Stream = LoadAudioOGG(file);
                break;
            default:
                throw new System.Exception("This audio format is not supported");
        }
        
        VolumeDb = -20f;
        
    }

    private AudioStreamMP3 LoadAudioMP3(File file)
    {
        var audioStream = new AudioStreamMP3();
        audioStream.Data = file.GetBuffer((long)file.GetLen());
        return audioStream;
    }

    private AudioStreamOGGVorbis LoadAudioOGG(File file)
    {
        var audioStream = new AudioStreamOGGVorbis();
        audioStream.Data = file.GetBuffer((long)file.GetLen());
        return audioStream;
    }

    public override void _Process(float deltaTime)
    {
        if (CurrentTime >= 0)
        {
            Position = new Vector2(640, 360);
            Play();
            SetProcess(false);
        }
    }
}
