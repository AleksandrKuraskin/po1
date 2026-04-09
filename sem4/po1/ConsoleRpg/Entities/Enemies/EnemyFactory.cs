namespace ConsoleRpg.Entities.Enemies;

public class EnemyFactory
{
    private static readonly Random _rng = new Random();
    
    public static Enemy CreateRandomEnemy()
    {
        var hp = _rng.Next(20, 51);
        var attack = _rng.Next(5, 13);
        var armor = _rng.Next(0, 4);

        string[] names = { "Goblin", "Skeleton", "Zombie", "Bandit" };
        char[] symbols = { 'g', 's', 'z', 'b' };
            
        var index = _rng.Next(names.Length);

        return new Enemy(names[index], symbols[index], hp, attack, armor);
    }
    
    public static Enemy CreateOpEnemy()
    {
        var hp = 999;
        var attack = 999;
        var armor = 10;
        
        var bossNames = "O.P. Enemy";
        var bossSymbols = 'O';

        var index = _rng.Next(bossNames.Length);

        return new Enemy(bossNames, bossSymbols, hp, attack, armor);
    }
}