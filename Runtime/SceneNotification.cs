#if FOUNDATION_NOTIFICATIONCENTER
using Foundation;

namespace MiniSceneManager {
	public static class SceneNotification {
		/// <summary>
		/// The name of the notification invoked immediately after a scene is loaded.
		/// </summary>
		public static readonly
#if FOUNDATION_NOTIFICATIONCENTER_TYPESAFE
			Notification<object, string>.Name
#else
			Notification.Name
#endif
				SceneDidLoad = new("MiniSceneManager.Scene.DidLoad");

		/// <summary>
		/// The name of the notification invoked immediately before a scene is unloaded.
		/// </summary>
		public static readonly
#if FOUNDATION_NOTIFICATIONCENTER_TYPESAFE
			Notification<object, string>.Name
#else
			Notification.Name
#endif
				SceneWillUnload = new("MiniSceneManager.Scene.WillUnload");
	}
}
#endif