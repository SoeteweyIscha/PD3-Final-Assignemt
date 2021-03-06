//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly DestroyComponent destroyComponent = new DestroyComponent();

    public bool isDestroy {
        get { return HasComponent(GameComponentsLookup.Destroy); }
        set {
            if (value != isDestroy) {
                if (value) {
                    AddComponent(GameComponentsLookup.Destroy, destroyComponent);
                } else {
                    RemoveComponent(GameComponentsLookup.Destroy);
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherDestroy;

    public static Entitas.IMatcher<GameEntity> Destroy {
        get {
            if (_matcherDestroy == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Destroy);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherDestroy = matcher;
            }

            return _matcherDestroy;
        }
    }
}
