using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinIceBlock : MonoBehaviour
{
    public ThinIceImage BlockImage { get; set; }

    public Vector2 Position { get; set; }

    private ThinIceOS _os;

    private static readonly List<ThinIceGame.Level.TileType> ImpassableTiles = new()
    {
        ThinIceGame.Level.TileType.Wall,
        ThinIceGame.Level.TileType.Water
    };

    void Start()
    {
        _os = transform.parent.GetComponent<ThinIceOS>();
    }

    public bool CanPush(ThinIcePuffle.Direction direction)
    {
        ThinIceTile tileInDirection = _os.GetTile(ThinIcePuffle.GetDestination(Position, direction)).GetComponent<ThinIceTile>();
        return !ImpassableTiles.Contains(tileInDirection.TileType);
    }

    public void Move(ThinIcePuffle.Direction direction)
    {
        StartCoroutine(MoveAnimation(direction));
    }

    private IEnumerator MoveAnimation(ThinIcePuffle.Direction direction)
    {
        while (CanPush(direction))
        {
            Vector2 destination = ThinIcePuffle.GetDestination(Position, direction);
            Vector2 destinationPosition = _os.GetTilePosition(destination);
            Vector2 originalPosition = GetComponent<RectTransform>().anchoredPosition;
            Vector2 delta = destinationPosition - originalPosition;
            int movementDuration = 3;
            for (int i = 0; i < movementDuration; i++)
            {
                GetComponent<RectTransform>().anchoredPosition = originalPosition + (i + 1) * delta / movementDuration;
                yield return null;
            }
            Position = destination;
        }
    }
}
