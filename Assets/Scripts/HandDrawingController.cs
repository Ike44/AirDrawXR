using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using Mediapipe.Tasks.Vision.HandLandmarker;

public class HandDrawingController : MonoBehaviour
{
    [Header("MediaPipe Components")]
    [SerializeField, Tooltip("Specific HandLandmarkerRunner component in your scene")]
    public HandLandmarkerRunner handLandmarkerRunner;
    
    [SerializeField, Tooltip("The HandLandmarkerResultAnnotationController that visualizes hand landmarks")]
    public HandLandmarkerResultAnnotationController annotationController;
    
    [Header("Drawing Components")]
    [SerializeField, Tooltip("The ardrawline component that handles line creation")]
    public ardrawline drawingController;
    
    [SerializeField, Tooltip("The AR Camera (typically Main Camera)")]
    public Camera arCamera;
    
    [SerializeField, Tooltip("Optional: Text element for debugging")]
    public Text debugText;
    
    [Header("Pinch Gesture Settings")]
    [SerializeField, Range(0.01f, 0.2f), Tooltip("Distance threshold for pinch detection")]
    public float pinchThreshold = 0.05f;
    
    [SerializeField, Range(0.0f, 0.1f), Tooltip("Extra distance before unpinching")]
    public float pinchHysteresis = 0.015f;
    
    [SerializeField, Range(0.1f, 0.99f), Tooltip("Higher values create smoother but more delayed lines")]
    public float smoothingFactor = 0.8f;
    
    // Drawing state
    public bool isDrawing = false;
    public Vector3 drawPosition;
    private Vector3 smoothedPosition;
    private bool hasPreviousPosition = false;
    
    void Start()
    {
        // Verify required references are assigned
        if (handLandmarkerRunner == null)
        {
            Debug.LogError("HandLandmarkerRunner reference is missing - assign it in the Inspector");
            enabled = false;
            return;
        }
        
        if (annotationController == null)
        {
            Debug.LogError("HandLandmarkerResultAnnotationController reference is missing - assign it in the Inspector");
            enabled = false;
            return;
        }
        
        if (drawingController == null)
        {
            Debug.LogError("Drawing controller reference is missing - assign it in the Inspector");
            enabled = false;
            return;
        }
        
        if (arCamera == null)
        {
            Debug.LogError("AR Camera reference is missing - assign it in the Inspector");
            enabled = false;
            return;
        }
        
        if (debugText != null)
        {
            debugText.text = "Hand Drawing Ready";
        }
        
        // Log initialization
        Debug.Log("Hand Drawing Controller initialized and ready for tracking");
    }
    
    void Update()
    {
        DetectPinchGesture();
    }
    
    void DetectPinchGesture()
    {
        // Access the current hand landmarks from the annotation controller
        var handLandmarks = annotationController.GetHandLandmarks(); // Using our extension method
        
        // Check if we have valid hand tracking results
        if (handLandmarks == null || handLandmarks.Count == 0)
        {
            if (debugText != null)
            {
                debugText.text = "No hands detected";
            }
            
            if (isDrawing)
            {
                StopDrawing();
            }
            Debug.Log("No hands detected.");
            return;
        }
        
        Debug.Log($"Hand detected! Landmarks count: {handLandmarks[0].landmarks.Count}");
        
        if (handLandmarks[0].landmarks.Count < 21)
        {
            Debug.LogWarning($"Incomplete hand landmarks: only {handLandmarks[0].landmarks.Count}/21 detected");
            if (isDrawing)
            {
                StopDrawing();
            }
            return;
        }
        
        // Get the first detected hand landmarks
        var landmarks = handLandmarks[0].landmarks;
        
        // Get thumb tip (4) and index finger tip (8)
        var thumbTip = landmarks[4];
        var indexTip = landmarks[8];
        
        // Convert to Unity vectors
        Vector3 thumbPosition = new Vector3(thumbTip.x, thumbTip.y, thumbTip.z);
        Vector3 indexPosition = new Vector3(indexTip.x, indexTip.y, indexTip.z);
        
        // Calculate distance for pinch detection
        float pinchDistance = Vector3.Distance(thumbPosition, indexPosition);
        Debug.Log($"Pinch distance: {pinchDistance}");
        
        // Determine pinch state with hysteresis
        bool isPinching = isDrawing ? 
            (pinchDistance < pinchThreshold + pinchHysteresis) : 
            (pinchDistance < pinchThreshold);
        
        Debug.Log($"Is pinching: {isPinching}");
        
        if (isPinching)
        {
            Debug.Log("Pinch gesture detected.");
            
            // Calculate midpoint between thumb and index finger
            Vector3 midpoint = (thumbPosition + indexPosition) * 0.5f;
            
            // Transform from MediaPipe coordinate space to world space
            Vector3 worldPosition = TransformHandPositionToWorld(midpoint);
            
            // Apply smoothing
            if (!hasPreviousPosition)
            {
                smoothedPosition = worldPosition;
                hasPreviousPosition = true;
            }
            else
            {
                smoothedPosition = Vector3.Lerp(smoothedPosition, worldPosition, 1f - smoothingFactor);
            }
            
            drawPosition = smoothedPosition;
            
            if (!isDrawing)
            {
                StartDrawing();
            }
            else
            {
                UpdateDrawing();
            }
        }
        else if (isDrawing)
        {
            Debug.Log("Pinch released - stopping drawing.");
            StopDrawing();
        }
    }
    
