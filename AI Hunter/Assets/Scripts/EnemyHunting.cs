using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyHunting : MonoBehaviour
{
    const float minPathupdateTime = .2f;
    const float pathUpdateMoveThreshhold = .5f;
    //Setting up changable variables for enemies speeds
    public float enemySpeed = 1f;
    public Transform Player;
    public Transform mainTarget;
    Rigidbody rb;
    private bool targetChange = false;
    public Transform[] listOfPlayers;
    public bool aPlayerIsStun = true;
    Vector3[] path;
    int targetIndex = 0;
    private bool playerinbound = true;
    PathFinding pathFinder;
    Transform closestPlayer;
    public GameObject[] DPS;
    public GameObject[] HEALER;
    public GameObject[] TANK;
    Transform oldTarget;
    public float oldSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
       
        DPS = GameObject.FindGameObjectsWithTag("DPS");
        HEALER = GameObject.FindGameObjectsWithTag("Healer");
        TANK = GameObject.FindGameObjectsWithTag("Tank");
        GameObject[] players = DPS.Concat(HEALER).ToArray().Concat(TANK).ToArray();
        players = DPS.Concat(HEALER).ToArray().Concat(TANK).ToArray();
        listOfPlayers = new Transform[players.Length];

        for (int i = 0; i < listOfPlayers.Length; ++i)
            listOfPlayers[i] = players[i].transform;

        //mainTarget = GameObject.FindGameObjectWithTag("MainObjective").GetComponent<Transform>();
        //Transform closestPlayer = FindClosestPlayer(listOfPlayers);
        //pathFinder.StartFindPath(transform.position, closestPlayer.position);
        //PathRequestManager.RequestPath(transform.position, closestPlayer.position, OnPathFound);
        Transform closestPlayer = FindClosestPlayer(listOfPlayers);
        oldTarget = closestPlayer;
        StartCoroutine("UpdatePath");

    }

    // Update is called once per frame
    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        closestPlayer = FindClosestPlayer(listOfPlayers);
        PathRequestManager.RequestPath(transform.position, closestPlayer.position, OnPathFound);

        float sqrMoveThreshhold = pathUpdateMoveThreshhold * pathUpdateMoveThreshhold;
        Vector3 targetPosOld = closestPlayer.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathupdateTime);
            if ((closestPlayer.position - targetPosOld).sqrMagnitude > sqrMoveThreshhold)
            {
                PathRequestManager.RequestPath(transform.position, closestPlayer.position, OnPathFound);
                targetPosOld = closestPlayer.position;
            }
        }
    }
    void Update()
    {
        // new multiplayer chase code
        
        Transform closestPlayer = FindClosestPlayer(listOfPlayers);
        if(oldTarget != closestPlayer)
        {
            oldTarget = closestPlayer;
            StartCoroutine("UpdatePath");
        }
        
        //PathRequestManager.RequestPath(this.transform.position, closestPlayer.position, OnPathFound);
        /*if (targetChange)
        {
            //Vector3 objectivePosition = Vector3.MoveTowards(transform.position, mainTarget.position, enemySpeed * Time.fixedDeltaTime);
            //rb.MovePosition(objectivePosition);
            PathRequestManager.RequestPath(transform.position, mainTarget.position, OnPathFound);
        }
        */
        /*
        Vector3 position = Vector3.MoveTowards(transform.position, closestPlayer.position, enemySpeed * Time.fixedDeltaTime);
        rb.MovePosition(position);
        
        */

    }

    Transform FindClosestPlayer(Transform[] players)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Transform potentialTarget in players)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {

                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }


        }

        return bestTarget;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "MainObjective")
        {
            targetChange = true;

        }
        if (other.gameObject.name == "Player")
        {
            playerinbound = false;
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            if (path.Length > 0)
            {
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");

            }

        }
    }



    IEnumerator FollowPath()
    {

        Vector3 currentWaypoint = path[0];
        targetIndex = 0;
        //print(lengthOfCurrent);
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                //path[targetIndex].y = 0;

                currentWaypoint = path[targetIndex];
            }

            //print(currentWaypoint);

            //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            //Debug.Log("currentWaypoint y = " + currentWaypoint.y);
            currentWaypoint.y = 2.5f;
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, enemySpeed * Time.deltaTime);
            yield return null;
            //Vector3 positioning = transform.position;
            //rb.MovePosition(positioning);

        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
