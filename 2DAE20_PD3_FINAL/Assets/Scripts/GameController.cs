using UnityEngine;
using Entitas;

public class GameController : MonoBehaviour {

    public int GridWidth;
    public int GridHeight;
    public float MaxSelectDist; // The maximum distance allowed between the mouse and a hexagon in order to select it,
                                // this value is given to the SelectButtonSystem
    private Systems _systems;

    public Building _buildingState; // Public for testing
    
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
            ;

    }

    public enum Building
    {
        Tower = 0,
        Wall = 1
    }


}
