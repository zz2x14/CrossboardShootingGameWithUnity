using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneUIController : MonoBehaviour
{
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    private void OnEnable()
    {
        ButtonPressedBehavior.buttonActionTable.Add(startButton.gameObject.name,OnStartButtonClick);
        ButtonPressedBehavior.buttonActionTable.Add(optionsButton.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehavior.buttonActionTable.Add(quitButton.gameObject.name, OnQuitButtonClick);
    }

    private void OnDisable()
    {
       ButtonPressedBehavior.buttonActionTable.Clear();
    }

    private void Start()
    {
        GameManager.Instance.GameState = GameState.Playing;//游戏开始时地默认状态为进行中
        Time.timeScale = 1f;//游戏暂停时回到主菜单恢复正常的timescale

        UIInput.Instance.SelectedUI(startButton);
    }

    public void OnStartButtonClick()
    {
        mainMenuCanvas.enabled = false;
        SceneLoadManager.Instance.LoadGameplayScene();
    }

    public void OnOptionsButtonClick()
    {
        
    }

    public void OnQuitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//退出编辑器运行状态
#else
        Application.Quit();
#endif        
    }

}
