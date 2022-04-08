using Godot;

public class CheckBoxSetting : Setting<bool>
{
    private CheckBox _checkBox;

    public void Init(string name, bool value)
    {
        _name = GetNode<Label>("Name");
        _checkBox = GetNode<CheckBox>("CheckBox");
        SetCheckBox(value);
        _info = GetNode<Label>("Info");

        _name.Text = name;
        Value = value;
    }

    public void SetCheckBox(bool value)
    {
        _checkBox.Pressed = value;
        _checkBox.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
        _checkBox.Connect("pressed", this, nameof(Pressed));
    }

    public void Pressed() => Value = _checkBox.Pressed;
}
