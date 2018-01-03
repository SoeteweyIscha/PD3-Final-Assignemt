using System.Collections.Generic;
using Entitas;
using UnityEngine;


public class PathfindingSystem : ReactiveSystem<GameEntity>
{

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

    protected override void Execute(List<GameEntity> entities)
    {
        //Get all hexes
        var entitiesArray = _contexts.game.GetEntities(GameMatcher.Hex);
        FindPath(entitiesArray);
        Clear();
    }

    private void FindPath(GameEntity[] entities)
    {
        var _tileArray = entities;
        foreach (var e in _tileArray)
        {
            e.view.View.GetComponent<Renderer>().material.color = e.view.BaseColor;
        }
        

        _spawnpoint = _tileArray[0];
        _target = _tileArray[9 * _rows + 9];
        Debug.Log(_tileArray[3 * _rows + 3]);

        Queue<GameEntity> queue = new Queue<GameEntity>(); //1. backstack

        Dictionary<GameEntity, GameEntity> parents = new Dictionary<GameEntity, GameEntity>();


        bool[] visited = new bool[_tileArray.Length];

        //Start van de start positie
        GameEntity start = _spawnpoint;
        queue.Enqueue(start); //2b

        bool found = false;
        while (queue.Count > 0 && !found) // 3 loop doorheen de stack
        {
            GameEntity current = queue.Dequeue();

            var unvisitedNeighbours = FilterNeighbours(current, visited, _tileArray);
            foreach (var neighbour in unvisitedNeighbours)
            {
                queue.Enqueue(neighbour);
                parents[neighbour] = current;

            }

            visited[ current.gridPos.x * _rows + current.gridPos.y] = true;

            if (current == _target)
            {
                found = true;
                Debug.Log("Found It");
                Debug.Log(queue.Count);
                foreach (var e in queue);
            }
        }

        GameEntity pathPart = _target;
        while (pathPart != _spawnpoint)
        {
            pathPart.view.View.GetComponent<Renderer>().material.color = Color.red;
            pathPart = parents[pathPart];
        }

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
            if (row < _rows - 1 && !visited[arrayPos] & tileArray[arrayPos].isWalkAble)
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
            if (column > 0 && !visited[arrayPos] & tileArray[arrayPos].isWalkAble)
            {
                UnvisitedNeighbours.Add(tileArray[arrayPos]);
            }

            arrayPos = (column + 1) * _rows + row - 1;
            if (column < _columns - 1 && row > _rows && column < _columns && !visited[arrayPos] && tileArray[arrayPos].isWalkAble)
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

}
