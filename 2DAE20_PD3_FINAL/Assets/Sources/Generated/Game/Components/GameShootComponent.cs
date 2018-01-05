//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly ShootComponent shootComponent = new ShootComponent();

    public bool isShoot {
        get { return HasComponent(GameComponentsLookup.Shoot); }
        set {
            if (value != isShoot) {
                if (value) {
                    AddComponent(GameComponentsLookup.Shoot, shootComponent);
                } else {
                    RemoveComponent(GameComponentsLookup.Shoot);
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

    static Entitas.IMatcher<GameEntity> _matcherShoot;

    public static Entitas.IMatcher<GameEntity> Shoot {
        get {
            if (_matcherShoot == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Shoot);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherShoot = matcher;
            }

            return _matcherShoot;
        }
    }
}
