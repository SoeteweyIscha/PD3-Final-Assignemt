using Entitas;
using System.Collections.Generic;

[Game]
public class PathComponent : IComponent {

    public int CurrentNode;
    public List<GameEntity> Path;

}
