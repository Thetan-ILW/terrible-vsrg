using Godot;
using System;

public class TimeLogic : Node
{
    private Audio _audio;

    public float CurrentTime { get; private set; }
    private float _addTime = 1000;
    private float _timeRate = 1;
    private bool _isPaused = false;

    public TimeLogic(Audio audio, float time, float timeRate)
    {
        CurrentTime = time;
        _audio = audio;
        _timeRate = timeRate;
    }

    public void Process(float deltaTime)
    {
        CurrentTime += deltaTime * _addTime;
    }

    public void SetPause()
    {
        if (!_isPaused)
        {
            _addTime = 0;
            _isPaused = true;
        }
        else
        {
            _addTime = 1000;
            CurrentTime -= 750; // legal cheats :)
            _isPaused = false;
        }
    }
}
