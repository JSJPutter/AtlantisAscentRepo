using UnityEngine;

public class StaticObstacle : Obstacle
{
    protected override void Start()
    {
        base.Start();

        isDestructible = false;
    }
}
