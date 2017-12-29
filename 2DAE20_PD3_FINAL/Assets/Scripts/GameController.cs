using UnityEngine;
using Entitas;

public class GameController : MonoBehaviour {

    public int GridWidth;
    public int GridHeight;

    private Systems _systems;
	// Use this for initialization
	void Start ()
    {
        var contexts = Contexts.sharedInstance;

        _systems = CreateSystems( contexts );

        _systems.Initialize();
	}

    private Systems CreateSystems( Contexts contexts )
    {
        return new Feature("Game")
            .Add(new GridGenerationSystem( contexts, GridWidth, GridHeight ))
            .Add(new GridViewSystem(contexts));

    }
}
