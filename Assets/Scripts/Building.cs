using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Building : MonoBehaviour
{
    private BuildingTypeSO buildingType;
    private HealthSystem healthSystem;
    private void Start()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);

        healthSystem.OnDied += HealthSystem_OnDied;
    }
    private void Update()
    {
        
    }
    private void HealthSystem_OnDied(object sender, System.EventArgs e){
        Destroy(gameObject);
    }
}
