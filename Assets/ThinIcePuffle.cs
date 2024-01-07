using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinIcePuffle : MonoBehaviour
{
    public Sprite PuffleImage;

    public Sprite BaseImage;

    public Vector2 Position { get; set; }

    public bool IsMoving { get; set; }

    private ThinIceOS _os;

    private List<ThinIceGame.Level.TileType> _impassableTiles = new()
    {
        ThinIceGame.Level.TileType.Wall,
        ThinIceGame.Level.TileType.Water
    };

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

    // Update is called once per frame
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
                Vector2 targetPosition = Position;
                switch (direction)
                {
                    case Direction.Left:
                        targetPosition.x -= 1;
                        break;
                    case Direction.Right:
                        targetPosition.x += 1;
                        break;
                    case Direction.Up:
                        targetPosition.y += 1;
                        break;
                    case Direction.Down:
                        targetPosition.y -= 1;
                        break;
                }
                if (CanMove(targetPosition))
                {
                    StartCoroutine(Move(targetPosition));
                }
            }
        }
    }

    private bool CanMove(Vector2 targetPosition)
    {
        
        Vector2 targetTilePosition = targetPosition - _os.GameData.CurrentLevel.Origin;
        ThinIceGame.Level.TileType targetTile = _os.TileObjects[(int)targetTilePosition.x, (int)targetTilePosition.y].TileType;
        
        if (_impassableTiles.Contains(targetTile))
        {
            return false;
        }
        else if (targetTile == ThinIceGame.Level.TileType.Lock)
        {
            return HasKey;
        }
        else
        {
            return true;
        }
    }

    private IEnumerator Move(Vector2 targetPosition)
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
        IsMoving = false;
    }
}
