public class LaserTrail : TrailBase<Laser>
{
    public override void UpdateTrailPositon()
    {
        speed = currentObj.LaserSpeed;
        base.UpdateTrailPositon();
    }
}