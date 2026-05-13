using System;
using System.Collections.Generic;
using ConsoleRpg.Model.Core;
using ConsoleRpg.Model.Systems.Logging;
using ConsoleRpg.Model.Entities;
using ConsoleRpg.Model.Entities.Enemies;
using ConsoleRpg.Controller.States;
using ConsoleRpg.Model.Items;
using ConsoleRpg.Model.Systems.Attacking;
using ConsoleRpg.Model.Systems.Sound.SoundEvents;
using ConsoleRpg.Model.Systems.Stats;

namespace ConsoleRpg.Controller.Commands;

public class AttackCommand(IAttackVisitor attackVisitor) : ICommand
{
    private readonly IAttackVisitor _attackVisitor = attackVisitor;

    private CombatStats GetTotalStats(Player p)
    {
        var weaponLeft = p.Equipment.LeftHand;
        var weaponRight = p.Equipment.RightHand;
        
        var statsLeft = weaponLeft != null
            ? weaponLeft.Accept(_attackVisitor, p) 
            : _attackVisitor.VisitNonWeapon(null, p);

        if (weaponRight != null && weaponLeft == weaponRight)
        {
            return statsLeft;
        }

        var statsRight = weaponRight != null
            ? weaponRight.Accept(_attackVisitor, p)
            : _attackVisitor.VisitNonWeapon(null, p);

        return new CombatStats
        {
            Attack = statsLeft.Attack + statsRight.Attack,
            Defense = statsLeft.Defense + statsRight.Defense
        };
    }
    
    public void Execute(Game game)
    {
        var p = game.Player;
        var map = game.MapContext.Map;
        
        var tile = map.GetTile(p.X, p.Y);
        var enemy = tile.Enemy;

        if (enemy == null)
        {
            LogManager.Instance.Log("No enemies in sight to attack.");
            return;
        }

        var weapons = new HashSet<IItem>();
        if (p.Equipment.RightHand != null) weapons.Add(p.Equipment.RightHand);
        if (p.Equipment.LeftHand != null) weapons.Add(p.Equipment.LeftHand);

        if (weapons.Count > 0)
        {
            foreach (var w in weapons)
            {
                var sound = new AttackSound(p, w);
                p.MakeNoise(sound);
            }
        }
        else
        {
            var sound = new MoveSound(p);
            p.MakeNoise(sound);
        }
        
        
        var stats = GetTotalStats(p);
        
        var enemyArmor = enemy.Stats.GetStat(StatType.Armor).Value;
        var damageDealt = Math.Max(0, stats.Attack - enemyArmor);
        
        enemy.TakeDamage(damageDealt);
        LogManager.Instance.Log($"Attacking ({enemy.Name}) for {damageDealt} dmg. ({stats.Attack} reduced by {enemyArmor} armor)");
        
        if (!enemy.Alive)
        {
            LogManager.Instance.Log($"You have slayed {enemy.Name}!", LogType.Success);
            tile.Enemy = null;
            return;
        }
        
        var enemyAttack = enemy.Stats.GetStat(StatType.Strength).Value;
        var damageReceived = Math.Max(0, enemyAttack - stats.Defense);
        
        p.TakeDamage(damageReceived);
        LogManager.Instance.Log($"{enemy.Name} fights back dealing {damageReceived} dmg. ({enemyAttack} reduced by your {stats.Defense} defense)");
        enemy.ActedThisTurn = true;

        if (!p.Alive)
        {
            LogManager.Instance.Log("You died! Game over.", LogType.Error);
            game.ChangeInputState(new GameOverState(game.LogFilePath));
            
        }
        
        game.ProcessEnemiesTurn();
    }
}