using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using System;

public class EnemyBuilderSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;

    public EnemyBuilderSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.BuildEnemy);
    }


    protected override bool Filter(GameEntity entity)
    {
        return entity.isBuildEnemy;
    }


    protected override void Execute(List<GameEntity> entities)
    {
        var builders = _contexts.game.GetEntities(GameMatcher.BuildEnemy);

        foreach (var entity in builders)
        {

            entity.isEnemy = true;
            entity.ReplaceGridPos(0, 0);
            entity.ReplaceVectorPos(Vector3.zero);
            entity.ReplaceHealth(5, null);
            entity.ReplacePath(0, GameController.StartPath);
            entity.isTargeting = true;
            Debug.Log(entity.path.Path.Capacity);
            entity.AddMove(1, (entity.path.Path[0].vectorPos.Position-entity.vectorPos.Position).normalized);
            entity.isEnemy = true;
            entity.isBuildEnemy = false;
        }
        Clear();
    }

}
