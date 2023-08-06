#nullable enable
namespace BulletBallet.actors.characters.classes;

public class Entity
{
    public string Name { get; }
    public bool DebugMode { get; }

    public Entity_Movement2D Movement { get; private set; }
    
    //*-------------------------------------------------------------------------*//
    
    public Entity(string name, bool isControlledByPlayer, bool? debugMode = null)
    {
        Name = name;
        DebugMode = debugMode ?? Nucleus.DEBUG_MODE;
        Movement = new Entity_Movement2D(isControlledByPlayer);
    }
}