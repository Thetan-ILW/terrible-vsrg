using Godot;
using System;

public class TimeLogic : Node
{
    private Audio _audio;

    public float CurrentTime { get; private set; }
    private const float TimeMultiply = 1000;
    private const float PauseTimeMultiply = 0;
    private float _addTime = TimeMultiply;
    private float _timeRate = 1;

    private bool _isPaused = false;
    private float _lastPauseTime = 0;
    private float _afterPauseTimeDecrease = 0;
    private float _pauseCooldown = 0;

    public TimeLogic(Audio audio, float time, float timeRate, float afterPauseTimeDecrease, float pauseCooldown)
    {
        CurrentTime = time;
        _audio = audio;
        _timeRate = timeRate;

        _lastPauseTime = time - pauseCooldown;
        _afterPauseTimeDecrease = afterPauseTimeDecrease;
        _pauseCooldown = pauseCooldown;
    }

    public void Process(float deltaTime)
    {
        CurrentTime += deltaTime * _addTime;
    }

    public void SetPause()
    {
        if (!_isPaused && CurrentTime > _lastPauseTime + _pauseCooldown)
        {
            _addTime = PauseTimeMultiply;
            _isPaused = true;
        }
        else if (_isPaused)
        {
            _addTime = TimeMultiply;

            if (CurrentTime - _afterPauseTimeDecrease < 0f)
                CurrentTime = 0f;
            else
                CurrentTime += _afterPauseTimeDecrease; // legal cheats :)
            
            _isPaused = false;
            _lastPauseTime = CurrentTime;
        }
    }
}
