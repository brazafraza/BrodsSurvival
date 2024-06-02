using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Animator doorAnimator;
    public CameraLook cameraLook;

    public float interactionDistance = 5f; // The maximum distance from which the door can be interacted with
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
            

            // Check if the ray hit this door object
          //  Debug.Log("Looking at" + hit.collider.gameObject.name);
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
        //Debug.Log("Door open play");
        doorAnimator.Play("Open");
        isOpen = true;
    }

    void CloseDoor()
    {
       // Debug.Log("Door close play");
        doorAnimator.Play("Close");
        isOpen = false;
    }
}
