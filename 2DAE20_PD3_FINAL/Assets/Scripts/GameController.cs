using UnityEngine;
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
    static public int Money = 100;
    static public GameEntity startTile;

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

        Debug.Log("hi there");

        GetValuesOnStart();

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
        }
    }
}
