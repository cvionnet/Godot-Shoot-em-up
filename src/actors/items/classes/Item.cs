// Inherite from Godot.Object to allow the class to be send in Signal or CallGroup (https://github.com/godotengine/godot/issues/36351)
// 💢 : "Attempted to convert an unmarshallable managed type to Variant"

using BulletBallet.actors.classes;

namespace BulletBallet.actors.items.classes;

public sealed partial class Item : Instance
{
    public GameManager.ItemsActionList ActionName {get; set;}  // To tell the receiver which action to realize in its list
    public GameManager.ItemsSendTo SendTo {get; set;}          // Who is concerned by the action to do
}