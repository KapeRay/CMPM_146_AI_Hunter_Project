using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPathfinding : MonoBehaviour
{
    const float minPathupdateTime = .2f;
    const float pathUpdateMoveThreshhold = .5f;
    //Setting up changable variables for enemies speeds
    public float playerSpeed = 1f;
    Rigidbody rb;
    private bool targetChange = false;
    //public Transform[] listOfPlayers;
    Vector3[] path;
    int targetIndex = 0;
    private bool playerinbound = true;
    PathFinding pathFinder;
    Transform closestPlayer;
    private Vector3 endingPos;
    public MeshFilter plane;
    PlayerControl control;
    // Start is called before the first frame update
    void Start()
    {
        //endingPos = transform.position;
        control = GetComponent<PlayerControl>();
        
    }
    IEnumerator UpdatePath()
    {
        //endingPos = control.mousePos;
        //Debug.Log(endingPos);
        //Debug.Log(mousePos);
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        //closestPlayer.position = mousePos;
        
        PathRequestManager.RequestPath(transform.position, endingPos, OnPathFound);

        float sqrMoveThreshhold = pathUpdateMoveThreshhold * pathUpdateMoveThreshhold;
        Vector3 targetPosOld = endingPos;

        while (true)
        {
            yield return new WaitForSeconds(minPathupdateTime);
            if ((endingPos - targetPosOld).sqrMagnitude > sqrMoveThreshhold)
            {
                PathRequestManager.RequestPath(transform.position, endingPos, OnPathFound);
                targetPosOld = endingPos;
            }
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !gameObject.GetComponent<CharacterHealth>().characterIsDead)
        {
            StartCoroutine(UpdatePath());
        }
        endingPos = control.mousePos;
        //Debug.Log(endingPos);
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
            Debug.Log("currentWaypoint y = " + currentWaypoint.y);
            currentWaypoint.y = 1;
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, playerSpeed * Time.deltaTime);
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
