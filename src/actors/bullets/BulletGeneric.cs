using System;
using System.Reflection;
using BulletBallet.actors.bullets.classes;

namespace BulletBallet.actors.items;

/// <summary>
/// Responsible for :
/// - store item properties
/// - load item sprite
/// - kill item when timer ends
/// - emitting particles before dying
/// - sending message to other scenes (characters ...)
/// </summary>
public partial class BulletGeneric : Area2D
{
    public Bullet BulletProperties { get; private set; }

    // private Timer _timerTTL;
    // private Sprite2D _spriteGlowCircle;
    private GpuParticles2D _particleWhenPicked;

    // private AnimatedSprite2D instanceSprite;
    private Sprite2D instanceSprite;

    //*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        // _timerTTL = GetNode<Timer>("TimerTTL");
        // _spriteGlowCircle = GetNode<Sprite2D>("Glow_circle");
        _particleWhenPicked = GetNode<GpuParticles2D>("ParticleWhenPicked");

        Connect("body_shape_entered", new Callable(this, nameof(onBodyCharacterShapeEntered)));
        // _timerTTL.Connect("timeout", new Callable(this, nameof(onDestroyBullet_Timeout)));

        Initialize_BulletGeneric();
    }

    #endregion

    //*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    /// <summary>
    /// When a character collides with a bullet, send information to BulletBrain
    /// </summary>
    private void onBodyCharacterShapeEntered(int body_id, Node body, int body_shape, int local_shape)
    {
        // bool actionSend = false;
        //
        // if(body != null)
        // {
        //     switch (BulletProperties.SendTo)
        //     {
        //         case GameManager.ItemsSendTo.CHARACTER:
        //             // Call a method from a specific character
        //             if (body.IsInGroup("Player"))
        //             {
        //                 ((characters.player.Player)body).Item_Action(BulletProperties, body.Name);
        //                 actionSend = true;
        //             }
        //             else if (body.IsInGroup("Pnj"))
        //             {
        //                 ((characters.pnj.Pnj)body).Item_Action(BulletProperties, body.Name);
        //                 actionSend = true;
        //             }
        //
        //             break;
        //         case GameManager.ItemsSendTo.OTHER_CHARACTERS:
        //         case GameManager.ItemsSendTo.ALL_CHARACTERS:
        //             // Call a method from all characters
        //             GetTree().CallGroup("Player", "Item_Action", BulletProperties, body.Name);
        //             GetTree().CallGroup("Pnj", "Item_Action", BulletProperties, body.Name);
        //             actionSend = true;
        //
        //             break;
        //     }
        //
        //     if (actionSend)
        //         PickedUp_Bullet();
        // }
    }

    /// <summary>
    /// Destroy the node when time is out
    /// </summary>
    // private void onDestroyBullet_Timeout()
    // {
    //     Destroy_Bullet();
    // }

    #endregion

    //*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_BulletGeneric()
    { }

    /// <summary>
    /// When the bullet has been picked up (hide the sprite and display the particles)
    /// </summary>
    // private async void PickedUp_Bullet()
    // {
    //     instanceSprite.Visible = false;
    //     _particleWhenPicked.Emitting = true;
    //
    //     // Create a timer of 1 sec then continue to execute the code after  (process is not blocked)
    //     await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
    //
    //     Destroy_Bullet();
    // }

    /// <summary>
    /// Delete the bullet
    /// </summary>
    // private void Destroy_Bullet()
    // {
    //     Nucleus.SignalManager.EmitSignal(SignalManager.SignalName.ItemGeneric_ItemBrain_Touched, Name);    // to delete the bullet from the main list
    //     CallDeferred("queue_free");
    // }

    /// <summary>
    /// Create a new bullet sprite instance
    /// </summary>
    private void Add_BulletSprite()
    {
        try
        {
            PackedScene scene = ResourceLoader.Load<PackedScene>(BulletProperties.SpritePath);
            if (scene != null)
            {
                instanceSprite = (Sprite2D)scene.Instantiate();
                // _spriteGlowCircle.AddSibling(instanceSprite);         // to set the sprite position in the node tree (eg : to allow particles to be above the sprite)
            }
            else
            {
                throw new NullReferenceException();
            }
        }
        catch (Exception ex)
        {
            Nucleus.Logs.Error($"Error while loading Path = {BulletProperties.SpritePath}", ex, GetType().Name, MethodBase.GetCurrentMethod().Name);
        }
    }

    /// <summary>
    /// Create the time to live timer
    /// </summary>
    // private void Start_TTLTimer()
    // {
    //     _timerTTL.WaitTime = BulletProperties.Ttl;
    //     _timerTTL.Start();
    // }

    /// <summary>
    /// Initialize bullet properties + load sprite / start TTL timer   (called from BulletsBrain)
    /// </summary>
    public void Initialize_BulletProperties(Bullet bulletProperties)
    {
        BulletProperties = bulletProperties;
        Name = "Bullet";  //ItemProperties.ActionName;
        AddToGroup("Bullets");

        Add_BulletSprite();
        // Start_TTLTimer();
    }

    #endregion
}