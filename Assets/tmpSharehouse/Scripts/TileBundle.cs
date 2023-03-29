using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructures.PriorityQueue;

public enum ETouchType
{
    Move,
    Lock,
    Unlock,
}
public class TileBundle : MonoBehaviour
{
    int tileMoveWeight = 10;

    GameObject[][] tiles;

    [SerializeField]
    GameObject player;
    [SerializeField]
    public Vector2Int playerPos;
    [SerializeField]
    public Vector2Int haveToWalkTile;

    float speed = 10f;

    [SerializeField]
    ETouchType eTouchType = ETouchType.Move;

    private void Awake()
    {
        tiles = new GameObject[transform.childCount][];

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject tileRow = transform.GetChild(i).gameObject;
            GameObject[] row = new GameObject[tileRow.transform.childCount];

            for (int j = 0; j < tileRow.transform.childCount; j++)
            {
                row[j] = tileRow.transform.GetChild(j).gameObject;
                row[j].GetComponent<Tile>().tilePosX = j;
                row[j].GetComponent<Tile>().tilePosY = i;
            }

            tiles[i] = row;
        }

        //플레이어 시작위치 바뀌면 해당 좌표 수정해 주어야함
        playerPos = new Vector2Int(0, 0);
        haveToWalkTile = playerPos;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Tile targetTile = FindClickedTile();

            if (targetTile != null)
            {
                if(eTouchType == ETouchType.Lock)
                {
                    targetTile.eTileType = ETileType.Lock;
                    targetTile.spriteRenderer.color = new Color(1, 0, 0, 0.6f);
                }else if(eTouchType == ETouchType.Move)
                {
                    targetTile.spriteRenderer.color = new Color(0, 0, 1, 0.6f);
                    ResetTiles();
                    StartCoroutine(C_AStar(playerPos, new Vector2Int(targetTile.tilePosX, targetTile.tilePosY)));
                }else if(eTouchType == ETouchType.Unlock)
                {
                    targetTile.eTileType = ETileType.Unlock;
                    targetTile.spriteRenderer.color = new Color(1, 1, 1, 0.6f);
                }
            }
        }

        if (playerPos != haveToWalkTile)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position,
                tiles[haveToWalkTile.y][haveToWalkTile.x].transform.position, Time.deltaTime * speed);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    Debug.Log(tiles[i][j].GetComponent<Tile>() + " next : " + tiles[i][j].GetComponent<Tile>().postTile);
                }
            }
        }
    }

    IEnumerator C_AStar(Vector2Int curIdx, Vector2Int targetIdx)
    {
        yield return new WaitForSeconds(0.1f);

        AStar(curIdx, targetIdx);
    }
    public void AStar(Vector2Int curIdx, Vector2Int targetIdx)
    {
        //데이터는 x, y값이 ','로 구분되어 저장
        PriorityQueue<string, int> priorityQueue = new PriorityQueue<string, int>(1);
        priorityQueue.Insert("first", int.MaxValue);
        priorityQueue.Insert(curIdx.x.ToString() + "," + curIdx.y.ToString(), 0);

        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { -1, 1, 0, 0 };
        while (priorityQueue.Top() != "first")
        {
            string[] data = priorityQueue.Top().Split(",");
            int posX = int.Parse(data[0]);
            int posY = int.Parse(data[1]);
            priorityQueue.Pop();

            //Debug.Log("current pos : " + posX + ", " + posY);

            Tile curTile = tiles[posY][posX].GetComponent<Tile>();
            //이미 방문한 노드라면 넘김
            if (curTile.isClosed) continue;
            curTile.isClosed = true;

            //목표 달성 조건
            if (posX == targetIdx.x && posY == targetIdx.y)
            {
                SetPostTileOfTiles(curIdx, targetIdx);
                Tile postTile = tiles[curIdx.y][curIdx.x].GetComponent<Tile>().postTile;
                haveToWalkTile = new Vector2Int(postTile.tilePosX, postTile.tilePosY);
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                int nextPosX = posX + dx[i], nextPosY = posY + dy[i];
                //다음노드가 범위 밖인 경우
                if (nextPosY < 0 || nextPosY >= tiles.Length || nextPosX < 0 || nextPosX >= tiles[nextPosY].Length) continue;

                Tile nextTile = tiles[nextPosY][nextPosX].GetComponent<Tile>();
                //다음 노드가 논리적으로 갈 필요 없는 경우
                if (nextTile.eTileType == ETileType.Lock || nextTile.isClosed) continue;

                if (nextTile.f == -1)
                {
                    nextTile.SetF(curTile.g + tileMoveWeight, DistanceTo(new Vector2Int(nextPosX, nextPosY), targetIdx));
                }
                else
                {
                    nextTile.SetF(curTile.g + tileMoveWeight);
                }

                //Debug.Log("insert queue : " + nextPosX + ", " + nextPosY + " weight : " + nextTile.f + " next h : " + nextTile.h);
                priorityQueue.Insert(nextPosX.ToString() + "," + nextPosY.ToString(), nextTile.f);
                nextTile.preTile = curTile;
            }
        }
    }

    public int DistanceTo(Vector2Int curIdx, Vector2Int targetIdx)
    {
        return (Mathf.Abs(curIdx.x - targetIdx.x) + Mathf.Abs(curIdx.y - targetIdx.y)) * tileMoveWeight;
    }

    public void ResetTiles()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                tiles[i][j].GetComponent<Tile>().ResetTile();
            }
        }
    }

    //AStar함수 내부에서만 사용되어야함
    void SetPostTileOfTiles(Vector2Int startPos, Vector2Int targetPos)
    {
        int posX = targetPos.x, posY = targetPos.y;
        Tile curTile = tiles[posY][posX].GetComponent<Tile>();

        while (!(startPos.x == curTile.tilePosX && startPos.y == curTile.tilePosY) &&
            curTile.preTile != curTile)
        {
            Tile preTile = curTile.preTile;
            preTile.postTile = curTile;
            curTile = preTile;
        }
    }

    Tile FindClickedTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(ray.origin, ray.direction * 20);
        Tile targetTile = null;
        float distanceTileToTouch = float.MaxValue;
        //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1.0f);

        for (int i = 0; i < raycastHit2D.Length; i++)
        {
            if (raycastHit2D[i].collider != null)
            {
                Tile tmpTile = raycastHit2D[i].collider.GetComponent<Tile>();
                if (tmpTile != null)
                {
                    float tmpDistance = Vector2.Distance(tmpTile.transform.position, player.GetComponent<PlayerController>().ExchangeScreenPosToWorldPos(Input.mousePosition));
                    if (distanceTileToTouch > tmpDistance)
                    {
                        targetTile = tmpTile;
                        distanceTileToTouch = tmpDistance;
                    }
                }
            }
        }

        return targetTile;
    }

    public void TouchTypeToMove()
    {
        eTouchType = ETouchType.Move;
    }
    public void TouchTypeToLock()
    {
        eTouchType = ETouchType.Lock;
    }
    public void TouchTypeToUnLock()
    {
        eTouchType = ETouchType.Unlock;
    }
}
