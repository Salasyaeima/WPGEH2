using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class MainMenuFunction : MonoBehaviour
{
    Button startButton;
    Button optionButton;
    Button exitButton;
    Slider volumeSlider;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        startButton = root.Q<Button>("Start");
        optionButton = root.Q<Button>("Option");
        exitButton = root.Q<Button>("Exit");
        
        startButton.clicked += startButtonClicked;
        optionButton.clicked += optionButtonFirstClick;
    }

    void startButtonClicked(){
        SceneManager.LoadScene("Rooms");
    }

    void optionButtonFirstClick(){
        startButton.style.display = DisplayStyle.None;
        exitButton.style.display = DisplayStyle.None;
        optionButton.style.display = DisplayStyle.None;



    }
}
