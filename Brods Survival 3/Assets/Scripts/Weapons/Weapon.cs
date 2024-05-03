using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Player player;
    private AudioSource audioS;

    [HideInInspector] public Animator anim;
    [HideInInspector] public Slot slotEquippedOn;

    public GameObject bulletHolePrefab;

    public ItemSO weaponData;
    public bool isAutomatic;
    [Space]
    public Transform shootPoint;
    public LayerMask shootableLayers;

    [Header("Aiming")]
    public float aimSpeed = 10f;
    public Vector3 hipPos;
    public Vector3 aimPos;
    public bool isAiming;

    [HideInInspector] public bool isReloading;
    [HideInInspector] public bool hasTakenOut;

    private float currentFireRate;
    private float fireRate;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        anim = GetComponentInChildren<Animator>();
        audioS = GetComponent<AudioSource>();

        transform.localPosition = hipPos;

        fireRate = weaponData.fireRate;

        currentFireRate = weaponData.fireRate;
    }

    private void Update()
    {

        UpdateAnimations();
        

        if (weaponData.itemType == ItemSO.ItemType.Weapon)
        {
            if (currentFireRate < fireRate)
            {
                currentFireRate += Time.deltaTime;
            }

            if(Input.GetKeyDown(KeyCode.R))
            {
                Start_Reload();
            }



            UpdateAiming();

            if (isAutomatic)
            {
                if (Input.GetButton("Fire1"))
                {
                    if (!weaponData.shotgunFire)
                        Shoot();
                    else
                        ShotgunShoot();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (!weaponData.shotgunFire)
                        Shoot();
                    else
                        ShotgunShoot();
                }
            
            }

        }
        else if (weaponData.itemType == ItemSO.ItemType.MeleeWeapon)
        {

        }


    }

    #region Fire Weapon Functions


    public void Shoot()
    {
        if (currentFireRate < fireRate || isReloading || !hasTakenOut || player.running || slotEquippedOn.stackSize <= 0)
        {
            return;
        }

        RaycastHit hit;

        Vector3 shootDir = shootPoint.forward;
        if (isAiming)
        {
            shootDir.x += Random.Range(-weaponData.aimSpread, weaponData.aimSpread);
            shootDir.y += Random.Range(-weaponData.aimSpread, weaponData.aimSpread);
        }
        else
        {
            shootDir.x += Random.Range(-weaponData.hipSpread, weaponData.hipSpread);
            shootDir.y += Random.Range(-weaponData.hipSpread, weaponData.hipSpread);
        }


        if(Physics.Raycast(shootPoint.position, shootDir, out hit, weaponData.range, shootableLayers))
        {
            GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

            Debug.Log($"Hitted : {hit.transform.name}");
        }

        anim.CrossFadeInFixedTime("Shoot_M9", 0.015f);
        Debug.Log("shoot");

        GetComponentInParent<CameraLook>().RecoilCamera(Random.Range(-weaponData.horizontalRecoil, weaponData.horizontalRecoil), Random.Range(weaponData.minVerticalRecoil, weaponData.maxVerticalRecoil)); 

        audioS.PlayOneShot(weaponData.shootSound);

        currentFireRate = 0;

        slotEquippedOn.stackSize--;
        slotEquippedOn.UpdateSlot();


    
    }

    public void ShotgunShoot()
    {
        if (currentFireRate < fireRate || isReloading || !hasTakenOut || player.running || slotEquippedOn.stackSize <= 0)
        {
            return;
        }

        for (int i = 0; i < weaponData.pelletsPerShot; i++)
        {

        

          RaycastHit hit;

            Vector3 shootDir = shootPoint.forward;
         if (isAiming)
         {
             shootDir.x += Random.Range(-weaponData.aimSpread, weaponData.aimSpread);
             shootDir.y += Random.Range(-weaponData.aimSpread, weaponData.aimSpread);
         }
         else
         {
             shootDir.x += Random.Range(-weaponData.hipSpread, weaponData.hipSpread);
             shootDir.y += Random.Range(-weaponData.hipSpread, weaponData.hipSpread);
         }


        if (Physics.Raycast(shootPoint.position, shootDir, out hit, weaponData.range, shootableLayers))
         {
                GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Debug.Log($"Hitted : {hit.transform.name}");
         }
        }

        anim.CrossFadeInFixedTime("Shoot_M9", 0.015f);
        //Debug.Log("shoot");

        GetComponentInParent<CameraLook>().RecoilCamera(Random.Range(-weaponData.horizontalRecoil, weaponData.horizontalRecoil), Random.Range(weaponData.minVerticalRecoil, weaponData.maxVerticalRecoil));

        audioS.PlayOneShot(weaponData.shootSound);

        currentFireRate = 0;

        slotEquippedOn.stackSize--;
        slotEquippedOn.UpdateSlot();



    }

    public void Start_Reload()
    {
        if (isReloading || slotEquippedOn.stackSize >= weaponData.magSize || player.running || !hasTakenOut || !CheckForBullets(weaponData.bulletData, weaponData.magSize))
            return;

        audioS.PlayOneShot(weaponData.reloadSound);

        anim.CrossFadeInFixedTime("Reload_M9", 0f);

        isReloading = true;
    }

    public void Finish_Reload()
    {
        //reloading = false in anim

        isReloading = false;

        TakeBullets(weaponData.bulletData, weaponData.magSize);
    }

    private bool CheckForBullets(ItemSO bulletData, int magSize)
    {
        InventoryManager inventory = GetComponentInParent<Player>().GetComponentInChildren<InventoryManager>();

        int amountfound = 0;

        for (int b = 0; b < inventory.inventorySlots.Length; b++)
        {
            if (!inventory.inventorySlots[b].IsEmpty)
            {
                if (inventory.inventorySlots[b].data == bulletData)
                {
                    amountfound += inventory.inventorySlots[b].stackSize;
                }
            }
        }


        if (amountfound < 1)
            return false;

        return true;
    }

    public void TakeBullets(ItemSO bulletData, int magSize)
    {
        InventoryManager inventory = GetComponentInParent<Player>().GetComponentInChildren<InventoryManager>();

        int ammoToReload = weaponData.magSize - slotEquippedOn.stackSize;
        int ammoInTheInventory = 0;

        // CHECK FOR THE BULLETS
        for (int i = 0; i < inventory.inventorySlots.Length; i++)
        {
           

            if (!inventory.inventorySlots[i].IsEmpty)
            {
                if (inventory.inventorySlots[i].data == bulletData)
                {
                    ammoInTheInventory += inventory.inventorySlots[i].stackSize;
                }
            }
        }


        int ammoToTakeFromInventory = (ammoInTheInventory >= ammoToReload) ? ammoToReload : ammoInTheInventory;


        // TAKE THE BULLETS FROM THE INVENTORY
        for (int i = 0; i < inventory.inventorySlots.Length; i++)
        {
            if (!inventory.inventorySlots[i].IsEmpty && ammoToTakeFromInventory > 0)
            {
                if (inventory.inventorySlots[i].data == bulletData)
                {
                    if (inventory.inventorySlots[i].stackSize <= ammoToTakeFromInventory)
                    {
                        slotEquippedOn.stackSize += inventory.inventorySlots[i].stackSize;
                        ammoToTakeFromInventory -= inventory.inventorySlots[i].stackSize;
                        inventory.inventorySlots[i].Clean();
                    }
                    else if (inventory.inventorySlots[i].stackSize > ammoToTakeFromInventory)
                    {
                        slotEquippedOn.stackSize = weaponData.magSize;
                        inventory.inventorySlots[i].stackSize -= ammoToTakeFromInventory;
                        ammoToTakeFromInventory = 0;
                        inventory.inventorySlots[i].UpdateSlot();
                    }
                }
            }
        }


        slotEquippedOn.UpdateSlot();

    }


    public void UpdateAiming()
    {
        if (Input.GetButton("Fire2") && !player.running && !isReloading)
        {
            //Debug.Log("aiming");
            transform.localPosition = Vector3.Slerp(transform.localPosition, aimPos, aimSpeed * Time.deltaTime);
            isAiming = true;
        }
        else           
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, hipPos, aimSpeed * Time.deltaTime);
            isAiming = false;
        }
    }



    #endregion

    public void UpdateAnimations()
    {
        anim.SetBool("Running", player.running);
    }

    public void Equip(Slot slot)
    {
        gameObject.SetActive(true);

        GetComponentInParent<CameraFOV_Handler>().weapon = this;

        slotEquippedOn = slot;

        transform.localPosition = hipPos;
    }

    public void UnEquip()
    {
        GetComponentInParent<CameraFOV_Handler>().weapon = null;

        gameObject.SetActive(false);

        slotEquippedOn = null;


    }
}
