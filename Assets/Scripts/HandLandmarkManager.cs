// using System.Collections.Generic;
// using Mediapipe.Tasks.Vision.HandLandmarker;
// using UnityEngine;
// using Mediapipe.Unity.Sample.HandLandmarkDetection;

// public class HandLandmarkManager : MonoBehaviour
// {
//     [SerializeField] private HandLandmarkerRunner handLandmarkerRunner;
//     [SerializeField] private GameObject landmarkPrefab;
//     [SerializeField] private Camera arCamera;

//     private GameObject[] landmarkDots;

//     private void Start()
//     {
//         if (handLandmarkerRunner == null)
//         {
//             Debug.LogError("HandLandmarkerRunner is not assigned!");
//             return;
//         }

//         if (landmarkPrefab == null)
//         {
//             Debug.LogError("Landmark prefab is not assigned!");
//             return;
//         }

//         // Initialize landmark dots
//         landmarkDots = new GameObject[21];
//         for (int i = 0; i < 21; i++)
//         {
//             landmarkDots[i] = Instantiate(landmarkPrefab, Vector3.zero, Quaternion.identity);
//             landmarkDots[i].SetActive(false);
//         }

//         // Subscribe to the landmark output event
//         handLandmarkerRunner.OnHandLandmarkDetectionOutput += OnHandLandmarkDetectionOutput;
//     }

//     private void OnHandLandmarkDetectionOutput(HandLandmarkerResult result, Image image, long timestamp)
//     {
//         if (result == null || result.Landmarks == null || result.Landmarks.Count == 0)
//         {
//             HideLandmarks();
//             return;
//         }

//         // Use the first detected hand's landmarks
//         var landmarks = result.Landmarks[0];
//         ShowLandmarks(landmarks);
//     }

//     private void ShowLandmarks(IList<NormalizedLandmark> landmarks)
//     {
//         if (landmarkDots == null || arCamera == null) return;

//         for (int i = 0; i < landmarks.Count && i < landmarkDots.Length; i++)
//         {
//             var normPos = landmarks[i];
//             Vector3 viewportPos = new Vector3(normPos.X, 1 - normPos.Y, normPos.Z); // Flip Y-axis
//             Vector3 worldPos = arCamera.ViewportToWorldPoint(new Vector3(viewportPos.x, viewportPos.y, 0.5f));

//             landmarkDots[i].transform.position = worldPos;
//             landmarkDots[i].SetActive(true);
//         }
//     }

//     private void HideLandmarks()
//     {
//         if (landmarkDots == null) return;

//         foreach (var dot in landmarkDots)
//         {
//             if (dot != null)
//                 dot.SetActive(false);
//         }
//     }
// }