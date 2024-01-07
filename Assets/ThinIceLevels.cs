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
                    throw new Exception("Invalid tile type: " + tile);
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
                tiles[j, i] = tileMap[i][j];
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

    public static ThinIceGame.Level[] Levels = new ThinIceGame.Level[]
    {
            Level1,
            Level2,
            Level3
    };
}