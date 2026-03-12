namespace ConsoleRpg.Items;

public class BigSword : Weapon
{
    public override char Symbol => 'B';
    public override string Name => "Big Sword";
    
    public override bool IsTwoHanded { get; protected set; } = true;
}