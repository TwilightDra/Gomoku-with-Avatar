using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    public Transform target;
    float speed = 5f;
    Vector3[] path;
    int targetIndex;

    private void Start()
    {
        //StartCoroutine(UpdatePath());
    }

    public void StartPathFind()
    {
        PathProcess.PathProcessing(transform.position, target.position, OnPathFound);
        //Debug.Log("Drive.cs:" + transform.position + "  " + target.position);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            Debug.Log("OnPathFound");
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
        else
        {
            Debug.Log("Failed");
            this.GetComponent<Transform>().position = new Vector3(-6f, 0f, -6f);
            /*path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");*/
            GameObject.Find("GameManager").GetComponent<GameManagement>().DispPiece();
        }
    }

    /*IEnumerator UpdatePath()
    {
        if (target != null)
        {
            float sqrMoveThreshold = 0.2f;
            Vector3 targetPosOld = target.position;
            Debug.Log("Test");
            while (true)
            {
                PathProcess.PathProcessing(transform.position, target.position, OnPathFound);
                yield return new WaitForSeconds(0.2f);
                if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
                {
                    PathProcess.PathProcessing(transform.position, target.position, OnPathFound);
                    targetPosOld = target.position;
                }
            }
        }
    }*/

    
    IEnumerator FollowPath()
    {
        bool followingPath = true;
        //int pathIndex = 0;
        Vector3 currentWaypoint = path[0];
        Debug.Log("XunLu");
        while (followingPath)
        {
            if (transform.position == currentWaypoint)
            {
                if (targetIndex >= path.Length-1)
                {
                    path = null;
                    targetIndex = 0;
                    followingPath = false;
                    GameObject.Find("GameManager").GetComponent<GameManagement>().DispPiece();
                    yield break;
                }
                else targetIndex++;
                currentWaypoint = path[targetIndex];
            }
            if (followingPath)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
               // Debug.Log("trans:" + transform.position + "   " + currentWaypoint);
            }
            
            yield return null;
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
