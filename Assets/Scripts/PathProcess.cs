using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathProcess : MonoBehaviour
{
    Queue<PathProcessSt> pathProcessStQueue = new Queue<PathProcessSt>();
    PathProcessSt currentPathProcess;

    static PathProcess instance;
    AStarFinding aStarFinding;

    bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        aStarFinding = GetComponent<AStarFinding>();
    }

    public static void PathProcessing(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathProcessSt newPathProcess = new PathProcessSt(pathStart, pathEnd, callback);
        instance.pathProcessStQueue.Enqueue(newPathProcess);
        instance.TryProcessNext();
        Debug.Log("PathProcessing");
    }

    void TryProcessNext()
    {
        if(!isProcessingPath && pathProcessStQueue.Count > 0)
        {
            currentPathProcess = pathProcessStQueue.Dequeue();
            isProcessingPath = true;
            aStarFinding.StartFindPath(currentPathProcess.pathStart, currentPathProcess.pathEnd);
            Debug.Log("TryNext");
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathProcess.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
        Debug.Log("Finished");
    } 
}
struct PathProcessSt
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callback;

    public PathProcessSt(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
    {
        pathStart = _start;
        pathEnd = _end;
        callback = _callback;
    }
}
