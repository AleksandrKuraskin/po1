using System;
using System.Collections.Generic;
using System.Text.Json;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Items;
using ConsoleRpg.Shared.Systems.Attacking;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Sound.SoundEvents;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Server.CommandHandlers;

public class AttackCommandHandler : IServerCommandHandler
{
    public string CommandName => "ATTACK";

    private CombatStats GetTotalStats(Player p, IAttackVisitor visitor)
    {
        var weaponLeft = p.Equipment.LeftHand;
        var weaponRight = p.Equipment.RightHand;
        
        var statsLeft = weaponLeft != null
            ? weaponLeft.Accept(visitor, p) 
            : visitor.VisitNonWeapon(null, p);

        if (weaponRight != null && weaponLeft == weaponRight)
        {
            return statsLeft;
        }

        var statsRight = weaponRight != null
            ? weaponRight.Accept(visitor, p)
            : visitor.VisitNonWeapon(null, p);

        return new CombatStats
        {
            Attack = statsLeft.Attack + statsRight.Attack,
            Defense = statsLeft.Defense + statsRight.Defense
        };
    }

    public void Handle(string payload, IServerModel server, Player player)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var data = JsonSerializer.Deserialize<AttackData>(payload, options);
        if (data == null) return;

        IAttackVisitor visitor = data.VisitorName switch
        {
            "Normal" => new NormalAttackVisitor(),
            "Stealth" => new StealthAttackVisitor(),
            "Magic" => new MagicAttackVisitor(),
            _ => new NormalAttackVisitor()
        };

        var tile = server.MapContext.Map.GetTile(player.X, player.Y);
        var enemy = tile.Enemy;

        if (enemy == null)
        {
            LogManager.Instance.Log("No enemies in sight to attack.", LogType.Info, LogScope.Private, player.Name);
            return;
        }

        var weapons = new HashSet<IItem>();
        if (player.Equipment.RightHand != null) weapons.Add(player.Equipment.RightHand);
        if (player.Equipment.LeftHand != null) weapons.Add(player.Equipment.LeftHand);

        if (weapons.Count > 0)
        {
            foreach (var w in weapons)
            {
                var sound = new AttackSound(player, w);
                player.MakeNoise(sound);
            }
        }
        else
        {
            var sound = new MoveSound(player);
            player.MakeNoise(sound);
        }

        var stats = GetTotalStats(player, visitor);
        
        var enemyArmor = enemy.Stats.GetStat(StatType.Armor).Value;
        var damageDealt = Math.Max(0, stats.Attack - enemyArmor);
        
        enemy.TakeDamage(damageDealt);
        LogManager.Instance.Log($"Attacking ({enemy.Name}) for {damageDealt} dmg. ({stats.Attack} reduced by {enemyArmor} armor)", LogType.Info, LogScope.Global, null, player.Name);
        
        if (!enemy.Alive)
        {
            LogManager.Instance.Log($"You have slayed {enemy.Name}!", LogType.Success, LogScope.Global, null, player.Name);
            tile.Enemy = null;
            server.ProcessEnemiesTurn();
            return;
        }
        
        var enemyAttack = enemy.Stats.GetStat(StatType.Strength).Value;
        var damageReceived = Math.Max(0, enemyAttack - stats.Defense);
        
        player.TakeDamage(damageReceived);
        LogManager.Instance.Log($"{enemy.Name} fights back dealing {damageReceived} dmg. ({enemyAttack} reduced by your {stats.Defense} defense)", LogType.Info, LogScope.Global, null, player.Name);
        enemy.ActedThisTurn = true;

        if (!player.Alive)
        {
            LogManager.Instance.Log("You died! Game over.", LogType.Error, LogScope.Private, player.Name, player.Name);
        }
        
        server.ProcessEnemiesTurn();
    }

    private class AttackData { public string VisitorName { get; set; } = ""; }
}
