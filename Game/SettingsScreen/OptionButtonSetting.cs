using Godot;

public class OptionButtonSetting : Setting<int>
{
    private OptionButton _optionButton;

    public void Init(string name, string[] items, int value)
    {
        _name = GetNode<Label>("Name");
        _optionButton = GetNode<OptionButton>("OptionButton");
        SetOptionButton(items, value);
        _info = GetNode<Label>("Info");

        _name.Text = name;
        Value = value;
    }

    private void SetOptionButton(string[] items, int value)
    {
        foreach (string item in items)
            _optionButton.AddItem(item);

        _optionButton.Selected = value;
        _optionButton.Connect("item_selected", this, nameof(ItemSelected));
    }

    public void ItemSelected(int id) => Value = id;
}
