using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Barracuda;
using System.Collections;
using Unity.Collections;
using System.Linq;
using UnityEngine.UI;

public class ARHandInputHandler_og : MonoBehaviour
{
    [Header("AR Components")]
    public ARCameraManager arCameraManager;

    [Header("Barracuda Model")]
    public NNModel onnxModel;

    [Header("Landmark Visualization")]
    public GameObject landmarkPrefab;
    private GameObject[] landmarkDots;

    [Header("Drawing")]
    public ardrawline drawingController;
    private bool isPinching = false;
    private Vector3 pinchPosition;

    [Header("Debug")]
    public RawImage debugImage;
    public Text handScoreText;

    private Model runtimeModel;
    private IWorker worker;
    private Texture2D cameraTexture;

    private int handVisibleFrameCount = 0;
    private const int handVisibleFrameThreshold = 15; // Increased from 5
    private bool IsProcessingImage = false;

    // Add these variables at class level
    private Vector3[] previousLandmarks = new Vector3[21];
    private bool hasPreviousLandmarks = false;

    private float currentHandScore = 0f;

    private string[] modelOutputNames; 

    void Start()
    {
        // Load model asynchronously to avoid freezing the main thread
        StartCoroutine(InitializeModel());

        if (landmarkPrefab == null)
        {
            Debug.LogError("Landmark prefab is not assigned!");
            return;
        }

        // Instantiate landmark dots
        landmarkDots = new GameObject[21];
        for (int i = 0; i < 21; i++)
        {
            landmarkDots[i] = Instantiate(landmarkPrefab, Vector3.zero, Quaternion.identity);
            landmarkDots[i].transform.SetParent(transform);
            landmarkDots[i].SetActive(false);

            // Scale landmarks to be more visible
            landmarkDots[i].transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            // Make landmark more visible with emission
            Renderer renderer = landmarkDots[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.yellow;
                
                // Add emission for better visibility in AR
                if (renderer.material.HasProperty("_EmissionColor"))
                {
                    renderer.material.EnableKeyword("_EMISSION");
                    renderer.material.SetColor("_EmissionColor", Color.yellow * 2.0f);
                }
            }
        }
    }

