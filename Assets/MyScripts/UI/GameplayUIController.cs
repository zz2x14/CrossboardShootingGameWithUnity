using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private Canvas hUDCanvas;
    [SerializeField] private Canvas pauseMenuCanvas;

    [Header("��Ϸ�˵���ť")]
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
        //ע�⣺�˴�һ��ʼ����ע���¼� - ���������ټ��غ���ʹ�ó����˱��� - ԭ���¼����ĵ������ѱ�������δע�����Ѿ�Ϊnullȷ��Ҫʹ������
        //�Ƿ�δ�Լ������������˼TODO��

        resumeButton.onClick.RemoveAllListeners();
        optionsButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();//�мӱ��м�
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
