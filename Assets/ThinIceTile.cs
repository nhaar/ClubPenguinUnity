using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ThinIceTile : MonoBehaviour
{
    public ThinIceGame.Level.TileType TileType { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTile(ThinIceGame.Level.TileType tileType)
    {
        ThinIceOS thinIceOS = transform.parent.GetComponent<ThinIceOS>();

        Sprite tileSprite = tileType switch
        {
            ThinIceGame.Level.TileType.Empty => thinIceOS.EmptyTile,
            ThinIceGame.Level.TileType.Ice => thinIceOS.IceTile,
            ThinIceGame.Level.TileType.Water => thinIceOS.WaterTile,
            ThinIceGame.Level.TileType.Wall => thinIceOS.WallTile,
            ThinIceGame.Level.TileType.Goal => thinIceOS.GoalTile,
            ThinIceGame.Level.TileType.Teleporter => thinIceOS.TeleporterTile,
            ThinIceGame.Level.TileType.PlaidTeleporter => thinIceOS.PlaidTeleporterTile,
            ThinIceGame.Level.TileType.Lock => thinIceOS.LockTile,
            ThinIceGame.Level.TileType.Button => thinIceOS.ButtonTile,
            ThinIceGame.Level.TileType.FakeWall => thinIceOS.WallTile,
            ThinIceGame.Level.TileType.ThickIce => thinIceOS.ThickIceTile,
            ThinIceGame.Level.TileType.BlockHole => thinIceOS.BlockHoleTile,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        GetComponent<ThinIceImage>().ChangeImage(tileSprite);
        TileType = tileType;
    }

    public void OnPuffleExit()
    {
        if (TileType == ThinIceGame.Level.TileType.ThickIce)
        {
            ChangeTile(ThinIceGame.Level.TileType.Ice);
        }
        else if (TileType == ThinIceGame.Level.TileType.Ice)
        {
            ChangeTile(ThinIceGame.Level.TileType.Water);
        }
    }
}
