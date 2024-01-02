# Mini Scene Manager
A super simple scene management system for Unity.

## NOTICE
This package is not considered production-ready.  Breaking changes are common and support is limited.  Use at your own risk.

## Installation

**Recommended Installation** (Unity Package Manager)
- "Add package from git URL..."
- `https://github.com/ryanslikesocool/MiniSceneManager.git`

## Dependencies
MiniSceneManager requires one dependency
- [Addressables](https://docs.unity3d.com/Packages/com.unity.addressables@1.21/manual/index.html)

...and has one optional dependency
- [Notification Center](https://github.com/ryanslikesocool/UnityFoundation-NotificationCenter)

## Usage
MiniSceneManager makes use of Unity's Addressables system, and works under the assumption that scenes are marked as addressables.

Call one of the `SceneManager`'s static methods to start using it.
```cs
// load the scene at the given addressable path.
SceneManager.LoadScene("Scene/Addressable/Path");

// unload the scene at the given addressable path.
SceneManager.UnloadScene("Scene/Addressable/Path");
```

Multiple functions are included to check the state of a scene.
```cs
// is the scene at the given addressable path created?
// this includes scenes that are currently loading.
bool sceneIsCreated = SceneManager.IsCreated("Scene/Addressable/Path");

// is the scene at the given addressable path loaded?
// this only includes scenes that have finished loading.
bool sceneIsLoaded = SceneManager.IsLoaded("Scene/Addressable/Path");
```

-----

If [Notification Center](https://github.com/ryanslikesocool/UnityFoundation-NotificationCenter) is available in the same project, multiple events will be posted when scenes load and unload.
```cs
NotificationCenter.Default[SceneNotifications.SceneDidLoad].received
	+= SceneStateReceiver;

NotificationCenter.Default[SceneNotifications.SceneWillUnload].received
	+= SceneStateReceiver;

void SceneStateReceiver(in Notification notification) {
	string addressablePath = notification.ReadData<string>();

	if (notification.name == SceneNotifications.SceneDidLoad) {
		Debug.Log($"The scene at the addressable path {addressablePath} finished loading.");
	} else if (notification.name == SceneNotifications.SceneWillUnload) {
		Debug.Log($"The scene at the addressable path {addressablePath} is about to unload.");
	}
}
```
