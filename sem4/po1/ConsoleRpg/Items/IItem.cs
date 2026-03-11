using ConsoleRpg.Core;
using ConsoleRpg.Entities;

namespace ConsoleRpg.Items;

public interface IItem : IGameObject
{
    void OnPickUp(Player player, Logger logger);
    void TryEquip(Player player, Logger logger);
    void OnDrop(Map map, int x, int y, Logger logger);
}