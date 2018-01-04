﻿using Entitas;
using UnityEngine;

public class TargetingSystem : IExecuteSystem
{
    private Contexts _contexts;
    private IGroup<GameEntity> _entities;
    private int _range;

    public TargetingSystem( Contexts contexts, int range)
    {
        _contexts = contexts;
        _range = range;
    }

    public void Execute()
    {
        //All gameEntities with targetComponent
        var objects = _contexts.game.GetEntities(GameMatcher.Targeting);

        var enemies = _contexts.game.GetEntities(GameMatcher.Enemy);

        foreach (GameEntity e in objects)
        {
            if (e.isEnemy)
                //look in movement directio
                e.view.View.transform.LookAt(e.path.Path[e.path.CurrentNode].vectorPos.Position);
            else
            {
                foreach (var enemy in enemies)
                {
                    Vector3 offset = e.baseTile.tile.vectorPos.Position - enemy.vectorPos.Position;
                    float sqrLen = offset.sqrMagnitude;
                    if (sqrLen < _range * _range)
                        e.view.View.transform.LookAt(enemy.vectorPos.Position);
                }
            }
        }
    }
}
