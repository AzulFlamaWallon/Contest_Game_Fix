public class PelletTrail : TrailBase<Pellet>
{
    public override void UpdateTrailPositon()
    {
        speed = currentObj.pelletSpeed;
        base.UpdateTrailPositon();
    }
}