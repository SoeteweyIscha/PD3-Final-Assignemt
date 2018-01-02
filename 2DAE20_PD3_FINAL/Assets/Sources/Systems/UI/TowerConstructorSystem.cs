using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;
using System.Runtime.Remoting.Contexts;

public class TowerConstructorSystem : ReactiveSystem<GameEntity> {

    private Contexts _contexts;

    public TowerConstructorSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Buiding);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasBuiding;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            //Create Entity
            GameEntity temp = _contexts.game.CreateEntity();

            temp.AddBuiding(e.buiding.state);

            //Create Gameobject to render
            Vector3 pos = new Vector3(e.worldPos.x, e.worldPos.y, e.worldPos.z);
            GameObject tempView = null;

            //Add components specific to building type to the entity
            switch (e.buiding.state)
            {
                case GameController.Building.Tower:
                    tempView = GameObject.Instantiate(Resources.Load<GameObject>("Tower"), pos, Quaternion.identity);
                    temp.AddHealth(3);
                    break;

                case GameController.Building.Wall:
                    tempView = GameObject.Instantiate(Resources.Load<GameObject>("Wall"), pos, Quaternion.identity);
                    temp.AddHealth(6);
                    break;
            }

            temp.AddView(tempView, tempView.GetComponentInChildren<Renderer>().material.color);
            temp.AddGridPos(e.gridPos.x, e.gridPos.y);
            temp.isTargeting = true;
            // link tile to tower
            temp.AddBaseTile(e);
            

        }

        Clear();
    }
}
