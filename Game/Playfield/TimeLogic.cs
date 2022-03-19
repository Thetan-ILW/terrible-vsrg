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

    public TimeLogic(Audio audio, float time, float timeRate, float afterPauseTimeDecrease, float pauseCooldown)
    {
        CurrentTime = time;
        _audio = audio;
        _timeRate = timeRate;

        _lastPauseTime = time - pauseCooldown;
        _afterPauseTimeDecrease = afterPauseTimeDecrease;
        _pauseCooldown = pauseCooldown;
        _audioStartTimer = new System.Threading.Thread(AudioStartTimer);
        _audioStartTimer.Start();
    }

    public void Process(float deltaTime)
    {
        CurrentTime += deltaTime * _addTime;
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
            _addTime = _pauseTimeMultiply;
            _audio.StreamPaused = true;
            _isPaused = true;
        }
        else if (_isPaused)
        {
            _addTime = _timeMultiply;

            if (CurrentTime - _afterPauseTimeDecrease < 0f)
                CurrentTime = 0f;
            else
                CurrentTime += _afterPauseTimeDecrease; // legal cheats :)
            
            _isPaused = false;
            _audio.Play(CurrentTime / 1000);
            _audio.StreamPaused = false;
            _lastPauseTime = CurrentTime;
        }
    }
}
