using System;
using Entitas;
using UnityEngine;

public class TimerSystem : IExecuteSystem {

    private Contexts _contexts;

    public TimerSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Execute()
    {
        var entities = _contexts.game.GetEntities(GameMatcher.Timer);

        foreach (var e in entities)
        {
            e.timer.currentTime += Time.deltaTime;
            if (e.timer.currentTime >= e.timer.MaxTime)
            {
                e.isShoot = true;
                e.RemoveTimer();
            }
                
        }
    }
}
