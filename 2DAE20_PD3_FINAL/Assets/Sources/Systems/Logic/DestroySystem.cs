using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class DestroySystem : ReactiveSystem <GameEntity>
{

    Contexts _contexts;

    public DestroySystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isDestroy;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Destroy);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        var objects = entities;

        foreach (GameEntity e in objects)
        {
            //Destroy view Object in scene
            if (e.hasView)
                UnityEngine.Object.Destroy(e.view.View);

            e.Destroy();
        }
    }




}
