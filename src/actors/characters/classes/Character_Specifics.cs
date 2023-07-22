namespace BulletBallet.actors.characters.classes;

/// <summary>
/// Extends the Character class with properties and methods specific to this game
/// </summary>
public partial class Character
{
    #region CHARACTER

    /// <summary>
    /// Update the score and send a signal to update the UI
    /// </summary>
    /// <param name="point">Can be positif or negative</param>
    public void Update_Score(int point)
    {
        // To avoid having a negative score
        if (point < 0 && (Score + point < 0))
        {
            Score = 0;
            return;
        }

        Score += point;
        Nucleus.SignalManager.EmitSignal(SignalManager.SignalName.Player_UiPlayer_UpdatedScore, Score);
    }

    #endregion

    #region ACTIONS

    // Store the active item action (null if no action) and its optional value
    private GameManager.ItemsActionList? _itemActionActive = null;
    private float _itemActionOptionalValue;

    /// <summary>
    /// When an item has been touched and send an action to a character
    /// </summary>
    /// <param name="itemProperties">All properties of the item touched</param>
    /// <param name="itemTouchedBy">The name of the node that hits the item</param>
    /// <param name="timerActionDuration">The Timer used in the character scene to set how much time the action will be active</param>
    public void ActionFrom_Item(items.classes.Item itemProperties, string itemTouchedBy, Timer timerActionDuration)
    {
        bool actionToExecute = false;

        // Who to apply the item effect ?
        switch (itemProperties.SendTo)
        {
            case GameManager.ItemsSendTo.CHARACTER:
                if (_itemActionActive == null)
                {
                    // What to apply ?
                    switch (itemProperties.ActionName)
                    {
                        case GameManager.ItemsActionList.CHARACTER_FASTER:
                            actionToExecute = true;
                            MaxSpeed *= itemProperties.OptionalValue;
                            break;
                    }
                }

                break;
            case GameManager.ItemsSendTo.OTHER_CHARACTERS:
                if (itemTouchedBy != Name && _itemActionActive == null)
                {
                    // What to apply ?
                    switch (itemProperties.ActionName)
                    {
                        case GameManager.ItemsActionList.OTHER_CHARACTERS_SLOWER:
                            actionToExecute = true;
//                            Steering.Speed = MaxSpeed.x - (MaxSpeed.x * 0.3f);
                            MaxSpeed *= itemProperties.OptionalValue;
                            break;
                    }
                }

                break;
            case GameManager.ItemsSendTo.ALL_CHARACTERS:
                if (_itemActionActive == null)
                {
                    // What to apply ?
                    switch (itemProperties.ActionName)
                    {
                        case GameManager.ItemsActionList.ALL_CHARACTERS_FASTER:
                            actionToExecute = true;
                            MaxSpeed *= itemProperties.OptionalValue;
                            break;
                    }
                }

                break;
        }

        // Save active state properties and start timer if an action has been founded
        if (actionToExecute)
        {
            _itemActionActive = itemProperties.ActionName;
            _itemActionOptionalValue = itemProperties.OptionalValue;

            timerActionDuration.WaitTime = itemProperties.ActionDuration;
            timerActionDuration.Start();

            //GD.Print("=> " + Name + " // ActionName: " + pItemProperties.ActionName + " // ActionDuration:" + pItemProperties.ActionDuration);
        }
    }

    /// <summary>
    /// Reset the active action (called by the ActionDuration timer on characters)
    /// </summary>
    public void ActionEnd_Item()
    {
        if (_itemActionActive != null)
        {
            switch (_itemActionActive)
            {
                case GameManager.ItemsActionList.CHARACTER_FASTER:
                case GameManager.ItemsActionList.OTHER_CHARACTERS_SLOWER:
                case GameManager.ItemsActionList.ALL_CHARACTERS_FASTER:
                    //GD.Print($"=> {Name} - End action {_itemActionActive}");
                    _itemActionActive = null;
                    MaxSpeed *= 1/_itemActionOptionalValue;
                    break;
            }
        }
    }

    #endregion

    #region DASH MOVEMENTS

    public float Dash_SpeedBoost { get; set; }      // default percentage to boost the character maxspeed
    public bool IsDashing { get; set; } = false;

    // Save the max speed the character can raise (a dash will overcome this value)
    public Vector2 MaxSpeed_Default {
        get => _maxSpeed_Default;
        set {
            _maxSpeed_Default = value;
            MaxSpeed = _maxSpeed_Default;
        }
    }
    private Vector2 _maxSpeed_Default;

    #endregion

}