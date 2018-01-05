﻿using UnityEngine;
using Entitas;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    //Game variables, these influence the game

    public int GridWidth;
    public int GridHeight;
    public int TurretRange = 5;

    public float RocksVariable = 10; // Influences the amount of rocks placed in the scene
    public float MaxSelectDist; // The maximum distance allowed between the mouse and a hexagon in order to select it,
                                // this value is given to the SelectButtonSystem
    
    public Building _buildingState; // Public for testing
    public Transform TestTarget; //public for testing

    //Bascics needed for behind the scenes
    private Systems _systems;

    //Globals
    public static List<GameEntity> StartPath = new List<GameEntity>();


    public Building BuildingState
    {
        get { return _buildingState; }
        set { _buildingState = value; }
    }

	void Start ()
    {
        var contexts = Contexts.sharedInstance;

        _systems = CreateSystems( contexts );

        _systems.Initialize();

        var entity = contexts.game.CreateEntity();

        entity.isEnemy = true;
        entity.AddGridPos(0, 0);
        entity.AddVectorPos(Vector3.zero);
        entity.AddHealth(1);
        entity.AddPath(0, StartPath);
        entity.isTargeting = true;
        entity.AddMove(1, Vector3.zero);

        GameObject pre = Resources.Load<GameObject>("ShipPrefab");
        GameObject temp = GameObject.Instantiate(pre);
        entity.AddView(temp, temp.GetComponent<Renderer>().material.color);
    }

    private void Update()
    {
        _systems.Execute();
        
    }

    private Systems CreateSystems( Contexts contexts )
    {
        return new Feature("Game")

            // INITIALISE
            .Add(new GridGenerationSystem(contexts, GridWidth, GridHeight))
            .Add(new GridViewSystem(contexts))

            //EXECUTE & REACTIVE
            .Add(new PlaceObjectSystem(contexts, MaxSelectDist, this))
            .Add(new TowerConstructorSystem(contexts))
            .Add(new PathfindingSystem(contexts, GridHeight, GridWidth))
            .Add(new RockGenerationSystem(contexts, RocksVariable))
            .Add(new TargetingSystem(contexts, TurretRange))
            .Add(new EnemyMovementSystem(contexts))
            .Add(new MoveSystem(contexts))
            ;

    }

    public enum Building
    {
        Tower = 0,
        Wall = 1,
    }


}
