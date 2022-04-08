using Godot;

public class Setting<T> : HBoxContainer
{
    protected Label _name;
    protected Label _info;

    private T _value;
    public T Value
    {
        get { return _value;}
        set { _value = value; _info.Text = _value.ToString(); }
    }
}
