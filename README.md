# Minimal_DI

This is as simple as possible DI solution for Unity

Features:
- Multi-context support
- AsSingle service registration support only
- Extensions for registration, and injection of fields
- Injection component for MonoBehaviour auto-resolution
- Compatible with AOT IL2CPP builds

## Usage example

To use MinDI you need to initialize it **MinDI.DefaultInit()**, it will create default context.

If you are using **InjectComponent** you need to set ExecutionOrder of your Registration script lower than -20000 so it executes before **InjectComponent** starts Injection of registered dependencies.

Register components on **Awake()** in your **Dependency Registration** script:

```c
[DefaultExecutionOrder(-30000)] 
public class GameEntry: MonoBehaviour
{
    public Camera MainCamera; //serialized Component class
    public Player Player; //serialized custom MonoBehaviour class
    public PlayerProgress playerProgress = new PlayerProgress(); // non-serialized non-MonoMehaviour class
   
    private void Awake()
    {
        MinDI.DefaultInit();
        // MinDI.Contexts["MyContext"] = new ServiceContainer();  - for custom context
        MinDI.Default.RegisterPublicFields(this);
        // MinDI.Contexts["MyContext"].RegisterPublicFields(this); - for custom context
    }
}
```
**Player**, **Camera** and **PlayerProgress** now will be registered and can be injected and used.

UsageExample:
```c
[RequireComponent(typeof(InjectComponent))] 
public class SampleScript: MonoBehaviour
{
    [Inject] private Camera _mainCamera;
    [Inject] private PlayerProgress _playerProgress;
    [Inject] private Player _player;

    private void Awake()
    {
        _mainCamera.transform.position = _player.transform.position + Vector3.up*10;
        _player.Score = _playerProgress.Score;
    }
}
```
For auto-injection use **InjectComponent** in will scan all components on **GameObject** and will resolve all fields tagged with **[Inject]** Attribute.

For manual resolution use extension method **Resolve()** :

```c
public class SampleScript: MonoBehaviour
{
    [Inject] private Camera _mainCamera;
    [Inject] private PlayerProgress _playerProgress;
    [Inject] private Player _player;

    private void Awake()
    {
        this.Resolve(); // injects all dependencies without InjectComponent

        _mainCamera.transform.position = _player.transform.position + Vector3.up * 10;
        _player.Score = _playerProgress.Score;
    }
}

