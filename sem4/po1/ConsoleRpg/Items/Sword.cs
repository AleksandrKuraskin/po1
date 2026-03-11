namespace ConsoleRpg.Items;

public class Sword : Weapon
{
    public override char Symbol => 'S';
    public override string Name => "Normal Sword";
    public override bool IsTwoHanded { get; protected set; } = false;
}