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

    public void GetLevelData(bool updateTotal)
    {
        if (updateTotal)
        {
            TotalTiles = CurrentLevel.GetTotalTileCount();
        }
    }

    public void ChangeLevel(int levelNumber)
    {
        CurrentLevelNumber = levelNumber;
        CurrentLevel = ThinIceLevels.Levels[CurrentLevelNumber - 1];
        GetLevelData(true);
        ClearedTiles = 0;
        PointsOnLevelStart += PointsThisLevel;
        PointsThisLevel = 0;
    }

    public void Reset()
    {
        GetLevelData(false);
        ClearedTiles = 0;
        PointsThisLevel = 0;
    }

    public Vector2 AbsoluteSpawnLocation => CurrentLevel.PuffleSpawnLocation + CurrentLevel.Origin;
}