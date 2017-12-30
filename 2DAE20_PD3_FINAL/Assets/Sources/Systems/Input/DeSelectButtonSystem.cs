using System;
using Entitas;
using UnityEngine;

public class DeSelectButtonSystem : IExecuteSystem {

    private Contexts _contexts;
    private IGroup<GameEntity> _objects;

    public DeSelectButtonSystem(Contexts contexts)
    {
        _contexts = contexts;
        
    }

    public void Execute()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //Find all objects to be reset
            _objects = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Hex));
            foreach (var e in _objects)
            {
                e.isClick = true;
                e.isSelected = false;
            }
        }
    }

}
