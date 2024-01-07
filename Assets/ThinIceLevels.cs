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

        // Split the CSV string into lines
        string[] lines = csv.Split('\n');

        // If the CSV has a header, skip the first line
        int startIndex = lines[0].Contains(",") ? 1 : 0;

        for (int i = startIndex; i < lines.Length; i++)
        {
            // Split each line into fields
            string[] fields = lines[i].Split(',');

            // Trim each field to remove leading/trailing whitespaces
            fields = fields.Select(field => field.Trim()).ToArray();

            records.Add(fields);
        }

        return records;
    }

    public static ThinIceGame.Level Level1 = new ThinIceGame.Level(
        LevelDecode(@"
Wall15
Wall1,Goal1,Ice12,Wall1
Wall15"), new Vector2(1, 9), new Vector2(13, 1));

    public static ThinIceGame.Level[] Levels = new ThinIceGame.Level[]
    {
            Level1
    };
}