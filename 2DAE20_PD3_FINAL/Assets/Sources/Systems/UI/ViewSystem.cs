using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using UnityEngine.UI;
using System;

public class ViewSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{

    private Contexts _contexts;
    private GameObject _towerModel;
    private GameObject _wallModel;
    private GameObject _hex;
    private GameObject _enemy;
    private GameObject _rock;
    private GameObject _homeBase;

    //GameObjects to work as parents for the objects in the scene
    private GameObject _towerShell;
    private GameObject _wallShell;
    private GameObject _hexShell;
    private GameObject _enemyShell;
    private GameObject _rockShell;

    public ViewSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    } 

    public void Initialize()
    {
        //Load all Models on startup
        _towerModel = Resources.Load<GameObject>("Tower");
        _wallModel = Resources.Load<GameObject>("Wall");
        _hex = Resources.Load<GameObject>("Hex");
        _enemy = Resources.Load<GameObject>("Ship");
        _rock = Resources.Load<GameObject>("Rock");
        _homeBase = Resources.Load<GameObject>("Base");

        _towerShell = new GameObject("towerSHELL");
        _wallShell = new GameObject("wallSHELL");
        _hexShell = new GameObject("HexSHELL");
        _enemyShell = new GameObject("EnemySHELL");
        _rockShell = new GameObject("RockSHELL");

    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf    (   
                                                            GameMatcher.Hex, 
                                                            GameMatcher.Wall, 
                                                            GameMatcher.Tower, 
                                                            GameMatcher.Enemy, 
                                                            GameMatcher.Bullet, 
                                                            GameMatcher.Rock,
                                                            GameMatcher.HomeBase)
                                                            );
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasVectorPos;
    }


    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (!e.hasView)
            {
                if (e.isTower)
                {
                    GameObject temp = GameObject.Instantiate(_towerModel, e.vectorPos.Position, Quaternion.identity, _towerShell.transform);
                    e.AddView(temp, temp.GetComponentInChildren<Renderer>().material.color);
                }

                else if (e.isWall)
                {
                    GameObject temp = GameObject.Instantiate(_wallModel, e.vectorPos.Position, Quaternion.identity, _wallShell.transform);
                    e.AddView(temp, temp.GetComponentInChildren<Renderer>().material.color);
                }

                else if (e.isHomeBase)
                {
                    GameObject temp = GameObject.Instantiate(_homeBase, e.vectorPos.Position, Quaternion.identity);
                    e.AddView(temp, temp.GetComponentInChildren<Renderer>().material.color);
                    e.health.Display = temp.GetComponentInChildren<Text>();
                }

                else if (e.isRock)
                {
                    GameObject temp = GameObject.Instantiate(_rock, e.vectorPos.Position, Quaternion.identity, _rockShell.transform);
                    e.AddView(temp, temp.GetComponentInChildren<Renderer>().material.color);
                }

                else if (e.isHex)
                {
                    GameObject temp = GameObject.Instantiate(_hex, e.vectorPos.Position, Quaternion.identity, _hexShell.transform);
                    e.AddView(temp, temp.GetComponent<Renderer>().material.color);
                }

                else if (e.isEnemy)
                {
                    GameObject temp = GameObject.Instantiate(_enemy, e.vectorPos.Position, Quaternion.identity, _enemyShell.transform);
                    e.AddView(temp, temp.GetComponent<Renderer>().material.color);
                    e.health.Display = temp.GetComponentInChildren<Text>();
                }
            }
        }
        Clear();
    }

}
