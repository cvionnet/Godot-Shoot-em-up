using BulletBallet.actors.classes;

namespace BulletBallet.objects.bullets.classes;

public sealed partial class Bullet : Instance
{
    public Vector2 Velocity {get; set;}
    public float AngularVelocity {get; set;}
    public float CurrentAngle {get; set;}
    
    
    //public int MaxVisibleInstance {get; set;}                   // How much instance of this item to display on screen
    // public float OptionalValue {get; set;}                      // To pass a value to the receiver  (eg : the max speed percent to apply,...)

    // public GameManager.ItemsActionList ActionName {get; set;}  // To tell the receiver which action to realize in its list
    // public GameManager.ItemsSendTo SendTo {get; set;}          // Who is concerned by the action to do
}