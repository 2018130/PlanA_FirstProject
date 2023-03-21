using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructures.PriorityQueue;

public class TileBundle : MonoBehaviour
{
    int tileMoveWeight = 10;

    GameObject[][] tiles;

    [SerializeField]
    GameObject player;
    public Vector2Int playerPos;

    public Vector2Int haveToWalkTile;
    float speed = 10f;
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
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(ray.origin, ray.direction, 1000f);
            Tile targetTile = null;
            float distanceTileToTouch = float.MaxValue;

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

            if (targetTile != null)
            {
                targetTile.spriteRenderer.color = new Color(0, 0, 1, 0.6f);
                ResetTiles();
                AStar(playerPos, new Vector2Int(targetTile.tilePosX, targetTile.tilePosY));
            }
        }

        if (playerPos != haveToWalkTile)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position,
                tiles[haveToWalkTile.y][haveToWalkTile.x].transform.position, 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            for(int i = 0; i < tiles.Length; i++)
            {
                for(int j = 0; j < tiles[i].Length; j++)
                {
                    Debug.Log(tiles[i][j].GetComponent<Tile>() + " next : " + tiles[i][j].GetComponent<Tile>().postTile);
                }
            }
        }
    }

    public void AStar(Vector2Int curIdx, Vector2Int targetIdx)
    {
        //데이터는 x, y값이 ','로 구분되어 저장
        PriorityQueue<string, int> priorityQueue = new PriorityQueue<string, int>(1);
        priorityQueue.Insert("first", int.MaxValue);
        priorityQueue.Insert(curIdx.x.ToString() + "," + curIdx.y.ToString(), DistanceTo(curIdx, targetIdx));

        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { -1, 1, 0, 0 };
        while (priorityQueue.Top() != "first")
        {
            string[] data = priorityQueue.Top().Split(",");
            int posX = int.Parse(data[0]);
            int posY = int.Parse(data[1]);
            priorityQueue.Pop();

            Tile curTile = tiles[posY][posX].GetComponent<Tile>();
            curTile.isClosed = true;
            if (posX == targetIdx.x && posY == targetIdx.y)
            {
                Tile nextTile = tiles[playerPos.y][playerPos.x].GetComponent<Tile>().postTile;
                haveToWalkTile = new Vector2Int(nextTile.tilePosX, nextTile.tilePosY);
                return;
            }

            int nextLowerF = int.MaxValue;
            Tile nextLowerTile = null;

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

                priorityQueue.Insert(nextPosX.ToString() + "," + nextPosY.ToString(), nextTile.f);
                if(nextLowerF > nextTile.f)
                {
                    nextLowerF = nextTile.f;
                    nextLowerTile = nextTile;
                }
            }

            if (nextLowerTile != null) curTile.postTile = nextLowerTile;
        }
    }

    public int DistanceTo(Vector2Int curIdx, Vector2Int targetIdx)
    {
        return Mathf.Abs(curIdx.x - targetIdx.x) + Mathf.Abs(curIdx.y - targetIdx.y) * tileMoveWeight;
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
}
