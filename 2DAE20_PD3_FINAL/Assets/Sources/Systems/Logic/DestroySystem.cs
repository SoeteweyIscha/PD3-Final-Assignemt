﻿using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class DestroySystem : ReactiveSystem <GameEntity>
{

    public DestroySystem(Contexts contexts) : base(contexts.game)
    {
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
        foreach (GameEntity e in entities)
        {
            //Destroy view Object in scene
            if (e.hasView)
                UnityEngine.Object.Destroy(e.view.View);
            if (e.isEnemy)
            {
                GameController.Counter += 1;
            }

            e.Destroy();
        }

        Clear();
    }




}
