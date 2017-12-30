using System;
using Entitas;
using UnityEngine;

public class GridViewSystem : IInitializeSystem
{

    private IGroup<GameEntity> renderTiles;

    public GridViewSystem(Contexts context)
    {
        renderTiles = context.game.GetGroup(GameMatcher.AllOf(GameMatcher.Hex));
    }


    public void Initialize()
    {

        // Find tile prefab in Resources folder
        GameObject tile = Resources.Load<GameObject>("HexPrefab");
        GameObject shell = new GameObject("Shell");
        foreach (var e in renderTiles)
        {
            Vector3 pos = new Vector3(e.worldPos.x, e.worldPos.y, e.worldPos.z);
            GameObject temp = UnityEngine.Object.Instantiate(tile, pos, Quaternion.identity, shell.transform);

            // Assigning the created tile to the corresponding entity
            e.AddView(temp, temp.GetComponent<Renderer>().material.color);

        }
    }
}
