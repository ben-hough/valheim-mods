using BepInEx.Configuration;
using Jotunn.Entities;
using UnityEngine;

namespace GlymorsFeasts
{
    /// <summary>
    /// One registerable feast dish. Add a new class implementing this interface
    /// and append an instance to <see cref="Plugin.Dishes"/> when the dish is ready.
    /// </summary>
    internal interface IFeastDish
    {
        string Id { get; }
        string DisplayName { get; }
        bool ShouldRegister();
        void BindConfig(ConfigFile config);
        void AddLocalization(CustomLocalization loc);
        void Register(Sprite icon);
        void ApplyRegisteredStats();
        bool TryEnsurePlacedFeast(Feast feast);
    }
}