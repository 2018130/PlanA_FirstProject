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

    GameObject shareHouseCat;
    [SerializeField]
    public Vector2Int playerPos;
    [SerializeField]
    public Vector2Int haveToWalkTile;

    float speed = 5f;

    [SerializeField]
    ETouchType eTouchType = ETouchType.Move;

    bool isEndOfTileSync = false;

    private void Awake()
    {
        shareHouseCat = FindObjectOfType<SharehouseCat>().gameObject;

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

        //�÷��̾� ������ġ �ٲ�� �ش� ��ǥ ������ �־����
        //playerPos = new Vector2Int(2, 9);
        playerPos = new Vector2Int(4, 27);
        haveToWalkTile = playerPos;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && shareHouseCat.GetComponent<SharehouseCat>().GetOpenedPanelCount() == 0)
        {
            Tile targetTile = FindClickedTile();

            if (targetTile != null)
            {
                if (eTouchType == ETouchType.Lock)
                {
                    targetTile.LockTile();
                }
                else if (eTouchType == ETouchType.Move)
                {
                    ResetTiles();
                    StartCoroutine(C_AStar(playerPos, new Vector2Int(targetTile.tilePosX, targetTile.tilePosY)));
                }
                else if (eTouchType == ETouchType.Unlock)
                {
                    targetTile.eTileType = ETileType.Unlock;
                }
            }
        }

        //�÷��̾� ���� Ÿ�Ϸ� �����ϴ� ���
        if (isEndOfTileSync && playerPos != haveToWalkTile)
        {
            shareHouseCat.transform.position = Vector3.MoveTowards(shareHouseCat.transform.position,
                tiles[haveToWalkTile.y][haveToWalkTile.x].transform.position, Time.deltaTime * speed);
            shareHouseCat.GetComponent<SharehouseCat>().PlayWalkingAnimation(true);
        }
        else
        {
            shareHouseCat.GetComponent<SharehouseCat>().PlayWalkingAnimation(false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    Debug.Log((j + 1) + ", " + (i + 1)  + " next : " + tiles[i][j].GetComponent<Tile>().postTile);
                }
            }
        }
    }

    IEnumerator C_AStar(Vector2Int curIdx, Vector2Int targetIdx)
    {
        isEndOfTileSync = false;
        AStar(curIdx, targetIdx);

        yield return new WaitForSeconds(0.3f);

        isEndOfTileSync = true;
    }

    public void AStar(Vector2Int curIdx, Vector2Int targetIdx)
    {
        //�����ʹ� x, y���� ','�� ���еǾ� ����
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
            //�̹� �湮�� ����� �ѱ�
            if (curTile.isClosed) continue;
            curTile.isClosed = true;

            //��ǥ �޼� ����
            if (posX == targetIdx.x && posY == targetIdx.y)
            {
                SetPostTileOfTiles(curIdx, targetIdx);
                Tile postTile = tiles[curIdx.y][curIdx.x].GetComponent<Tile>().postTile;
                haveToWalkTile = new Vector2Int(postTile.tilePosX, postTile.tilePosY);
                return;
            }

            int preF = 0;
            for (int i = 0; i < 4; i++)
            {
                int nextPosX = posX + dx[i], nextPosY = posY + dy[i];
                //������尡 ���� ���� ���
                if (nextPosY < 0 || nextPosY >= tiles.Length || nextPosX < 0 || nextPosX >= tiles[nextPosY].Length) continue;

                Tile nextTile = tiles[nextPosY][nextPosX].GetComponent<Tile>();
                //���� ��尡 ���������� �� �ʿ� ���� ���
                if (nextTile.eTileType == ETileType.Lock || nextTile.isClosed) continue;

                if (nextTile.f == -1)
                {
                    nextTile.SetF(curTile.g + tileMoveWeight, DistanceTo(new Vector2Int(nextPosX, nextPosY), targetIdx));
                    nextTile.preTile = curTile;
                }
                else
                {
                    preF = nextTile.f;
                    nextTile.SetF(curTile.g + tileMoveWeight);
                }

                priorityQueue.Insert(nextPosX.ToString() + "," + nextPosY.ToString(), nextTile.f);
                if (preF != nextTile.f)
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

    //AStar�Լ� ���ο����� ���Ǿ����
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
        Vector2  touchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(touchPoint, new Vector2(0.001f, 0), 1f, 1 << LayerMask.NameToLayer("Tile"));

        if(hit.collider != null)
        {
            return hit.collider.GetComponent<Tile>();
        }

        return null;
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