using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public enum Scene {
        GameScene,
        MenuScene,
    }

    public static void Load (Scene scene){
        SceneManager.LoadScene(scene.ToString());
    }
}
