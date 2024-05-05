using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public LayerMask interactableLayers;
    public float interactionRange = 2f;
    public KeyCode interactionKey = KeyCode.E;
    void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            Interact();
        }
    }

    private void Interact()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, interactableLayers))
        {
            Pickup pickup = hit.transform.GetComponent<Pickup>();
            Storage storage = hit.transform.GetComponent<Storage>();

            if (pickup != null)
            {
                GetComponentInParent<WindowHandler>().inventory.AddItem(pickup);
            }

            if (storage != null)
            {
                if (!storage.opened)
                {
                    GetComponentInParent<WindowHandler>().inventory.opened = true;

                    storage.Open(GetComponentInParent<WindowHandler>().storage);
                }
            }
        }
    }
}
