using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Animator doorAnimator;
    public CameraLook cameraLook;

    public float interactionDistance = 3.0f; // The maximum distance from which the door can be interacted with
    private bool isOpen = false;

    private void Start()
    {
        // Find and assign the CameraLook component
        cameraLook = FindObjectOfType<CameraLook>();
        doorAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check for interaction input (e.g., mouse click or "E" key press)
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithDoor();
        }
    }

    void InteractWithDoor()
    {
        // Cast a ray from the cameraLook's position forward
        Ray ray = new Ray(cameraLook.transform.position, cameraLook.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Make text in interaction handler say "open door" and HUD turn red
            // Add your interaction handler code here

            // Check if the ray hit this door object
            if (hit.collider.gameObject == gameObject)
            {
                ToggleDoor();
            }
        }
    }

    void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        doorAnimator.Play("Open");
        isOpen = true;
    }

    void CloseDoor()
    {
        doorAnimator.Play("Close");
        isOpen = false;
    }
}
