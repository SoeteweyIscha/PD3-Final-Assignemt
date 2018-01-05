using System;
using Entitas;
using UnityEngine;

public class ShootSystem : IExecuteSystem, IInitializeSystem {

    private Contexts _contexts;
    private int _turretRange;
    private GameObject _bullet;
    private GameObject _empty;

    public ShootSystem(Contexts contexts, int turretRange, GameObject bullet)
    {
        _contexts = contexts;
        _turretRange = turretRange;
        _bullet = bullet;
    }

    public void Execute()
    {
        var towers = _contexts.game.GetEntities(GameMatcher.Shoot);
        var enemies = _contexts.game.GetEntities(GameMatcher.Enemy);

        if (towers.Length != 0 && enemies.Length != 0)
        {

            foreach (GameEntity tower in towers)
            {
                foreach (GameEntity enemy in enemies)
                {
                    float dist = StaticFunctions.SqrDistance(tower.baseTile.tile.vectorPos.Position, enemy.vectorPos.Position);
                    if (dist < _turretRange * _turretRange)
                    {

                        Vector3 aimDirection = tower.view.View.gameObject.transform.forward;
                        Vector3 spawnPos = tower.view.View.transform.position + aimDirection;

                        GameObject bulletObject = GameObject.Instantiate<GameObject>(_bullet, spawnPos, Quaternion.LookRotation(aimDirection), _empty.transform);
                        Color c = bulletObject.GetComponent<Renderer>().material.color;

                        //Makebullet;
                        GameEntity temp = _contexts.game.CreateEntity();
                        temp.isBullet = true;
                        temp.AddVectorPos(spawnPos);
                        temp.AddMove(15, aimDirection);
                        temp.AddView(bulletObject, c);


                        Debug.Log("Shot");
                        tower.isShoot = false;
                        tower.AddTimer(0, GameController.TurretReloadTime);
                        break;
                    }
                }
            }

        }
    }

    public void Initialize()
    {
        _empty = new GameObject("BulletShell");
    }
}
