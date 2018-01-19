using System.Collections;
using Entitas;
using UnityEngine;

public class RockGenerationSystem : IInitializeSystem {

    private Contexts _contexts;
    private float _chance;

    public RockGenerationSystem(Contexts contexts, float chance)
    {
        _contexts = contexts;
        _chance = chance;
    }

    public void Initialize()
    {
        //Get tiles
        var tiles = _contexts.game.GetEntities(GameMatcher.Hex);

        //Generate rocks at random positions
        foreach (GameEntity e in tiles)
        {
            bool canPlaceRock = true;
            foreach (GameEntity pathpart in GameController.StartPath)
            {
                if (e.gridPos.x == pathpart.gridPos.x && e.gridPos.y == pathpart.gridPos.y)
                {
                    canPlaceRock = false;
                    break;
                }
            }

            if (e == GameController.startTile)
                canPlaceRock = false;

            if (canPlaceRock)
            {
                int random = (int)Random.Range(0, 100f);
                if (random <= _chance)
                {
                    e.isRock = true;
                    e.isWalkAble = false;
                    e.isClick = false;
                }
            }
            canPlaceRock = false;
            
        }

    }

}
