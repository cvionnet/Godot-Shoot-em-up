// Inherite from Godot.Object to allow the class to be send in Signal or CallGroup (https://github.com/godotengine/godot/issues/36351)
// 💢 : "Attempted to convert an unmarshallable managed type to Variant"

using BulletBallet.actors.classes;

namespace BulletBallet.objects.items.classes;

public sealed partial class Item : Instance
{
    public int MaxVisibleInstance {get; set;}                   // How much instance of this item to display on screen
    public bool UniqueInstance {get; set;}                      // If true, the item is displayed once
    public float Ttl {get; set;}                                // How much time the item is visible (exists)
 
    public float ActionDuration {get; set;}                     // How much time the action applies
    public float OptionalValue {get; set;}                      // To pass a value to the receiver  (eg : the max speed percent to apply,...)
    
    public GameManager.ItemsActionList ActionName {get; set;}  // To tell the receiver which action to realize in its list
    public GameManager.ItemsSendTo SendTo {get; set;}          // Who is concerned by the action to do
}