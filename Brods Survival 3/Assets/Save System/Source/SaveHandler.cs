using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveHandler : MonoBehaviour
{
    [Header("Savable Objects")]
    [Tooltip("You must drag here every savable object that you wish to load")]
    public List<SavableObject> savableObjects;

    public void Load()
    {
        // CHECK THAT DIRECTORY & FILE HAVE BEEN CREATED
        if (!Directory.Exists(Application.dataPath + "/saves"))
        {
            return;
        }

        if (!File.Exists(Application.dataPath + "/saves/GameSave.save"))
        {
            return;
        }


        // LOAD THE FILE TO SAVE DATA
        SaveData.Load();


        // DESTROY INSTANTIATED SAVABLE OBJECT
        SavableObject[] objectsToDestroy = FindObjectsOfType<SavableObject>();

        for (int i = 0; i < objectsToDestroy.Length; i++)
        {
            if (objectsToDestroy[i].instantiate)
                Destroy(objectsToDestroy[i].gameObject);
        }



        // RUN THROUGH SAVABLE OBJECTS LIST AND LOAD THE DATA TO THEM
        foreach(SAVE_SavableObject save in SaveData.Singleton.savableObjects)
        {
            SavableObject obj = null;

            
            // FIND OBJECT IN SAVABLE OBJECTS
            for (int i = 0; i < savableObjects.Count; i++)
            {
                if (save.Id == savableObjects[i].ID)
                {
                    obj = savableObjects[i];
                    break;
                }
            }


            // IF THE OBJECT IS NOT FOUND THAN JUST RETURN
            if (obj == null)
            {
                Debug.LogError($"SAVE SYSTEM : The object to load was not found, make sure the object is in the Savable Objects list of SaveHandler");
                return;
            }



            // LOAD DATA INTO THE OBJECT
            if (obj.instantiate)
            {
                SavableObject instance = Instantiate(obj.gameObject).GetComponent<SavableObject>();

                instance.ID = save.Id;
                instance.transform.position = save.position;
                instance.transform.rotation = save.rotation;
            }
            else
            {
                obj.ID = save.Id;
                obj.transform.position = save.position;
                obj.transform.rotation = save.rotation;
            }

        }

    }

    public void Save()
    {
        // INITIALIZE THE SAVABLE OBJECT FILES LIST IN SAVE DATA
        SaveData.Singleton.savableObjects = new List<SAVE_SavableObject>();

        SavableObject[] objectsToSave = FindObjectsOfType<SavableObject>();


        foreach (SavableObject obj in objectsToSave)
        {
            // SAVE THE SAVABLE OBJECTS DATA TO THE SAVABLE OBJECT SAVE SCRIPT
            SAVE_SavableObject save = new SAVE_SavableObject();

            save.Id = obj.ID;
            save.position = obj.transform.position;
            save.rotation = obj.transform.rotation;

            SaveData.Singleton.savableObjects.Add(save);

        }

        // SAVE IT TO SAVE DATA
        if (SaveData.Save())
        {
            Debug.Log("SAVE SYSTEM : Game was saved successfully.");
        }
        else
        {
            Debug.LogWarning("SAVE SYSTEM : An error occured while saving the game.");
        }    
        
    }

    public void DeleteSaveFile()
    {
        if (!Directory.Exists(Application.dataPath + "/saves"))
        {
            return;
        }

        if (!File.Exists(Application.dataPath + "/saves/GameSave.save"))
        {
            return;
        }

        File.Delete(Application.dataPath + "/saves/GameSave.save");


    }
}
