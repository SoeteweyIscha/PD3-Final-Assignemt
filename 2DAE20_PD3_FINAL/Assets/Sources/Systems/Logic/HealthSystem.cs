using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using System;

public class HealthSystem : IExecuteSystem {

    private Contexts _contexts;

    public HealthSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Execute()
    {
        var entities = _contexts.game.GetEntities(GameMatcher.Health);

        foreach (var e in entities)
        {
            e.health.Display.text = e.health.Healthpoints.ToString();
            if (e.health.Healthpoints <= 0)
            {
                e.isDestroy = true;
                GameController.Money += 10;
                if (e.isHomeBase)
                {
                    GameController.gameLoop = false;
                }
            }
        }
    }
}
