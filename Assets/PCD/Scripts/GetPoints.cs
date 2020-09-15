using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class GetPoints : MonoBehaviour {
    [SerializeField]
    private String m_fileName;
    [SerializeField]
    private GameObject m_pointCloud;
    [SerializeField]
    private GameObject m_point;

    private void OnEnable () {
        // File.Move ("Assets/PCD/phpbos0Ft.pcd", "Assets/PCD/Points/phpbos0Ft.txt");
        File.Delete ("Assets/PCD/Points/" + m_fileName + ".txt");
        File.Copy ("Assets/PCD/" + m_fileName + ".pcd", "Assets/PCD/Points/" + m_fileName + ".txt");

        string fileName = "Assets/PCD/Points/" + m_fileName + ".txt";
        string points = GetLine (fileName, 10);

        points = Regex.Replace (points, "[^0-9]+", string.Empty);
        int totalPoints = Convert.ToInt32 (points);
        Debug.Log (totalPoints);

        InstantiatePoints (fileName, totalPoints);
    }

    void InstantiatePoints (string fileName, int totalPoints) {
        for (int i = 12; i < totalPoints; i++) {
            Vector3 xyz = GetXYZValue (fileName, i);

            var newPoint = Instantiate (m_point, xyz, Quaternion.identity);
            newPoint.transform.parent = m_pointCloud.transform;
            Debug.Log ("Instantiated " + i);
        }

        PrefabUtility.SaveAsPrefabAsset (m_pointCloud, "Assets/PCD/Prefabs/" + m_fileName + ".prefab");
    }

    string GetLine (string fileName, int line) {
        using (var sr = new StreamReader (fileName)) {
            for (int i = 1; i < line; i++)
                sr.ReadLine ();
            return sr.ReadLine ();
        }
    }

    Vector3 GetXYZValue (string fileName, int line) {
        string data = GetLine (fileName, line);

        char[] seperators = { ',', ' ' };

        String[] strlist = data.Split (seperators,
            StringSplitOptions.None);

        float x_val = float.Parse (strlist[0]);
        float y_val = float.Parse (strlist[1]);
        float z_val = float.Parse (strlist[2]);

        Vector3 xyz = new Vector3 (x_val, y_val, z_val);
        Debug.Log (xyz.ToString ("F5"));

        return xyz;
    }
}