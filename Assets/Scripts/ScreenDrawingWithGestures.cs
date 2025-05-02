using System.Collections.Generic;
using UnityEngine;

public class ScreenDrawingWithGestures : MonoBehaviour
{
    [Header("Line Settings")]
    public GameObject linePrefab; // Prefab with a LineRenderer component
    public Camera mainCamera;     // Reference to the main camera

    private LineRenderer currentLine;
    private List<Vector2> points = new List<Vector2>();

    [Header("Gesture Settings")]
    public HandDrawingController handDrawingController; // Reference to the HandDrawingController

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Automatically assign the main camera if not set
        }

        if (linePrefab == null)
        {
            Debug.LogError("Line prefab is missing! Assign a prefab with a LineRenderer component.");
        }

        if (handDrawingController == null)
        {
            Debug.LogError("HandDrawingController reference is missing! Assign it in the Inspector.");
        }
    }

    void Update()
    {
        // Check if a pinch gesture is detected
        if (handDrawingController.isDrawing)
        {
            Vector3 handPosition = handDrawingController.drawPosition;

            // Convert the hand position to screen space
            Vector2 screenPosition = mainCamera.WorldToScreenPoint(handPosition);

            if (currentLine == null)
            {
                StartLine(screenPosition);
            }
            else
            {
                UpdateLine(screenPosition);
            }
        }
        else
        {
            // Stop drawing when the pinch gesture ends
            if (currentLine != null)
            {
                StopLine();
            }
        }
    }

    void StartLine(Vector2 screenPosition)
    {
        // Instantiate a new line from the prefab
        GameObject newLine = Instantiate(linePrefab);
        currentLine = newLine.GetComponent<LineRenderer>();

        if (currentLine == null)
        {
            Debug.LogError("LineRenderer component missing on the line prefab!");
            return;
        }

        // Initialize the line
        currentLine.positionCount = 0;
        points.Clear();

        // Add the first point
        AddPoint(screenPosition);

        Debug.Log("Started a new line.");
    }

    void UpdateLine(Vector2 screenPosition)
    {
        // Add a new point if the distance from the last point is significant
        if (points.Count == 0 || Vector2.Distance(points[points.Count - 1], screenPosition) > 10f)
        {
            AddPoint(screenPosition);
        }
    }

    void StopLine()
    {
        Debug.Log("Stopped drawing the line.");
        currentLine = null;
    }

    void AddPoint(Vector2 screenPosition)
    {
        points.Add(screenPosition);
        currentLine.positionCount = points.Count;

        // Update the LineRenderer positions
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(points[i].x, points[i].y, mainCamera.nearClipPlane));
            currentLine.SetPosition(i, worldPosition);
        }

        Debug.Log($"Added point at {screenPosition}. Total points: {points.Count}");
    }
}