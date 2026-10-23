# Mini Scene Manager
A super simple scene management system for Unity.

## NOTICE
**This package is under development and not considered production-ready.**
<br/>
Breaking changes are common, documentation is incomplete, and support is limited.
Use at your own risk.

## Installation (Unity Package Manager)
- Select "Add package from git URL..." from the plus (+) menu in the package manager window.
- Paste the package's git url.
```
https://github.com/ryanslikesocool/MiniSceneManager.git
```

## Dependencies
| Name | Required |
| - | - |
| [Addressables][addressables] | Yes |
| [Notification Center][notification-center] | No |

## Usage
This package makes use of Unity's Addressables system, and works under the assumption that scenes are marked as addressables.

Call one of the `SceneManager`'s static methods to start using it.
```cs
// Load the scene at the given addressable path.
SceneManager.LoadScene("Scene/Addressable/Path");

// Unload the scene at the given addressable path.
SceneManager.UnloadScene("Scene/Addressable/Path");
```

Multiple functions are included to check the state of a scene.
```cs
// Is the scene at the given addressable path created?
// This includes scenes that are currently loading.
bool sceneIsCreated = SceneManager.IsCreated("Scene/Addressable/Path");

// Is the scene at the given addressable path loaded?
// This only includes scenes that have finished loading.
bool sceneIsLoaded = SceneManager.IsLoaded("Scene/Addressable/Path");
```

-----

To faciliate inter-scene communication, create a class inheriting from `SceneAccessor`.
It provides multiple virutal methods that can be overridden to observe the scene's lifecycle.
Only one instance of a `SceneAccessor` subclass should exist in a scene.
```cs
class MySceneAccessor: SceneAccessor {
	// Called immediately after the containing scene is loaded.
	public override void OnSceneDidLoad() { /* ... */ }

	// Called immediately before the containing scene is unloaded.
	public override void OnSceneWillUnload() { /* ... */ }
}
```

A scene accessor may be accessed from another scene with `SceneManager`.
```cs
bool mySceneAccessorExists = SceneManager.TryGetAccessor(
	"Scene/Addessable/Path",
	out MySceneAccessor mySceneAccessor
);
```

-----

If [Notification Center][notification-center] is available in the same project, multiple events will be posted when scenes load and unload.
```cs
NotificationCenter.Default.AddObserver(SceneNotifications.SceneDidLoad, SceneStateReceiver);
NotificationCenter.Default.AddObserver(SceneNotifications.SceneWillUnload, SceneStateReceiver);

void SceneStateReceiver(in Notification notification) {
	string addressablePath = notification.ReadData<string>();

	if (notification.name == SceneNotifications.SceneDidLoad) {
		Debug.Log($"The scene at the addressable path {addressablePath} finished loading.");
	} else if (notification.name == SceneNotifications.SceneWillUnload) {
		Debug.Log($"The scene at the addressable path {addressablePath} is about to unload.");
	}
}
```

[addressables]: https://docs.unity3d.com/Packages/com.unity.addressables@1.21/manual/index.html
[notification-center]: https://github.com/ryanslikesocool/UnityFoundation-NotificationCenter