    IEnumerator ProcessCameraImage(XRCpuImage image)
    {
        try
        {
            XRCpuImage.ConversionParams conversionParams = new XRCpuImage.ConversionParams
            {
                inputRect = new RectInt(0, 0, image.width, image.height),
                outputDimensions = new Vector2Int(256, 256), // Changed from 224x224
                outputFormat = TextureFormat.RGB24,
                transformation = XRCpuImage.Transformation.MirrorY
            };

            int size = image.GetConvertedDataSize(conversionParams);
            var buffer = new NativeArray<byte>(size, Allocator.Temp);

            try
            {
                image.Convert(conversionParams, buffer);
            }
            finally
            {
                image.Dispose();
            }

            // Destroy previous texture to prevent memory leaks
            if (cameraTexture != null)
                Destroy(cameraTexture);

            cameraTexture = new Texture2D(256, 256, TextureFormat.RGB24, false); // Changed from 224x224
            cameraTexture.LoadRawTextureData(buffer);
            cameraTexture.Apply();
            buffer.Dispose();

            // Execute model inference
            Tensor input = null;
            Tensor handScore = null;
            Tensor landmarks = null;
            Tensor handType = null;

            try
            {
                input = TextureToTensor(cameraTexture);
                worker.Execute(input);
                
                // Use new output names from your model
                handScore = worker.PeekOutput("scores");  // Instead of "Identity_1"
                landmarks = worker.PeekOutput("landmarks"); // Instead of "Identity"
                handType = worker.PeekOutput("lr");       // Instead of "Identity_2"
                
                // Rest of the code stays similar...
                
                // Add null checks to prevent exceptions
                if (handScore == null)
                    handScore = new Tensor(1, 1, 1, 1, new float[] { 0.0f });

                if (handType == null)
                    handType = new Tensor(1, 1, 1, 1, new float[] { 0.0f });

                // Normalize the hand score (it's way too high)
                float rawScore = handScore[0];
                float normalizedScore = 1.0f / (1.0f + Mathf.Exp(-rawScore/25f)); // Sigmoid normalization
                
                Debug.Log($"Raw hand score: {rawScore}, Normalized: {normalizedScore}");
                
                currentHandScore = normalizedScore;
                
                // Use normalized score for detection threshold
                if (normalizedScore > 0.35f)
                {
                    handVisibleFrameCount = handVisibleFrameThreshold;
                }
                else
                {
                    handVisibleFrameCount = Mathf.Max(0, handVisibleFrameCount - 1);
                }
                bool handVisible = handVisibleFrameCount > 0;
                
                Debug.Log($"Hand score: {normalizedScore}, Hand type: {handType[0]}");

                // For testing - always show landmarks to verify model output
                // handVisible = true;

                if (handVisible)
                {
                    Vector3[] landmarkPositions = ExtractLandmarks(landmarks);
                    landmarkPositions = AdjustForHandType(landmarkPositions, handType[0]);
                    CompensateForDeviceOrientation(ref landmarkPositions);
                    ShowLandmarks(landmarkPositions);
                    DetectGesture(landmarkPositions);
                }
                else
                {
                    // Hide landmarks when no hand is detected
                    HideLandmarks();
                }

                // Add this block to update debug UI
                if (debugImage != null)
                {
                    debugImage.texture = cameraTexture;
                }

                if (handScoreText != null)
                {
                    handScoreText.text = $"Hand: {normalizedScore:F2}";
                    handScoreText.color = handVisible ? Color.green : Color.red;
                }
            }
            finally
            {
                // Ensure tensors are always disposed
                if (input != null) input.Dispose();
                if (handScore != null) handScore.Dispose();
                if (landmarks != null) landmarks.Dispose();
                if (handType != null) handType.Dispose();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error processing camera image: " + e.Message);
        }

        yield return null;
    }

    void ShowLandmarks(Vector3[] landmarks)
    {
        if (landmarkDots == null || landmarks == null) return;

        // Add this at the beginning of ShowLandmarks() for testing
        Debug.Log($"Landmark positions: {string.Join(", ", landmarks.Select(l => l.ToString()))}");

        // Get the AR camera
        Camera arCamera = arCameraManager.GetComponent<Camera>();
        if (arCamera == null) return;

        // Position landmarks in world space
        for (int i = 0; i < landmarks.Length && i < landmarkDots.Length; i++)
        {
            if (landmarkDots[i] == null) continue;

            Vector3 normPos = landmarks[i];

            // Convert normalized coordinates to viewport space (0-1 range)
            Vector3 viewportPos = new Vector3(
                normPos.x,
                normPos.y,
                StabilizeDepth(normPos, currentHandScore).z);

            // Project into world space - do this BEFORE using worldPos
            Vector3 worldPos = arCamera.ViewportToWorldPoint(viewportPos);

            // Use depth from model if available
            float depthAdjustment = normPos.z * 0.2f;  // Increased from 0.1f

            // Add extra scale based on distance for better visibility
            float distanceFromCamera = Vector3.Distance(worldPos, arCamera.transform.position);
            landmarkDots[i].transform.localScale = new Vector3(
                0.025f * distanceFromCamera, // Increased from 0.015f
                0.025f * distanceFromCamera,
                0.025f * distanceFromCamera);

            // Add depth adjustment along camera's forward direction
            worldPos += arCamera.transform.forward * depthAdjustment;

            // Apply position with smoothing for stability
            landmarkDots[i].transform.position = Vector3.Lerp(
                landmarkDots[i].transform.position,
                worldPos,
                Time.deltaTime * 8f);  // Reduced from 15f for smoother transitions

            // Make the landmark visible
            landmarkDots[i].SetActive(true);
        }

        // Add this to ShowLandmarks
        Debug.Log($"Camera position: {arCamera.transform.position}, forward: {arCamera.transform.forward}");
        Debug.Log($"First landmark world position: {landmarkDots[0].transform.position}");
    }

    // Fix the incomplete method
    private Vector3 GetWorldPinchPosition()
    {
        Camera arCamera = arCameraManager.GetComponent<Camera>();
        if (arCamera == null) return Vector3.zero;

        // Convert to viewport position
        Vector3 viewportPos = new Vector3(pinchPosition.x, pinchPosition.y, 0.5f);

        // Project to world space at appropriate distance
        return arCamera.ViewportToWorldPoint(new Vector3(viewportPos.x, viewportPos.y, 0.5f));
    }

    // Method 1: InitializeModel
    IEnumerator InitializeModel()
    {
        runtimeModel = ModelLoader.Load(onnxModel);
        
        // Log detailed model structure
        string[] inputNames = runtimeModel.inputs.Select(i => i.name).ToArray();
        string[] outputNames = runtimeModel.outputs.ToArray();
        
        Debug.Log("Model structure loaded:");
        Debug.Log($"Inputs ({inputNames.Length}): {string.Join(", ", inputNames)}");
        Debug.Log($"Outputs ({outputNames.Length}): {string.Join(", ", outputNames)}");
        
        // Show each output shape if possible
        Debug.Log("Detailed output info:");
        foreach (var output in outputNames)
        {
            try {
                var shape = runtimeModel.GetShapeByName(output);
                if (shape.HasValue)
                {
                    // Access dimensions directly instead of using LINQ
                    var tensorShape = shape.Value;
                    Debug.Log($"{output} shape: (n:{tensorShape[0]}, h:{tensorShape[1]}, w:{tensorShape[2]}, c:{tensorShape[3]})");
                }
                else
                {
                    Debug.Log($"{output} shape: unknown");
                }
            }
            catch (System.Exception e) {
                Debug.Log($"{output} shape: error - {e.Message}");
            }
        }
        
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, runtimeModel);
        
        Debug.Log("Hand detection model loaded successfully");
        
        yield return null;
    }

