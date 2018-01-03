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
            Vector3 enemyPos = enemy.vectorPos.Position;
            GameEntity nextTile = enemy.path.Path[enemy.path.CurrentNode + 1];
            Vector3 nextTargetPos = nextTile.vectorPos.Position;

            //Check distance to current node
            if (Vector3.Distance(enemyPos, nextTargetPos) < _range)
            {
                enemy.path.CurrentNode++;
                nextTile = enemy.path.Path[enemy.path.CurrentNode + 1];
                nextTargetPos = nextTile.vectorPos.Position;
            }


        }
    }
}
