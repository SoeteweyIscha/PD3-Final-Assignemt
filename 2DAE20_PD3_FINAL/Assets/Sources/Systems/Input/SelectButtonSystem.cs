using Entitas;
using UnityEngine;

public class SelectButtonSystem : IExecuteSystem
{

    private Contexts _contexts;
    private IGroup<GameEntity> _clickables;

    public SelectButtonSystem(Contexts contexts)
    {
        _contexts = contexts;
        _clickables = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Click));
    }

    public void Execute()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var objects = _clickables.GetEntities();
            var mousePos = Input.mousePosition;
            mousePos.z = 10.0f;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            GameEntity closest = null;
            float shortestDist = Mathf.Infinity;

            foreach (var e in objects)
            {
                Vector3 epos = new Vector3(e.worldPos.x, e.worldPos.y, e.worldPos.z);
                float distance = Vector3.Distance(mousePos, epos);
                if (distance < shortestDist)
                {
                    shortestDist = distance;
                    closest = e;
                }
            }

            Debug.Log(objects.Length);

            Debug.Log(closest.gridPos.x + "," + closest.gridPos.y);
            
        }
    }
}
