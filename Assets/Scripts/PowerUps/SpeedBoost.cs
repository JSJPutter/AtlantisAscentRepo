using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : PowerUp
{
    public float speed;
    protected override void Apply()
    {
        powerUpName = "Magnet";
        player.ApplySpeedBoost(speed, duration);
    }

    public override void Remove()
    {
        player.RemoveMagnet();
    }
}
