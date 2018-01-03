using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;


public class PathfindingSystem : ReactiveSystem<GameEntity>, IInitializeSystem
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
        var enemiesArray = _contexts.game.GetEntities(GameMatcher.Enemy);

        _spawnpoint = entitiesArray[0];
        _target = entitiesArray[9 * _rows + 9];
        Vector3 targetPos = _target.vectorPos.Position; 

        //Add distanceComponent
        foreach (GameEntity e in entitiesArray)
        {
            Vector3 localPos = e.vectorPos.Position;
            e.AddDistance( Vector3.Distance(localPos, targetPos));
        }

        if (enemiesArray != null)
        {
            foreach (GameEntity enemy in enemiesArray)
            {
                enemy.ReplacePath(0, FindPath(entitiesArray, enemy));
            }
        }

        GameController.StartPath = FindPath(entitiesArray, _spawnpoint);

        //Remove DistanceComponent
        foreach (GameEntity e in entitiesArray)
        {
            e.RemoveDistance();
        }
        Clear();
    }


    public void Initialize()
    {
        //Get all hexes
        var entitiesArray = _contexts.game.GetEntities(GameMatcher.Hex);


        _spawnpoint = entitiesArray[0];
        _target = entitiesArray[9 * _rows + 9];
        Debug.Log(entitiesArray[3 * _rows + 3]);
        Vector3 targetPos = _target.vectorPos.Position;

        foreach (GameEntity e in entitiesArray)
        {
            Vector3 localPos = e.vectorPos.Position;
            e.AddDistance(Vector3.Distance(localPos, targetPos));
        }

        GameController.StartPath = FindPath(entitiesArray, _spawnpoint);


        //Remove DistanceComponent
        foreach (GameEntity e in entitiesArray)
        {
            e.RemoveDistance();
        }
    }


    private List<GameEntity> FindPath(GameEntity[] entities, GameEntity startPosition)
    {
        List<GameEntity> finalPath = new List<GameEntity>();

        var _tileArray = entities;

        foreach (var e in _tileArray)
        {
            e.view.View.GetComponent<Renderer>().material.color = e.view.BaseColor;
        }

        Dictionary<GameEntity, float> pathValues = new Dictionary<GameEntity, float>();

        PriorityQueue<GameEntity> queue = new PriorityQueue<GameEntity>((left, right) =>
        {
            //var distance1 = Mathf.Sqrt(Mathf.Pow(left.Row - _endCell.Row, 2) + Mathf.Pow(left.Column - _endCell.Column,2));
            //var distance2 = Mathf.Sqrt(Mathf.Pow(right.Row - _endCell.Row, 2) + Mathf.Pow(right.Column - _endCell.Column, 2));

            //return distance1.CompareTo(distance2);

            return pathValues[left].CompareTo(pathValues[right]);

        });


        //Queue<GameEntity> queue = new Queue<GameEntity>(); //1. backstack

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
            pathPart.view.View.GetComponent<Renderer>().material.color = Color.red;
            finalPath.Add(pathPart);
            pathPart = parents[pathPart];
        }

        finalPath.Reverse();

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