    Vector3 TransformHandPositionToWorld(Vector3 handPosition)
    {
        // Log raw hand position for debugging
        Debug.Log($"Raw hand position: {handPosition}");
        
        // Scale the coordinates to a reasonable size in meters
        // You may need to adjust this scale based on your needs
        float scale = 0.5f;
        
        // Convert to Unity's coordinate system and scale
        Vector3 unityPosition = new Vector3(
            handPosition.x * scale,     // Left-right stays the same
            handPosition.y * scale,     // Up-down stays the same
            -handPosition.z * scale     // Convert from away-from-camera to towards-camera
        );
        
        // Transform to world space relative to camera
        Vector3 worldPos = arCamera.transform.position + 
                          arCamera.transform.right * unityPosition.x +
                          arCamera.transform.up * unityPosition.y +
                          arCamera.transform.forward * unityPosition.z;
        
        // Log the calculated world position
        Debug.Log($"Transformed position: {worldPos}");
        
        return worldPos;
    }
    
    void StartDrawing()
    {
        isDrawing = true;
        
        // Create a new line using the drawing controller
        drawingController.spawnPos = drawPosition;
        drawingController.createLineCenter();
        
        // Debug line creation
        Debug.Log($"Started drawing at position: {drawPosition}");
        
        // Check if the line was actually created
        if (drawingController.currentLine != null)
        {
            Debug.Log($"Line created: {drawingController.currentLine.name} at {drawingController.currentLine.transform.position}");
            
            // Check line renderer settings
            LineRenderer lr = drawingController.currentLine.GetComponent<LineRenderer>();
            if (lr != null)
            {
                Debug.Log($"Line renderer: Width={lr.startWidth}, Positions={lr.positionCount}, Visible={lr.enabled}");
                
                // Make sure the line is visible
                lr.enabled = true;
                
                // Check if the material is valid and if it's set to be visible in AR
                if (lr.material != null)
                {
                    Debug.Log($"Line material: {lr.material.name}, Shader: {lr.material.shader.name}");
                    
                    // Make sure the material uses an appropriate shader for AR
                    if (lr.material.HasProperty("_ZWrite"))
                    {
                        lr.material.SetInt("_ZWrite", 1);
                    }
                    
                    if (lr.material.HasProperty("_SrcBlend"))
                    {
                        lr.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    }
                    
                    // Add emission for better visibility
                    if (lr.material.HasProperty("_EmissionColor"))
                    {
                        lr.material.EnableKeyword("_EMISSION");
                        lr.material.SetColor("_EmissionColor", Color.white);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Failed to create line object");
        }
        
        if (debugText != null)
        {
            debugText.text = "Drawing Started at " + drawPosition.ToString("F2");
        }
    }
    
    void UpdateDrawing()
    {
        if (isDrawing)
        {
            // Update the line with new positions
            drawingController.spawnPos = drawPosition;
            drawingController.updateLineCenter();
            
            // Debug the update (less frequent to avoid log spam)
            if (Time.frameCount % 20 == 0)
            {
                Debug.Log($"Drawing updated: {drawPosition}");
            }
            
            if (debugText != null)
            {
                debugText.text = "Drawing at " + drawPosition.ToString("F2");
            }
        }
    }
    
    void StopDrawing()
    {
        isDrawing = false;
        hasPreviousPosition = false;
        
        Debug.Log("Drawing stopped");
        
        if (debugText != null)
        {
            debugText.text = "Drawing Stopped";
        }
    }
}