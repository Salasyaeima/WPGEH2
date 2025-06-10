using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("Referensi Objek")]
    public Image menuVisual;
    public StarterAssetsInputs playerInputs;
    public GameObject mainMenu;
    public GameObject menuOptions;

    private bool isMenuOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuOpen)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void ResumeGame()
    {
        isMenuOpen = false;
        Time.timeScale = 1f;
        menuVisual.gameObject.SetActive(false);

        mainMenu.SetActive(true);
        menuOptions.SetActive(false);

        playerInputs.cursorInputForLook = true;
        playerInputs.SetCursorStateExternal(true);

        AudioListener.pause = false;
    }

    public void PauseGame()
    {
        isMenuOpen = true;
        Time.timeScale = 0f;
        menuVisual.gameObject.SetActive(true);

        mainMenu.SetActive(true);
        menuOptions.SetActive(false);

        playerInputs.cursorInputForLook = false;
        playerInputs.SetCursorStateExternal(false);
        playerInputs.look = Vector2.zero;

        AudioListener.pause = true;
    }

    public void ShowOptionsMenu()
    {
        mainMenu.SetActive(false);
        menuOptions.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        menuOptions.SetActive(false);
    }

    
}