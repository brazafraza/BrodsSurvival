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
    public QuestUI questUi;

    public Tutorial tutorial;

    public GameObject hudCrosshair;
    void Update()
    {
        
            Interact();
        
    }

    private void Interact()
    {
        RaycastHit hit;

        if (questUi.playerLookingAtTut && !tutorial.tutorialActive)
        {
            Debug.Log("text should appear");
            interactionText.text = "Press 'E' to read stone tablet";
            hudCrosshair.SetActive(false);
            interactionText.gameObject.SetActive(true);

        }


        else if (questUi.playerLookingAtNPC && !questUi.menuOpen)
        {
            Debug.Log("text should appear");
            interactionText.text = $"Press 'E' to speak to the island";
            hudCrosshair.SetActive(false);
            interactionText.gameObject.SetActive(true);

        }
        else if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, interactableLayers))
        {
            hudCrosshair.SetActive(false);
            Pickup pickup = hit.transform.GetComponent<Pickup>();
            Storage storage = hit.transform.GetComponent<Storage>();
            Water water = hit.transform.GetComponent<Water>();

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

                if (water != null)
                {
                    water.Drink(GetComponentInParent<PlayerStats>());
                }
            }

            if (pickup != null || storage != null || water != null)
            {
                interactionText.gameObject.SetActive(true);

                

                if (pickup != null)
                {
                    interactionText.text = $"Press 'E' to pickup x{pickup.stackSize} {pickup.data.itemName}";
                }

                if (storage != null)
                {
                    interactionText.text = $"Open";
                }

                if (water != null)
                {
                    interactionText.text = "Drink";
                }
            }
            else
            {
               // Debug.Log("1st inactive");
                interactionText.gameObject.SetActive(false);
                hudCrosshair.SetActive(true);
            }


        }

        else
        {
           // Debug.Log("2nd inactive");
            interactionText.gameObject.SetActive(false);
            hudCrosshair.SetActive(true);
        }
    }
}