    // Method 2: TextureToTensor
    private Tensor TextureToTensor(Texture2D texture)
    {
        float[] data = new float[3 * 256 * 256]; // Changed from 224x224
        Color32[] pixels = texture.GetPixels32();
        
        for (int i = 0; i < pixels.Length; i++)
        {
            // Normalize to -1 to 1 range
            data[i * 3 + 0] = (pixels[i].r / 127.5f) - 1.0f;
            data[i * 3 + 1] = (pixels[i].g / 127.5f) - 1.0f;
            data[i * 3 + 2] = (pixels[i].b / 127.5f) - 1.0f;
        }
        
        return new Tensor(1, 256, 256, 3, data); // Changed from 224x224
    }

    // Method 3: ExtractLandmarks
    private Vector3[] ExtractLandmarks(Tensor landmarkTensor)
    {
        Vector3[] landmarks = new Vector3[21];
        
        Debug.Log($"Landmark tensor shape: {landmarkTensor.shape}, length: {landmarkTensor.length}");
        
        // New shape is (n:1, h:1, w:3, c:21)
        // This means we have 21 landmarks with 3 coordinates each
        
        if (landmarkTensor.length == 63) // 21 landmarks × 3 coords
        {
            // Extract landmarks from new tensor shape
            for (int i = 0; i < 21; i++)
            {
                // Access coordinates based on the new layout
                float x = landmarkTensor[0, 0, 0, i]; // X coordinate
                float y = landmarkTensor[0, 0, 1, i]; // Y coordinate
                float z = landmarkTensor[0, 0, 2, i]; // Z coordinate
                
                // Store normalized coordinates
                landmarks[i] = new Vector3(x, 1.0f - y, z);
            }
            
            // Apply temporal smoothing as before
            if (hasPreviousLandmarks)
            {
                // More aggressive smoothing for stability
                for (int i = 0; i < 21; i++)
                {
                    landmarks[i] = Vector3.Lerp(previousLandmarks[i], landmarks[i], 0.15f); // Reduced from 0.3 for less jitter
                }
            }
            
            // Store current landmarks for next frame
            previousLandmarks = landmarks.ToArray();
            hasPreviousLandmarks = true;
        }
        else
        {
            Debug.LogError($"Landmark tensor has unexpected shape or length: {landmarkTensor.length}");
            
            // Return default hand pose
            for (int i = 0; i < 21; i++)
            {
                landmarks[i] = new Vector3(0.5f, 0.5f, 0f);
            }
        }

        return landmarks;
    }

    // Add this after ExtractLandmarks but before ShowLandmarks:
    private Vector3[] AdjustForHandType(Vector3[] landmarks, float handType)
    {
        // If handType > 0.5, it's right hand and we need to mirror landmarks
        if (handType > 0.5f)
        {
            for (int i = 0; i < landmarks.Length; i++)
            {
                // Mirror X coordinates (keep landmarks[i].y and landmarks[i].z the same)
                landmarks[i] = new Vector3(1.0f - landmarks[i].x, landmarks[i].y, landmarks[i].z);
            }
            Debug.Log("Right hand detected - mirroring landmarks");
        }
        else
        {
            Debug.Log("Left hand detected - using original landmarks");
        }
        
        return landmarks;
    }

    // Method 4: HideLandmarks
    private void HideLandmarks()
    {
        if (landmarkDots == null) return;
        
        foreach (GameObject dot in landmarkDots)
        {
            if (dot != null)
                dot.SetActive(false);
        }
    }

    // Method 5: DetectGesture
    // In DetectGesture method, make pinch detection more stable
    // Add pinch momentum to avoid flickering from hand tremors
    private float pinchConfidence = 0f;
    private const float pinchThreshold = 0.15f;
    private const float pinchHysteresis = 0.03f; // Prevents flickering

