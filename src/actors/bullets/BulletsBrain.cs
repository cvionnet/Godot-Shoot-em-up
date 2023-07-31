using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BulletBallet.actors.bullets.classes;
using BulletBallet.actors.items;
using BulletBallet.utils.NucleusFW.SpawnFactory;

namespace BulletBallet.actors.bullets;

/// <summary>
/// Responsible for :
/// - initialize properties for a new random bullets
/// - adding random bullets to the scene
/// </summary>
public partial class BulletsBrain : Node2D
{
    //! How to add a new bullet : see file "README.drawio"  (How to)

    [Export]
    private int _number;
    
    [Export] private PackedScene BulletGenericScene;
    [Export] private float MinTimerNewBullet = 10.0f;
    [Export] private float MaxTimerNewBullet = 30.0f;

    private Spawn_Factory _spawnBullets;
    private Timer _timerNewBullet;

    private List<BulletGeneric> _listBullets = new List<BulletGeneric>();     // TODO : performance => use an ARRAY ?  (et ajouter l'id dans les propriétés du BulletGeneric)
    private List<Bullet> _listUniqueBullets = new List<Bullet>();           // bullets that appear only once during the game
    private BulletGeneric _bulletTouched = new BulletGeneric();

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        _spawnBullets = GetNode<Spawn_Factory>("Spawn_Bullets");
        _timerNewBullet = GetNode<Timer>("TimerNewBullet");

        _timerNewBullet.Connect("timeout", new Callable(this, nameof(onNewBulletTimer_Timeout)));

        Nucleus.SignalManager.Connect(SignalManager.SignalName.BulletGeneric_BulletBrain_Touched, new Callable(this, nameof(onBullet_Touched)));

        Initialize_BulletsBrain();
    }

    // Use to add warning in the Editor   (must add the [Tool] attribute on the class)
    public override string[] _GetConfigurationWarnings()
    {
        return (BulletGenericScene == null) ? new [] { "The object BulletsGenericScene must not be empty !" } : new [] { "" };
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    /// <summary>
    /// Time to create a new bullet and start a new timer
    /// </summary>
    private void onNewBulletTimer_Timeout()
    {
        Create_Bullet();
        Initialize_TimerNewBullet();
    }

    /// <summary>
    /// When a player / pnj has collided with an bullet, send information to final receiver
    /// </summary>
    /// <param name="bulletName">The name of the bullet node</param>
    private void onBullet_Touched(string bulletName)
    {
        // Get the touched bullet properties
        _bulletTouched = _listBullets.Find(i => i.Name == bulletName);

        _listBullets.Remove(_bulletTouched);
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_BulletsBrain()
    {
        _spawnBullets.Load_NewScene(BulletGenericScene.ResourcePath);

        Initialize_TimerNewBullet();
    }

    /// <summary>
    /// Create a new item instance
    /// </summary>
    private void Create_Bullet()
    {
        // Select a random item
        Bullet bulletProperties = new classes.Bullet();
        
        // TODO: add a list of figures (star, wall, sinusoidal...) + type of bullets (= default bullet N&B, and modulate from 4 different colors)
        var random_bullet = (GameManager.ItemsActionList) Nucleus_Maths.Rnd.RandiRange(0, Enum.GetNames(typeof(GameManager.ItemsActionList)).Length-1);

        // Apply item properties
        switch (random_bullet)
        {
            case GameManager.ItemsActionList.CHARACTER_FASTER:
                bulletProperties.SpritePath = "res://src/actors/items/itemSprites/ItemSprite_Player_GainSpeed.tscn";
                bulletProperties.SendTo = GameManager.ItemsSendTo.CHARACTER;
                bulletProperties.ActionName = random_bullet;
                bulletProperties.ActionDuration = 3.0f;
                bulletProperties.MaxVisibleInstance = 2;
                bulletProperties.Ttl = 10.0f;
                bulletProperties.OptionalValue = 1.5f;    // the percent to apply on MaxSpeed's character
                break;
            case GameManager.ItemsActionList.OTHER_CHARACTERS_SLOWER:
                bulletProperties.SpritePath = "res://src/actors/items/itemSprites/ItemSprite_OtherPlayers_LooseSpeed.tscn";
                bulletProperties.SendTo = GameManager.ItemsSendTo.OTHER_CHARACTERS;
                bulletProperties.ActionName = random_bullet;
                bulletProperties.ActionDuration = 5.0f;
                bulletProperties.MaxVisibleInstance = 2;
                bulletProperties.Ttl = 10.0f;
                bulletProperties.OptionalValue = 0.5f;    // the percent to apply on MaxSpeed's characters
                break;
            case GameManager.ItemsActionList.ALL_CHARACTERS_FASTER:
                bulletProperties.SpritePath = "res://src/actors/items/itemSprites/ItemSprite_AllPlayers_GainSpeed.tscn";
                bulletProperties.SendTo = GameManager.ItemsSendTo.ALL_CHARACTERS;
                bulletProperties.ActionName = random_bullet;
                bulletProperties.ActionDuration = 5.0f;
                bulletProperties.MaxVisibleInstance = 2;
                bulletProperties.Ttl = 10.0f;
                bulletProperties.OptionalValue = 1.5f;    // the percent to apply on MaxSpeed's characters
                break;
        }

        // Only load the item if it is unique and already in the game AND its max instance number is not reached
        if (!_listUniqueBullets.Any(i => i.ActionName == bulletProperties.ActionName)
            && _listBullets.Count(i => i.BulletProperties.ActionName == random_bullet) < bulletProperties.MaxVisibleInstance)
        {
            // Initialize the item
            if (bulletProperties.SpritePath != null)
            {
                Vector2 newPosition = new Vector2(Nucleus_Maths.Rnd.RandfRange(40.0f, Nucleus.ScreenWidth-40.0f), Nucleus_Maths.Rnd.RandfRange(40.0f, Nucleus.ScreenHeight-40.0f));
                BulletGeneric instance = _spawnBullets.Add_Instance<BulletGeneric>(null, newPosition);
                _listBullets.Add(instance);
                if (bulletProperties.UniqueInstance)
                    _listUniqueBullets.Add(bulletProperties);

                instance.Initialize_BulletProperties(bulletProperties);
            }
            else
            {
                Nucleus.Logs.Error($"Error while loading Item Action '{random_bullet}' (path is null))", new NullReferenceException(), GetType().Name, MethodBase.GetCurrentMethod().Name);
            }
        }
        else
        {
            bulletProperties = null;
            //GD.Print($"{random_item} max number");
        }
    }

    private void Initialize_TimerNewBullet()
    {
        _timerNewBullet.WaitTime = Nucleus_Maths.Rnd.RandfRange(MinTimerNewBullet, MaxTimerNewBullet);
        _timerNewBullet.Start();
    }

    #endregion
}