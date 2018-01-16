﻿using UnityEngine;
using Entitas;
using System.Collections.Generic;
using System.Collections;

public class GameController : MonoBehaviour {

    //Game variables, these influence the game

    public int GridWidth;
    public int GridHeight;
    public int TurretRange = 2;
    static public float TurretReloadTime = 1.5f;
    static public int Money = 100;
    public int publicMoney;
    public GameObject Bullet;

    public float RocksVariable = 10; // Influences the amount of rocks placed in the scene
    public float MaxSelectDist; // The maximum distance allowed between the mouse and a hexagon in order to select it,
                                // this value is given to the SelectButtonSystem
    
    public Building _buildingState; // Public for testing
    public Transform TestTarget; //public for testing

    //Bascics needed for behind the scenes
    private Systems _systems;
    private Contexts _gameContext;

    private bool _coroutineStarted = false;

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

        _gameContext = contexts;

        _systems = CreateSystems( contexts );

        _systems.Initialize();
    }

    private void Update()
    {
        _systems.Execute();
        publicMoney = Money;

        if (!_coroutineStarted)
        {
            StartCoroutine("CreateEnemies");
            _coroutineStarted = true;
        }
    }

    private Systems CreateSystems( Contexts contexts )
    {
        return new Feature("Game")

            // INITIALISE
            .Add(new GridGenerationSystem(contexts, GridWidth, GridHeight))
            //.Add(new GridViewSystem(contexts))

            //EXECUTE & REACTIVE
            .Add(new EnemyBuilderSystem(contexts))
            .Add(new PlaceObjectSystem(contexts, MaxSelectDist, this, TurretReloadTime))
            //.Add(new TowerConstructorSystem(contexts, TurretReloadTime))
            .Add(new PathfindingSystem(contexts, GridHeight, GridWidth))
            .Add(new RockGenerationSystem(contexts, RocksVariable))
            .Add(new EnemyMovementSystem(contexts))
            .Add(new ViewSystem(contexts))
            .Add(new MoveSystem(contexts))
            .Add(new TargetingSystem(contexts, TurretRange))
            .Add(new TimerSystem(contexts))
            .Add(new ShootSystem(contexts, TurretRange, Bullet))
            .Add(new ColliderSystem(contexts, GridWidth, GridHeight))
            .Add(new HealthSystem(contexts))
            .Add(new DestroySystem(contexts))
            ;

    }

    public enum Building
    {
        Tower = 0,
        Wall = 1,
    }

    public IEnumerator CreateEnemies()
    {
        for (;;)
        {
            GameEntity spawner = _gameContext.game.CreateEntity();
            spawner.isBuildEnemy = true;
            yield return new WaitForSeconds(1.5f);
        }
    }

}
