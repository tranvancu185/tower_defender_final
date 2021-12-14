using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyWaveUI : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager enemyWaveManager;
    private Camera mainCamera;
    private TextMeshProUGUI waveNumberText;
    private TextMeshProUGUI waveMessageText;
    private RectTransform enemyWaveSpawnPositionIndicator;
    private RectTransform enemyClosestPositionIndicator;
    private void Awake(){
        waveNumberText = transform.Find("waveNumberText").GetComponent<TextMeshProUGUI>();
        waveMessageText = transform.Find("waveMessageText").GetComponent<TextMeshProUGUI>();
        enemyWaveSpawnPositionIndicator = transform.Find("enemyWaveSpawnPositionIndicator").GetComponent<RectTransform>();
        enemyClosestPositionIndicator = transform.Find("enemyClosestPositionIndicator").GetComponent<RectTransform>();
    }

    private void Start(){
        mainCamera = Camera.main;
        enemyWaveManager.OnWaveNumberChanged += EnemyManager_OnWaveNumberChanged;
        SetWaveNumberText("Wave "+ enemyWaveManager.GetWaveNumber());

    }
    private void EnemyManager_OnWaveNumberChanged(object sender, System.EventArgs e){
        SetWaveNumberText("Wave "+ enemyWaveManager.GetWaveNumber());

    }
    private void Update(){
        HandleNextWaveMessage();
        HandleEnemyWaveSpawnPositionIndicator();
        HandleEnemyClosestPositionIndicator();
    }
    private void HandleNextWaveMessage(){
        float nextWaveSpawnTimer = enemyWaveManager.GetNextWaveSpawnTimer();
        if(nextWaveSpawnTimer <= 0f){
            SetMessageText("");
        }else {
            SetMessageText("Next Wave in "+ nextWaveSpawnTimer.ToString("F1")+"s");
        }
    }
    private void HandleEnemyClosestPositionIndicator(){
        float targetMaxRadius = 9999f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(mainCamera.transform.position, targetMaxRadius);

        Enemy targetEnemy = null;
        foreach(Collider2D collider2D in collider2DArray){
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null){
                if (targetEnemy == null){
                    targetEnemy = enemy;
                } else {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, targetEnemy.transform.position)){
                        targetEnemy = enemy;
                    }
                }
            }
        }
        if(mainCamera.transform.position != null){
            Vector3 dirToClosestEnemy = (targetEnemy.transform.position - mainCamera.transform.position).normalized;
            enemyClosestPositionIndicator.anchoredPosition = dirToClosestEnemy *250;
            enemyClosestPositionIndicator.eulerAngles = new Vector3(0,0, UtilsClass.GetAngleFromVector(dirToClosestEnemy));

            float distanceToClosestEnemy= Vector3.Distance(targetEnemy.transform.position, mainCamera.transform.position);
            enemyClosestPositionIndicator.gameObject.SetActive(distanceToClosestEnemy > mainCamera.orthographicSize * 1.5f);
        }else {
            enemyClosestPositionIndicator.gameObject.SetActive(false);
        }
    }
    private void HandleEnemyWaveSpawnPositionIndicator(){
        Vector3 dirToNextSpawnPosition = (enemyWaveManager.GetSpawnPosition() - mainCamera.transform.position).normalized;
        enemyWaveSpawnPositionIndicator.anchoredPosition = dirToNextSpawnPosition *300f;
        enemyWaveSpawnPositionIndicator.eulerAngles = new Vector3(0,0, UtilsClass.GetAngleFromVector(dirToNextSpawnPosition));

        float distanceToNextSpawnPosition = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), mainCamera.transform.position);
        enemyWaveSpawnPositionIndicator.gameObject.SetActive(distanceToNextSpawnPosition > mainCamera.orthographicSize * 1.5f);
    }
    private void SetMessageText(string message){
        waveMessageText.SetText(message);
    }

    private void SetWaveNumberText(string text){
        waveNumberText.SetText(text);

    }
}
