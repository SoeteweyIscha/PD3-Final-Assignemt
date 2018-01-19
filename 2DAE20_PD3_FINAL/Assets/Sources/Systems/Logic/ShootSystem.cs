using System;
using Entitas;
using UnityEngine;

public class ShootSystem : IExecuteSystem, IInitializeSystem {

    private Contexts _contexts;
    private int _turretRange;
    private int _turretDamage;
    private int _sniperRange;
    private int _sniperDamage;
    private GameObject _bullet;
    private GameObject _empty;

    public ShootSystem(Contexts contexts, int turretRange, GameObject bullet, int turretDamage)
    {
        _contexts = contexts;
        _turretRange = turretRange;
        _bullet = bullet;
        _sniperRange = 2 * _turretRange;
        _turretDamage = turretDamage;
        _sniperDamage = 2 * turretDamage + 2;
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
                    float dist = StaticFunctions.SqrDistance(tower.vectorPos.Position, enemy.vectorPos.Position);

                    if (tower.isTower && dist < _turretRange * _turretRange)
                    {

                        Vector3 aimDirection = tower.view.View.gameObject.transform.forward;
                        Vector3 spawnPos = tower.view.View.transform.position + aimDirection;

                        GameObject bulletObject = GameObject.Instantiate<GameObject>(_bullet, spawnPos, Quaternion.LookRotation(aimDirection), _empty.transform);
                        Color c = bulletObject.GetComponent<Renderer>().material.color;

                        //Makebullet;
                        GameEntity temp = _contexts.game.CreateEntity();

                        temp.AddBullet(_turretDamage);
                        temp.AddMove(15, aimDirection);

                        temp.AddVectorPos(spawnPos);

                        temp.AddView(bulletObject, c);


                        tower.isShoot = false;
                        tower.AddTimer(0, GameController.TurretReloadTime);
                        break;
                    }

                    else if (tower.isSniper && dist < _sniperRange * _sniperRange)
                    {
                        Vector3 aimDirection = tower.view.View.gameObject.transform.forward;
                        Vector3 spawnPos = tower.view.View.transform.position + aimDirection;

                        GameObject bulletObject = GameObject.Instantiate<GameObject>(_bullet, spawnPos, Quaternion.LookRotation(aimDirection), _empty.transform);
                        Color c = bulletObject.GetComponent<Renderer>().material.color;

                        //Makebullet;
                        GameEntity temp = _contexts.game.CreateEntity();

                        temp.AddBullet(_sniperDamage);
                        temp.AddMove(30, aimDirection);

                        temp.AddVectorPos(spawnPos);

                        temp.AddView(bulletObject, c);


                        tower.isShoot = false;
                        tower.AddTimer(0, GameController.TurretReloadTime * 1.5f);
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
