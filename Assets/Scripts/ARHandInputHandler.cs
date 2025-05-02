using UnityEngine;

public class ARHandInputHandler : MonoBehaviour
{
    [Header("Drawing")]
    public ardrawline drawingController;

    private bool isPinching = false;
    private Vector3 pinchPosition;

    public void DetectGesture(Vector3[] landmarks)
    {
        if (landmarks == null || landmarks.Length < 9) return;

        Vector3 thumbTip = landmarks[4];
        Vector3 indexTip = landmarks[8];

        float pinchDist = Vector3.Distance(thumbTip, indexTip);

        if (pinchDist < 0.02f) // Adjust threshold as needed
        {
            if (!isPinching)
            {
                isPinching = true;
                pinchPosition = (thumbTip + indexTip) * 0.5f;
                drawingController.StartPinchDrawing(pinchPosition);
            }
            else
            {
                pinchPosition = (thumbTip + indexTip) * 0.5f;
                drawingController.UpdatePinchDrawing(pinchPosition);
            }
        }
        else if (isPinching)
        {
            isPinching = false;
            drawingController.StopPinchDrawing();
        }
    }
}
