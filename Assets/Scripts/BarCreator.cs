using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BarCreator : MonoBehaviour, IPointerDownHandler
{
    public static BarCreator Instance;

    public GameObject roadBar;
    public GameObject tileBar;

    public bool barCreationStarted = false;
    public Bar currentBar;
    public GameObject barToInstantiate;
    public Transform barParent;
    public Point currentStartPoint;
    public Point currentEndPoint;
    public GameObject pointToInstantiate;
    public Transform pointParent;
    public bool gameStart = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameStart) return;

        if (!barCreationStarted)
        {
            barCreationStarted = true;
            StartBarCreation(Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(eventData.position)));
        } else
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                FinishBarCreation();
            } 

        }
    }

    private void StartBarCreation(Vector2 startPos)
    {
        if (gameStart) return;

        currentBar = Instantiate(barToInstantiate, barParent).GetComponent<Bar>();
        currentBar.startPos = startPos;

        if (GameManger.AllPoints.ContainsKey(startPos))
        {
            currentStartPoint = GameManger.AllPoints[startPos];
        }
        else
        {
            currentStartPoint = Instantiate(pointToInstantiate, startPos, Quaternion.identity, pointParent).GetComponent<Point>();
            GameManger.AllPoints.Add(startPos, currentStartPoint);
        }

        
        currentEndPoint = Instantiate(pointToInstantiate, startPos, Quaternion.identity, pointParent).GetComponent<Point>();
    }

    public void FinishBarCreation()
    {
        if (gameStart) return;

        if (GameManger.AllPoints.ContainsKey(currentEndPoint.transform.position))
        {
            Destroy(currentEndPoint.gameObject);
            currentEndPoint = GameManger.AllPoints[currentEndPoint.transform.position];
        }
        else
        {
            GameManger.AllPoints.Add(currentEndPoint.transform.position, currentEndPoint);
        }

        currentStartPoint.connectedBar.Add(currentBar);
        currentEndPoint.connectedBar.Add(currentBar);

        currentBar.startJoint.connectedBody = currentStartPoint.rbd;
        currentBar.startJoint.anchor = currentBar.transform.InverseTransformPoint(currentBar.startPos);

        currentBar.endJoint.connectedBody = currentEndPoint.rbd;
        currentBar.endJoint.anchor = currentBar.transform.InverseTransformPoint(currentEndPoint.transform.position);

        StartBarCreation(currentEndPoint.transform.position);
    }

    public void DeleteCurrentBar()
    {
        if (gameStart) return;

        if (currentBar != null && currentStartPoint != null && currentEndPoint != null)
        {
            Destroy(currentBar.gameObject);
            if (currentStartPoint.connectedBar.Count == 0 && currentStartPoint.runtime == true) Destroy(currentStartPoint.gameObject);
            if (currentEndPoint.connectedBar.Count == 0 && currentEndPoint.runtime == true) Destroy(currentEndPoint.gameObject);
        }
    }

    private void Update()
    {
        if (gameStart) return;

        if (barCreationStarted)
        {
            Vector2 endPos = (Vector2)Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Vector2 dir = endPos - currentBar.startPos;
            Vector2 clampedPos = currentBar.startPos + Vector2.ClampMagnitude(dir, currentBar.maxLength);

            currentEndPoint.transform.position = (Vector2)Vector2Int.FloorToInt(clampedPos);
            currentEndPoint.pointID = currentEndPoint.transform.position;
            currentBar.UpdateCreatingBar(currentEndPoint.transform.position);
        }
    }
}
