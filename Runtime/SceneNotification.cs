#if FOUNDATION_NOTIFICATIONCENTER
using Foundation;

namespace MiniSceneManager {
	public static class SceneNotification {
		public static readonly Notification.Name SceneDidLoad = new Notification.Name("MiniSceneManager.Scene.DidLoad");
		public static readonly Notification.Name SceneWillUnload = new Notification.Name("MiniSceneManager.Scene.WillUnload");
	}
}
#endif