using Godot;

public class SongSelect : Control
{
    private Main _main;
    private LineEdit _chartPathLineEdit;
    private HSlider _timeRateSlider;

    public void Init(Main main)
    {
        _main = main;
        _chartPathLineEdit = GetNode<LineEdit>("VBoxContainer/HBoxContainer/LineEdit");
        _timeRateSlider = GetNode<HSlider>("VBoxContainer/HBoxContainer2/HSlider");
    }

    public void StartChart()
    {
        _main.StartChart(
            _chartPathLineEdit.Text,
            (float)_timeRateSlider.Value
        );
    }
}
