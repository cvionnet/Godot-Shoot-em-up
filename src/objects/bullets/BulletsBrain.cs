using System.Collections.Generic;
using System.Reflection;
using BulletBallet.objects.bullets.classes;
using BulletBallet.utils.NucleusFW.SpawnFactory;
using Timer = Godot.Timer;

namespace BulletBallet.objects.bullets;

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
        Create_Attack();
        //Initialize_TimerNewBullet();
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

        //Initialize_TimerNewBullet();

        Create_Attack();
    }

    /// <summary>
    /// Create a new attack
    /// </summary>
    private void Create_Attack()
    {
        // Generate random bullets and attack
        Bullet bulletProperties = new Bullet();
        bulletProperties.SpritePath = Generate_RandomBullet();

        Attack attackProperties = new Attack();
        Generate_RandomAttack(ref attackProperties);

        Spawn_SpiralPattern(bulletProperties, attackProperties);

        //TODO: IMPLEMENT ALGO FOR OTHER ATTACK
        //TODO: object pooling  (pool XX  or  pool of a certain size (say, 100 bullets), and then create them on the fly

    }

    private string Generate_RandomBullet()
    {
        var randomBullet = (GameManager.BulletTypeList) Nucleus_Maths.Rnd.RandiRange(0, Enum.GetNames(typeof(GameManager.BulletTypeList)).Length-1);
        return $"res://src/objects/bullets/bulletSprites/Bullet{randomBullet.GetHashCode() + 1}.tscn";
    }

    private void Generate_RandomAttack(ref Attack attackProperties)
    {
        // TODO: add a list of figures (star, wall, sinusoidal...) + type of bullets (= default bullet N&B, and modulate from 4 different colors)
        var randomAttack = (GameManager.BulletAttackList) Nucleus_Maths.Rnd.RandiRange(0, Enum.GetNames(typeof(GameManager.BulletAttackList)).Length-1);

        switch (randomAttack)
        {
            case GameManager.BulletAttackList.SPIRAL:
                attackProperties.NumberOfBullets = 20;
                attackProperties.Speed = 300.0f;
                attackProperties.AngularVelocity = 2.0f;
                attackProperties.CurrentAngle = 0.0f;                
                //bulletProperties.SendTo = GameManager.ItemsSendTo.CHARACTER;
                //bulletProperties.OptionalValue = 1.5f;    // the percent to apply on MaxSpeed's character
                break;
        }        
    }

    private void Spawn_SpiralPattern(Bullet bulletProperties, Attack attackProperties)
    {
        float angleIncrement = 360.0f / attackProperties.NumberOfBullets;

        for (int i = 0; i < attackProperties.NumberOfBullets; i++)
        {
            float angle = (attackProperties.CurrentAngle + angleIncrement * i) % 360.0f;
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.DegToRad(angle)), Mathf.Sin(Mathf.DegToRad(angle)));
            Vector2 velocity = direction * attackProperties.Speed;

            bulletProperties.Velocity = velocity;
            bulletProperties.CurrentAngle = attackProperties.CurrentAngle;
            bulletProperties.AngularVelocity = attackProperties.AngularVelocity;
            Spawn_Bullet(bulletProperties);
        }        
    }

    private void Spawn_Bullet(Bullet bulletProperties)
    {
        if (bulletProperties.SpritePath != null)
        {
            //Vector2 newPosition = new Vector2(Nucleus_Maths.Rnd.RandfRange(40.0f, Nucleus.ScreenWidth-40.0f), Nucleus_Maths.Rnd.RandfRange(40.0f, Nucleus.ScreenHeight-40.0f));
            BulletGeneric instance = _spawnBullets.Add_Instance<BulletGeneric>(null, Position);
            _listBullets.Add(instance);
            instance.Initialize_BulletProperties(bulletProperties);
        }
        else
        {
            Nucleus.Logs.Error($"Error while loading Bullet Action", new NullReferenceException(), GetType().Name, MethodBase.GetCurrentMethod().Name);
        }
    }
    
    
    // private void Initialize_TimerNewBullet()
    // {
    //     _timerNewBullet.WaitTime = Nucleus_Maths.Rnd.RandfRange(MinTimerNewBullet, MaxTimerNewBullet);
    //     _timerNewBullet.Start();
    // }

    #endregion
}