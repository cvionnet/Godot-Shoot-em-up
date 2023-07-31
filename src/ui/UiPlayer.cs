namespace BulletBallet.ui;

/// <summary>
/// Responsible for :
/// - displaying score
/// </summary>
public partial class UiPlayer : CanvasLayer
{
    private Label _score;
    private Label _time;
    private Timer _timerTime;

    private int _timeLeft;

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        _score = GetNode<Label>("Control/Score");
        _time = GetNode<Label>("Control/Time");
        _timerTime = GetNode<Timer>("TimerTime");

        _timerTime.Connect("timeout", new Callable(this, nameof(onTimerTime_Timeout)));
        Nucleus.SignalManager.Connect(SignalManager.SignalName.Player_UiPlayer_UpdatedScore, new Callable(this, nameof(onPlayer_UpdateScore)));              // emitted from Move_Player

        Initialize_UI_Player();
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    /// <summary>
    /// When the Game send a signal to update the score
    /// </summary>
    /// <param name="score">The score to display</param>
    private void onPlayer_UpdateScore(int score) => _score.Text = $"Score : {score}";

    private void onTimerTime_Timeout()
    {
        if (_timeLeft > 0)
        {
            _timeLeft--;
            _time.Text = _timeLeft.ToString();
        }
        else
        {
            _timerTime.Stop();
            Nucleus.SignalManager.EmitSignal(SignalManager.SignalName.UiPlayer_GameBrain_LevelTimeout);
        }
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_UI_Player()
    {
        _timeLeft = Nucleus.GameManager.LevelActive.RoundTime;
        _time.Text = _timeLeft.ToString();

        _timerTime.Start();
    }

    #endregion
}