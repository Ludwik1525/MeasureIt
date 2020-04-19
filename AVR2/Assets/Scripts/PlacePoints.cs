using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacePoints : MonoBehaviour
{
    public GameObject pointPrefab;

    public List<GameObject> placedPoints;
    public List<GameObject> placedLines;
    public List<float> distances;

    public GameObject placedPointsRoot;
    public GameObject placedLinesRoot;
    public GameObject linePrefab;

    public Button spawnButton;
    public Button clearButton;
    public Button hamburgerButton;

    private GameObject spawnCircle;
    private GameObject placedPoint;

    private Pose hitPose;
    private ARRaycastManager arRaycastManager;

    public GameObject warningTooClose;

    string currentUnit;
    public Text totalLength;

    void Awake()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        spawnCircle = transform.GetChild(0).gameObject;
        spawnCircle.SetActive(false);

        spawnButton.onClick.AddListener(PlacePoint);
        clearButton.onClick.AddListener(DeletePoints);

        placedPoints = new List<GameObject>();
        warningTooClose.SetActive(false);

        currentUnit = "CM";
    }

    void Update()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon);

        if (hits.Count > 0)
        {

            if (Input.GetTouch(0).position.y > Screen.height / 6)
            {
                spawnCircle.transform.position = hits[0].pose.position;
                spawnCircle.transform.rotation = hits[0].pose.rotation;

                if (!spawnCircle.activeInHierarchy)
                {
                    spawnCircle.SetActive(true);
                }
            }

        }
        else
        {
            spawnCircle.SetActive(false);
        }
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    // function to place a point
    public void PlacePoint()
    {
        if(hamburgerButton.GetComponent<HamburgerOpener>().isOpen)
        hamburgerButton.GetComponent<HamburgerOpener>().ToggleHamburger();

        // if there is at least 1 point already, also create a line
        if (placedPoints.Count > 0)
        {
            if (!IsTooClose())
            {
                placedPoint = Instantiate(pointPrefab, spawnCircle.transform.localPosition, spawnCircle.transform.localRotation);
                placedPoint.transform.SetParent(placedPointsRoot.transform);
                placedPoints.Add(placedPoint);
                warningTooClose.SetActive(false);
                
                PlaceLine(placedPoints[placedPoints.Count - 2].transform.position, placedPoints[placedPoints.Count - 1].transform.position);
            }
            // if a point is too close to any of the other points, display a warning
            else
            {
                warningTooClose.SetActive(false);
                warningTooClose.SetActive(true);
            }
        }
        // if there are no points yet, don't create lines
        else
        {
            placedPoint = Instantiate(pointPrefab, spawnCircle.transform.localPosition, spawnCircle.transform.localRotation);
            placedPoint.transform.SetParent(placedPointsRoot.transform);
            placedPoints.Add(placedPoint);
        }
    }

    // a bool checking if the point to be added is not too close to any of the other placed points
    bool IsTooClose()
    {
        foreach (GameObject point in placedPoints)
        {
            float distanceSqr = (spawnCircle.transform.position - point.transform.position).sqrMagnitude;

            if (distanceSqr < 0.01f)
            {
                return true;
            }
        }
        return false;
    }

    // a method placing a line between the last and the newest point
    void PlaceLine(Vector3 startPos, Vector3 endPos)
    {
        // calculating the center point between two endpoints
        Vector3 centerPos = new Vector3(startPos.x + endPos.x, startPos.y + endPos.y, startPos.z + endPos.z) / 2f;

        // calculating the length of the line
        float distance = Vector3.Distance(startPos, endPos);

        // save the distance
        distances.Add(distance);

        // instantiating the line in the center point with an appropriate rotation
        GameObject line = Instantiate(linePrefab);

        line.transform.SetParent(placedLinesRoot.transform);

        line.transform.position = centerPos;

        // setting the line's length
        line.transform.GetChild(0).localScale = new Vector3(distance, 0.001f, 0.03f);

        // rotating the line appropriately
        line.transform.GetChild(0).LookAt(endPos);
        line.transform.GetChild(0).Rotate(0, 90, 0);

        // adding the rotation script
        line.transform.GetChild(1).gameObject.AddComponent<LabelRotator>();

        placedLines.Add(line);

        // setting the measurement
        DisplayDistance(line, (distance*100));
    }

    // calculating the length of the line
    public void DisplayDistance(GameObject line, float distance)
    {
        string result = "";

        // checking what unit is currently in use
        switch(currentUnit)
        {
            case "MM":
                distance = distance * 10;
                result = distance.ToString("0.##") + " mm";
                break;
            case "CM":
                result = distance.ToString("0.##") + " cm";
                break;
            case "DM":
                distance = distance / 10;
                result = distance.ToString("0.##") + " dm";
                break;
            case "M":
                distance = distance / 100;
                result = distance.ToString("0.##") + " m";
                break;
        }

        line.transform.GetChild(1).GetComponent<TextMesh>().text = result;
        totalLength.text = "Total\nlength:\n" + CalculateTotalLength(currentUnit);
    }

    // deleting all the points, all the lines and all the measurements' values
    void DeletePoints()
    {
        hamburgerButton.GetComponent<HamburgerOpener>().ToggleHamburger();

        foreach (GameObject point in placedPoints)
        {
            Destroy(point);
        }

        foreach(GameObject line in placedLines)
        {
            Destroy(line);
        }

        placedPoints.Clear();
        placedLines.Clear();
        distances.Clear();

        totalLength.text = "Total\nlength:\n" + CalculateTotalLength(currentUnit);
    }

    // changing the unit which is currently in use
    public void ChangeUnit(string unit)
    {
        currentUnit = unit;
        totalLength.text = "Total\nlength:\n" + CalculateTotalLength(currentUnit);

        if (placedLines.Count > 0)
        {
            for(int i = 0; i < placedLines.Count; i++)
            {
                DisplayDistance(placedLines[i], (distances[i] * 100));
            }

        }
    }

    // calculating the total length
    public string CalculateTotalLength(string unit)
    {
        float total = 0;
        string result = "";

        foreach(float distance in distances)
        {
            total += (distance * 100);
        }

        switch(unit)
        {
            case "MM":
                total = total * 10;
                result = total.ToString("0.##") + " mm";
                break;
            case "CM":
                result = total.ToString("0.##") + " cm";
                break;
            case "DM":
                total = total / 10;
                result = total.ToString("0.##") + " dm";
                break;
            case "M":
                total = total / 100;
                result = total.ToString("0.##") + " m";
                break;
        }

        return result;
    }
}
