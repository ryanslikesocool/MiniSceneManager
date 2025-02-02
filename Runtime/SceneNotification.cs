#if FOUNDATION_NOTIFICATIONCENTER
using Foundation;

namespace MiniSceneManager {
	public static class SceneNotification {
		public static readonly
#if FOUNDATION_NOTIFICATIONCENTER_TYPESAFE
			Notification<object, string>.Name
#else
			Notification.Name
#endif
				SceneDidLoad = new("MiniSceneManager.Scene.DidLoad");

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