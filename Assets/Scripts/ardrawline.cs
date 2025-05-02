using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ardrawline : MonoBehaviour
{
    [Header("UI References")]
    public Text text;  // Make this public so you can assign it in the Inspector
    public Camera camera;
    private List<Vector3> list = new List<Vector3>();
    public GameObject linePrefab;
    public GameObject currentLine;
    private Vector3 camPos;
    private Vector3 camDirection;
    private Quaternion camRotation;
    float spawnDistance = 1f;
    private LineRenderer lineRenderer;
    public bool draw = false;
    public Text drawMode;


    // Start is called before the first frame update
    void Start()
    {
        // Check if text is assigned
        if (text != null) {
            text.text = "false";
        }
        
        if (drawMode != null) {
            drawMode.text = "Draw";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (draw)
        {

            if (Input.GetMouseButtonDown(0))
            {
                createLine();


            }
            else if (Input.GetMouseButton(0))
            {
                updateLine();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                list.Clear();
            }

        }

    }

   

    public void updateLineCenter()
    {
        Vector3 spawnPos = getSpawnPos();

        // Check if list is empty to prevent index out of range error
        if (list.Count == 0)
        {
            Debug.Log("List was empty, adding initial point");
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, spawnPos);
            lineRenderer.SetPosition(1, spawnPos);
            list.Add(spawnPos);
            return;
        }

        // Now we can safely check distance
        if (Vector3.Distance(spawnPos, list[list.Count - 1]) > 0.01f)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, spawnPos);
            list.Add(spawnPos);
            Debug.Log($"Added new point to line at {spawnPos}. Total points: {lineRenderer.positionCount}");
        }
        else
        {
            Debug.Log("Spawn position too close to the last point, skipping update.");
        }
    }

    public void createLineCenter()
    {
        Vector3 spawnPos = getSpawnPos();
        
        // Clear the list for new line
        list.Clear();
        
        // Create the line object
        currentLine = Instantiate(linePrefab, spawnPos, camRotation);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component missing on linePrefab!");
            return;
        }
        
        // Initialize with 2 points
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, spawnPos);
        lineRenderer.SetPosition(1, spawnPos); // Start with both at the same position
        
        // Add the starting point to the list
        list.Add(spawnPos);
        
        Debug.Log($"Line created at {spawnPos}. LineRenderer initialized with 2 points.");
    }

    private Vector3 getSpawnPos()
    {
        // If the class field spawnPos is set from outside (hand tracking), use that
        if (this.spawnPos != Vector3.zero)
        {
            return this.spawnPos;
        }
        
        // Otherwise use the default camera-based positioning
        camPos = camera.transform.position;
        camDirection = camera.transform.forward;
        camRotation = camera.transform.rotation;
        Vector3 calculatedPos = camPos + (camDirection * spawnDistance);
        
        return calculatedPos;
    }

 

    private void createLine()
    {
        // Add these null checks at the beginning
        if (linePrefab == null)
        {
            Debug.LogError("Line prefab is missing!");
            return;
        }
        
        if (camera == null)
        {
            Debug.LogError("Camera reference is missing!");
            return;
        }
        
        // Rest of your original code
        var pos = computeScreenToWorld();

        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component missing on linePrefab!");
            return;
        }
        
        lineRenderer.SetPosition(0, pos);
        list.Add(pos);
        pos = computeScreenToWorld();
        lineRenderer.SetPosition(1, pos);
        list.Add(pos);
            
        text.text = "true";


    }

    private void updateLine()
    {
        var pos = computeScreenToWorld();
        if(Vector3.Distance(pos,list[list.Count-1]) >0.01f)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, pos);
            list.Add(pos);
        }
    }

    private Vector3 computeScreenToWorld()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        return ray.GetPoint(1f);

    }

    public void handleDrawing()
    {
        if(drawMode.text == "View")
        {
            draw = false;
            drawMode.text = "Draw";

        }else if(drawMode.text == "Draw")
        {
            draw = true;
            drawMode.text = "View";
        }
    }

    // Add these public methods at the end of the ardrawline class

    // Make spawnPos public so it can be set from outside
    public Vector3 spawnPos;

    // Expose these methods for external control
    public void StartPinchDrawing(Vector3 position)
    {
        spawnPos = position;
        createLineCenter();
    }

    public void UpdatePinchDrawing(Vector3 position)
    {
        spawnPos = position;
        updateLineCenter();
    }

    public void StopPinchDrawing()
    {
        // Any cleanup needed when drawing stops
    }
}
