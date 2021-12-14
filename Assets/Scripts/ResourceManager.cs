using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance {get; private set;}

    public event EventHandler OnResourceAmountChanged;

    private Dictionary<ResourceTypeSO, int> resoureceAmountDictionary;

    private void Awake(){
        Instance = this;
        resoureceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

        ResourceTypeListSO resourceTypeList  = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        foreach (ResourceTypeSO resourceType in resourceTypeList.list){
            resoureceAmountDictionary[resourceType] = 0;
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.T)){ 
            ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
            AddResource(resourceTypeList.list[0],2);
        }
    }

    private void TestResoureceAmountDictionary(){
        foreach (ResourceTypeSO resourceType in resoureceAmountDictionary.Keys){
            Debug.Log(resourceType.nameString);
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount){
        resoureceAmountDictionary[resourceType] += amount;
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetResourceAmount(ResourceTypeSO resourceType){ 
        return resoureceAmountDictionary[resourceType];
    }

    public bool CanAfford(ResourceAmount[] resourceAmountArray) {
        foreach (ResourceAmount resourceAmount in resourceAmountArray) {
            if (GetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount)
            {

            }
            else {
                return false;
            }
        }
        return true;
    }

    public void SpendResources(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            resoureceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;       
        }
    }
}
