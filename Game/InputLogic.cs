using Godot;
using System;

public abstract class InputLogic // Реагирует на нажатия игрока
{
    protected int[] _inputMap;
    public bool[] KeyState;

    public float CurrentTime = 0;
    public int NextExistingNote = 0;

    protected ScoreSystem _scoreSystem;

    public void ChangeKeyState(int key, bool state)
    {
        KeyState[key] = state;
    }

    public void Process(InputEventKey keyEvent, ref Note[] notes)
    {
        if (keyEvent.Pressed)
        {
            int column = 0;
            foreach (int key in _inputMap)
            {
                if (keyEvent.Scancode == key)
                {
                    if (KeyState[column] == true)
                        return; // Спасёт от даблсетаперов, и хуйни как при зажатии кнопки при печати, но самое по себе решение тупое блять как и я
                    
                    ChangeKeyState(column, true);
                    ProcessPress(column, CurrentTime, ref notes);
                    return;
                }
                column++;
            }
        }

        if (!keyEvent.Pressed) // Тоесть release
        {
            int column = 0;
            foreach (int key in _inputMap)
            {
                if (keyEvent.Scancode == key)
                {
                    ChangeKeyState(column, false);
                    return;
                }
                column++;
            }
        }
    
    }

    public abstract void ProcessPress(int column, float time, ref Note[] note);
}

public class EarlyInputLogic : InputLogic
{
    // Считаем самую раннюю ноту попадающую под зону попадания
    public EarlyInputLogic(int inputMode, ScoreSystem scoreSystem, int[] inputMap)
    {
        this._inputMap = inputMap;
        this._scoreSystem = scoreSystem;
        KeyState = new bool[inputMode];
    }

    public override void ProcessPress(int column, float time, ref Note[] notes)
    {
        float deltaTime = 0;
        float earlyTime = 0;
        float lateTime = 0;

        for (int i = NextExistingNote; i != notes.GetLength(0) ; i++)
        {
            ref Note n = ref notes[i];

            if (n.column != column)
                continue;

            earlyTime = n.time - 155;
            lateTime = n.time + 155;

            if (time < earlyTime)
                return;    

            if (time > earlyTime && time < lateTime)
            {
                deltaTime = n.time - time;
                _scoreSystem.ProcessHit(deltaTime);
                n.isExist = false;
                NextExistingNote = i + 1;
                return;
            }
        }
    }
}

public class NearestInputLogic : InputLogic
{
    // Ищем раннюю и позднюю ноту относительно текущего времени
    // И считаем только ту которая ближе к текущему времени
    public NearestInputLogic(int inputMode, ScoreSystem scoreSystem, int[] inputMap)
    {
        this._inputMap = inputMap;
        this._scoreSystem = scoreSystem;
        KeyState = new bool[inputMode];
    }

    public override void ProcessPress(int column, float time, ref Note[] notes)
    {
       throw new NotImplementedException();
    }
}