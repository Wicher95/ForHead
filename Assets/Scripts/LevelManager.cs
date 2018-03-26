using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static string category;

    public AudioClip button;

    private AudioSource source;

    public string scene;
    void Start()
    {
        source = GetComponent<AudioSource>();
        LevelName.level = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            source.PlayOneShot(button, 1f);
            SceneManager.LoadScene("Menu");
        }
    }

    public void StartGame()
    {
        scene = "Game";
        source.PlayOneShot(button,1f);
        LevelName.level = EventSystem.current.currentSelectedGameObject.name;
        category = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}