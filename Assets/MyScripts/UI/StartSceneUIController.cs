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
        GameManager.Instance.GameState = GameState.Playing;//��Ϸ��ʼʱ��Ĭ��״̬Ϊ������
        Time.timeScale = 1f;//��Ϸ��ͣʱ�ص����˵��ָ�������timescale

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
        UnityEditor.EditorApplication.isPlaying = false;//�˳��༭������״̬
#else
        Application.Quit();
#endif        
    }

}
