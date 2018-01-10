﻿using System.Collections;
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

        foreach (var e in builders)
        {
            var entity = _contexts.game.CreateEntity();

            entity.isEnemy = true;
            entity.AddGridPos(0, 0);
            entity.AddVectorPos(Vector3.zero);
            entity.AddHealth(1);
            entity.AddPath(0, GameController.StartPath);
            entity.isTargeting = true;
            entity.AddMove(1, Vector3.zero);

            GameObject pre = Resources.Load<GameObject>("ShipPrefab");
            GameObject temp = GameObject.Instantiate(pre);
            entity.AddView(temp, temp.GetComponent<Renderer>().material.color);

            e.Destroy();
        }
        Clear();
    }

}
