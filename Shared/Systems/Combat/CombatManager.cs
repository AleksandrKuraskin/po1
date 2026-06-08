using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Entities.Enemies;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Systems.Combat;

public static class CombatManager
{
    // Wywoływane przez AttackCommand (tura gracza)
    public static void PlayerAttacks(Map.Map map, Player player, Enemy enemy, CombatStats playerStats)
    {
        int enemyArmor = enemy.Stats.GetStat(StatType.Armor).Value;
        int damageDealt = Math.Max(0, playerStats.Attack - enemyArmor);
        
        enemy.TakeDamage(damageDealt);
        LogManager.Instance.Log(
            $"{player.Name} attacks {enemy.Name} for {damageDealt} dmg (Attack: {playerStats.Attack}, Armor: {enemyArmor}).",
            type: LogType.Combat
        );

        if (!enemy.Alive)
        {
            LogManager.Instance.Log($"{enemy.Name} has been slain by {player.Name}!", type: LogType.Success);
            enemy.Die(map);
            return;
        }

        // Kontratak przeciwnika (w ramach tej samej tury wymiany ciosów)
        int enemyAttack = enemy.Stats.GetStat(StatType.Strength).Value;
        int damageReceived = Math.Max(0, enemyAttack - playerStats.Defense);
        
        player.TakeDamage(damageReceived);
        LogManager.Instance.Log(
            $"{enemy.Name} counter-attacks {player.Name} for {damageReceived} dmg (Attack: {enemyAttack}, Defense: {playerStats.Defense}).",
            type: LogType.Combat
        );

        if (!player.Alive)
        {
            LogManager.Instance.Log($"{player.Name} died from the counter-attack.", type: LogType.Error);
            player.Die(map);
        }
    }

    // Wywoływane przez ChaseState (tura przeciwnika)
    public static void EnemyAttacks(Map.Map map, Enemy enemy, Player player)
    {
        int enemyAttack = enemy.Stats.GetStat(StatType.Strength).Value;
        int damageDealt = enemyAttack; // Player has 0 defense

        player.TakeDamage(damageDealt);
        LogManager.Instance.Log(
            $"{enemy.Name} attacks {player.Name} for {damageDealt} dmg (Attack: {enemyAttack}, Defense: 0).",
            type: LogType.Combat
        );

        if (!player.Alive)
        {
            LogManager.Instance.Log($"{player.Name} has been slain by {enemy.Name}!", type: LogType.Error);
            player.Die(map);
        }
    }
}
