using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using System;

public class EnemyMovementSystem : IExecuteSystem {

    Contexts _contexts;
    private float _range = 0.07f;
    private float _speed = 1; //Units per second

    public EnemyMovementSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Execute()
    {
        var enemies = _contexts.game.GetEntities<GameEntity>(GameMatcher.Enemy);

        foreach (var enemy in enemies)
        {
            //Create Vectors
            Vector3 enemyPos = new Vector3(enemy.worldPos.x, enemy.worldPos.y, enemy.worldPos.z);
            GameEntity nextTile = enemy.path.Path[enemy.path.CurrentNode + 1];
            Vector3 nextTargetPos = new Vector3(nextTile.worldPos.x, nextTile.worldPos.y, nextTile.worldPos.z);

            //Check distance to current node
            if (Vector3.Distance(enemyPos, nextTargetPos) < _range)
            {
                enemy.path.CurrentNode++;
                nextTile = enemy.path.Path[enemy.path.CurrentNode + 1];
                nextTargetPos = new Vector3(nextTile.worldPos.x, nextTile.worldPos.y, nextTile.worldPos.z);
            }


        }
    }
}
