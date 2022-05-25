using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneUIController : MonoBehaviour
{
    [SerializeField] private Button startGameButton;

    private void OnEnable()
    {
       startGameButton.onClick.AddListener(OnStartGameButtonClick);
    }

    private void OnDisable()
    {
        startGameButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        GameManager.Instance.GameState = GameState.Playing;//游戏开始时地默认状态为进行中
        Time.timeScale = 1f;//游戏暂停时回到主菜单恢复正常的timescale
    }

    public void OnStartGameButtonClick()
    {
        SceneLoadManager.Instance.LoadGameplayScene();
    }

}
