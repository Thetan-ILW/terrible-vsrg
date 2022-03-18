using Godot;

public class Audio : AudioStreamPlayer
{
    public float CurrentTime = -1f;
    private float _playbackRate = 1f;

    public double Volume = 0.1;

    public Audio() {}
    public Audio(float playbackRate, double volume)
    {
        _playbackRate = playbackRate;
        Volume = volume;
    }

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
        
        VolumeDb = Linear2Db(Volume);
    }

    public void SetPlaybackSpeed(float rate)
    {
        PitchScale = rate;
    }

    private float Linear2Db(double linear)
    {   //https://github.com/godotengine/godot/blob/master/core/math/math_funcs.h wtf
        return (float)(System.Math.Log(linear) * 8.6858896380650365530225783783321);
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
            Play(CurrentTime / 1000);
            SetProcess(false);
        }
    }
}
