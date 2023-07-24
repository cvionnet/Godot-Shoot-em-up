using System.Collections.Generic;
using BulletBallet.scenes.classes;

namespace BulletBallet;

/// <summary>
/// HOW TO :
/// - In Godot, add this file as Autoload :  Project > Project Settings > AutoLoad    (Node name = GameManager)
/// - In Nucleus.cs: add a new property of GameManager
/// - In GameBrain.cs: connect to node  GetNode<GameManager>("/root/GameManager");
/// </summary>
public partial class GameManager : Node
{
    // GAME
    public List<Level> LevelList { get; set; } = new List<Level>();
    public Level LevelActive { get; set; }
    
    // PLAYER
    public float ZoomLevelGame { get; } = 0.5f;
    public float ZoomLevelZoomOut { get; } = 1.0f;

    // ITEM - To store all items actions
    public enum ItemsActionList { CHARACTER_FASTER, OTHER_CHARACTERS_SLOWER, ALL_CHARACTERS_FASTER }
    public enum ItemsSendTo { CHARACTER, OTHER_CHARACTERS, ALL_CHARACTERS }
}