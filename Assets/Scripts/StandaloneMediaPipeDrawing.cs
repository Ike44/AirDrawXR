using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity;
using Mediapipe.Unity.Sample.HandLandmarkDetection;

public class StandaloneMediaPipeDrawing : MonoBehaviour
{
    [Header("MediaPipe Components")]
    [SerializeField] private HandLandmarkerRunner handLandmarkerRunner;
    [SerializeField] private HandLandmarkerResultAnnotationController annotationController;
    
    [Header("Line Settings")]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Camera mainCamera;
    [Range(0.001f, 0.01f), SerializeField] private float lineWidth = 0.002f; // Much thinner default
    [Range(0f, 1f), SerializeField] private float lineOpacity = 1.0f;
    
    [Header("Debug")]
    [SerializeField] private Text debugText;
    
    [Header("Gesture Settings")]
    [SerializeField, Range(0.01f, 0.2f)] 
    private float pinchThreshold = 0.05f;
    
    [SerializeField, Range(0.0f, 0.1f)] 
    private float pinchHysteresis = 0.015f;
    
    [Header("Tracking Settings")]
    [SerializeField, Range(0f, 0.95f)]
    private float positionSmoothing = 0.7f; // Smoothing factor for hand position
    
    [SerializeField, Range(0f, 50f)]
    private float minPointDistance = 5f; // Minimum distance between points
    
    [SerializeField, Range(0f, 2f)]
    private float depthOffset = 0.2f; // Z-distance from camera
    
    [Header("UI Settings")]
    [SerializeField] private Button drawButton;
    [SerializeField] private Text drawButtonText;
    
    // Line drawing state
    private LineRenderer currentLine;
    private List<Vector2> points = new List<Vector2>();
    private bool isDrawing = false;
    private bool drawingEnabled = false; // Drawing mode toggle
    
    // Hand tracking state
    private bool handVisible = false;
    private bool hasPreviousPinch = false;
    private Vector3 pinchPosition;
    private Vector2 smoothedScreenPosition;
    
    // Add this field to track all lines
    private List<GameObject> createdLines = new List<GameObject>();

    private void Start()
    {
        // Find required components if not assigned
        if (handLandmarkerRunner == null)
            handLandmarkerRunner = FindObjectOfType<HandLandmarkerRunner>();
            
        if (annotationController == null)
            annotationController = FindObjectOfType<HandLandmarkerResultAnnotationController>();
            
        if (mainCamera == null)
            mainCamera = Camera.main;
            
        if (linePrefab == null || handLandmarkerRunner == null || annotationController == null || mainCamera == null)
        {
            Debug.LogError("Required components are missing! Please assign them in the inspector.");
            enabled = false;
            return;
        }
        
        // Create extension method script if not already in the project
        EnsureMediaPipeExtensionsExist();
        
        // Set up the draw button
        SetupDrawButton();
        
        // Start with drawing disabled
        drawingEnabled = false;
        UpdateDrawButtonText();
        
        if (debugText != null)
            debugText.text = "MediaPipe Drawing: Ready (Drawing Disabled)";
    }
    
    private void SetupDrawButton()
    {
        if (drawButton == null)
        {
            Debug.LogWarning("Draw button not assigned. Please assign a button in the inspector.");
            return;
        }
        
        // Set up button click event
        drawButton.onClick.AddListener(ToggleDrawingMode);
        
        // Initialize button text
        UpdateDrawButtonText();
    }
    
    // Toggle drawing mode on/off
    public void ToggleDrawingMode()
    {
        drawingEnabled = !drawingEnabled;
        
        if (!drawingEnabled && isDrawing)
        {
            // Stop any active drawing
            StopLine();
            isDrawing = false;
            hasPreviousPinch = false;
            smoothedScreenPosition = Vector2.zero;
        }
        
        // Update UI
        UpdateDrawButtonText();
        
        if (debugText != null)
            debugText.text = drawingEnabled ? "Drawing Mode: ON" : "Drawing Mode: OFF";
        
        Debug.Log("Drawing mode: " + (drawingEnabled ? "ENABLED" : "DISABLED"));
    }
    
    private void UpdateDrawButtonText()
    {
        if (drawButtonText != null)
            // drawButtonText.text = drawingEnabled ? "Drawing: ON" : "Drawing: OFF";
            drawButtonText.text = drawingEnabled ? "View" : "Draw";
    }
    
    private void Update()
    {
        DetectPinchGesture();
    }
    
