using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Gradient gradient;
    [SerializeField] private float secondsPerday = 10f;

    private Light2D light2D;
    private float dayTime;
    private float dayTimeSpeed;

    private void Awake() {
        light2D = GetComponent<Light2D>();
        dayTimeSpeed = 1 / secondsPerday;
    }

    private void Update() {
        dayTime += Time.deltaTime * dayTimeSpeed;
        light2D.color = gradient.Evaluate(dayTime % 1f);
    }
}
