using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
#if FOUNDATION_NOTIFICATIONCENTER
using Foundation;
#endif

namespace MiniSceneManager {
	public sealed class SceneManager {
		private static SceneManager instance { get; set; } = default;

		private LoadSceneParameters defaultLoadParameters = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.None);
		private UnloadSceneOptions defaultUnloadOptions = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects;

		private Dictionary<string, SceneState> scenes;

		// MARK: - Lifecycle

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void _Initialize() {
			instance = new SceneManager();
		}

		private SceneManager() {
			scenes = new Dictionary<string, SceneState>();

#if FOUNDATION_NOTIFICATIONCENTER
			SceneNotification.LoadScene.received += OnLoadSceneNotification;
			SceneNotification.UnloadScene.received += OnUnloadSceneNotification;
#endif
		}

		~SceneManager() {
#if FOUNDATION_NOTIFICATIONCENTER
			SceneNotification.LoadScene.received -= OnLoadSceneNotification;
			SceneNotification.UnloadScene.received -= OnUnloadSceneNotification;
#endif

			scenes.Clear();
			scenes = null;
		}

		// MARK: - Modification

		/// <summary>
		/// Override the default load parameters.
		/// </summary>
		/// <param name="loadParameters">The new default load parameters.</param>
		public static void SetDefaultLoadSceneParameters(LoadSceneParameters loadParameters)
			=> instance.defaultLoadParameters = loadParameters;

		/// <summary>
		/// Override the default unload options.
		/// </summary>
		/// <param name="unloadOptions">The new default unload options.</param>
		public static void SetDefaultUnloadOptions(UnloadSceneOptions unloadOptions)
			=> instance.defaultUnloadOptions = unloadOptions;

		/// <summary>
		/// Load a scene at the given path.
		/// </summary>
		/// <param name="path">The path of the scene to load.</param>
		/// <param name="loadParameters">Load parameters for the scene.  Leave this <see langword="null"/> to use the default.</param>
		public static void LoadScene(in ScenePath path, LoadSceneParameters? loadParameters = null)
			=> LoadScene(path.path, loadParameters);

		/// <summary>
		/// Load a scene at the given path.
		/// </summary>
		/// <param name="path">The path of the scene to load.</param>
		/// <param name="loadParameters">Load parameters for the scene.  Leave this <see langword="null"/> to use the default.</param>
		public static void LoadScene(in string path, LoadSceneParameters? loadParameters = null)
			=> instance._LoadScene(path, loadParameters);

		/// <summary>
		/// Unload a scene at the given path.
		/// </summary>
		/// <param name="path">The path of the scene to unload.</param>
		/// <param name="unloadOptions">Unload options for the scene.  Leave this <see langword="null"/> to use the default.</param>
		public static void UnloadScene(in ScenePath path, UnloadSceneOptions? unloadOptions = null)
			=> UnloadScene(path.path, unloadOptions);

		/// <summary>
		/// Unload a scene at the given path.
		/// </summary>
		/// <param name="path">The path of the scene to unload.</param>
		/// <param name="unloadOptions">Unload options for the scene.  Leave this <see langword="null"/> to use the default.</param>
		public static void UnloadScene(in string path, UnloadSceneOptions? unloadOptions = null)
			=> instance._UnloadScene(path, unloadOptions);

		/// <summary>
		/// Attempt to retrieve the scene accessor for a scene.
		/// </summary>
		/// <returns>The scene accessor for the scene at the given path.  <see langword="null"/> if the scene or its accessor is unavailable.</returns>
		public static bool TryGetAccessor(in ScenePath path, out SceneAccessor accessor)
			=> TryGetAccessor(path.path, out accessor);

		/// <summary>
		/// Attempt to retrieve the scene accessor for a scene.
		/// </summary>
		/// <returns>The scene accessor for the scene at the given path.  <see langword="null"/> if the scene or its accessor is unavailable.</returns>
		public static bool TryGetAccessor(in string path, out SceneAccessor accessor)
			=> instance._TryGetAccessor(path, out accessor);

		/// <summary>
		/// Check if a scene is created.  This includes scenes that are currently loading.
		/// </summary>
		/// <returns><see langword="true"/> if the scene at the given path is created; <see langword="false"/> otherwise.</returns>
		public static bool IsCreated(in ScenePath path)
			=> IsCreated(path.path);

		/// <summary>
		/// Check if a scene is created.  This includes scenes that are currently loading.
		/// </summary>
		/// <returns><see langword="true"/> if the scene at the given path is created; <see langword="false"/> otherwise.</returns>
		public static bool IsCreated(in string path)
			=> instance._IsCreated(path);

		/// <summary>
		/// Check if a scene is loaded.  This only includes scenes that have finished loading.
		/// </summary>
		/// <returns><see langword="true"/> if the scene at the given path is loaded; <see langword="false"/> otherwise.</returns>
		public static bool IsLoaded(in ScenePath path)
			=> IsLoaded(path.path);

		/// <summary>
		/// Check if a scene is loaded.  This only includes scenes that have finished loading.
		/// </summary>
		/// <returns><see langword="true"/> if the scene at the given path is loaded; <see langword="false"/> otherwise.</returns>
		public static bool IsLoaded(in string path)
			=> instance._IsLoaded(path);

		// MARK: - Private

		private void _LoadScene(in string path, LoadSceneParameters? loadParameters = null) {
			if (!scenes.TryAdd(path, new SceneState())) {
				return;
			}

			AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(
				key: path,
				loadSceneParameters: loadParameters ?? defaultLoadParameters
			);

			scenes[path].Init(
				operationHandle: handle,
				addressablePath: path
			);
		}

		private void _UnloadScene(in string path, UnloadSceneOptions? unloadOptions = null) {
			if (!_TryGetSceneState(path, out SceneState state)) {
				return;
			}

			state.Deactivate();
			Addressables.UnloadSceneAsync(
				scene: state.sceneInstance,
				unloadOptions: unloadOptions ?? defaultUnloadOptions
			);
			state.Deinit();
			scenes.Remove(path);
		}

		private bool _TryGetSceneState(in string path, out SceneState state) => scenes.TryGetValue(path, out state);

		private bool _TryGetAccessor(in string path, out SceneAccessor accessor) {
			if (_TryGetSceneState(path, out SceneState state)) {
				accessor = state.accessor;
				return true;
			}
			accessor = null;
			return false;
		}

		private bool _IsCreated(in string path)
			=> scenes.TryGetValue(path, out SceneState sceneState) && sceneState.IsCreated;

		private bool _IsLoaded(in string path)
			=> scenes.TryGetValue(path, out SceneState sceneState) && sceneState.IsLoaded;

		// MARK: - Notifications

#if FOUNDATION_NOTIFICATIONCENTER
		private void OnLoadSceneNotification(in Notification notification) {
			string scenePath = GetScenePath(notification);
			_LoadScene(scenePath);
		}

		private void OnUnloadSceneNotification(in Notification notification) {
			string scenePath = GetScenePath(notification);
			_UnloadScene(scenePath);
		}

		private string GetScenePath(in Notification notification) => notification.data switch {
			string path => path,
			ScenePath path => path.path,
			_ => throw new System.Exception($"Failed to get scene path from notification data: {notification.data}")
		};
#endif
	}
}