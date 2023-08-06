using BulletBallet.utils.NucleusFW.SpawnFactory;

namespace BulletBallet.actors.characters;

/// <summary>
/// Responsible for :
/// - adding Players to the scene (on a safe platform)
/// - adding random PNJs to the scene (on a safe platform)
/// </summary>
public partial class CharactersBrain : Node2D
{
    //! How to add a new character : see file "README.drawio"  (How to)

    private Spawn_Factory _spawnPlayers;
    //private Spawn_Factory _spawnPNJs;

    //private List<Pnj> _listPNJs = new List<Pnj>();     // TODO : performance => use an ARRAY ?  (et ajouter l'id dans les propriétés du ItemGeneric)

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        _spawnPlayers = GetNode<Spawn_Factory>("Spawn_Player");
        //_spawnPNJs = GetNode<Spawn_Factory>("Spawn_Pnj");

        Initialize_CharactersBrain();
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_CharactersBrain()
    {
        Generate_Player(1);
        //Generate_PNJ();
    }

    /// <summary>
    /// Spawn PNJ on the map
    /// </summary>
    // private void Generate_PNJ()
    // {
    //     if(_spawnPNJs.Load_NewScene("res://src/actors/pnj/Pnj.tscn"))
    //     {
    //         for (int i = 0; i < Nucleus.GameManager.LevelActive.PnjNumberToDisplay; i++)
    //         {
    //             _spawnPNJs.Add_Instance<pnj.Pnj>(GetNode("Spawn_Pnj"), new Vector2(100,100), 0, "Pnj");
    //             //_listPNJs.Add(instance);
    //         }
    //     }
    // }

    /// <summary>
    /// Spawn a Player on the map
    /// </summary>
    private void Generate_Player(int numberofPlayers)
    {
        if(_spawnPlayers.Load_NewScene("res://src/actors/player/Player.tscn"))
        {
            for (int i = 0; i < numberofPlayers; i++)
            {
                _spawnPlayers.Add_Instance<player.Player>(GetNode("Spawn_Player"), new Vector2(100,100), 0, "Player");
                //_listPlayers.Add(instance);
            }
        }
    }

    #endregion
}