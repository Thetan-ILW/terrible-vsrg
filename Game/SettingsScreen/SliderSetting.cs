using Godot;

public class SliderSetting : Setting<float>
{
    private Slider _slider;

    public void Init(string name, float value, float min, float max, float step)
    {
        _name = GetNode<Label>("Name");
        _slider = GetNode<HSlider>("HSlider");
        SetSlider(value, min, max, step);
        _info = GetNode<Label>("Info");
        
        _name.Text = name;
        Value = value;
    }

    private void SetSlider(float value, float min, float max, float step)
    {
        _slider.MinValue = min;
        _slider.MaxValue = max;
        _slider.Step = step;
        _slider.Value = value;
        _slider.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
        _slider.Connect("value_changed", this, nameof(ValueChanged));
    }

    public void ValueChanged(float value) => Value = value;
}
