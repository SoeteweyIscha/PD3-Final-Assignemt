using Entitas;
using UnityEngine;

public class TargetingSystem : IExecuteSystem
{
    private Contexts _contexts;
    private IGroup<GameEntity> _entities;

    private Transform _target;

    public TargetingSystem( Contexts contexts, Transform target)
    {
        _contexts = contexts;
        _target = target;

    }

    public void Execute()
    {
        _entities = _contexts.game.GetGroup(GameMatcher.Targeting);

        var objects = _entities.GetEntities();

        
        foreach (GameEntity e in objects)
        {
            e.view.View.transform.LookAt(_target);
        }

        _entities = null;
    }
}
