using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public Level[] levels;
    public GameObject Controls;
    public GameObject Credits;
    public GameObject OnionKing;
    public GameObject LevelsMenu;
    public GameObject mainMenu;
    public Image levelImage;
    private int levelSelected = 1;

    public void SelectLevel(int level){
        levelSelected = level;
        levelImage.sprite = levels[level - 1].image;
    }

    public void ShowCredits() {
        Credits.SetActive(true);
        Controls.SetActive(false);
        OnionKing.SetActive(false);
    }

    public void ShowControls() {
        Controls.SetActive(true);
        Credits.SetActive(false);
        OnionKing.SetActive(false);
    }

    public void PlaySelectedLevel(){
        ChangeScene(levels[levelSelected-1].name);
    }

    public void ShowLevelMenu(){
        LevelsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ShowMainMenu(){
        LevelsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ChangeScene(string SceneName){
        SceneManager.LoadScene(SceneName);
    }

    public void Quit() {
        Application.Quit();
    }
}
