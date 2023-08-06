using System.Reflection;
using BulletBallet.utils.NucleusFW.StateMachine;

namespace BulletBallet.actors.characters.pnj.states;

public partial class StateMachine_Pnj : StateMachine_Core<Pnj>
{
    [Export] public NodePath InitialStateNode { get; set; }

    private bool _isReady = false;

    //*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        _SceneReady();
    }

    public override void _Process(double delta)
    {
        if(_isReady) base.Update(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        if(_isReady) base.Physics_Update(delta);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if(_isReady) base.Input_State(@event);
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    /// <summary>
    /// Wait for the owner to be ready (owner = Node at the top of the scene), to be sure to access safely to nodes
    /// </summary>
    async private void _SceneReady() => await ToSignal(Owner, "ready");

    /// <summary>
    /// Initialize the State Machine node reference in Utils (will be used by the States)
    /// </summary>
    /// <param name="rootNode"></param>
    public void Init_StateMachine(Pnj rootNode)
    {
        if(InitialStateNode != null)
        {
            base.Initialize_StateMachine_Core(InitialStateNode, rootNode);
            _isReady = true;
        }
        else
        {
            Nucleus.Logs.Error($"State Machine node is null (owner : {rootNode.Name}", new NullReferenceException(), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
            //GetTree().Quit();      // Quit game
        }
    }

    #endregion
}