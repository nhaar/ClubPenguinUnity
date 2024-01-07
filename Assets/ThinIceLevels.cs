using System.IO;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public static class ThinIceLevels
{
    public static ThinIceGame.Level.TileType[,] LevelDecode(string levelString)
    {
        List<string[]> records = ParseCsv(levelString.Trim());

        List<List<ThinIceGame.Level.TileType>> tileMap = new();
        foreach (string[] record in records)
        {
            List<ThinIceGame.Level.TileType> row = new();
            foreach (string tile in record)
            {
                Match tileMatch = Regex.Match(tile, @"(?<tile>[a-zA-Z]+)(?<amount>\d+)");
                if (tileMatch.Success)
                {
                    ThinIceGame.Level.TileType tileType = tileMatch.Groups["tile"].Value switch
                    {
                        "Empty" => ThinIceGame.Level.TileType.Empty,
                        "Ice" => ThinIceGame.Level.TileType.Ice,
                        "Water" => ThinIceGame.Level.TileType.Water,
                        "ThickIce" => ThinIceGame.Level.TileType.ThickIce,
                        "Wall" => ThinIceGame.Level.TileType.Wall,
                        "Goal" => ThinIceGame.Level.TileType.Goal,
                        "Teleporter" => ThinIceGame.Level.TileType.Teleporter,
                        "PlaidTeleporter" => ThinIceGame.Level.TileType.PlaidTeleporter,
                        "Lock" => ThinIceGame.Level.TileType.Lock,
                        "Button" => ThinIceGame.Level.TileType.Button,
                        "FakeWall" => ThinIceGame.Level.TileType.FakeWall,
                        "BlockHole" => ThinIceGame.Level.TileType.BlockHole,
                        _ => throw new Exception("Invalid tile type: " + tile)
                    };
                    int amount = int.Parse(tileMatch.Groups["amount"].Value);
                    for (int i = 0; i < amount; i++)
                    {
                        row.Add(tileType);
                    }
                }
                else
                {
                    throw new Exception($"Invalid tile type: {tile} in level string\n{levelString}");
                }
            }
            tileMap.Add(row);
        }

        // we transpose so that horizontal is first argument and vertical is second
        ThinIceGame.Level.TileType[,] tiles = new ThinIceGame.Level.TileType[tileMap[0].Count, tileMap.Count];
        for (int i = 0; i < tileMap.Count; i++)
        {
            for (int j = 0; j < tileMap[i].Count; j++)
            {
                try
                {
                    tiles[j, i] = tileMap[i][j];
                }
                catch
                {
                    throw new Exception($"Incorrect tile map (error in {j},{i}) :\n" + levelString);
                }
            }
        }

        return tiles;
    }

    public static List<string[]> ParseCsv(string csv)
    {
        List<string[]> records = new List<string[]>();

        string[] lines = csv.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            // Split each line into fields
            string[] fields = lines[i].Split(',');

            // Trim each field to remove leading/trailing whitespaces
            fields = fields.Select(field => field.Trim()).ToArray();

            records.Add(fields);
        }

        return records;
    }

    public static ThinIceGame.Level Level1 = new(
        LevelDecode(@"
Wall15
Wall1,Goal1,Ice12,Wall1
Wall15"), new Vector2(1, 9), new Vector2(13, 1));

    public static ThinIceGame.Level Level2 = new(
        LevelDecode(@"
Empty12,Wall3
Empty12,Wall1,Goal1,Wall1
Empty12,Wall1,Ice1,Wall1
Wall7,Empty1,Wall5,Ice1,Wall1
Wall1,Ice5,Wall1,Empty1,Wall1,Ice5,Wall1
Wall5,Ice1,Wall3,Ice1,Wall5
Empty4,Wall1,Ice5,Wall1,Empty4
Empty4,Wall7,Empty4
"), new Vector2(1, 6), new Vector2(1, 4));

    public static ThinIceGame.Level Level3 = new(
        LevelDecode(@"
Wall3,Empty8,Wall3
Wall1,Goal1,Wall1,Empty8,Wall1,Ice1,Wall1
Wall1,Ice1,Wall5,Empty4,Wall1,Ice1,Wall1
Wall1,Ice1,Wall2,Ice2,Wall6,Ice1,Wall1
Wall1,Ice12,Wall1
Wall1,Ice2,Wall4,Ice2,Wall2,Ice2,Wall1
Wall4,Empty2,Wall8
"), new Vector2(2, 6), new Vector2(12, 1));

    public static ThinIceGame.Level Level4 = new(
        LevelDecode(@"
Wall5,Empty6,Wall5
Wall1,Ice3,Wall8,Ice3,Wall1
Wall1,Ice3,Wall2,Ice4,Wall2,Ice3,Wall1
Wall2,Ice12,Wall2
Empty1,Wall1,Ice1,Wall4,Ice2,Wall4,Ice1,Wall1,Empty1
Empty1,Wall1,Ice1,Wall1,Empty1,Wall1,Ice4,Wall1,Empty1,Wall1,Ice1,Wall1,Empty1
Empty1,Wall1,Ice1,Wall1,Empty1,Wall1,Ice4,Wall1,Empty1,Wall1,Goal1,Wall1,Empty1
Empty1,Wall3,Empty1,Wall6,Empty1,Wall3,Empty1
"), new Vector2(1, 1), new Vector2(2, 6));

    public static ThinIceGame.Level Level5 = new(
        LevelDecode(@"
Empty12,Wall3,Empty2
Empty12,Wall1,Ice1,Wall1,Empty2
Empty3,Wall10,Ice1,Wall3
Wall4,Ice12,Wall1
Wall1,Goal1,Ice14,Wall1
Wall3,Ice13,Wall1
Empty2,Wall15
"), new Vector2(1, 6), new Vector2(13, 1));

    public static ThinIceGame.Level Level6 = new(
        LevelDecode(@"
Empty12,Wall3,Empty2
Empty5,Wall5,Empty2,Wall1,Goal1,Wall1,Empty2
Empty2,Wall4,Ice3,Wall4,Ice1,Wall3
Wall3,Ice9,Wall1,Ice3,Wall1
Wall1,Ice11,Wall1,Ice3,Wall1
Wall4,Ice3,Wall1,Ice8,Wall1
Empty3,Wall14
"), new Vector2(1, 6), new Vector2(1, 4));

    public static ThinIceGame.Level Level7 = new(
        LevelDecode(@"
Wall15,Empty3
Wall1,Ice13,Wall1,Empty3
Wall1,Ice1,Wall11,Ice1,Wall4
Wall1,Ice1,Wall3,Goal1,Ice7,ThickIce1,Ice3,Wall1
Wall1,Ice1,Wall11,Ice1,Wall2,Ice1,Wall1
Wall1,Ice1,Wall11,Ice1,Wall2,Ice1,Wall1
Wall1,Ice1,Wall14,Ice1,Wall1
Wall1,Ice1,Wall4,Ice2,Wall2,Ice4,Wall2,Ice1,Wall1
Wall1,Ice16,Wall1
Wall5,Ice6,Wall7
Empty4,Wall8,Empty6
"), new Vector2(1, 2), new Vector2(13, 5));

    public static ThinIceGame.Level Level8 = new(
        LevelDecode(@"
Wall5,Empty3,Wall5,Empty4
Wall1,Ice3,Wall5,Ice3,Wall1,Empty4
Wall1,Ice1,Wall1,Ice1,Wall2,Ice1,Wall2,Ice1,Wall1,Ice1,Wall1,Empty4
Wall1,Ice2,ThickIce1,Ice2,ThickIce1,Ice2,ThickIce1,Ice2,Wall4,Empty1
Wall3,Ice1,Wall2,Ice1,Wall2,Ice1,Wall4,Goal1,Wall1,Empty1
Empty1,Wall2,Ice1,Wall2,Ice1,Wall2,Ice1,Wall4,Ice1,Wall1,Empty1
Empty1,Wall1,Ice4,ThickIce1,Ice2,ThickIce1,Ice2,Wall1,Ice2,Wall2
Empty1,Wall1,Ice3,Wall1,Ice1,Wall2,Ice1,Wall1,Ice1,Wall1,Ice3,Wall1
Empty1,Wall1,Ice5,Wall2,Ice3,Wall2,Ice2,Wall1
Empty1,Wall1,Ice2,Wall10,Ice2,Wall1
Empty1,Wall2,Ice13,Wall1
Empty2,Wall15
"), new Vector2(0, 3), new Vector2(6, 2));

    public static ThinIceGame.Level Level9 = new(
        LevelDecode(@"
Wall16,Empty1
Wall1,Ice14,Wall2
Wall1,Ice1,Wall4,Ice1,ThickIce1,Ice6,ThickIce1,Ice1,Wall1
Wall1,Ice1,ThickIce1,Ice13,Wall1
Wall1,Ice1,ThickIce1,Wall5,Ice1,Wall4,Ice1,Wall3
Wall1,Ice1,ThickIce1,Wall5,Goal1,Wall1,Ice2,Wall1,Ice1,Wall1,Empty2
Wall1,Ice1,ThickIce1,Wall7,Ice2,Wall1,Ice1,Wall1,Empty2
Wall1,Ice2,Wall4,Ice5,Wall1,Ice1,Wall1,Empty2
Wall1,Ice3,ThickIce2,Ice2,Wall3,Ice1,Wall3,Empty2
Wall1,Ice1,Wall1,Ice5,Wall3,Ice1,Wall1,Empty4
Wall1,Ice3,Wall3,Ice5,Wall1,Empty4
Wall5,Empty1,Wall7,Empty4
"), new Vector2(1, 0), new Vector2(13, 7));

    public static ThinIceGame.Level Level10 = new(
        LevelDecode(@"
Wall19
Wall1,Ice17,Wall1
Wall1,Ice3,ThickIce8,Ice4,ThickIce1,Ice1,Wall1
Wall1,Ice15,ThickIce1,Ice1,Wall1
Wall1,Ice6,Wall6,Ice3,ThickIce1,Ice1,Wall1
Wall1,Ice6,Wall2,Ice1,Wall2,ThickIce2,Ice2,ThickIce1,Ice1,Wall1
Wall1,Ice6,Wall2,Ice1,Wall2,ThickIce2,Ice4,Wall1
Wall1,Ice17,Wall1
Wall1,Ice17,Wall1
Wall1,Ice6,Wall3,Ice4,Wall3,Ice1,Wall1
Wall1,Ice9,ThickIce4,Lock1,Ice1,Wall1,Ice1,Wall1
Wall1,Ice2,Wall2,Ice2,Wall3,Ice4,Wall1,Goal1,Wall1,Ice1,Wall1
Wall1,Ice2,Wall2,ThickIce2,Ice2,Wall1,Ice4,Wall3,Ice1,Wall1
Wall1,Ice17,Wall1
Wall19
"), Vector2.zero, new Vector2(9, 5), new List<Vector2Int>()
        {
            Vector2Int.one
        });

    public static ThinIceGame.Level Level11 = new(
        LevelDecode(@"
Empty13,Wall3,Empty2
Empty13,Wall1,Goal1,Wall1,Empty2
Empty11,Wall3,Ice1,Wall3
Empty1,Wall11,Ice2,ThickIce1,Ice2,Wall1
Empty1,Wall1,Ice8,Wall2,ThickIce1,Wall1,Ice1,Wall1,ThickIce1,Wall1
Empty1,Wall1,Ice3,Wall4,Ice2,Wall1,ThickIce1,Ice1,ThickIce1,Ice1,ThickIce1,Wall1
Empty1,Wall1,Ice4,ThickIce1,Ice1,Wall1,Ice2,Wall3,Ice1,Wall3
Wall3,ThickIce1,Ice1,Wall1,Ice2,Wall1,Ice2,ThickIce1,Ice1,Wall1,Ice1,Wall3
Wall1,Ice2,ThickIce1,Wall3,Ice1,Wall3,Ice1,Wall2,Lock1,Wall3
Wall1,Ice1,Wall1,Ice3,Wall1,ThickIce1,Ice4,Wall1,Ice1,ThickIce3,Wall1
Wall1,Ice1,Wall1,Ice1,Wall1,Ice1,Wall1,ThickIce1,Wall1,Ice3,Wall1,Ice1,Wall2,ThickIce1,Wall1
Wall1,Ice2,ThickIce1,Wall1,Ice1,Wall1,Ice1,Wall1,Ice3,Wall1,Ice2,Wall1,ThickIce1,Wall1
Wall3,Ice2,ThickIce2,Ice1,Wall1,Ice3,Wall4,ThickIce1,Wall1
Empty2,Wall1,Ice2,Wall1,Ice2,Wall1,Ice2,ThickIce6,Wall1
Empty2,Wall16
"), new Vector2(1, 0), new Vector2(14, 11), new List<Vector2Int>()
        {
            new Vector2Int(6, 9)
        });

    public static ThinIceGame.Level Level12 = new(
        LevelDecode(@"
Empty6,Wall4,Empty1,Wall6,Empty2
Empty6,Wall1,Ice2,Wall3,Ice2,Wall1,Ice1,Wall1,Empty2
Empty6,Wall1,Ice1,ThickIce1,Ice3,ThickIce1,Ice1,Wall1,Ice1,Wall1,Empty2
Empty5,Wall3,Ice1,Wall3,Ice1,Wall2,Ice1,Wall2,Empty1
Wall6,Ice2,ThickIce1,Wall3,Ice3,ThickIce1,Ice1,Wall1,Empty1
Wall1,Ice2,Wall3,Ice1,Wall1,Ice1,Wall1,Ice1,Wall1,Ice1,Wall2,Ice1,Wall2,Empty1
Wall1,Ice1,ThickIce1,Ice3,ThickIce1,Ice1,ThickIce1,Ice1,ThickIce1,Wall1,ThickIce1,Ice2,ThickIce1,Wall1,Empty2
Wall1,Ice1,ThickIce1,Ice4,Wall1,Ice1,Wall1,Ice1,Wall1,Ice1,Wall2,Ice1,Wall1,Empty2
Wall1,Ice1,ThickIce2,Wall1,ThickIce2,Ice1,ThickIce1,Ice1,ThickIce1,Ice1,ThickIce1,Ice1,Wall1,Ice1,Wall3
Wall1,Ice1,ThickIce2,Wall1,Ice2,Wall1,Ice1,Wall1,Ice1,Wall1,Ice1,Wall2,Ice3,Wall1
Wall1,Ice1,ThickIce2,Wall2,Ice2,ThickIce1,Ice1,ThickIce1,Ice1,ThickIce1,Ice1,Wall3,Lock1,Wall1
Wall1,Ice1,ThickIce2,Wall2,Ice2,Wall2,Ice1,Wall1,Ice2,Wall1,Goal1,Ice2,Wall1
Wall1,Ice1,ThickIce2,Ice1,Wall14
Wall1,Ice4,Wall1,Empty13
Wall6,Empty13
"), Vector2.zero, new Vector2(15, 1), new List<Vector2Int>()
        {
            new Vector2Int(4, 13)
        });

    public static ThinIceGame.Level Level13 = new(
        LevelDecode(@"
Empty7,Wall4,Empty1,Wall3,Empty3
Empty2,Wall6,Ice2,Wall3,Ice1,Wall1,Empty3
Empty2,Wall1,Ice2,Wall1,Ice3,ThickIce1,Ice2,ThickIce2,Wall1,Empty3
Empty2,Wall1,Ice7,Wall2,Ice2,Wall1,Empty3
Empty2,Wall1,Ice1,Wall3,Ice7,Wall2,Empty2
Empty2,Wall1,Ice1,Wall1,BlockHole1,Wall5,ThickIce3,Ice1,Wall1,Empty2
Empty2,Wall1,ThickIce1,Ice7,ThickIce1,Ice2,Wall2,Empty2
Empty2,Wall1,Ice1,Wall1,Ice1,Wall2,Ice3,ThickIce3,Wall1,Empty3
Wall3,Ice1,Wall1,Ice1,Wall1,Ice6,ThickIce1,Wall1,Empty3
Wall1,Ice1,Wall1,Ice1,Wall1,Ice1,Wall1,Ice4,ThickIce3,Wall1,Empty3
Wall1,ThickIce1,Wall1,Ice1,Wall1,Ice1,Wall5,ThickIce1,Ice1,Wall5
Wall1,ThickIce1,Wall1,Ice1,ThickIce1,Ice1,Wall1,Goal1,Ice1,Lock1,Ice1,ThickIce1,Wall2,Ice3,Wall1
Wall1,ThickIce1,Wall2,ThickIce1,Wall6,Ice3,ThickIce1,Ice2,Wall1
Wall1,ThickIce4,Wall1,Empty4,Wall8
Wall6,Empty12
"), Vector2.zero, new Vector2(15, 11), new List<Vector2Int>()
        {
            new Vector2Int(1, 9)
        }, new List<Vector2Int>()
        {
            new Vector2Int(5, 9)
        });

    public static ThinIceGame.Level Level14 = new(
        LevelDecode(@"
Wall6,Empty3,Wall4,Empty5
Wall1,Ice4,Wall1,Empty3,Wall1,Ice2,Wall3,Empty3
Wall1,Ice4,Wall3,Empty1,Wall1,Ice4,Wall1,Empty3
Wall2,Ice5,Wall3,Ice4,Wall2,Empty2
Empty1,Wall1,Ice4,ThickIce1,Ice2,Wall1,Ice3,ThickIce1,Ice1,Wall2,Empty1
Wall2,Ice1,Wall2,Ice2,Wall1,Ice1,Wall2,Ice3,ThickIce1,Ice1,Wall1,Empty1
Wall1,Ice2,Wall2,Ice3,ThickIce1,Ice6,Wall3
Wall1,Ice3,Wall1,Ice2,ThickIce1,Ice9,Wall1
Wall1,Ice3,Wall1,Ice1,Wall1,Ice1,Wall3,Ice1,Wall1,Ice4,Wall1
Wall2,Ice2,Wall1,Ice2,ThickIce1,Ice2,Wall1,ThickIce1,Ice3,Wall3
Wall2,Ice1,Wall6,Ice1,Wall1,Ice3,ThickIce1,Ice1,Wall1,Empty1
Empty1,Wall1,Ice1,Wall4,Ice3,Wall1,Ice2,ThickIce1,Ice1,Wall2,Empty1
Empty1,Wall1,Ice1,Lock1,Ice1,Goal1,Wall1,Ice3,Wall1,Ice3,Wall2,Empty2
Empty1,Wall1,BlockHole1,Wall4,Ice3,Wall2,Ice2,Wall1,Empty3
Empty1,Wall3,Empty2,Wall9,Empty3
"), Vector2.zero, new Vector2(7, 11), new List<Vector2Int>()
        {
            new Vector2Int(16, 7)
        }, new List<Vector2Int>()
        {
            new Vector2Int(7, 8)
        });

    public static ThinIceGame.Level Level15 = new(
        LevelDecode(@"
Wall19
Wall1,Ice8,Wall1,ThickIce2,Wall1,ThickIce2,Ice3,Wall1
Wall1,Ice1,Wall1,Ice7,ThickIce4,Ice1,Wall1,Ice2,Wall1
Wall1,Ice3,Wall1,Ice5,ThickIce4,Ice1,Wall1,Lock1,Wall2
Wall1,Ice9,ThickIce4,Ice1,Wall1,Ice1,Goal1,Wall1
Wall1,Ice5,ThickIce3,Ice1,ThickIce1,Ice4,Wall4
Wall1,Ice5,ThickIce1,Ice1,ThickIce3,Ice7,Wall1
Wall1,Ice5,ThickIce1,Ice11,Wall1
Wall2,ThickIce4,Ice6,ThickIce1,Ice5,Wall1
Wall1,BlockHole1,ThickIce4,Ice12,Wall1
Wall1,Ice2,Wall1,Ice10,Wall2,Ice2,Wall1
Wall1,Ice13,Wall5
Wall1,Ice1,Wall1,Ice1,Wall1,Ice1,Wall1,Ice1,Wall1,Ice9,Wall1
Wall1,Ice3,Wall3,Ice11,Wall1
Wall19
"), Vector2.zero, new Vector2(5, 12), new List<Vector2Int>()
        {
            new Vector2Int(1, 9)
        }, new List<Vector2Int>()
        {
            new Vector2Int(5, 8)
        });

    public static ThinIceGame.Level Level16 = new(
        LevelDecode(@"
Wall19
Wall1,Ice2,ThickIce1,Ice3,Wall2,Ice4,Wall1,Ice4,Wall1
Wall1,Ice1,Wall1,Ice11,ThickIce1,Ice3,Wall1
Wall1,Ice3,Wall2,Ice4,Wall3,Ice2,Wall4
Wall3,Ice1,Wall3,Ice3,Wall3,Ice5,Wall1
Wall2,Ice2,Wall3,Ice1,Wall1,Ice1,Wall2,Ice5,Wall2
Wall2,Ice5,ThickIce1,Ice1,ThickIce1,Ice7,Wall2
Wall3,Ice3,Wall1,Ice1,Wall1,Ice1,Wall1,Ice7,Wall1
Wall3,Ice2,ThickIce1,Ice1,ThickIce1,Ice1,ThickIce1,Ice8,Wall1
Wall1,Ice5,Wall1,Ice1,Wall1,Ice1,Wall9
Wall1,Ice2,ThickIce1,Ice3,ThickIce1,Ice1,ThickIce1,Ice2,Wall1,Ice4,ThickIce1,Wall1
Wall1,Ice3,Wall1,Ice2,ThickIce1,Ice1,ThickIce1,Ice2,Wall1,Lock1,Wall1,Goal1,Wall1,ThickIce1,Wall1
Wall1,Ice7,Wall1,Ice4,ThickIce1,Wall3,ThickIce1,Wall1
Wall1,Ice2,Wall1,Ice4,Wall1,BlockHole1,Wall1,Ice2,ThickIce5,Wall1
Wall19
"), Vector2.zero, new Vector2(17, 4), new List<Vector2Int>()
        {
            new Vector2Int(17, 13)
        }, new List<Vector2Int>()
        {
            new Vector2Int(14, 5)
        });

    public static ThinIceGame.Level Level17 = new(
        LevelDecode(@"
Wall19
Wall1,Ice5,Wall2,BlockHole1,Wall1,Ice3,Wall1,Ice1,Wall1,Ice2,Wall1
Wall1,Ice5,Wall1,Ice4,Wall1,Ice1,Wall1,ThickIce1,Ice3,Wall1
Wall1,Ice5,Wall1,Ice2,Wall3,ThickIce1,Ice3,Wall1,Ice1,Wall1
Wall1,Ice1,Wall3,Ice1,Wall1,Ice2,Wall1,Ice1,ThickIce1,Ice1,Wall1,Ice2,Wall1,Ice1,Wall1
Wall1,Ice1,Wall1,Goal1,Wall1,Ice1,Wall1,Ice2,Wall2,Ice1,ThickIce1,Ice1,ThickIce1,Ice1,Wall1,Ice1,Wall1
Wall1,ThickIce1,Ice4,Wall1,Ice2,Wall1,Ice1,ThickIce1,Ice3,Wall2,Ice1,Wall1
Wall1,Lock1,Wall5,Ice2,Wall2,Ice1,ThickIce1,Ice1,Wall2,Ice2,Wall1
Wall1,Ice1,Wall1,Ice6,Wall1,Ice2,ThickIce1,Wall2,Ice1,ThickIce1,Ice1,Wall1
Wall1,Ice1,Wall1,Ice6,Wall1,Ice3,Wall2,Ice3,Wall1
Wall1,Ice1,Wall6,Ice1,Wall8,Ice1,Wall1
Wall1,Ice1,Teleporter1,Wall1,Ice12,Wall1,Ice1,Wall1
Wall1,Ice13,Wall3,Ice1,Wall1
Wall1,Ice9,Teleporter1,Ice7,Wall1
Wall19
"), Vector2.zero, new Vector2(15, 11), new List<Vector2Int>()
        {
            new Vector2Int(14, 1)
        }, new List<Vector2Int>()
        {
            new Vector2Int(5, 11)
        });

    public static ThinIceGame.Level[] Levels = new ThinIceGame.Level[]
    {
            Level1,
            Level2,
            Level3,
            Level4,
            Level5,
            Level6,
            Level7,
            Level8,
            Level9,
            Level10,
            Level11,
            Level12,
            Level13,
            Level14,
            Level15,
            Level16,
            Level17
    };
}