﻿using UnityEngine;
using Entitas;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    //Game variables, these influence the game
    public int GridWidth;
    public int GridHeight;
    public int TurretRange = 2;
    static public float TurretReloadTime = 1.2f;
    public int publicMoney;
    public GameObject Bullet;
    public static bool gameLoop = true;
    public float RocksVariable = 10; // Influences the amount of rocks placed in the scene
    public float MaxSelectDist; // The maximum distance allowed between the mouse and a hexagon in order to select it
    private float _widthOffSet = 1.6f;
    private float _heightOffSet = 1.8f;

    [SerializeField]
    private GameObject _gameOverMenu;
    
    // this value is given to the SelectButtonSystem
    public Building _buildingState; // Public for testing

    //Bascics needed for behind the scenes
    private Systems _systems;
    private Contexts _gameContext;

    private bool _coroutineStarted = false;

    //Globals
    static public List<GameEntity> StartPath = new List<GameEntity>();
    static public int Money = 120;
    static public GameEntity startTile;
    static public int Difficulty;
    static public int Counter;

    //UI
    [SerializeField]
    private Text _money;

    public Building BuildingState
    {
        get { return _buildingState; }
        set { _buildingState = value; }
    }

	void Awake ()
    {
        gameLoop = true;
        StartPath = null;

        GetValuesOnStart();

        PlaceCamera();

        var contexts = Contexts.sharedInstance;

        _gameContext = contexts;

        _systems = CreateSystems( contexts );

        _systems.Initialize();

    }

    private void Update()
    {
        if (gameLoop)
        {
            _systems.Execute();
            publicMoney = Money;
            SetUIMoney();
            DifficultyHandler();
        }

        else
        {
            StopCoroutine("CreateEnemies");
            
            _systems.ClearReactiveSystems();
            _gameContext.game.Reset();
            _gameContext.game.DestroyAllEntities();
            _systems.TearDown();

            _gameOverMenu.SetActive(true);
        }

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
            .Add(new ShootSystem(contexts, TurretRange, Bullet, 2))
            .Add(new ColliderSystem(contexts, GridWidth, GridHeight))
            .Add(new HealthSystem(contexts))
            .Add(new DestroySystem(contexts))
            ;

    }

    public enum Building
    {
        Tower = 0,
        Wall = 1,
        Sniper = 2
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

    private void GetValuesOnStart()
    {
        GameObject capsule = GameObject.FindGameObjectWithTag("Data");
        if (capsule != null)
        {
            ParsingScript script = capsule.GetComponent<ParsingScript>();

            GridWidth = script.Width;
            GridHeight = script.Height;
            RocksVariable = script.RocksVariable;
        }

    }

    public void SetUIMoney()
    {
        _money.text = Money.ToString();
    }

    public void ChangeBuildingVar(int value)
    {
        switch (value)
        {
            case 1:
                _buildingState = Building.Tower;
                break;
            case 0:
                _buildingState = Building.Wall;
                break;
            case 2:
                _buildingState = Building.Sniper;
                break;
        }
    }

    private void PlaceCamera()
    {
        Vector3 cameraPos = Vector3.zero;
        Camera mainCam = Camera.main;

        float xPos = (_widthOffSet * GridWidth) * 0.5f;
        float yPos = (_heightOffSet * GridHeight) * 0.5f;

        cameraPos = new Vector3(xPos, 20, yPos);

        mainCam.transform.position = cameraPos;
        if (GridHeight > GridWidth)
            mainCam.orthographicSize = GridHeight + 1;

        else
            mainCam.orthographicSize = GridWidth;
    }

    private void DifficultyHandler()
    {
        if (Counter > 10)
        {
            Difficulty += 3;
            Counter = 0;
        }
    }
}
