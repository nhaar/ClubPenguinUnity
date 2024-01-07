using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinIceGame : MonoBehaviour
{
    public class Level
    {
        public Vector2 Origin { get; set; }

        public Vector2 PuffleSpawnLocation { get; set; }

        public int Width { get; set; }
        
        public int Height { get; set; }

        public TileType[,] Tiles { get; set; }

        // relative to origin
        public List<Vector2Int> KeyPositions { get; set; }

        public static int MaxWidth = 19;

        public static int MaxHeight = 15;

        public enum TileType
        {
            Empty,
            Ice,
            Water,
            ThickIce,
            Wall,
            Goal,
            Teleporter,
            PlaidTeleporter,
            Lock,
            Button,
            FakeWall,
            BlockHole
        }

        private List<TileType> _zeroCountTiles = new()
        {
            TileType.Empty,
            TileType.Water,
            TileType.Wall,
            TileType.Button,
            TileType.BlockHole
        };

        public Level(TileType[,] tiles, Vector2 origin, Vector2 puffleSpawnLocation, List<Vector2Int> keyPositions = null)
        {
            Tiles = tiles;
            Width = tiles.GetLength(0);
            Height = tiles.GetLength(1);
            Origin = origin;
            PuffleSpawnLocation = puffleSpawnLocation;
            KeyPositions = keyPositions;
        }

        public int GetTotalTileCount()
        {
            float total = 0;
            
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    TileType tile = Tiles[i, j];
                    if (!_zeroCountTiles.Contains(tile))
                    {
                        if (tile == TileType.Teleporter)
                        {
                            total += 0.5f;
                        }
                        else if (tile == TileType.ThickIce)
                        {
                            total += 2;
                        }
                        else
                        {
                            total++;
                        }
                    }
                    
                }
            }
            return (int)total;
        }

        public bool IsPointOutOfBound(Vector2 point)
        {
            return point.x < Origin.x || point.x >= Origin.x + Width || point.y < Origin.y || point.y >= Origin.y + Height;
        }
    }
}
