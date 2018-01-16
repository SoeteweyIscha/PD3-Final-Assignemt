﻿using System;
using Entitas;
using UnityEngine;

public class ColliderSystem : IExecuteSystem {

    private Contexts _contexts;
    private int _detectionRange = 1;

    public ColliderSystem( Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Execute()
    {
        var enemies = _contexts.game.GetEntities(GameMatcher.Enemy);

        var buildings = _contexts.game.GetEntities(GameMatcher.AnyOf(GameMatcher.Tower, GameMatcher.Wall, GameMatcher.HomeBase));

        var bullets = _contexts.game.GetEntities(GameMatcher.Bullet);

        var homeBase = _contexts.game.GetEntities(GameMatcher.HomeBase);

        foreach (GameEntity enemy in enemies)
        {
            Vector3 enemyPos = enemy.vectorPos.Position;
            foreach (GameEntity bullet in bullets)
            {
                float range = StaticFunctions.SqrDistance(enemyPos, bullet.vectorPos.Position);
                if (range < _detectionRange * _detectionRange && !bullet.isDestroy)
                {
                    bullet.isDestroy = true;
                    enemy.health.Healthpoints -= 2;
                    break;
                }
            }
        }

        foreach (GameEntity building in buildings)
        {
            Vector3 buildingPos = building.view.View.transform.position;
            foreach (GameEntity enemy in enemies)
            {
                float range = StaticFunctions.SqrDistance(buildingPos, enemy.vectorPos.Position);
                if (range < _detectionRange * _detectionRange && enemy.isDestroy == false)
                {
                    enemy.isDestroy = true;

                    if (building.isHomeBase)
                    {
                        building.health.Healthpoints -= enemy.health.Healthpoints;
                        Debug.Log("Homebase health: " + building.health.Healthpoints);
                    }
                    else
                    {
                        building.isDestroy = true;
                    }
                    break;
                } 
            }
        }

    }
}