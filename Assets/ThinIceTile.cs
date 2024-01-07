using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ThinIceTile : MonoBehaviour
{
    public ThinIceGame.Level.TileType TileType { get; set; }

    public bool HasKey { get; set; }

    public ThinIceImage Key { get; set; }

    public GameObject Block { get; set; }

    public void ChangeTile(ThinIceGame.Level.TileType tileType)
    {
        ThinIceOS thinIceOS = transform.parent.GetComponent<ThinIceOS>();
        HasKey = false;

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

    public void OnPuffleEnter(ThinIcePuffle.Direction direction)
    {
        ThinIceOS thinIceOS = transform.parent.GetComponent<ThinIceOS>();
        if (TileType == ThinIceGame.Level.TileType.Goal)
        {
            thinIceOS.GoToNextLevel();
        }
        else if (TileType == ThinIceGame.Level.TileType.Lock)
        {
            // if we're entering the lock we need to have the key
            ChangeTile(ThinIceGame.Level.TileType.Ice);
            thinIceOS.Puffle.UseKey();
        }
        if (Block != null)
        {
            // same as above, we know the block can move already
            Block.GetComponent<ThinIceBlock>().Move(direction);
        }
        if (HasKey)
        {
            RemoveKey();
            thinIceOS.Puffle.GetKey();
        }
    }

    public void AddKey()
    {
        HasKey = true;
        ThinIceOS thinIceOS = transform.parent.GetComponent<ThinIceOS>();
        Key = ThinIceImage.AddImage(thinIceOS.Key, "Key", Vector2.zero, gameObject, thinIceOS.BaseImage).GetComponent<ThinIceImage>();
    }

    public void RemoveKey()
    {
        HasKey = false;
        Destroy(Key.gameObject);
        Key = null;
    }
}
