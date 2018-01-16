//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public VectorPosComponent vectorPos { get { return (VectorPosComponent)GetComponent(GameComponentsLookup.VectorPos); } }
    public bool hasVectorPos { get { return HasComponent(GameComponentsLookup.VectorPos); } }

    public void AddVectorPos(UnityEngine.Vector3 newPosition) {
        var index = GameComponentsLookup.VectorPos;
        var component = CreateComponent<VectorPosComponent>(index);
        component.Position = newPosition;
        AddComponent(index, component);
    }

    public void ReplaceVectorPos(UnityEngine.Vector3 newPosition) {
        var index = GameComponentsLookup.VectorPos;
        var component = CreateComponent<VectorPosComponent>(index);
        component.Position = newPosition;
        ReplaceComponent(index, component);
    }

    public void RemoveVectorPos() {
        RemoveComponent(GameComponentsLookup.VectorPos);
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

    static Entitas.IMatcher<GameEntity> _matcherVectorPos;

    public static Entitas.IMatcher<GameEntity> VectorPos {
        get {
            if (_matcherVectorPos == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.VectorPos);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherVectorPos = matcher;
            }

            return _matcherVectorPos;
        }
    }
}