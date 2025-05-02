using UnityEngine;

public class DrawingModeSwitcher : MonoBehaviour
{
    public GameObject arDrawLineManager; // GameObject managing 3D drawing
    public GameObject screenDrawingManager; // GameObject managing 2D screen drawing

    void Start()
    {
        // Ensure only one mode is active at the start
        Set3DDrawingMode();
    }

    public void Set3DDrawingMode()
    {
        arDrawLineManager.SetActive(true);
        screenDrawingManager.SetActive(false);
        Debug.Log("Switched to 3D Drawing Mode.");
    }

    public void Set2DDrawingMode()
    {
        arDrawLineManager.SetActive(false);
        screenDrawingManager.SetActive(true);
        Debug.Log("Switched to 2D Screen Drawing Mode.");
    }
}