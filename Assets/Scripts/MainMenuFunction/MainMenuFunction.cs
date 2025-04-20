using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class MainMenuFunction : MonoBehaviour
{
    private VisualElement settingPanel;
    Button startButton;
    Button optionButton;
    Button exitButton;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        startButton = root.Q<Button>("Start");
        optionButton = root.Q<Button>("Option");
        exitButton = root.Q<Button>("Exit");
        settingPanel = root.Q<VisualElement>("SettingMenu");
        settingPanel.style.display = DisplayStyle.None;

        startButton.clicked += startButtonClicked;
        optionButton.clicked += optionButtonClicked;
    }

    private void OnDisable()
    {
        startButton.clicked -= startButtonClicked;
        optionButton.clicked -= optionButtonClicked;
    }

    void startButtonClicked(){
        SceneManager.LoadScene("Rooms");
    }

    void optionButtonClicked(){
        startButton.style.display = DisplayStyle.None;
        exitButton.style.display = DisplayStyle.None;
        optionButton.style.display = DisplayStyle.None;
        
        settingPanel.style.display = DisplayStyle.Flex;
    }
}