    private void DetectGesture(Vector3[] landmarks)
    {
        Vector3 thumbTip = landmarks[4];
        Vector3 indexTip = landmarks[8];
        
        float pinchDist = Vector3.Distance(thumbTip, indexTip);
        
        // Update pinch confidence with momentum
        if (pinchDist < pinchThreshold)
            pinchConfidence = Mathf.Min(1.0f, pinchConfidence + Time.deltaTime * 5f);
        else if (pinchDist > pinchThreshold + pinchHysteresis)
            pinchConfidence = Mathf.Max(0.0f, pinchConfidence - Time.deltaTime * 5f);
        
        // Use confidence threshold for stable pinch detection
        bool currentPinch = pinchConfidence > 0.5f;
        
        // Only trigger at the start of a pinch
        if (currentPinch && !isPinching)
        {
            if (drawingController != null)
            {
                pinchPosition = (thumbTip + indexTip) * 0.5f;
                drawingController.StartPinchDrawing(GetWorldPinchPosition());
            }
        }
        // Update drawing while pinch continues
        else if (currentPinch && isPinching)
        {
            if (drawingController != null)
            {
                pinchPosition = (thumbTip + indexTip) * 0.5f;
                drawingController.UpdatePinchDrawing(GetWorldPinchPosition());
            }
        }
        // Stop drawing when pinch ends
        else if (!currentPinch && isPinching)
        {
            if (drawingController != null)
            {
                drawingController.StopPinchDrawing();
            }
        }
        
        // Update pinch state
        isPinching = currentPinch;
    }

    // Method 6: Update - Add if missing
    void Update()
    {
        if (IsProcessingImage || arCameraManager == null) return;
        
        if (arCameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            IsProcessingImage = true;
            StartCoroutine(ProcessImageCoroutine(image));
        }
    }

    // Method 7: ProcessImageCoroutine
    IEnumerator ProcessImageCoroutine(XRCpuImage image)
    {
        yield return ProcessCameraImage(image);
        IsProcessingImage = false;
    }

    // Add an additional position stability method
    private Vector3 StabilizeDepth(Vector3 position, float confidence)
    {
        // Use an exponential moving average for depth
        float stabilityFactor = Mathf.Clamp01(confidence * 0.5f);
        float targetDepth = 0.3f; // Keep hand at consistent distance when low confidence
        
        // Blend between raw depth and stable target depth based on confidence
        float depth = Mathf.Lerp(targetDepth, position.z, stabilityFactor);
        
        return new Vector3(position.x, position.y, depth);
    }

    // Add device orientation compensation
    private void CompensateForDeviceOrientation(ref Vector3[] landmarks)
    {
        // Get current device orientation
        DeviceOrientation orientation = Input.deviceOrientation;
        
        // Adjust landmark coordinates based on orientation
        switch (orientation)
        {
            case DeviceOrientation.LandscapeLeft:
                for (int i = 0; i < landmarks.Length; i++)
                    landmarks[i] = new Vector3(1f - landmarks[i].y, landmarks[i].x, landmarks[i].z);
                break;
            case DeviceOrientation.LandscapeRight:
                for (int i = 0; i < landmarks.Length; i++)
                    landmarks[i] = new Vector3(landmarks[i].y, 1f - landmarks[i].x, landmarks[i].z);
                break;
            case DeviceOrientation.PortraitUpsideDown:
                for (int i = 0; i < landmarks.Length; i++)
                    landmarks[i] = new Vector3(1f - landmarks[i].x, 1f - landmarks[i].y, landmarks[i].z);
                break;
            default:
                // Portrait is default, no changes needed
                break;
        }
    }

    private Vector3[] ReshapeAndExtractLandmarks(Tensor tensor)
    {
        Vector3[] landmarks = new Vector3[21];
        
        // Try different reshape approaches based on tensor length
        if (tensor.length == 21*3) // Standard format: 21 landmarks × 3 coordinates
        {
            for (int i = 0; i < 21; i++)
            {
                landmarks[i] = new Vector3(
                    tensor[i*3],
                    tensor[i*3 + 1],
                    tensor[i*3 + 2]
                );
            }
        }
        else if (tensor.length == 21*2) // Some models only output x,y
        {
            for (int i = 0; i < 21; i++)
            {
                landmarks[i] = new Vector3(
                    tensor[i*2],
                    tensor[i*2 + 1],
                    0f
                );
            }
        }
        else if (tensor.length >= 21 && tensor.length < 63) // Try single array format
        {
            for (int i = 0; i < 21 && i < tensor.length; i++)
            {
                landmarks[i] = new Vector3(
                    tensor[i] / 224f, // Normalize to 0-1 range assuming 224×224 input
                    0.5f, // Default Y position 
                    0f
                );
            }
        }
        
        return landmarks;
    }
}
