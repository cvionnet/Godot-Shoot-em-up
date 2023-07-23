using System.Reflection;

namespace BulletBallet.utils.NucleusFW.SceneManager;

/// <summary>
/// Responsible for :
/// - loading the 1st screen
/// - transitioning to other screens
/// </summary>
public partial class SceneManager : Node
{
	private SceneTransition _sceneTransition;

	private Node _actualScene;
	private string _nextScene;

	//*-------------------------------------------------------------------------*//

	#region GODOT METHODS

	public override void _Ready()
	{
		_sceneTransition = GetNode<SceneTransition>("SceneTransition");

		LoadFirstScene();
	}

	#endregion

	//*-------------------------------------------------------------------------*//

	#region SIGNAL CALLBACKS

	/// <summary>
	/// When a signal to change scene hab been emited from a scene
	/// </summary>
	private void _onCall_TransitionScene(string nextScene)
	{
		_nextScene = nextScene;

		// Call the animation
		_sceneTransition.Transition_Scene();
	}

	/// <summary>
	/// (From SceneTransition) When the transition animation is finished
	/// </summary>
	private void _onSceneTransition_Finished()
	{
		Nucleus.Logs.Info(_nextScene);
		PackedScene scene = ResourceLoader.Load<PackedScene>($"res://src/scenes/{_nextScene}.tscn");

		if (scene != null)
		{
			// If exists, unload the actual scene
			if (_actualScene != null)
				_actualScene.QueueFree();

			// Load the new scene + connect to its signal
			_actualScene = scene.Instantiate();
			AddChild(_actualScene);
			MoveChild(_actualScene, 0);    // To display the transition scene before this scene
		}
		else
		{
			//GD.PrintErr($"SceneManager._onSceneTransition_Finished - The scene {_nextScene} does not exists");
			NucleusFW.Nucleus.Logs.Error($"The scene {_nextScene} does not exists !", new TargetException(), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
		}
	}

	#endregion

	//*-------------------------------------------------------------------------*//

	#region USER METHODS

	private void LoadFirstScene()
	{
		_nextScene = "screens/Menu";
		_onSceneTransition_Finished();      // to add a .Connect on the 1st scene
	}

	/// <summary>
	/// (called by GameBrain) Connect to signal after GameBrain has been initialized (because it contains the Nucleus_Utils.State_Manager initialization)
	/// </summary>
	public void Initialize_SceneManager()
	{
		NucleusFW.Nucleus.SignalManager.Connect(SignalManager.SignalName.Generic_TransitionScene, new Callable(this, nameof(_onCall_TransitionScene)));
		NucleusFW.Nucleus.SignalManager.Connect(SignalManager.SignalName.SceneTransition_AnimationFinished, new Callable(this, nameof(_onSceneTransition_Finished)));
	}

	#endregion
}