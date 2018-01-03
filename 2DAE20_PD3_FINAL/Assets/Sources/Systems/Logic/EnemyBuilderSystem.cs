using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using System;

public class EnemyBuilder : ReactiveSystem<GameEntity>
{
    Contexts _contexts;

    public EnemyBuilder(Contexts contexts) : base(contexts.game)
    {

    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        throw new NotImplementedException();
    }


    protected override bool Filter(GameEntity entity)
    {
        throw new NotImplementedException();
    }


    protected override void Execute(List<GameEntity> entities)
    {
        var entity = _contexts.game.CreateEntity();

        entity.AddGridPos(0, 0);
        entity.AddVectorPos(Vector3.zero);
        entity.AddHealth(1);
        entity.AddPath(0, GameController.StartPath);

        GameObject  pre = Resources.Load<GameObject>("ShipPrefab");
        GameObject temp = GameObject.Instantiate(pre);
        entity.AddView(temp, temp.GetComponent<Renderer>().material.color);
            
            

    }

}
