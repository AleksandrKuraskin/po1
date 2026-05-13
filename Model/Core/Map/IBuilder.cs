namespace ConsoleRpg.Model.Core.Map;

public interface IBuilder
{
    IBuilder StartFilledDungeon();
    IBuilder StartEmptyDungeon();
    IBuilder AddCentralHall(int width, int height);
    IBuilder AddCorridors();
    IBuilder AddRooms();
    IBuilder AddWeapons(int count);
    IBuilder AddItems(int count);
    IBuilder AddEnemies(int count);
}