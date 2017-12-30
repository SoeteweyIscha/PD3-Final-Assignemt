using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using System;

public class ViewSelectedSystem : ReactiveSystem<GameEntity>
{

    
     //Constructor
    public ViewSelectedSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Selected);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isSelected;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.view.View.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        Clear();
    }


}
