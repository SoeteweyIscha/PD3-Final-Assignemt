﻿using Entitas;
using UnityEngine;

public class PlaceObjectSystem : IExecuteSystem
{

    private Contexts _contexts;
    private IGroup<GameEntity> _clickables;
    private float _maxSelectRange;
    private GameController _gameController;
    private GameController.Building _stateToSet;
    private float _turretReloadTime;


    public PlaceObjectSystem(Contexts contexts, float range, GameController gameController, float turretReloadTime)
    {
        _contexts = contexts;
        
        _maxSelectRange = range;
        _gameController = gameController;
        _turretReloadTime = turretReloadTime;
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


            //Prepares the tile for a an object to be place upon it
            if (closest != null && GameController.Money >= 50)
            {
                //closest.AddBuiding(_stateToSet);
                closest.isClick = false;

                GameEntity temp = _contexts.game.CreateEntity();
                temp.AddVectorPos(closest.vectorPos.Position);
                temp.AddGridPos(closest.gridPos.x, closest.gridPos.y);

                switch (_stateToSet)
                {
                    case GameController.Building.Tower:
                        temp.isTower = true;
                        temp.AddHealth(3);
                        temp.isTargeting = true;
                        temp.AddTimer(0, _turretReloadTime);

                        //Subtract Money
                        GameController.Money -= 50;
                        break;

                    case GameController.Building.Wall:
                        temp.isWall = true;
                        temp.AddHealth(6);

                        //Subtract Money
                        GameController.Money -= 40;
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
