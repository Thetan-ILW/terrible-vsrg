using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

public enum InputLogicType {Earliest, Nearest}

public class Settings
{
    // Gameplay
    public InputLogicType InputLogic;
    public float ScrollSpeed;
    public float PrepareTime;
    public Dictionary<int, int[]> InputMap;

    // Video
    public int MaxFPS;
    public bool Vsync;

    // Audio
    public float MusicVolume;
    public float AudioOffset;
}

public class SettingsManager
{
    private string _settingsPath = "Userdata/settings.json";
    private Settings _settings;
    public Settings Settings{ get { return _settings; } }

    public SettingsManager()
    {
        if (!System.IO.File.Exists(_settingsPath))
        {
            CreateDefault();
            SaveFile();
            return;
        }

        LoadFile();
    }

    private void CreateDefault()
    {
        _settings = new Settings()
        {
            InputLogic = InputLogicType.Earliest,
            ScrollSpeed = 1f,
            PrepareTime = 2_000f,
            InputMap = new Dictionary<int, int[]>(),
            MaxFPS = 240,
            MusicVolume = 50f,
            AudioOffset = 0f
        };

        _settings.InputMap.Add(4, new int[] {(int)KeyList.A, (int)KeyList.S, (int)KeyList.K, (int)KeyList.L});
        _settings.InputMap.Add(7, new int[] {(int)KeyList.A, (int)KeyList.S, (int)KeyList.D, (int)KeyList.Space, (int)KeyList.L, (int)KeyList.Semicolon, (int)KeyList.Apostrophe});
    }

    public void SaveFile()
    {
        string output = JsonConvert.SerializeObject(_settings);
        System.IO.File.WriteAllText(_settingsPath, output);
    }

    private void LoadFile()
    {
        string input = System.IO.File.ReadAllText(_settingsPath);
        _settings = JsonConvert.DeserializeObject<Settings>(input);
    }
}