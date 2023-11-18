using UnityEngine;

namespace MiniSceneManager {
	[CreateAssetMenu(menuName = "Developed With Love/Mini Scene Manager/Scene Path")]
	public sealed class ScenePathReference : ScriptableObject {
		[SerializeField] private string _path = default;
		public string path => _path;

		public static implicit operator ScenePath(ScenePathReference reference) => new ScenePath(reference._path);
	}
}