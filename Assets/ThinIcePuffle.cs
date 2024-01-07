using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThinIcePuffle : MonoBehaviour
{
    public Sprite PuffleImage;

    public Sprite BaseImage;

    // absolute scale
    public Vector2 Position { get; set; }

    public bool IsMoving { get; set; }

    private ThinIceOS _os;

    private List<ThinIceGame.Level.TileType> _impassableTiles = new()
    {
        ThinIceGame.Level.TileType.Wall,
        ThinIceGame.Level.TileType.Water
    };

    // in the future for new levels: could have a number of keys as opposed to this
    // not necessary in vanilla
    public bool HasKey { get; set; }

    private Vector2 _tileSize;

    private Vector2 _originTile;

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
        None
    }

    void Start()
    {
        gameObject.AddComponent<ThinIceImage>();
        gameObject.GetComponent<ThinIceImage>().SourceImage = PuffleImage;
        gameObject.GetComponent<ThinIceImage>().BaseImage = BaseImage;
        _os = transform.parent.GetComponent<ThinIceOS>();
        RectTransform tileObjectTrans = _os.TileObjects[0, 0].GetComponent<RectTransform>();
        _originTile = tileObjectTrans.anchoredPosition;
        _tileSize = tileObjectTrans.sizeDelta;
        HasKey = false;
        IsMoving = false;
    }

    void Update()
    {
        if (IsMoving)
        {

        }
        else
        {
            // preserving the original code's arrow key priority
            Direction direction = Direction.None;
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                direction = Direction.Left;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                direction = Direction.Right;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                direction = Direction.Up;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                direction = Direction.Down;
            }
            if (direction != Direction.None)
            {
                Vector2 targetPosition = GetDestination(Position, direction);
                if (CanMove(targetPosition, direction))
                {
                    StartCoroutine(Move(targetPosition, direction));
                }
            }
        }
    }

    private bool CanMove(Vector2 targetPosition, Direction direction)
    {        
        Vector2 targetTilePosition = targetPosition;
        ThinIceTile targetTile = _os.TileObjects[(int)targetTilePosition.x, (int)targetTilePosition.y];
        ThinIceGame.Level.TileType tileType = targetTile.TileType;

        if (_impassableTiles.Contains(tileType))
        {
            return false;
        }
        else if (tileType == ThinIceGame.Level.TileType.Lock)
        {
            return HasKey;
        }
        else if (targetTile.Block != null)
        {
            return targetTile.Block.GetComponent<ThinIceBlock>().CanPush(direction);
        }
        else
        {
            return true;
        }
    }

    private IEnumerator Move(Vector2 targetPosition, Direction direction)
    {
        IsMoving = true;
        Vector2 targetTilePosition = _os.TileObjects[(int)targetPosition.x, (int)targetPosition.y].GetComponent<RectTransform>().anchoredPosition;
        Vector2 originalPosition = GetComponent<RectTransform>().anchoredPosition;
        Vector2 delta = targetTilePosition - originalPosition;

        // takes 4 frames to move 1 tile (as per original game)
        int moveLength = 4;
        for (int i = 0; i < moveLength; i++)
        {
            GetComponent<RectTransform>().anchoredPosition = originalPosition + delta * (i + 1) / moveLength;
            yield return null;
        }
        _os.TileObjects[(int)Position.x, (int)Position.y].OnPuffleExit();
        Position = targetPosition;
        _os.TileObjects[(int)targetPosition.x, (int)targetPosition.y].OnPuffleEnter(direction);
        IsMoving = false;
    }

    public void TeleportTo(Vector2 targetPosition)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = transform.parent.gameObject.GetComponent<ThinIceOS>().GetTilePosition(targetPosition);
        Position = targetPosition;
    }

    public void GetKey()
    {
        HasKey = true;
    }

    public void UseKey()
    {
        HasKey = false;
    }

    public static Vector2 GetDestination(Vector2 position, Direction direction)
    {
        Vector2 destination = position;
        switch (direction)
        {
            case Direction.Left:
                destination.x -= 1;
                break;
            case Direction.Right:
                destination.x += 1;
                break;
            case Direction.Up:
                destination.y -= 1;
                break;
            case Direction.Down:
                destination.y += 1;
                break;
        }
        return destination;
    }
}
