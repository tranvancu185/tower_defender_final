using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private ResourceGeneratorData resourceGeneratorData;
    private float timer;
    private float timerMax;

    private void Awake(){
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        timerMax = resourceGeneratorData.timerMax;
    }

    private void Start(){
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, resourceGeneratorData.resourceDetectionRadius);

        int nearbyResourceAmount = 0;
        foreach(Collider2D collider2D in collider2DArray){
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
            if(resourceNode != null){
                if(resourceNode.resourceType == resourceGeneratorData.resourceType){
                    nearbyResourceAmount++;
                }
                
            }
        }

        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);

        if(nearbyResourceAmount == 0){
            enabled = false;
        }else{
            timerMax = (resourceGeneratorData.timerMax / 2f) + 
                        resourceGeneratorData.timerMax * 
                        (1 - (float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount);
        }

        Debug.Log("nearbyResourceAmount: "+nearbyResourceAmount+ " ; timerMax: " + timerMax);
    }

    private void Update() {
        timer -= Time.deltaTime;
        if(timer <= 0f){
            timer += timerMax;
            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 1);

        }
    }
}
