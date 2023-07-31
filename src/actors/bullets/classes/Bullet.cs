using BulletBallet.actors.classes;

namespace BulletBallet.actors.bullets.classes;

public sealed partial class Bullet : Instance
{
    public GameManager.ItemsActionList ActionName {get; set;}  // To tell the receiver which action to realize in its list
    public GameManager.ItemsSendTo SendTo {get; set;}          // Who is concerned by the action to do
}