using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create (Vector3 position){
        Transform pfEnemy = Resources.Load<Transform>("pfEnemy");
        Transform enemyTransform = Instantiate(pfEnemy, position, Quaternion.identity);

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }
    private Transform targetTransform;
    private Rigidbody2D rigidbody2D;

    private HealthSystem healthSystem;
    private float lookForTargetTimer;
    private float lookForTargerTimerMax = .2f;
    private void Start(){
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (BuildingManager.Instance.GetHQBuilding() != null){
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        lookForTargetTimer = Random.Range(0f, lookForTargerTimerMax);
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e){
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
        CinemachineSnake.Instance.SnakeCamera(5f, .1f);
        ChromaticAberrationEffect.Instance.SetWeight(.5f);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e){
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        CinemachineSnake.Instance.SnakeCamera(7f, .15f);
        ChromaticAberrationEffect.Instance.SetWeight(.5f);
        Instantiate(Resources.Load<Transform>("pfEnemyDieParticles"), transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void Update()
    {
        HandleMovement();
        HandleTargetting(); 
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        Building building = collision.gameObject.GetComponent<Building>();
        if (building != null){
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);
            this.healthSystem.Damage(999);
        }    
    }

    private void HandleMovement(){
        if (targetTransform != null){
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;

            float moveSpeed = 6f;
            rigidbody2D.velocity = moveDir * moveSpeed;
        } else {
            rigidbody2D.velocity = Vector2.zero;
        }
    }
    private void HandleTargetting(){
        lookForTargetTimer -= Time.deltaTime;
        if(lookForTargetTimer < 0f){
            lookForTargetTimer += lookForTargerTimerMax;
            LookForTargets();
        }
    }
    private void LookForTargets(){
        float targetMaxRadius = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach(Collider2D collider2D in collider2DArray){
            Building building = collider2D.GetComponent<Building>();
            if (building != null){
                if (targetTransform == null){
                    targetTransform = building.transform;
                } else {
                    if (Vector3.Distance(transform.position, building.transform.position) < Vector3.Distance(transform.position, targetTransform.position)){
                        targetTransform = building.transform;
                    }
                }
            }
        }

        if (targetTransform == null){
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }
    }
}
