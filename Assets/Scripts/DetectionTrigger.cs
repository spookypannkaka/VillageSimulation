using UnityEngine;

/* 
    Attach this to an object with a collider if you need to know exactly which collider another object enters.
    Add collider types as needed.
*/
public class DetectionTrigger : MonoBehaviour
{
    public enum DetectionType
    {
        FOV,
        DetectionRadius
    }

    [Tooltip("The type of detection area this represents (e.g., FOV, DetectionRadius).")]
    public DetectionType detectionType;

    [Tooltip("Reference to the controller that will handle the detection.")]
    public VillagerController controller;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (detectionType)
            {
                case DetectionType.FOV:
                    controller.SetPlayerInFOV(true);
                    break;
                case DetectionType.DetectionRadius:
                    controller.SetPlayerInRadius(true);
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (detectionType)
            {
                case DetectionType.FOV:
                    controller.SetPlayerInFOV(false);
                    break;
                case DetectionType.DetectionRadius:
                    controller.SetPlayerInRadius(false);
                    break;
            }
        }
    }
}
