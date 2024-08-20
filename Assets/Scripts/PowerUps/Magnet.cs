
public class Magnet : PowerUp
{
    public float attractionRadius = 5f;

    protected override void Apply()
    {
        powerUpName = "Magnet";
        player.ApplyMagnet(attractionRadius, duration);
    }

    public override void Remove()
    {
        player.RemoveMagnet();
    }
}
