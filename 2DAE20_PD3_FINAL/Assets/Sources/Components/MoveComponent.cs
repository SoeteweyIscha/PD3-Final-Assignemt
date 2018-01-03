using Entitas;
using UnityEngine;

[Game]
public class MoveComponent : IComponent
{
    public int speed;
    public Vector3 direction;
}
