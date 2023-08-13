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
        Generate_RandomAttack();
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

        Generate_RandomAttack();
    }

    /// <summary>
    /// Create a new attack
    /// </summary>
    private void Generate_RandomAttack()
    {
        Attack attackProperties = new Attack();
        
        //TODO: IMPLEMENT ALGO FOR OTHER ATTACK
        //TODO: object pooling  (pool XX  or  pool of a certain size (say, 100 bullets), and then create them on the fly

        // TODO: add a list of figures (star, wall, sinusoidal...) + type of bullets (= default bullet N&B, and modulate from 4 different colors)
        // TODO: add variations (oscillations, bursts, waveforms...) 
        
        var randomAttack = (GameManager.BulletAttackList) Nucleus_Maths.Rnd.RandiRange(0, Enum.GetNames(typeof(GameManager.BulletAttackList)).Length-1);
        //var randomAttack = GameManager.BulletAttackList.SNAIL;

        switch (randomAttack)
        {
            case GameManager.BulletAttackList.SPIRAL:
                attackProperties.NumberOfBullets = 30;
                attackProperties.Speed = 100.0f;
                attackProperties.AngularVelocity = 2.0f;
                attackProperties.CurrentAngle = 0.0f;                
                //bulletProperties.SendTo = GameManager.ItemsSendTo.CHARACTER;
                //bulletProperties.OptionalValue = 1.5f;    // the percent to apply on MaxSpeed's character
                Spawn_SpiralPattern(attackProperties);
                break;
            case GameManager.BulletAttackList.WAVE:
                attackProperties.NumberOfBullets = 30;
                attackProperties.Speed = 50.0f;
                attackProperties.Length = 30.0f;
                attackProperties.Amplitude = 100.0f;                
                Spawn_WavePattern(attackProperties);
                break;
            case GameManager.BulletAttackList.FLOWER:
                attackProperties.Length = 3.0f;
                attackProperties.Speed = 3.0f;
                Spawn_FlowerPattern(attackProperties);
                break;
            case GameManager.BulletAttackList.GRID:
                attackProperties.Rows = 5;
                attackProperties.Columns = 5;
                attackProperties.Spacing = 20.0f;                
                attackProperties.Speed = 1.0f;
                Spawn_GridPattern(attackProperties);
                break;            
            case GameManager.BulletAttackList.SNAIL:
                attackProperties.NumberOfBullets = 30;
                attackProperties.Speed = 5.0f;
                attackProperties.StartAngle = 0.0f;
                attackProperties.AngleIncrement = 5.0f;
                attackProperties.StartRadius = 0.0f;
                attackProperties.RadiusIncrement = 2.0f;
                Spawn_SnailPattern(attackProperties);
                break;
        }        
    }
    
    // private void Initialize_Attack(Attack attackProperties)
    // {
    //     // Generate random bullet type
    //     Bullet bulletProperties = new Bullet();
    //     bulletProperties.SpritePath = Generate_RandomBullet();
    //     
    //     
    //     
    //     float angleIncrement = 360.0f / attackProperties.NumberOfBullets;
    //
    //     for (int i = 0; i < attackProperties.NumberOfBullets; i++)
    //     {
    //         float angle = (attackProperties.CurrentAngle + angleIncrement * i) % 360.0f;
    //         Vector2 direction = new Vector2(Mathf.Cos(Mathf.DegToRad(angle)), Mathf.Sin(Mathf.DegToRad(angle)));
    //         Vector2 velocity = direction * attackProperties.Speed;
    //
    //         bulletProperties.Velocity = velocity;
    //         bulletProperties.CurrentAngle = attackProperties.CurrentAngle;
    //         bulletProperties.AngularVelocity = attackProperties.AngularVelocity;
    //         bulletProperties.InitialPosition = this.GlobalPosition;
    //         Spawn_Bullet(bulletProperties);
    //     }
    // }
    
    private void Spawn_SpiralPattern(Attack attackProperties)
    {
        // Generate random bullet type
        Bullet bulletProperties = new Bullet();
        bulletProperties.SpritePath = Generate_RandomBullet();        
        
        float angleIncrement = 360.0f / attackProperties.NumberOfBullets;

        for (int i = 0; i < attackProperties.NumberOfBullets; i++)
        {
            float angle = (attackProperties.CurrentAngle + angleIncrement * i) % 360.0f;
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.DegToRad(angle)), Mathf.Sin(Mathf.DegToRad(angle)));
            Vector2 velocity = direction * attackProperties.Speed;

            bulletProperties.Velocity = velocity;
            bulletProperties.CurrentAngle = attackProperties.CurrentAngle;
            bulletProperties.AngularVelocity = attackProperties.AngularVelocity;
            bulletProperties.InitialPosition = this.GlobalPosition;
            Spawn_Bullet(bulletProperties);
        }
    }
    
    private void Spawn_WavePattern(Attack attackProperties)
    {
        // Generate random bullet type
        Bullet bulletProperties = new Bullet();
        bulletProperties.SpritePath = Generate_RandomBullet();        
        
        for (int i = 0; i < attackProperties.NumberOfBullets; i++)
        {
            bulletProperties.InitialPosition = this.GlobalPosition + new Vector2(i * attackProperties.Length, Mathf.Sin(i) * attackProperties.Amplitude);
            bulletProperties.Velocity = new Vector2(0, attackProperties.Speed);            
            
            //bulletProperties.InitialPosition = this.GlobalPosition;
            Spawn_Bullet(bulletProperties);
        }
    }
    
    private void Spawn_FlowerPattern(Attack attackProperties)
    {
        // Generate random bullet type
        Bullet bulletProperties = new Bullet();
        bulletProperties.SpritePath = Generate_RandomBullet();        
        
        for (float angle = 0; angle < 360; angle += 1.0f)
        {
            float radian = Mathf.DegToRad(angle);
            // This sinusoidal function will determine the shape of the petals
            float r = 100 * Mathf.Sin(attackProperties.Length * radian);
            Vector2 bulletPosition = this.GlobalPosition + new Vector2(r * Mathf.Cos(radian), r * Mathf.Sin(radian));
            var velocity = (bulletPosition - this.GlobalPosition) * attackProperties.Speed;

            bulletProperties.Velocity = velocity;
            bulletProperties.InitialPosition = this.GlobalPosition;
            Spawn_Bullet(bulletProperties);
        }
    }    
    
    private void Spawn_GridPattern(Attack attackProperties)
    {
        // Generate random bullet type
        Bullet bulletProperties = new Bullet();
        bulletProperties.SpritePath = Generate_RandomBullet();        

        for (int i = 0; i < attackProperties.Rows; i++)
        {
            for (int j = 0; j < attackProperties.Columns; j++)
            {
                Vector2 bulletPosition = this.GlobalPosition + new Vector2(j * attackProperties.Spacing, i * attackProperties.Spacing);
                var velocity = (bulletPosition - this.GlobalPosition) * attackProperties.Speed;

                bulletProperties.Velocity = velocity;
                bulletProperties.InitialPosition = bulletPosition;
                Spawn_Bullet(bulletProperties);
            }
        }        
    }
    
    private void Spawn_SnailPattern(Attack attackProperties)
    {
        // Generate random bullet type
        Bullet bulletProperties = new Bullet();
        bulletProperties.SpritePath = Generate_RandomBullet();        

        float currentAngle = attackProperties.StartAngle;
        float currentRadius = attackProperties.StartRadius;

        for (int i = 0; i < attackProperties.NumberOfBullets; i++)
        {
            float radian = Mathf.DegToRad(currentAngle);

            Vector2 bulletPosition = this.GlobalPosition + new Vector2(currentRadius * Mathf.Cos(radian), currentRadius * Mathf.Sin(radian));
            Vector2 bulletDirection = bulletPosition - this.GlobalPosition;
            var velocity = bulletDirection * attackProperties.Speed;

            bulletProperties.Velocity = velocity;
            bulletProperties.InitialPosition = bulletPosition;
            Spawn_Bullet(bulletProperties);

            currentAngle += attackProperties.AngleIncrement;
            currentRadius += attackProperties.RadiusIncrement;
        }        
    }        

    private string Generate_RandomBullet()
    {
        var randomBullet = (GameManager.BulletTypeList) Nucleus_Maths.Rnd.RandiRange(0, Enum.GetNames(typeof(GameManager.BulletTypeList)).Length-1);
        return $"res://src/objects/bullets/bulletSprites/Bullet{randomBullet.GetHashCode() + 1}.tscn";
    }

    private void Spawn_Bullet(Bullet bulletProperties)
    {
        if (bulletProperties.SpritePath != null)
        {
            //Vector2 newPosition = new Vector2(Nucleus_Maths.Rnd.RandfRange(40.0f, Nucleus.ScreenWidth-40.0f), Nucleus_Maths.Rnd.RandfRange(40.0f, Nucleus.ScreenHeight-40.0f));
            BulletGeneric instance = _spawnBullets.Add_Instance<BulletGeneric>(null, bulletProperties.InitialPosition);
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