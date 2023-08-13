namespace BulletBallet.objects.bullets.classes;

public sealed class Attack
{
    public int NumberOfBullets {get; set;}
    public float Speed {get; set;}
    
    public float AngularVelocity {get; set;}
    public float CurrentAngle {get; set;}
    
    public float Length {get; set;}
    public float Amplitude {get; set;}
    
    public int Rows {get; set;}
    public int Columns {get; set;}
    public float Spacing {get; set;}

    public float StartAngle {get; set;}
    public float AngleIncrement {get; set;}
    public float StartRadius {get; set;}
    public float RadiusIncrement {get; set;}
}