namespace BulletBallet.actors.classes;

public partial class Instance : GodotObject
{
    public string SpritePath { get; set; }                      // The path to load the sprite

    public int MaxVisibleInstance {get; set;}                   // How much instance of this item to display on screen
    public bool UniqueInstance {get; set;}                      // If true, the item is displayed once
    public float Ttl {get; set;}                                // How much time the item is visible (exists)

    public float ActionDuration {get; set;}                     // How much time the action applies
    public float OptionalValue {get; set;}                      // To pass a value to the receiver  (eg : the max speed percent to apply,...)
}