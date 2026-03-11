namespace ConsoleRpg.Items;

public class Bow : Weapon
{
    public override string Name => "Bow";
    public override char Symbol => 'B';
    public override bool IsTwoHanded { get; protected set; } = true;
}