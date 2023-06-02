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
    public ETileType eTileType = ETileType.Unlock;
    public SpriteRenderer spriteRenderer;
    public int tilePosX = 0, tilePosY = 0;

    public int f, g, h;
    public bool isClosed;

    public Tile preTile;
    public Tile postTile;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ResetTile();
    }

    private void Start()
    {
        if (eTileType == ETileType.Lock)
        {
            LockTile();
        }
        else
        {
            eTileType = ETileType.Unlock;
        }
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

    public void LockTile()
    {
        eTileType = ETileType.Lock;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<SharehouseCat>() != null)
        {
            //Debug.Log(gameObject);
            //transform.parent.parent.GetComponent<TileBundle>().playerPos = new Vector2Int(tilePosX, tilePosY);
            //transform.parent.parent.GetComponent<TileBundle>().haveToWalkTile = new Vector2Int(postTile.tilePosX, postTile.tilePosY);
        }
        else if (collision.tag == "Furniture")
        {
            LockTile();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        SharehouseCat shareHouseCat = collision.gameObject.GetComponent<SharehouseCat>();
        if (shareHouseCat != null)
        {
            if (Mathf.Approximately(collision.transform.position.x, transform.position.x) &&
                Mathf.Approximately(collision.transform.position.x, transform.position.x))
            {
                if (tilePosX > postTile.tilePosX)
                {
                    shareHouseCat.GetComponent<SharehouseCat>().FlipX(-1);
                }
                else if (tilePosX < postTile.tilePosX)
                {
                    shareHouseCat.GetComponent<SharehouseCat>().FlipX(1);
                }
                else
                {
                    if (tilePosY > postTile.tilePosY)
                    {
                        shareHouseCat.GetComponent<SharehouseCat>().FlipX(-1);
                    } else if (tilePosY < postTile.tilePosY)
                    {
                        shareHouseCat.GetComponent<SharehouseCat>().FlipX(1);
                    }
                }
                transform.parent.parent.GetComponent<TileBundle>().playerPos = new Vector2Int(tilePosX, tilePosY);
                transform.parent.parent.GetComponent<TileBundle>().haveToWalkTile = new Vector2Int(postTile.tilePosX, postTile.tilePosY);
            }
        }
    }
}
