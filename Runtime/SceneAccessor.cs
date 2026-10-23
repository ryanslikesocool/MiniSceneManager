using UnityEngine;

namespace MiniSceneManager {
	/// <summary>
	/// <see cref="SceneAccessor"/> acts as a bridge between scenes.
	/// Add a <see cref="SceneAccessor"/> subclass to a root Game Object in a scene and the <see cref="SceneManager"/> will find it and call lifecycle events.
	/// </summary>
	public abstract class SceneAccessor : MonoBehaviour {
		/// <summary>
		/// This method is called immediately after the containing scene is loaded.
		/// </summary>
		public virtual void OnSceneDidLoad() { }

		/// <summary>
		/// This method is called immediately before the containing scene is unloaded.
		/// </summary>
		public virtual void OnSceneWillUnload() { }
	}
}