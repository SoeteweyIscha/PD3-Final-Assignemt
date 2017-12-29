using Entitas;
using UnityEngine;


public class GridGenerationSystem : IInitializeSystem {

    private Contexts _context;
    private int _width;
    private int _height;

    private float _widthOffSet = 1.6f;
    private float _heightOffSet = 1.8f;

    public GridGenerationSystem(Contexts context, int x, int z)
    {
        _context = context;
        _width = x;
        _height = z;
    }

    public void Initialize()
    {
        for (int w = 0; w < _width; w++)
        {
            for (int h = 0; h < _height; h++)
            {
                var entity = _context.game.CreateEntity();
                entity.AddGridPos(w, h);
                entity.AddWorldPos(w * _widthOffSet   , 0, h * _heightOffSet + w % 2 * _heightOffSet / 2);
                entity.isHex = true;
            }
        }
    }
}
