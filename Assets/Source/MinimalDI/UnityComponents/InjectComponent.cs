using System;
using System.Collections.Generic;
using System.Linq;
using MinimalDI.Injection;
using UnityEngine;

namespace MinimalDI.UnityComponents
{
    /// Attach this component to MonoBehaviour that uses [Inject] Attribute for auto-resolution of dependencies,
    /// DefaultExecutionOrder(-20000) Should execute component Awake() before Unity InputSystem but After DI registration in EntryPoint,
    /// EntryPoint recommended order is -30000
    [DefaultExecutionOrder(-20000)] 
    [DisallowMultipleComponent]
    public class InjectComponent: MonoBehaviour
    {
        [SerializeField] private string contextName;
        [SerializeField] protected List<Component> injectableComponents;
        private IServiceProvider Context =>Core.MinDI.Contexts.GetValueOrDefault(contextName, Core.MinDI.Default);

        private void OnValidate()
        {
            injectableComponents = GetComponents<Component>().Where(InjectAttribute.IsPresentOnObject).ToList();
        }

        private void Awake() => Inject();

        private void Inject()
        {
            injectableComponents.ForEach(component => component.Resolve(Context));
        }
    }
}