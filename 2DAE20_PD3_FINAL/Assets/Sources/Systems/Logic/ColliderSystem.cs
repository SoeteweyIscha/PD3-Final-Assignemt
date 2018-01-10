using System;
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

        var buildings = _contexts.game.GetEntities(GameMatcher.BaseTile);

        var bullets = _contexts.game.GetEntities(GameMatcher.Bullet);

        foreach (GameEntity enemy in enemies)
        {
            Vector3 enemyPos = enemy.vectorPos.Position;
            foreach (GameEntity bullet in bullets)
            {
                float range = StaticFunctions.SqrDistance(enemyPos, bullet.vectorPos.Position);
                if (range < _detectionRange * _detectionRange && !bullet.isDestroy)
                {
                    GameController.Money += 10;
                    bullet.isDestroy = true;
                    enemy.isDestroy = true;
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
                    building.isDestroy = true;
                    break;
                } 
            }
        }
    }
}
