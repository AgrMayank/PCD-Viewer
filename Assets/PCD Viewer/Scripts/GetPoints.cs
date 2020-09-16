using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class GetPoints : MonoBehaviour
{
    [SerializeField]
    private String m_fileName;
    [SerializeField]
    private GameObject m_pointCloud;
    [SerializeField]
    private GameObject m_point;


    // Create Point Cloud from the given PCD file
    public void Start()
    {
        // Convert .pcd to .txt file
        File.Delete("Assets/PCD Viewer/PCD Files/" + m_fileName + ".txt");
        File.Copy("Assets/PCD Viewer/PCD Files/" + m_fileName + ".pcd", "Assets/PCD Viewer/PCD Files/" + m_fileName + ".txt");

        // Get file name
        string fileName = "Assets/PCD Viewer/PCD Files/" + m_fileName + ".txt";

        // Get total number of points in the file
        string points = GetLine(fileName, 10);
        points = Regex.Replace(points, "[^0-9]+", string.Empty);
        int totalPoints = Convert.ToInt32(points);
        Debug.Log(totalPoints);

        // Instantiate Points
        InstantiatePoints(fileName, totalPoints);
    }

    // Instantiates points under Point Cloud gameobject by getting the Vector3 values line by line
    void InstantiatePoints(string fileName, int totalPoints)
    {
        for (int i = 12; i < totalPoints; i++)
        {
            Vector3 xyz = GetXYZValue(fileName, i);

            var newPoint = Instantiate(m_point, xyz, Quaternion.identity);
            newPoint.transform.parent = m_pointCloud.transform;
            Debug.Log("Instantiated " + i);
        }

        // Save created point cloud file as a prefab
        PrefabUtility.SaveAsPrefabAsset(m_pointCloud, "Assets/PCD Viewer/Point Clouds/" + m_fileName + ".prefab");
        Debug.Log("Point Cloud saved to - \"Assets / PCD Viewer / Point Clouds / \" as " + m_fileName + ".prefab");
    }

    // Gets a single line from the file
    string GetLine(string fileName, int line)
    {
        using (var sr = new StreamReader(fileName))
        {
            for (int i = 1; i < line; i++)
                sr.ReadLine();
            return sr.ReadLine();
        }
    }

    // Returns Vector3 coordinates of a given line
    Vector3 GetXYZValue(string fileName, int line)
    {
        string data = GetLine(fileName, line);

        char[] seperators = { ',', ' ' };

        String[] strlist = data.Split(seperators,
            StringSplitOptions.None);

        float x_val = float.Parse(strlist[0]);
        float y_val = float.Parse(strlist[1]);
        float z_val = float.Parse(strlist[2]);

        Vector3 xyz = new Vector3(x_val, y_val, z_val);
        Debug.Log(xyz.ToString("F5"));

        return xyz;
    }
}