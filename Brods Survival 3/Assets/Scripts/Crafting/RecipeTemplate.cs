using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class RecipeTemplate : MonoBehaviour, IPointerDownHandler
{
    private CraftingManager crafting;
    [HideInInspector] public CraftingRecipeSO recipe;

    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI requirementText;
    public TextMeshProUGUI timerText;

    private void Start()
    {
        crafting = GetComponentInParent<CraftingManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            crafting.Try_Craft(this);
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            crafting.Cancel(this);
        }
    }
}
