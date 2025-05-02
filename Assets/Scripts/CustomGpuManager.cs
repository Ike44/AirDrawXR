using UnityEngine;
using Mediapipe.Unity;

public class CustomGpuManager : MonoBehaviour
{
    private void Awake()
    {
        if (GpuManager.GpuResources == null)
        {
            GpuManager.Initialize();
            Debug.Log("GPU Resources initialized.");
        }
    }
}