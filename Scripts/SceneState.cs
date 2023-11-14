using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
#if FOUNDATION_NOTIFICATIONCENTER
using Foundation;
#endif

namespace MiniSceneManager {
	internal sealed class SceneState {
		public SceneInstance sceneInstance { get; private set; } = default;
		public SceneAccessor accessor { get; private set; } = default;

		public bool IsCreated { get; private set; } = false;
		public bool IsLoaded => accessor != null;

		// MARK: - Lifecycle

		public SceneState() {
			sceneInstance = default;
			accessor = default;
			IsCreated = false;
		}

		public void Init(AsyncOperationHandle<SceneInstance> operationHandle, string addressablePath) {
			IsCreated = true;

			operationHandle.Completed += handle => {
				sceneInstance = handle.Result;

				SceneAccessor sceneAccessor = null;
				foreach (GameObject obj in sceneInstance.Scene.GetRootGameObjects()) {
					if (obj.TryGetComponent(out sceneAccessor)) {
						break;
					}
				}
				if (sceneAccessor != null) {
					this.accessor = sceneAccessor;
					accessor.OnSceneDidLoad();
				}

#if FOUNDATION_NOTIFICATIONCENTER
				NotificationCenter.Default.Post(SceneNotification.SceneDidLoad, null, addressablePath);
#endif
			};
		}

		public void Deactivate() {
#if FOUNDATION_NOTIFICATIONCENTER
			NotificationCenter.Default.Post(SceneNotification.SceneWillUnload, null, accessor);
#endif

			if (accessor != null) {
				accessor.OnSceneWillUnload();
			}
		}

		public void Deinit() {
			sceneInstance = default;
			IsCreated = false;

			this.accessor = null;
		}
	}
}