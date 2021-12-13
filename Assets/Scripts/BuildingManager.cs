using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingChanged;

    public class OnActiveBuildingTypeChangedEventArgs : EventArgs {
        public BuildingTypeSO activeBuildingType;
    }

    private Camera mainCamera;

    private BuildingTypeListSO buildingTypeList;

    private BuildingTypeSO activeBuildingType;

    private void Awake() {
        Instance = this;

        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);  
    }

    private void Start() {
        mainCamera = Camera.main;

        
    }

    // Update is called once per frame
    private void Update(){
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()){ 
            if (activeBuildingType != null && CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWolrdPosition()))
            {
                Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWolrdPosition(), Quaternion.identity);
            }
        }
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs{
            activeBuildingType = activeBuildingType
        });
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position){
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = collider2DArray.Length == 0;

        if(!isAreaClear) return false;

        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);

        foreach(Collider2D collider2D in collider2DArray){
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();

            if(buildingTypeHolder != null){
                if(buildingTypeHolder.buildingType == buildingType){
                    return false;
                }
            }
        }

        float maxConstructionRadius = 25;

        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);

        foreach(Collider2D collider2D in collider2DArray){
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();

            if(buildingTypeHolder != null){
                return true;
            }
        }

        return false;
    }
}
