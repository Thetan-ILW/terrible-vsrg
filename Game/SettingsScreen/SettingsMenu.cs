using Godot;

public class SettingsMenu : Control
{
    private Main _main;
    private Settings _settings;

    private VBoxContainer _gameplayTab;
    private VBoxContainer _videoTab;
    private VBoxContainer _audioTab;

    private PackedScene _sliderSettingScene;
    private PackedScene _checkBoxSettingScene;
    private PackedScene _optionButtonScene;

    // Gameplay
    private SliderSetting _scrollSpeed;
    private OptionButtonSetting _inputLogic;

    // Video
    private SliderSetting _maxFps;
    private CheckBoxSetting _vsync;

    // Audio
    private SliderSetting _musicVolume;
    private SliderSetting _audioOffset;

    public void Init(Main main, Settings settings)
    {
        _main = main;
        _settings = settings;

        _gameplayTab = GetNode<VBoxContainer>("MarginContainer/TabContainer/Gameplay/Elements");
        _videoTab = GetNode<VBoxContainer>("MarginContainer/TabContainer/Video/Elements");
        _audioTab = GetNode<VBoxContainer>("MarginContainer/TabContainer/Audio/Elements");

        _sliderSettingScene = GD.Load<PackedScene>("res://Game/SettingsScreen/Nodes/SliderSetting.tscn");
        _checkBoxSettingScene = GD.Load<PackedScene>("res://Game/SettingsScreen/Nodes/CheckBoxSetting.tscn");
        _optionButtonScene = GD.Load<PackedScene>("res://Game/SettingsScreen/Nodes/OptionSetting.tscn");

        AddSettings();
    }

    private void AddSettings()
    {
        // Gameplay
        _scrollSpeed = AddSlider(
            _gameplayTab, 
            "Scroll speed", 
            _settings.ScrollSpeed, 
            0.05f, 
            3f, 
            0.05f
        );

        _inputLogic = AddOptionButton(
            _gameplayTab,
            "Input logic",
            new string[] {InputLogicType.Earliest.ToString(), InputLogicType.Nearest.ToString()},
            (int)_settings.InputLogic
        );

        // Video
        _maxFps = AddSlider(
            _videoTab,
            "Max FPS",
            _settings.MaxFPS,
            30f,
            2000f,
            1f
        );

        _vsync = AddCheckBox(
            _videoTab,
            "Vsync",
            _settings.Vsync
        );

        // Audio
        _musicVolume = AddSlider(
            _audioTab,
            "Music volume",
            _settings.MusicVolume,
            0f,
            100f,
            1f
        );

        _audioOffset = AddSlider(
            _audioTab,
            "Global audio offset",
            _settings.AudioOffset,
            -200f,
            200f,
            1f
        );
    }

    private SliderSetting AddSlider(VBoxContainer tab, string name, float value, float min, float max, float step)
    {
        var slider = _sliderSettingScene.Instance<SliderSetting>();
        slider.Init(name, value, min, max, step);
        tab.AddChild(slider);
        return slider;
    }

    private CheckBoxSetting AddCheckBox(VBoxContainer tab, string name, bool value)
    {
        var checkBox = _checkBoxSettingScene.Instance<CheckBoxSetting>();
        checkBox.Init(name, value);
        tab.AddChild(checkBox);
        return checkBox;
    }

    private OptionButtonSetting AddOptionButton(VBoxContainer tab, string name, string[] items, int value)
    {
        var optionButton = _optionButtonScene.Instance<OptionButtonSetting>();
        optionButton.Init(name, items, value);
        tab.AddChild(optionButton);
        return optionButton;
    }

    public void Exit()
    {
        // Gameplay
        _settings.ScrollSpeed = _scrollSpeed.Value;
        _settings.InputLogic = (InputLogicType)_inputLogic.Value;

        // Video
        _settings.MaxFPS = (int)_maxFps.Value;
        _settings.Vsync = _vsync.Value;

        // Audio
        _settings.MusicVolume = _musicVolume.Value;
        _settings.AudioOffset = _audioOffset.Value;
        
        _main.HideSettings();
    }
}
