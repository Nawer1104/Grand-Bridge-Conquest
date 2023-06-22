using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Point : MonoBehaviour
{
    public bool runtime = true;
    public List<Bar> connectedBar;
    public Vector2 pointID;
    public Rigidbody2D rbd;

    private void Start()
    {
        if (!runtime)
        {
            rbd.bodyType = RigidbodyType2D.Static;
            pointID = transform.position;
            if (GameManger.AllPoints.ContainsKey(pointID) == false) GameManger.AllPoints.Add(pointID, this);
        }
    }

    private void Update()
    {
        if (!runtime)
        {
            if (transform.hasChanged == true)
            {
                transform.hasChanged = false;
                transform.position = Vector3Int.RoundToInt(transform.position);
            }
        }
    }
}
