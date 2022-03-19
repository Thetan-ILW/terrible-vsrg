using System.Collections.Generic;
using Godot;
//using Newtonsoft.Json;

public struct Settings
{
    public double MusicVolume;

    public float ScrollSpeed;
    public float PrepareTime;
    public float InputLogic;
    public float AudioOffset;
    public float InputOffset;
    public Dictionary<int, int[]> InputMap;
}

public class SettingsLoader
{
    public Settings GetSettings()
    {
        var inputMap = new Dictionary<int, int[]>();
        inputMap.Add(4, new int[4] {(int)KeyList.A ,(int)KeyList.S ,(int)KeyList.K, (int)KeyList.L});
        inputMap.Add(7, new int[7] {(int)KeyList.A, (int)KeyList.S, (int)KeyList.D, (int)KeyList.Space, (int)KeyList.J, (int)KeyList.K, (int)KeyList.L});
        
        Settings settings = new Settings();
        settings.MusicVolume = 0.1;
        settings.ScrollSpeed = 1.95f;
        settings.PrepareTime = 2_000f;
        settings.InputMap = inputMap;

        return settings;
    }
}