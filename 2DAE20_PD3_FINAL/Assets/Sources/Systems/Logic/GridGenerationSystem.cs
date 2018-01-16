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
        Vector3 TargetPos = new Vector3((_width - 1) * _widthOffSet, 0, (_height - 1) * _heightOffSet);
        for (int w = 0; w < _width; w++)
        {
            for (int h = 0; h < _height; h++)
            {
                
                // Create entities 
                var entity = _context.game.CreateEntity();
                entity.AddGridPos(w, h);

                
                // Give the entities a vectorPos, using modulo function to give correct offset
                entity.AddVectorPos(new Vector3(w * _widthOffSet   , 0, h * _heightOffSet + w % 2 * _heightOffSet / 2));
                float dist = StaticFunctions.SqrDistance(Vector3.zero, entity.vectorPos.Position) + (w + h);
                entity.AddDistance(dist);
                Debug.Log(dist);
                entity.isHex = true;
                entity.isWalkAble = true;
                entity.isClick = true;

                if (w == _width-1 && h == _height-1)
                {
                    entity.isHomeBase = true;
                    entity.isClick = false;
                    entity.AddHealth(20);
                }
            }
        }
    }
}
