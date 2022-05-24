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
        Time.timeScale = 1f;//��Ϸ��ͣʱ�ص����˵��ָ�������timescale
    }

    public void OnStartGameButtonClick()
    {
        SceneLoadManager.Instance.LoadGameplayScene();
    }

}
