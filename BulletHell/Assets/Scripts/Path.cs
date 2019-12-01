using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] private Color pathColour = Color.red;
    [SerializeField] private Color bezierColour = Color.red;
    
    [SerializeField, Range(1, 20)] private int lineDensity = 1;
    [SerializeField] public bool visualizePath;
    [SerializeField] public List<Transform> pathObjList = new List<Transform>();
    


    [HideInInspector] public List<Vector3> bezierObjList = new List<Vector3>();
    private Transform[] objArray;
    private int overload;

    void Start()
    {
        CreatePath();
    }

    private void OnDrawGizmos()
    {
        if (visualizePath)
        {
            pathObjList.Clear();
            bezierObjList.Clear();

            Gizmos.color = pathColour;

            // Set and create lists
            objArray = GetComponentsInChildren<Transform>(); // Fill array
            pathObjList.Clear(); // Clear list
            foreach (Transform obj in objArray)
            {
                if (obj != this.transform)
                {
                    pathObjList.Add(obj);
                }
            }

            // Draw Objects
            for (int ii = 0; ii < pathObjList.Count; ii++)
            {
                Vector3 pos = pathObjList[ii].position;
                if (ii > 0)
                {
                    Vector3 prevPos = pathObjList[ii - 1].position;
                    Gizmos.DrawLine(pos, prevPos);
                    Gizmos.DrawWireSphere(pos, 0.2f);
                }
            }
            
            // Check the overload
            if (pathObjList.Count % 2 == 0)
            {
                pathObjList.Add(pathObjList[pathObjList.Count - 1]);
                overload = 2;
            }
            else
            {
                pathObjList.Add(pathObjList[pathObjList.Count - 1]);
                pathObjList.Add(pathObjList[pathObjList.Count - 1]);
                overload = 3;
            }

            bezierObjList.Clear();

            // Curved path
            Vector3 lineStart = pathObjList[0].position;
            for (int ii = 0; ii < pathObjList.Count - overload; ii += 2)
            {
                for (int jj = 0; jj <= lineDensity; jj++)
                {
                    Vector3 lineEnd = GetPoint(pathObjList[ii].position, pathObjList[ii + 1].position, pathObjList[ii + 2].position, (float)jj / (float)lineDensity);

                    Gizmos.color = bezierColour;
                    Gizmos.DrawLine(lineStart, lineEnd);

                    Gizmos.DrawWireSphere(lineStart, 0.1f);

                    lineStart = lineEnd;

                    bezierObjList.Add(lineStart);

                }
            }
        }
    }

    private Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
    }

    private void CreatePath()
    {
        // Set and create lists
        objArray = GetComponentsInChildren<Transform>(); // Fill array
        pathObjList.Clear(); // Clear list
        foreach (Transform obj in objArray)
        {
            if (obj != this.transform)
            {
                pathObjList.Add(obj);
            }
        }

        // Check the overload
        if (pathObjList.Count % 2 == 0)
        {
            pathObjList.Add(pathObjList[pathObjList.Count - 1]);
            overload = 2;
        }
        else
        {
            pathObjList.Add(pathObjList[pathObjList.Count - 1]);
            pathObjList.Add(pathObjList[pathObjList.Count - 1]);
            overload = 3;
        }

        bezierObjList.Clear();

        // Curved path
        Vector3 lineStart = pathObjList[0].position;
        for (int ii = 0; ii < pathObjList.Count - overload; ii += 2)
        {
            for (int jj = 0; jj <= lineDensity; jj++)
            {
                Vector3 lineEnd = GetPoint(pathObjList[ii].position, pathObjList[ii + 1].position, pathObjList[ii + 2].position, (float)jj / (float)lineDensity);
                lineStart = lineEnd;
                bezierObjList.Add(lineStart);
            }
        }
    }
}
 