using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public static Dictionary<Vector2, Point> AllPoints = new Dictionary<Vector2, Point>();

    private void Awake()
    {
        AllPoints.Clear();
        Time.timeScale = 0;
    }
}
