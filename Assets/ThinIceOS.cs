using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ThinIceOS : MonoBehaviour
{
    public Sprite BaseImage;

    public Sprite Logo;

    public Sprite MenuArt;

    public Sprite SmallButton;

    public Sprite SmallButtonHover;

    public Font ButtonFont;

    public Sprite InstructionArt1;

    public Font InstructionFont;

    private Color _instructionColor = new Color(0f, 102 / 255f, 204 / 255f, 1f);

    public Sprite SquareButton;

    public Sprite SquareButtonHover;

    private int _buttonFontSize = 28;

    private int _buttonEdgePosition = 380;

    public Sprite PuffleImage;

    public Sprite EmptyTile;

    public Sprite IceTile;

    public Sprite WaterTile;

    public Sprite WallTile;

    public Sprite GoalTile;

    public Sprite TeleporterTile;

    public Sprite PlaidTeleporterTile;

    public Sprite LockTile;

    public Sprite ButtonTile;

    public Sprite ThickIceTile;

    public Sprite BlockHoleTile;

    public Sprite Key;

    public Sprite Block;

    public ThinIceTile[,] TileObjects { get; set; }

    public ThinIceGameData GameData { get; set; }

    public ThinIcePuffle Puffle { get; set; }

    public List<GameObject> Blocks { get; set; }

    private enum State
    {
        MainMenu,
        InstructionMenu,
        Game
    }

    void Start()
    {
        Blocks = new List<GameObject>();
        ChangeState(State.MainMenu);
    }
    void AddButton(Sprite button, Sprite buttonHover, string name, Vector2 position, string text, Font font, int fontSize, Action callback)
    {
        GameObject newObject = new GameObject(name);
        newObject.AddComponent<RectTransform>();
        newObject.AddComponent<ThinIceGameButton>();
        newObject.GetComponent<ThinIceGameButton>().Button = button;
        newObject.GetComponent<ThinIceGameButton>().Hovered = buttonHover;
        newObject.GetComponent<ThinIceGameButton>().BaseImage = BaseImage;
        newObject.GetComponent<ThinIceGameButton>().Text = text;
        newObject.GetComponent<ThinIceGameButton>().Font = font;
        newObject.GetComponent<ThinIceGameButton>().FontSize = fontSize;
        newObject.transform.SetParent(gameObject.transform);
        newObject.GetComponent<RectTransform>().anchoredPosition = position;
        newObject.GetComponent<ThinIceGameButton>().AddClickFunction(callback);
    }

    GameObject AddImage(Sprite image, string name, Vector2 position)
    {
        return ThinIceImage.AddImage(image, name, position, gameObject, BaseImage);
    }

    void AddLogo()
    {
        AddImage(Logo, "Logo", new Vector2(0, 380));
    }

    void AddText(string name, string text, Vector2 position, Font font, int fontSize, Color color, Vector2 size, TextAnchor alignment)
    {
        GameObject newObject = new GameObject(name);
        newObject.AddComponent<RectTransform>();
        newObject.AddComponent<Text>();
        newObject.GetComponent<Text>().text = text;
        newObject.GetComponent<Text>().font = font;
        newObject.GetComponent<Text>().fontSize = fontSize;
        newObject.GetComponent<Text>().alignment = alignment;
        newObject.GetComponent<Text>().color = color;
        newObject.transform.SetParent(gameObject.transform);
        newObject.GetComponent<RectTransform>().anchoredPosition = position;
        newObject.GetComponent<RectTransform>().sizeDelta = size;
    }

    void CreateLevelTiles()
    {
        TileObjects = new ThinIceTile[ThinIceGame.Level.MaxWidth, ThinIceGame.Level.MaxHeight];
        Vector2 origin = new Vector2(-480, 380);
        GameObject tile = AddImage(EmptyTile, "Tile1", origin);
        tile.AddComponent<ThinIceTile>();
        tile.GetComponent<ThinIceTile>().TileType = ThinIceGame.Level.TileType.Empty;
        tile.GetComponent<ThinIceTile>().Position = new Vector2(0, 0);
        TileObjects[0, 0] = tile.GetComponent<ThinIceTile>();
        Vector2 tileSize = tile.GetComponent<RectTransform>().sizeDelta;
        for (int i = 0; i < ThinIceGame.Level.MaxWidth; i++)
        {
            for (int j = 0; j < ThinIceGame.Level.MaxHeight; j++)
            {
                if (i == 0 && j == 0) continue;

                GameObject newTile = Instantiate(tile);
                newTile.name = "Tile" + (i + 1) + "-" + (j + 1);
                newTile.transform.SetParent(gameObject.transform);
                newTile.GetComponent<ThinIceTile>().Position = new Vector2(i, j);
                newTile.GetComponent<RectTransform>().anchoredPosition = origin + new Vector2(tileSize.x * i, -tileSize.y * j) / 2 ;
                TileObjects[i, j] = newTile.GetComponent<ThinIceTile>();
            }
        }
    }

    void ClearBlocks()
    {
        foreach (GameObject blockObject in Blocks)
        {
            Destroy(blockObject);
        }
        Blocks.Clear();
    }

    void DrawLevel()
    {
        ClearBlocks();
        ThinIceGame.Level level = GameData.CurrentLevel;
        int teleporterNumber = 0;
        ThinIceTile lastTeleporter = null;
        for (int j = 0; j < ThinIceGame.Level.MaxHeight; j++)
        {
            for (int i = 0; i < ThinIceGame.Level.MaxWidth; i++)
            {
                ThinIceGame.Level.TileType tileType;
                if (level.IsPointOutOfBound(new Vector2(i, j)))
                {
                    tileType = ThinIceGame.Level.TileType.Empty;
                }
                else
                {
                    tileType = level.Tiles[i - (int)level.Origin.x, j - (int)level.Origin.y];
                    
                }

                TileObjects[i, j].ChangeTile(tileType);
                ThinIceTile currentTile = TileObjects[i, j];

                // current system doesn't allow to specify which teleporter
                // links where, mostly only works for 1 teleporter pair
                if (tileType == ThinIceGame.Level.TileType.Teleporter)
                {
                    if (teleporterNumber % 2 == 0)
                    {
                        lastTeleporter = currentTile;
                    }
                    else
                    {
                        currentTile.LinkedTeleporter = lastTeleporter;
                        lastTeleporter.LinkedTeleporter = currentTile;
                        lastTeleporter = null;
                    }
                    teleporterNumber++;
                }
            }
        }
        foreach (Vector2Int keyPosition in level.KeyPositions ?? new List<Vector2Int>() { })
        {
            TileObjects[keyPosition.x, keyPosition.y].AddKey();
        }

        foreach (Vector2Int blockPosition in level.BlockPositions ?? new List<Vector2Int>() { })
        {
            GameObject blockObject = AddImage(Block, "Block", GetTilePosition(blockPosition));
            TileObjects[blockPosition.x, blockPosition.y].Block = blockObject;
            ThinIceBlock block = blockObject.AddComponent<ThinIceBlock>();
            block.Position = blockPosition;
            Blocks.Add(blockObject);
        }
    }

    void StartLevel(int levelNumber)
    {
        GameData.ChangeLevel(levelNumber);
        DrawLevel();
        if (!Puffle)
        {
            SpawnPuffle();
        }
        Puffle.TeleportTo(GameData.AbsoluteSpawnLocation);
        Puffle.HasKey = false;
    }

    public void GoToNextLevel()
    {
        StartLevel(GameData.CurrentLevelNumber + 1);
    }

    void SpawnPuffle()
    {
        GameObject puffleObject = new GameObject("Puffle");
        Puffle = puffleObject.AddComponent<ThinIcePuffle>();
        puffleObject.GetComponent<ThinIcePuffle>().PuffleImage = PuffleImage;
        puffleObject.GetComponent<ThinIcePuffle>().BaseImage = BaseImage;
        puffleObject.transform.SetParent(gameObject.transform);
        RectTransform rectTransform = puffleObject.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            puffleObject.AddComponent<RectTransform>();
        }
    }

    void ChangeState(State state)
    {
        Utils.RemoveAllChildren(gameObject);
        switch (state)
        {
            case State.MainMenu:
                AddImage(MenuArt, "MenuArt", Vector2.zero);
                AddLogo();
                AddButton(SmallButton, SmallButtonHover, "StartButton", new Vector2(0, -350), "START", ButtonFont, _buttonFontSize, () =>
                {
                    ChangeState(State.InstructionMenu);
                });
                break;
            case State.InstructionMenu:
                AddLogo();
                AddImage(InstructionArt1, "InstructionArt", new Vector2(0, -80));
                AddButton(SmallButton, SmallButtonHover, "PlayButton", new Vector2(-_buttonEdgePosition, -350), "PLAY", ButtonFont, _buttonFontSize, () =>
                {
                    ChangeState(State.Game);
                });
                AddButton(SmallButton, SmallButtonHover, "NextButton", new Vector2(_buttonEdgePosition, -350), "NEXT", ButtonFont, _buttonFontSize, () =>
                {
                    Debug.Log("NEXT PRESSED");
                });
                AddText("Instruction1", "Melt ice on your way through each maze. Once the ice is melted you can't walk back. Melt all the ice to solve the stage.", new Vector2(0, 200), InstructionFont, 32, _instructionColor, new Vector2(1000, 200), TextAnchor.MiddleCenter);
                break;
            case State.Game:
                AddText("PointsLabel", "POINTS", new Vector2(_buttonEdgePosition - 30, -_buttonEdgePosition), InstructionFont, 42, _instructionColor, new Vector2(250, 100), TextAnchor.MiddleCenter);
                AddText("PointsText", "0", new Vector2(580, -_buttonEdgePosition), InstructionFont, 42, _instructionColor, new Vector2(250, 100), TextAnchor.MiddleLeft);
                AddText("LevelLabel", "LEVEL", new Vector2(-420, 450), InstructionFont, 42, _instructionColor, new Vector2(250, 100), TextAnchor.MiddleCenter);
                AddText("LevelText", "1", new Vector2(-200, 450), InstructionFont, 42, _instructionColor, new Vector2(250, 100), TextAnchor.MiddleLeft);
                AddText("ClearedTileCount", "0", new Vector2(-150, 450), InstructionFont, 42, _instructionColor, new Vector2(250, 100), TextAnchor.MiddleRight);
                AddText("TileSeparator", "/", new Vector2(0, 450), InstructionFont, 42, _instructionColor, new Vector2(250, 100), TextAnchor.MiddleCenter);
                AddText("TotalTileCount", "12", new Vector2(150, 450), InstructionFont, 42, _instructionColor, new Vector2(250, 100), TextAnchor.MiddleLeft);
                AddButton(SquareButton, SquareButtonHover, "ResetButton", Vector2.one * (-_buttonEdgePosition), "RESET", ButtonFont, _buttonFontSize, () =>
                {
                    Debug.Log("RESET");
                });
                CreateLevelTiles();
                GameData = new ThinIceGameData();
                StartLevel(1);
                break;
        }
    }

    public Vector2 GetTilePosition(Vector2 position)
    {
        return GetTile(position).GetComponent<RectTransform>().anchoredPosition;
    }

    public GameObject GetTile(Vector2 position)
    {
        return TileObjects[(int)position.x, (int)position.y].gameObject;
    }
}
