using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;


public class PathfindingSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    private bool recalculatePath = true;
    private Contexts _contexts;
    private GameEntity _spawnpoint;
    private GameEntity _target;
    private int _rows;
    private int _columns;

    public PathfindingSystem(Contexts contexts, int rows, int columns) : base(contexts.game)
    {
        _contexts = contexts;
        _rows = rows;
        _columns = columns;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Buiding);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isHex;
    }


    // EXECUTE
    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in GameController.StartPath)
        {
            if (e.hasBuiding)
                recalculatePath = true;
        }

        if (recalculatePath == true)
        {
            //Get all hexes
            var entitiesArray = _contexts.game.GetEntities(GameMatcher.Hex);
            var enemiesArray = _contexts.game.GetEntities(GameMatcher.Enemy);

            _spawnpoint = entitiesArray[0];
            _target = entitiesArray[(_columns - 1) * _rows + _rows - 1];

            if (enemiesArray != null)
            {
                foreach (GameEntity enemy in enemiesArray)
                {
                    enemy.ReplacePath(0, FindPath(entitiesArray, enemy));
                    enemy.move.direction = CalculatedDirection(enemy.path.Path[0], enemy);
                }
            }

            GameController.StartPath = FindPath(entitiesArray, _spawnpoint);
            Clear();
        }
    }

    // INIT
    public void Initialize()
    {
        //Get all hexes
        var entitiesArray = _contexts.game.GetEntities(GameMatcher.Hex);


        _spawnpoint = entitiesArray[0];
        _target = entitiesArray[(_columns - 1) * _rows + _rows - 1];

        GameController.StartPath = FindPath(entitiesArray, _spawnpoint);
    }


    private List<GameEntity> FindPath(GameEntity[] entities, GameEntity startPosition)
    {
        //The Path to return
        List<GameEntity> finalPath = new List<GameEntity>();

        // Array of all tiles
        var _tileArray = entities;


        //Dictionary with pathvalues
        Dictionary<GameEntity, float> pathValues = new Dictionary<GameEntity, float>();

        // BACKSTACK
        PriorityQueue<GameEntity> queue = new PriorityQueue<GameEntity>((left, right) =>
        {
            return pathValues[left].CompareTo(pathValues[right]);

        });

        Dictionary<GameEntity, GameEntity> parents = new Dictionary<GameEntity, GameEntity>();


        bool[] visited = new bool[_tileArray.Length];

        //Start van de start positie
        GameEntity start = startPosition;
        queue.Enqueue(start); //2b
        pathValues[start] = 0;

        bool found = false;
        while (queue.Count > 0 && !found) // 3 loop doorheen de stack
        {
            GameEntity current = queue.Dequeue();

            var unvisitedNeighbours = FilterNeighbours(current, visited, _tileArray);
            foreach (var neighbour in unvisitedNeighbours)
            {

                pathValues[neighbour] = pathValues[current] + neighbour.distance.Distance;
                if (neighbour.hasBuiding)
                    pathValues[neighbour] /= 4;
                queue.Enqueue(neighbour);
                parents[neighbour] = current;

            }

            visited[ current.gridPos.x * _rows + current.gridPos.y] = true;

            if (current == _target)
            {
                found = true;
            }
        }


        GameEntity pathPart = _target;
        while (pathPart != startPosition)
        {
            finalPath.Add(pathPart);
            pathPart = parents[pathPart];
        }

        finalPath.Reverse();
        recalculatePath = false;

        return finalPath;

    }

    private List<GameEntity> FilterNeighbours(GameEntity current, bool[] visited, GameEntity[] tileArray)
    {
        int row = current.gridPos.y;
        int column = current.gridPos.x;

        List<GameEntity> UnvisitedNeighbours = new List<GameEntity>(6);

        int arrayPos = 0;

        // ODD COLUMNS
        if (column % 2 == 1)
        {
            arrayPos = (column - 1) * _rows + row;
            if (!visited[arrayPos] && tileArray[arrayPos].isWalkAble)
            {
                UnvisitedNeighbours.Add(tileArray[arrayPos]);
            }

            arrayPos = (column - 1) * _rows + row + 1;
            if (row < _rows - 1 && !visited[arrayPos] && tileArray[arrayPos].isWalkAble)
            {
                UnvisitedNeighbours.Add(tileArray[arrayPos]);
            }

            arrayPos = (column + 1) * _rows + row;
            if (column < _columns - 1 && !visited[arrayPos] && tileArray[arrayPos].isWalkAble)
            {
                UnvisitedNeighbours.Add(tileArray[arrayPos]);
            }

            arrayPos = (column + 1) * _rows + row + 1;
            if (column < _columns - 1 && row < _rows - 1 && !visited[arrayPos] && tileArray[arrayPos].isWalkAble)
            {
                UnvisitedNeighbours.Add(tileArray[arrayPos]);
            }

            arrayPos = column * _rows + row + 1;
            if (row < _rows - 1 && !visited[arrayPos] && tileArray[arrayPos].isWalkAble)
            {
                UnvisitedNeighbours.Add(tileArray[arrayPos]);
            }

            arrayPos = column * _rows + row - 1;
            if (row > 0 && !visited[arrayPos] && tileArray[arrayPos].isWalkAble)
            {
                UnvisitedNeighbours.Add(tileArray[arrayPos]);
            }
        }

        // EVEN COLUMNS     
        else
        {
            arrayPos = (column - 1) * _rows + row - 1;
            if ( column > 0 && row > 0 && !visited[arrayPos] && tileArray[arrayPos].isWalkAble)
            {
                UnvisitedNeighbours.Add(tileArray[arrayPos]);
            }

            arrayPos = (column - 1) * _rows + row;
            if (column > 0 && !visited[arrayPos] && tileArray[arrayPos].isWalkAble)
            {
                UnvisitedNeighbours.Add(tileArray[arrayPos]);
            }

            arrayPos = (column + 1) * _rows + row - 1;
            if (column < _columns - 1 && row > _rows && column < _columns && !visited[arrayPos])
            {
                UnvisitedNeighbours.Add(tileArray[arrayPos]);
            }

            arrayPos = (column + 1) * _rows + row;

            if (column < _columns - 1 && row < _rows && !visited[arrayPos] && tileArray[arrayPos].isWalkAble)
            {
                UnvisitedNeighbours.Add(tileArray[arrayPos]);
            }

            arrayPos = column * _rows + row + 1;
            if (row < _rows - 1 && !visited[arrayPos] && tileArray[arrayPos].isWalkAble)
            {
                UnvisitedNeighbours.Add(tileArray[arrayPos]);
            }

            arrayPos = column * _rows + row - 1;
            if (row > 0 && !visited[arrayPos] && tileArray[arrayPos].isWalkAble)
            {
                UnvisitedNeighbours.Add(tileArray[arrayPos]);
            }
        }

        return UnvisitedNeighbours;
    }

    private Vector3 CalculatedDirection(GameEntity target, GameEntity start)
    {
        Vector3 direction;

        Vector3 targetPos = target.vectorPos.Position;
        Vector3 startPos = start.vectorPos.Position;

        direction = (targetPos - startPos).normalized;

        return direction; 
    }

}
