using UnityEngine;

namespace MiniSceneManager {
	/// <summary>
	/// SceneAccessor acts as a bridge between scenes.
	/// Add a SceneAccessor subclass to a root gameobject in a scene and the SceneManager will find it and call lifecycle events.
	/// </summary>
	public abstract class SceneAccessor : MonoBehaviour {
		/// <summary>
		/// OnSceneDidLoad is called immediately after its containing scene is loaded.
		/// </summary>
		public virtual void OnSceneDidLoad() { }

		/// <summary>
		/// OnSceneWillUnload is called immediately before its containing scene is unloaded.
		/// </summary>
		public virtual void OnSceneWillUnload() { }
	}
}