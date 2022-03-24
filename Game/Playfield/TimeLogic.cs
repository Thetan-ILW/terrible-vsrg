using System.Diagnostics;

public class TimeLogic
{
    private Audio _audio;

    public float CurrentTime { get; private set; }

    private const float _timeMultiply = 1000;
    private const float _pauseTimeMultiply = 0;
    private float _addTime = _timeMultiply;
    private float _timeRate = 1;

    private bool _isPaused = false;
    private float _lastPauseTime = 0;
    private float _afterPauseTimeDecrease = 0;
    private float _pauseCooldown = 0;

    private delegate void ProcessType();
    private ProcessType _processType;

    private Stopwatch _timer;
    private float _decreaseTime;

    public TimeLogic(Audio audio, float prepareTime, float timeRate, float afterPauseTimeDecrease, float pauseCooldown)
    {
        CurrentTime = -prepareTime;
        _decreaseTime = prepareTime;
        _audio = audio;
        _timeRate = timeRate;

        _lastPauseTime = prepareTime - pauseCooldown;
        _afterPauseTimeDecrease = afterPauseTimeDecrease;
        _pauseCooldown = pauseCooldown;

        _timer = new Stopwatch();
        _processType = ProcessBeforeTimer;
    }

    public void Process(float deltaTime)
    {
        _processType();
    }

    private void RegularProcess()
    {
        ProcessTime();
    }

    private void ProcessBeforeAudio()
    {
        ProcessTime();
        if(CurrentTime > 0)
        {
            _audio.Play(CurrentTime / 1000);
            _audio.SetPlaybackSpeed(_timeRate);
            _processType = RegularProcess;
        }
    }

    private void ProcessBeforeTimer()
    {
        _timer.Start();
        _processType = ProcessBeforeAudio;
    }

    private void ProcessTime()
    {
        CurrentTime = (float)(_timer.Elapsed.TotalMilliseconds * _timeRate) - (_decreaseTime * _timeRate);
        _audio.CurrentTime = CurrentTime;
    }

    public void SetPause()
    {
        if (!_isPaused && CurrentTime > _lastPauseTime + _pauseCooldown)
        {
            _timer.Stop();
            _audio.StreamPaused = true;
            _isPaused = true;
        }
        else if (_isPaused)
        {
            if (CurrentTime - _afterPauseTimeDecrease < 0f)
                _decreaseTime = (float)_timer.Elapsed.TotalMilliseconds;
            else
                _decreaseTime += _afterPauseTimeDecrease; // legal cheats :)
            
            CurrentTime = (float)_timer.Elapsed.TotalMilliseconds - _decreaseTime;
            _isPaused = false;
            _timer.Start();
            _audio.Play(CurrentTime / 1000);
            _audio.StreamPaused = false;
            _lastPauseTime = CurrentTime;
        }
    }
}
