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

    private System.Threading.Thread _audioStartTimer;
    private Stopwatch _timer;
    private float _decreaseTime;

    public TimeLogic(Audio audio, float prepareTime, float timeRate, float afterPauseTimeDecrease, float pauseCooldown)
    {
        CurrentTime = 0f;
        _decreaseTime = prepareTime;
        _audio = audio;
        _timeRate = timeRate;

        _lastPauseTime = prepareTime - pauseCooldown;
        _afterPauseTimeDecrease = afterPauseTimeDecrease;
        _pauseCooldown = pauseCooldown;
        _audioStartTimer = new System.Threading.Thread(AudioStartTimer);
        _audioStartTimer.Start();
        _timer = new Stopwatch();
        _timer.Start();
    }

    public void Process(float deltaTime)
    {
        CurrentTime = (float)_timer.Elapsed.TotalMilliseconds - _decreaseTime;
        _audio.CurrentTime = CurrentTime;
    }

    public void AudioStartTimer()
    {
        for (;;)
        {
            if(CurrentTime > 0)
            {
                _audio.Play(CurrentTime / 1000);
                return;
            }
        }
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
