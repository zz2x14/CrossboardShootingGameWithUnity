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
        Time.timeScale = 1f;//游戏暂停时回到主菜单恢复正常的timescale
    }

    public void OnStartGameButtonClick()
    {
        SceneLoadManager.Instance.LoadGameplayScene();
    }

}
