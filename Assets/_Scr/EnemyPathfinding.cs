using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    private EnemyMove _enemyMove;
    
    [Header("Path Finding")]
    public GameObject target;
    // 맵을 격자로 분할한다.
    EnemyAstar grid;
    private Rigidbody2D rg2d;
    // 남은거리를 넣을 큐 생성.
    public Queue<Vector2> wayQueue = new Queue<Vector2>();

    [Header("Player Ctrl")]

    // 뭔가와 상호작용 하고 있을때는 walkable이 false 가 됨.
    public static bool walkable = true;

    // 플레이어 이동/회전 속도 등 저장할 변수
    // 장애물/NPC 판단시 멈추게 할 범위
    public float range;

    public bool isWalk;
    public bool isWalking;

    private void Awake()
    {
        _enemyMove = GetComponent<EnemyMove>();
        rg2d = GetComponent<Rigidbody2D>();
        // 격자 생성
        this.grid = GameObject.Find("Astar").GetComponent<EnemyAstar>();
        print(grid);
        //grid = GetComponent<Grid>();
        walkable = true;
    }
    private void Start()
    {
        // 초깃값 초기화.
        this.isWalking = false;
        this.range = 4f;
        DoMove();
    }

    public void DoMove()
    {
        this.StartFindPath((Vector2)this.transform.position, (Vector2)this.target.transform.position);
    }
    private float time = 0f;
    private void Update()
    {
        time += Time.deltaTime;
        if (time > 0.5f)
        {
            DoMove();
            time = 0f;
        }
    }


    // start to target 이동.
    public void StartFindPath(Vector2 startPos, Vector2 targetPos)
    {
        this.StopAllCoroutines();
        this.StartCoroutine(FindPath(startPos, targetPos));
    }

    // 길찾기 로직.
    IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {   
        // start, target의 좌표를 grid로 분할한 좌표로 지정.
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
        
        // target에 도착했는지 확인하는 변수.
        bool pathSuccess = false;

        if (!startNode.walkable)
            Debug.Log("Unwalkable StartNode 입니다.");

        // walkable한 targetNode인 경우 길찾기 시작.
        if(targetNode.walkable)
        {
            // openSet, closedSet 생성.
            // closedSet은 이미 계산 고려한 노드들.
            // openSet은 계산할 가치가 있는 노드들.
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            // closedSet에서 가장 최저의 F를 가지는 노드를 빼낸다. 
            while (openSet.Count > 0)
            {
                // currentNode를 계산 후 openSet에서 빼야 한다.
                Node currentNode = openSet[0];
                // 모든 openSet에 대해, current보다 f값이 작거나, h(휴리스틱)값이 작으면 그것을 current로 지정.
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                        currentNode = openSet[i];              
                }
                // openSet에서 current를 뺀 후, closedSet에 추가.
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                // 방금 들어온 노드가 목적지 인 경우
                if (currentNode == targetNode)
                {
                    // seeker가 위치한 지점이 target이 아닌 경우
                    if(pathSuccess == false)
                    {
                       // wayQueue에 PATH를 넣어준다.
                       PushWay( RetracePath(startNode, targetNode) ) ;
                    }
                    pathSuccess = true;
                    break;
                }

                // current의 상하좌우 노드들에 대하여 g,h cost를 고려한다.
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;

                    // F cost 생성.
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    // 이웃으로 가는 F cost가 이웃의 G보다 짧거나, 방문해볼 Openset에 그 값이 없다면,
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        // openSet에 추가.
                        if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    }
                }
            }
        }       
        
        yield return null;

        // 길을 찾았을 경우(계산 다 끝난경우) 이동시킴.
        if(pathSuccess == true)
        {
            print("found way");
            // 이동중이라는 변수 ON
            this.isWalking = true;
            // wayQueue를 따라 이동시킨다.
            
            if ((Vector2)this.transform.position == this.wayQueue.First())
            {
                Debug.Log("Dequeue");
                this.wayQueue.Dequeue();
            }
            _enemyMove.SetQueue(wayQueue);
            
            this.isWalking = false;
        }
    }

    // WayQueue에 새로운 PATH를 넣어준다.
    void PushWay(Vector2[] array)
    {
        this.wayQueue.Clear();
        foreach (Vector2 item in array) this.wayQueue.Enqueue(item);
    }

    // 현재 큐에 거꾸로 저장되어있으므로, 역순으로 wayQueue를 뒤집어준다. 
    Vector2[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode  = endNode;
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        // Grid의 path에 찾은 길을 등록한다.
        this.grid.path = path;
        Vector2[] wayPoints = SimplifyPath(path);
        return wayPoints;
    }

    // Node에서 Vector 정보만 빼낸다.
    Vector2[] SimplifyPath(List<Node> path)
    {
        List<Vector2> wayPoints = new List<Vector2>();

        for(int i = 0; i < path.Count; i++)
        {
            wayPoints.Add(path[i].worldPosition);
        }
        return wayPoints.ToArray();
    }

    // custom g cost 또는 휴리스틱 추정치를 계산하는 함수.
    // 매개변수로 들어오는 값에 따라 기능이 바뀝니다.
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        // 대각선 - 14, 상하좌우 - 10.
        if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}