using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSnake : MonoBehaviour
{
    public static CinemachineSnake Instance {get; private set;}
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineMultiChannelPerlin;
    private float timer;
    private float timerMax;   
    private float startingIntensity; 
    private void Awake() {
        Instance = this;

        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void Update() {
        if (timer < timerMax) {
            timer += Time.deltaTime;
            float amplitude = Mathf.Lerp(startingIntensity, 0f, timer / timerMax);
            cinemachineMultiChannelPerlin.m_AmplitudeGain = amplitude;
        }
    }
    public void SnakeCamera(float intensity, float timerMax){
        this.timerMax = timerMax;
        timer = 0f;
        startingIntensity = intensity;
        cinemachineMultiChannelPerlin.m_AmplitudeGain = intensity;
    }
}
