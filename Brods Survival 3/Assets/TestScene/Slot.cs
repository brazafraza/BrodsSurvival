
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Slot : MonoBehaviour
{
    public ItemSO data;
    public int stackSize;
    [Space]
    public Image icon;
    public TextMeshProUGUI stackText;


    private bool isEmpty;

    public bool IsEmpty => isEmpty;

    private void Start()
    {
        UpdateSlot();
    }

    private void UpdateSlot()
    {
        if (data == null)
        {
            isEmpty = true;

            icon.gameObject.SetActive(false);
            stackText.gameObject.SetActive(false);
        }
        else
        {
            isEmpty = false;

            icon.gameObject.SetActive(true);
            stackText.gameObject.SetActive(true);

        }
    }

    public void AddItemToSlot(ItemSO data_, int stackSize_)
    {
        data = data_;
        stackSize = stackSize_;
    }

}
