namespace BulletBallet;

/// <summary>
/// Responsible for :
/// - dealing with exit game actions
/// </summary>
public partial class Main : Node
{

	//*-------------------------------------------------------------------------*//

	#region GODOT METHODS

	public override void _Ready()
	{
		Initialize_Main();
	}

	public override void _Notification(int what)
	{
		// Exit the game
		if (what == NotificationWMCloseRequest)
		{
			Nucleus.Finalize_Nucleus();
			GetTree().Quit(); // default behavior
		}
	}

	#endregion

	//*-------------------------------------------------------------------------*//

	#region USER METHODS

	private void Initialize_Main()
	{ }

	#endregion
}