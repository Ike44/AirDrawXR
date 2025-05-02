using System.Collections.Generic;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Unity;
using UnityEngine;

public static class MediaPipeExtensions
{
    // Extension method to access hand landmarks
    public static List<NormalizedLandmarks> GetHandLandmarks(this HandLandmarkerResultAnnotationController controller)
    {
        // Use reflection to safely access the private field
        var targetField = typeof(HandLandmarkerResultAnnotationController)
            .GetField("_currentTarget", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (targetField == null)
            return null;
            
        var currentTarget = targetField.GetValue(controller);
        if (currentTarget == null)
            return null;
            
        // Direct cast since HandLandmarkerResult is a struct
        HandLandmarkerResult result = (HandLandmarkerResult)currentTarget;
        return result.handLandmarks;
    }
}