    private void DetectPinchGesture()
    {
        // Access the current hand landmarks directly from the annotation controller
        var handLandmarks = GetHandLandmarksFromMediaPipe();
        
        // Check if we have valid hand tracking results
        if (handLandmarks == null || handLandmarks.Count == 0 || handLandmarks[0].landmarks.Count < 21)
        {
            if (debugText != null)
                debugText.text = drawingEnabled ? "Drawing ON - No hands detected" : "Drawing OFF - No hands detected";
                
            if (isDrawing)
                StopLine();
                
            handVisible = false;
            return;
        }
        
        handVisible = true;
        if (debugText != null)
            debugText.text = drawingEnabled ? "Drawing ON - Hand detected" : "Drawing OFF - Hand detected";
        
        // Get thumb tip (4) and index finger tip (8)
        var landmarks = handLandmarks[0].landmarks;
        var thumbTip = landmarks[4];
        var indexTip = landmarks[8];
        
        // Convert to Unity vectors
        Vector3 thumbPosition = new Vector3(thumbTip.x, thumbTip.y, thumbTip.z);
        Vector3 indexPosition = new Vector3(indexTip.x, indexTip.y, indexTip.z);
        
        // Calculate distance for pinch detection
        float pinchDistance = Vector3.Distance(thumbPosition, indexPosition);
        
        // Determine pinch state with hysteresis
        bool isPinching = isDrawing ? 
            (pinchDistance < pinchThreshold + pinchHysteresis) : 
            (pinchDistance < pinchThreshold);
            
        // Only draw if drawing is enabled AND pinch is detected
        if (isPinching && drawingEnabled)
        {
            // Calculate midpoint between thumb and index finger for drawing position
            Vector3 midpoint = (thumbPosition + indexPosition) * 0.5f;
            pinchPosition = midpoint;
            
            // Convert to screen space
            Vector2 screenPosition = ConvertToScreenSpace(pinchPosition);
            
            // Apply smoothing for better tracking
            if (smoothedScreenPosition == Vector2.zero)
                smoothedScreenPosition = screenPosition;
            else
                smoothedScreenPosition = Vector2.Lerp(smoothedScreenPosition, screenPosition, 1f - positionSmoothing);
            
            if (!isDrawing)
                StartLine(smoothedScreenPosition);
            else
                UpdateLine(smoothedScreenPosition);
                
            hasPreviousPinch = true;
            
            if (debugText != null)
                debugText.text = $"Drawing: {screenPosition.x:F0},{screenPosition.y:F0} - Distance: {pinchDistance:F3}";
        }
        else if (isDrawing && hasPreviousPinch)
        {
            StopLine();
            hasPreviousPinch = false;
            smoothedScreenPosition = Vector2.zero;
        }
    }
    
    private Vector2 ConvertToScreenSpace(Vector3 handPosition)
    {
        // Convert normalized coordinates to screen space
        // MediaPipe coordinates are normalized (0-1, 0-1)
        Vector2 screenPosition = new Vector2(
            handPosition.x * UnityEngine.Screen.width,
            (1 - handPosition.y) * UnityEngine.Screen.height  // Flip Y axis
        );
        
        return screenPosition;
    }
    
    // Update the StartLine method to track created lines
    private void StartLine(Vector2 screenPosition)
    {
        // Instantiate a new line from the prefab
        GameObject newLine = Instantiate(linePrefab);
        currentLine = newLine.GetComponent<LineRenderer>();
        
        // Add the new line to our tracking list
        createdLines.Add(newLine);
        
        if (currentLine == null)
        {
            Debug.LogError("LineRenderer component missing on the line prefab!");
            return;
        }
        
        // Configure the line renderer with our settings
        currentLine.startWidth = lineWidth;
        currentLine.endWidth = lineWidth;
        
        // Adjust opacity if needed
        if (currentLine.material != null)
        {
            Color lineColor = currentLine.material.color;
            lineColor.a = lineOpacity;
            currentLine.material.color = lineColor;
        }
        
        // Initialize the line
        currentLine.positionCount = 0;
        points.Clear();
        
        // Add the first point
        AddPoint(screenPosition);
        
        isDrawing = true;
        Debug.Log("Started drawing a new line");
    }
    
    private void UpdateLine(Vector2 screenPosition)
    {
        // Add a new point if the distance from the last point is significant
        if (points.Count == 0 || Vector2.Distance(points[points.Count - 1], screenPosition) > minPointDistance)
        {
            AddPoint(screenPosition);
        }
    }
    
    private void StopLine()
    {
        isDrawing = false;
        Debug.Log("Stopped drawing the line");
    }
    
    private void AddPoint(Vector2 screenPosition)
    {
        points.Add(screenPosition);
        currentLine.positionCount = points.Count;
        
        // Update the LineRenderer positions
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
                new Vector3(points[i].x, points[i].y, mainCamera.nearClipPlane + depthOffset)
            );
            currentLine.SetPosition(i, worldPosition);
        }
    }
    
    // This method gets the hand landmarks from MediaPipe using extension methods
    private List<Mediapipe.Tasks.Components.Containers.NormalizedLandmarks> GetHandLandmarksFromMediaPipe()
    {
        if (annotationController == null)
            return null;
            
        // This uses the extension method defined in MediaPipeExtensions.cs
        return annotationController.GetHandLandmarks();
    }
    
    // This ensures the MediaPipeExtensions.cs file exists in the project
    private void EnsureMediaPipeExtensionsExist()
    {
        // This is just informational - you should make sure MediaPipeExtensions.cs exists
        Debug.Log("Make sure MediaPipeExtensions.cs is in your project with the GetHandLandmarks() extension method");
    }

    // Add a new method to clear all drawings
    public void ClearAllDrawings()
    {
        // Stop current drawing if in progress
        if (isDrawing)
        {
            StopLine();
            isDrawing = false;
            hasPreviousPinch = false;
            smoothedScreenPosition = Vector2.zero;
        }
        
        // Destroy all created lines
        foreach (GameObject line in createdLines)
        {
            if (line != null)
                Destroy(line);
        }
        
        // Clear the list
        createdLines.Clear();
        
        Debug.Log("All drawings cleared");
        
        if (debugText != null)
            debugText.text = "Drawings cleared";
    }
}