namespace BulletBallet;

/// <summary>
/// HOW TO :
/// - In Godot, add this file as Autoload :  Project > Project Settings > AutoLoad    (Node name = GameManager)
/// - In Nucleus.cs: add a new property of SignalManager
/// - In GameBrain.cs: connect to node  GetNode<SignalManager>("/root/SignalManager");
/// - To connect/emit signals: use Nucleus.SignalManager.Connect / .Emit
/// </summary>
public partial class SignalManager: Node
{
    [Signal] public delegate void Generic_TransitionSceneEventHandler(string nextScene);

    [Signal] public delegate void SceneTransition_AnimationFinishedEventHandler();

    [Signal] public delegate void UiPlayer_GameBrain_LevelTimeoutEventHandler();

    [Signal] public delegate void Player_UiPlayer_UpdatedScoreEventHandler(int score);

    [Signal] public delegate void ItemGeneric_ItemBrain_TouchedEventHandler(string itemName);
}
