using Godot;
using System;

public class Conveyor : Node2D
{
    private Skin _skin;
    private Note[] _note; // тут ссылка на ноты если чё
    private InputLogic _inputLogic;

    public float ScrollSpeed = 0;
    public float CurrentTime = 0;
    public int NextExistingNote = 0;

    public void Construct(ref Note[] note, Skin skin, InputLogic inputLogic, float scrollSpeed)
    {
        _note = note;
        _skin = skin;
        _inputLogic = inputLogic;
        ScrollSpeed = scrollSpeed;
    }

    public override void _Process(float delta)
    {
        Update();
    }

    public override void _Draw()
    {
        // Notes
        // Начинаем рисовать с самой ранней существующей ноты 
        for (int i = NextExistingNote; i != _note.GetLength(0) ; i++)
        {
            Note n = _note[i];
            // Считаем для каждой ноты из массива нот положение на экране
            float noteYPosition = (-n.time + CurrentTime + (_skin.Position.y / ScrollSpeed)) * ScrollSpeed;

            // Прерываем рисовку нот если хоть одна нота уже за экраном
            // Потому что после неё уже все ноты будут за границей видимости
            if (noteYPosition < -_skin.NoteSize.y)
                break; 

            if (n.isExist != true)
                continue;
                     
            DrawTexture(
                _skin.NoteImage[n.column],
                new Vector2(
                    _skin.Position.x + (n.column * _skin.NoteSize.x),
                    noteYPosition)
            );
        }

        // Keys
        for (int i = 0; i != _skin.InputMode; i++)
        {
            if (_inputLogic.KeyState[i] != false)
                continue;

            DrawTexture(
                _skin.KeyImage[i],
                new Vector2(
                    _skin.Position.x + (i * _skin.NoteSize.x),
                    _skin.Position.y
                )
            );
        }

        //Pressed keys
        for (int i = 0; i != _skin.InputMode; i++)
        {
            if (_inputLogic.KeyState[i] != true)
                continue;
            
            DrawTexture(
                _skin.KeyPressedImage[i],
                new Vector2(
                    _skin.Position.x + (i * _skin.NoteSize.x),
                    _skin.Position.y
                )
            );
        }
    }
}