using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using System;

public class MoveSystem : IExecuteSystem {

    private Contexts _contexts;

    public MoveSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Execute()
    {
        var entities = _contexts.game.GetEntities(GameMatcher.Move);

        foreach (var e in entities)
        {
            MoveComponent eMove = e.move;

            e.vectorPos.Position += eMove.direction * eMove.speed * Time.deltaTime;
            e.view.View.transform.position = e.vectorPos.Position;
        }
    }
}
