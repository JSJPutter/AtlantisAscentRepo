
public class MultiBlast : PowerUp
{
    public int extraBlasts = 3;

    protected override void Apply()
    {
        powerUpName = "Multi-Blast";
        // player.ApplyMultiBlast(extraBlasts, duration);
    }

    public override void Remove()
    {
        // player.RemoveMultiBlast();
    }
}