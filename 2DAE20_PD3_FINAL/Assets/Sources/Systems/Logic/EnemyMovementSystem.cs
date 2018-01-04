using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using System;

public class EnemyMovementSystem : IExecuteSystem
{

    Contexts _contexts;
    private float _range = 0.05f;
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
            Vector3 enemyPos = enemy.vectorPos.Position;
            GameEntity nextTile = enemy.path.Path[enemy.path.CurrentNode];
            Vector3 nextTargetPos = nextTile.vectorPos.Position;

            //Check distance to current node
            if (Vector3.Distance(enemyPos, nextTargetPos) < _range)
            {
                enemy.gridPos.x = nextTile.gridPos.x;
                enemy.gridPos.y = nextTile.gridPos.y;
                enemy.path.CurrentNode++;
                nextTile = enemy.path.Path[enemy.path.CurrentNode];
                nextTargetPos = nextTile.vectorPos.Position;
                enemy.move.direction = (nextTargetPos - enemyPos).normalized;

            }

        }

    }
}

