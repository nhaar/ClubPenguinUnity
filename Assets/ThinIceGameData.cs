using UnityEngine;

public class ThinIceGameData
{
    public ThinIceGame.Level CurrentLevel { get; set; }

    public int CurrentLevelNumber { get; set; }

    public int ClearedTiles { get; set; }

    public int TotalTiles { get; set; }

    public int PointsOnLevelStart { get; set; }

    public int PointsThisLevel { get; set; }


    public ThinIceGameData()
    {
        CurrentLevelNumber = 1;
        CurrentLevel = ThinIceLevels.Levels[CurrentLevelNumber - 1];
        ClearedTiles = 0;
        PointsOnLevelStart = 0;
    }

    public void GetLevelData(int levelNumber, bool updateTotal)
    {
        ThinIceGame.Level level = ThinIceLevels.Levels[CurrentLevelNumber - 1];
        if (updateTotal)
        {
            TotalTiles = level.GetTotalTileCount();
        }
    }

    public void ChangeLevel(int levelNumber)
    {
        CurrentLevelNumber = levelNumber;
        GetLevelData(levelNumber, true);
        ClearedTiles = 0;
        PointsOnLevelStart += PointsThisLevel;
        PointsThisLevel = 0;
    }

    public void Reset()
    {
        GetLevelData(CurrentLevelNumber, false);
        ClearedTiles = 0;
        PointsThisLevel = 0;
    }
}