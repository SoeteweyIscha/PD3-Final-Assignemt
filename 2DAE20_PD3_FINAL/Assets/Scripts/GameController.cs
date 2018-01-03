using UnityEngine;
using Entitas;

public class GameController : MonoBehaviour {

    //Game variables, these influence the game
    public int GridWidth;
    public int GridHeight;
    public float MaxSelectDist; // The maximum distance allowed between the mouse and a hexagon in order to select it,
                                // this value is given to the SelectButtonSystem
    
    public Building _buildingState; // Public for testing
    public Transform TestTarget; //public for testing

    //Bascis needed for behind the scenes
    private Systems _systems;


    
    
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
	}

    private void Update()
    {
        _systems.Execute();
    }

    private Systems CreateSystems( Contexts contexts )
    {
        return new Feature("Game")
            .Add(new GridGenerationSystem(contexts, GridWidth, GridHeight))
            .Add(new GridViewSystem(contexts))
            .Add(new PlaceObjectSystem(contexts, MaxSelectDist, this))
            .Add(new TowerConstructorSystem(contexts))
            .Add(new TargetingSystem(contexts, TestTarget))
            .Add(new PathfindingSystem(contexts, GridHeight, GridWidth))
            ;

    }

    public enum Building
    {
        Tower = 0,
        Wall = 1
    }


}
