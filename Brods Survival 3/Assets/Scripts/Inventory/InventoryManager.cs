using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Weapon[] weapons;
    [HideInInspector] public BuildingHandler building;

    [Header("debug")]
    public GameObject buildingGameObj;

    public bool opened;
    public KeyCode inventoryKey = KeyCode.Tab;

    [Header("Settings")]
    public int inventorySize = 24;
    public int hotbarSize = 6;

    [Header("Refs")]
    public GameObject dropModel;
    public Transform dropPos;
    public GameObject slotTemplate;
    public Transform contentHolder;
    public Transform hotbarContentHolder;
    public GameObject noSlotSelectedIndicator; // GameObject to activate when no slot is selected

    [HideInInspector] public Slot[] inventorySlots;
    private Slot[] hotbarSlots;
    private Slot selectedSlot; // Variable to keep track of the selected slot

    private void Start()
    {
        building = GetComponentInParent<WindowHandler>().building;

        GenerateSlots();
        GenerateHotbarSlots();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectHotbarSlot(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectHotbarSlot(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectHotbarSlot(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectHotbarSlot(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectHotbarSlot(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectHotbarSlot(5);
        }

        if (Input.GetKeyDown(inventoryKey))
        {
            opened = !opened;
        }

        if (GetComponentInParent<WindowHandler>().storage.opened)
        {
            GetComponentInParent<WindowHandler>().crafting.opened = false;
        }
        else
        {
            GetComponentInParent<WindowHandler>().crafting.opened = true;
        }

        if (opened)
        {
            transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            transform.localPosition = new Vector3(-10000, 0, 0);

            if (GetComponentInParent<WindowHandler>().storage.opened)
            {
                GetComponentInParent<WindowHandler>().storage.Close();
            }
        }

        UpdateSlotSelectionIndicator();
    }

    private void SelectHotbarSlot(int index)
    {
        if (index >= 0 && index < hotbarSlots.Length)
        {
            hotbarSlots[index].Try_Use();
            selectedSlot = hotbarSlots[index];
        }
    }

    private void UpdateSlotSelectionIndicator()
    {
        if (noSlotSelectedIndicator != null)
        {
            noSlotSelectedIndicator.SetActive(selectedSlot == null);
        }
    }

    private void GenerateSlots()
    {
        List<Slot> inventorySlots_ = new List<Slot>();

        // Generate slots
        for (int i = 0; i < inventorySize; i++)
        {
            Slot slot = Instantiate(slotTemplate.gameObject, contentHolder).GetComponent<Slot>();
            inventorySlots_.Add(slot);
        }

        inventorySlots = inventorySlots_.ToArray();
    }

    private void GenerateHotbarSlots()
    {
        List<Slot> inventorySlots_ = new List<Slot>();
        List<Slot> hotbarList = new List<Slot>();

        // Update hotbar slots
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots_.Add(inventorySlots[i]);
        }

        // Generate hotbar slots
        for (int i = 0; i < hotbarSize; i++)
        {
            Slot slot = Instantiate(slotTemplate.gameObject, hotbarContentHolder).GetComponent<Slot>();
            inventorySlots_.Add(slot);
            hotbarList.Add(slot);
        }

        inventorySlots = inventorySlots_.ToArray();
        hotbarSlots = hotbarList.ToArray();
    }

    public void DragDrop(Slot from, Slot to)
    {
        // Unequip from slot
        if (from.weaponEquipped != null)
        {
            from.weaponEquipped.UnEquip();
        }
        if (to.weaponEquipped != null)
        {
            to.weaponEquipped.UnEquip();
        }

        // Stop building
        if (from == building.slotInUse)
        {
            building.slotInUse = null;
        }

        if (to == building.slotInUse)
        {
            building.slotInUse = null;
        }

        // Swapping
        if (from.data != to.data)
        {
            ItemSO data = to.data;
            int stackSize = to.stackSize;

            to.data = from.data;
            to.stackSize = from.stackSize;

            from.data = data;
            from.stackSize = stackSize;
        }
        // Stacking
        else
        {
            if (from.data.isStackable)
            {
                if (from.stackSize + to.stackSize > from.data.maxStack)
                {
                    int amountLeft = (from.stackSize + to.stackSize) - from.data.maxStack;
                    from.stackSize = amountLeft;
                    to.stackSize = from.data.maxStack;
                }
                else
                {
                    to.stackSize += from.stackSize;
                    from.Clean();
                }
                from.UpdateSlot();
                to.UpdateSlot();
            }
            else
            {
                ItemSO data = to.data;
                int stackSize = to.stackSize;

                to.data = from.data;
                to.stackSize = from.stackSize;

                from.data = data;
                from.stackSize = stackSize;
            }
        }

        from.UpdateSlot();
        to.UpdateSlot();
        selectedSlot = null; // Reset the selected slot after drag and drop
    }

    public void AddItem(Pickup pickUp)
    {
        if (pickUp.data.isStackable)
        {
            Slot stackableSlot = null;

            // Try finding stackable slot
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (!inventorySlots[i].IsEmpty && inventorySlots[i].data == pickUp.data && inventorySlots[i].stackSize < pickUp.data.maxStack)
                {
                    stackableSlot = inventorySlots[i];
                    break;
                }
            }

            if (stackableSlot != null)
            {
                // If it cannot fit the picked up amount
                if (stackableSlot.stackSize + pickUp.stackSize > pickUp.data.maxStack)
                {
                    int amountLeft = (stackableSlot.stackSize + pickUp.stackSize) - pickUp.data.maxStack;

                    // Add it to the stackable slot
                    stackableSlot.AddItemToSlot(pickUp.data, pickUp.data.maxStack);

                    // Try find a new empty stack
                    for (int i = 0; i < inventorySlots.Length; i++)
                    {
                        if (inventorySlots[i].IsEmpty)
                        {
                            inventorySlots[i].AddItemToSlot(pickUp.data, amountLeft);
                            inventorySlots[i].UpdateSlot();
                            break;
                        }
                    }

                    Destroy(pickUp.gameObject);
                }
                // If it can fit the picked up amount
                else
                {
                    stackableSlot.AddStackAmount(pickUp.stackSize);
                    Destroy(pickUp.gameObject);
                }

                stackableSlot.UpdateSlot();
            }
            else
            {
                Slot emptySlot = null;

                // Find empty slot
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    if (inventorySlots[i].IsEmpty)
                    {
                        emptySlot = inventorySlots[i];
                        break;
                    }
                }

                // If we have an empty slot, then add the item
                if (emptySlot != null)
                {
                    emptySlot.AddItemToSlot(pickUp.data, pickUp.stackSize);
                    emptySlot.UpdateSlot();
                    Destroy(pickUp.gameObject);
                }
                else
                {
                    pickUp.transform.position = dropPos.position;
                }
            }
        }
        else
        {
            Slot emptySlot = null;

            // Find empty slot
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].IsEmpty)
                {
                    emptySlot = inventorySlots[i];
                    break;
                }
            }

            // If we have an empty slot, then add the item
            if (emptySlot != null)
            {
                emptySlot.AddItemToSlot(pickUp.data, pickUp.stackSize);
                emptySlot.UpdateSlot();
                Destroy(pickUp.gameObject);
            }
            else
            {
                pickUp.transform.position = dropPos.position;
            }
        }
    }

    public void AddItem(ItemSO data, int stackSize)
    {
        if (data.isStackable)
        {
            Slot stackableSlot = null;

            // Try finding stackable slot
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (!inventorySlots[i].IsEmpty && inventorySlots[i].data == data && inventorySlots[i].stackSize < data.maxStack)
                {
                    stackableSlot = inventorySlots[i];
                    break;
                }
            }

            if (stackableSlot != null)
            {
                // If it cannot fit the picked up amount
                if (stackableSlot.stackSize + stackSize > data.maxStack)
                {
                    int amountLeft = (stackableSlot.stackSize + stackSize) - data.maxStack;

                    // Add it to the stackable slot
                    stackableSlot.AddItemToSlot(data, data.maxStack);

                    // Try find a new empty stack
                    for (int i = 0; i < inventorySlots.Length; i++)
                    {
                        if (inventorySlots[i].IsEmpty)
                        {
                            inventorySlots[i].AddItemToSlot(data, amountLeft);
                            inventorySlots[i].UpdateSlot();
                            break;
                        }
                    }
                }
                // If it can fit the picked up amount
                else
                {
                    stackableSlot.AddStackAmount(stackSize);
                }

                stackableSlot.UpdateSlot();
            }
            else
            {
                Slot emptySlot = null;

                // Find empty slot
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    if (inventorySlots[i].IsEmpty)
                    {
                        emptySlot = inventorySlots[i];
                        break;
                    }
                }

                // If we have an empty slot, then add the item
                if (emptySlot != null)
                {
                    emptySlot.AddItemToSlot(data, stackSize);
                    emptySlot.UpdateSlot();
                }
                else
                {
                    DropItem(data, stackSize);
                }
            }
        }
        else
        {
            Slot emptySlot = null;

            // Find empty slot
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].IsEmpty)
                {
                    emptySlot = inventorySlots[i];
                    break;
                }
            }

            // If we have an empty slot, then add the item
            if (emptySlot != null)
            {
                emptySlot.AddItemToSlot(data, stackSize);
                emptySlot.UpdateSlot();
            }
            else
            {
                DropItem(data, stackSize);
            }
        }
    }

    public void DropItem(Slot slot)
    {
        Pickup pickup = Instantiate(dropModel, dropPos).GetComponent<Pickup>();
        pickup.transform.position = dropPos.position;
        pickup.transform.SetParent(null);

        pickup.data = slot.data;
        pickup.stackSize = slot.stackSize;

        slot.Clean();
    }

    public void DropItem(ItemSO data, int stackSize)
    {
        Pickup pickup = Instantiate(dropModel, dropPos).GetComponent<Pickup>();
        pickup.transform.position = dropPos.position;
        pickup.transform.SetParent(null);

        pickup.data = data;
        pickup.stackSize = stackSize;
    }
}

