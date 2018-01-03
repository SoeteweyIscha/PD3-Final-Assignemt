using Entitas;
using UnityEngine;

public class SelectButtonSystem : IExecuteSystem
{

    private Contexts _contexts;
    private IGroup<GameEntity> _clickables;
    private float _maxSelectRange;

    public SelectButtonSystem(Contexts contexts, float range)
    {
        _contexts = contexts;
        _clickables = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Click));
        _maxSelectRange = range;
    }

    public void Execute()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //list of clickable entities
            var objects = _clickables.GetEntities();

            //Making the mousepos a usable location
            var mousePos = Input.mousePosition;
            mousePos.z = 10.0f;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            //Variables for the closest object
            GameEntity closest = null;
            float shortestDist = Mathf.Infinity;

            //Find closest clickable entity in scene
            foreach (var e in objects)
            {
                Vector3 epos = e.worldPos.Position;
                float distance = Vector3.Distance(mousePos, epos);
                if (distance < shortestDist && distance < _maxSelectRange)
                {
                    shortestDist = distance;
                    closest = e;
                }
            }

            //Debugging stuff, wil be removed once player feedback is implemented
            if (closest != null)
            {

                closest.isClick = false;
                closest.isSelected = true;

            }

            else
            {
                Debug.Log("Nothing selected");
            }
            
        }
    }
}
