using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private Canvas hUDCanvas;
    [SerializeField] private Canvas pauseMenuCanvas;

    [Header("游戏菜单按钮")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    private void OnEnable()
    {
        playerInput.OnGamePause += PauseGame;
        playerInput.OnResumeGame += ResumeGame;

        resumeButton.onClick.AddListener(OnResumeButtonClick);
        optionsButton.onClick.AddListener(OnOptionsButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
    }

    private void OnDisable()
    {
        playerInput.OnGamePause -= PauseGame;
        playerInput.OnResumeGame -= ResumeGame;
        //注意：此处一开始忘了注销事件 - 场景重新再加载后再使用出现了报错 - 原本事件订阅的内容已被消除但未注销（已经为null确还要使用它）
        //是否未自己所理解的这个意思TODO：

        resumeButton.onClick.RemoveAllListeners();
        optionsButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();//有加必有减
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        playerInput.SwitchToDynamicMode();

        hUDCanvas.enabled = false;
        pauseMenuCanvas.enabled = true;

        playerInput.EnablePauseMenuInput();
    }

    public void ResumeGame()
    {
        OnResumeButtonClick();
    }

    public void OnResumeButtonClick()
    {
        Time.timeScale = 1f;
        playerInput.SWitchToFixedUpdateMode();

        pauseMenuCanvas.enabled = false;
        hUDCanvas.enabled = true;

        playerInput.EnableGameplayInput();
    }

    public void OnOptionsButtonClick()
    {

    }

    public void OnMainMenuButtonClick()
    {
        SceneLoadManager.Instance.LoadStartScene();
    }

}
