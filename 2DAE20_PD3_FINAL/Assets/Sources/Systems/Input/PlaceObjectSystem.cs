using Entitas;
using UnityEngine;

public class PlaceObjectSystem : IExecuteSystem
{

    private Contexts _contexts;
    private IGroup<GameEntity> _clickables;
    private float _maxSelectRange;
    private GameController _gameController;
    private GameController.Building _stateToSet;
    private float _turretReloadTime;
    private float _sniperReloadTime;


    public PlaceObjectSystem(Contexts contexts, float range, GameController gameController, float turretReloadTime)
    {
        _contexts = contexts;
        
        _maxSelectRange = range;
        _gameController = gameController;
        _turretReloadTime = turretReloadTime;
        _sniperReloadTime = 2 * turretReloadTime;
    }

    public void Execute()
    {

        _clickables = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Click));
        //Set State
        _stateToSet = _gameController.BuildingState;


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
                //Make Vector from variables
                Vector3 epos = e.vectorPos.Position;

                float distance = Vector3.Distance(mousePos, epos);
                if (distance < shortestDist && distance < _maxSelectRange)
                {
                    shortestDist = distance;
                    closest = e;
                    
                }
            }
            Debug.Log(shortestDist);

            //Prepares the tile for a an object to be place upon it
            if (closest != null)
            {
                //closest.AddBuiding(_stateToSet);
                closest.isClick = false;
                

                

                switch (_stateToSet)
                {
                    case GameController.Building.Tower:
                        if (GameController.Money >= 50)
                        {
                            GameEntity temp = _contexts.game.CreateEntity();
                            temp.AddVectorPos(closest.vectorPos.Position);
                            temp.AddGridPos(closest.gridPos.x, closest.gridPos.y);
                            closest.AddBuiding(GameController.Building.Tower);
                            temp.isTower = true;
                            temp.isTargeting = true;
                            temp.AddTimer(0, _turretReloadTime);

                            //Subtract Money
                            GameController.Money -= 50;
                        }
                        break;

                    case GameController.Building.Sniper:
                        if (GameController.Money >= 100)
                        {
                            GameEntity sniper = _contexts.game.CreateEntity();
                            sniper.AddVectorPos(closest.vectorPos.Position);
                            sniper.AddGridPos(closest.gridPos.x, closest.gridPos.y);
                            closest.AddBuiding(GameController.Building.Sniper);
                            sniper.isSniper = true;
                            sniper.isTargeting = true;
                            sniper.AddTimer(0, _sniperReloadTime);

                            GameController.Money -= 100;
                        }
                        break;


                    case GameController.Building.Wall:
                        if (GameController.Money >= 40)
                        {
                            GameEntity wall = _contexts.game.CreateEntity();
                            wall.AddVectorPos(closest.vectorPos.Position);
                            wall.AddGridPos(closest.gridPos.x, closest.gridPos.y);
                            closest.AddBuiding(GameController.Building.Wall);
                            wall.AddHealth(2, null);
                            wall.isWall = true;

                            //Subtract Money
                            GameController.Money -= 40;
                        }
                        break;
                }
            }

            //text for clarity
            else
            {
                Debug.Log("Nothing selected");
            }
            
        }
    }
}
