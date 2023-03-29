using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETileType
{
    None,
    Lock,
    Unlock,
}

public class Tile : MonoBehaviour
{
    public ETileType eTileType;
    public SpriteRenderer spriteRenderer;
    public int tilePosX = 0, tilePosY = 0;

    public int f, g, h;
    public bool isClosed;

    public Tile preTile;
    public Tile postTile;

    private void Awake()
    {
        eTileType = ETileType.Unlock;
        spriteRenderer = GetComponent<SpriteRenderer>();
        ResetTile();
    }

    //해당 타일 첫 방문 인경우
    public int SetF(int newG, int newH)
    {
        g = newG;
        h = newH;
        f = g + h;

        return f;
    }

    //해당 타일 방문 이력 있는 경우
    public int SetF(int newG)
    {
        g = newG > g ? g : newG;
        f = g + h;

        return f;
    }

    public void ResetTile()
    {
        f = -1; g = 0; h = 0;
        isClosed = false;
        preTile = this;
        postTile = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            //Debug.Log(gameObject);
            transform.parent.parent.GetComponent<TileBundle>().playerPos = new Vector2Int(tilePosX, tilePosY);
            transform.parent.parent.GetComponent<TileBundle>().haveToWalkTile = new Vector2Int(postTile.tilePosX, postTile.tilePosY);
        }
    }
}
