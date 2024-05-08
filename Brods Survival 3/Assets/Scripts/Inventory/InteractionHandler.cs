using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionHandler : MonoBehaviour
{
    public LayerMask interactableLayers;
    public float interactionRange = 2f;
    public KeyCode interactionKey = KeyCode.E;
    public TextMeshProUGUI interactionText;
    void Update()
    {
        
            Interact();
        
    }

    private void Interact()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, interactableLayers))
        {
            Pickup pickup = hit.transform.GetComponent<Pickup>();
            Storage storage = hit.transform.GetComponent<Storage>();

            if (Input.GetKeyDown(interactionKey))
            {

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

            if (pickup != null || storage != null)
            {
                interactionText.gameObject.SetActive(true);

                if (pickup != null)
                {
                    interactionText.text = $"Pickup x{pickup.stackSize} {pickup.data.itemName}";
                }

                if (storage != null)
                {
                    interactionText.text = $"Open";
                }
            }
            else
            {
                interactionText.gameObject.SetActive(false);
            }

       
        }
        else
        {
            interactionText.gameObject.SetActive(false);
        }
    }
}
