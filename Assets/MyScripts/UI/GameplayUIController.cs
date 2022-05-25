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

    [Header("��ͣ�ص���Ч")]
    [SerializeField] private AudioData pauseGameAuioData;
    [SerializeField] private AudioData resumeGameAuioData;

    private int buttonPressedParametersID = Animator.StringToHash("Pressed");

    private void OnEnable()
    {
        playerInput.OnGamePause += PauseGame;
        playerInput.OnResumeGame += ResumeGame;
        
        //������¼������ֵ� - ���¼���Ϊ���������߼�ִ�� - Ҳ����Ҫ��Ϊbutton������ӵ���¼���
        ButtonPressedBehavior.buttonActionTable.Add(resumeButton.gameObject.name,OnResumeButtonClick);
        ButtonPressedBehavior.buttonActionTable.Add(optionsButton.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehavior.buttonActionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClick);

        //resumeButton.onClick.AddListener(OnResumeButtonClick);
        //optionsButton.onClick.AddListener(OnOptionsButtonClick);
        //mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
    }

    private void OnDisable()
    {
        playerInput.OnGamePause -= PauseGame;
        playerInput.OnResumeGame -= ResumeGame;
        //ע�⣺�˴�һ��ʼ����ע���¼� - ���������ټ��غ���ʹ�ó����˱��� - ԭ���¼����ĵ������ѱ�������δע�����Ѿ�Ϊnullȷ��Ҫʹ������
        //�Ƿ�δ�Լ������������˼ TODO��

        //resumeButton.onClick.RemoveAllListeners();
        //optionsButton.onClick.RemoveAllListeners();
        //mainMenuButton.onClick.RemoveAllListeners();//�мӱ��м�

        ButtonPressedBehavior.buttonActionTable.Clear();//ͬ�����мӱ��м� - ת���µĳ���������û�и�����͸�ע����
    }

    public void PauseGame()
    {
        TimeController.Instance.Pause();

        AudioManager.Instance.PlaySFX(pauseGameAuioData);

        hUDCanvas.enabled = false;
        pauseMenuCanvas.enabled = true;

        playerInput.SwitchToDynamicMode();
        playerInput.EnablePauseMenuInput();

        UIInput.Instance.SelectedUI(resumeButton);//��ͣ��Ϸʱ ��һ����ť���ص���Ϸ��ť��Ĭ��Ϊѡ��״̬
    }

    public void ResumeGame()
    {
        AudioManager.Instance.PlaySFX(resumeGameAuioData);

        //����֮ǰѡ�����ĸ���ť ���»ص���Ϸ��ť�� - �����˳���ť�Ĳ��Ŷ��� - ִ���߼�
        resumeButton.Select();//button�ȼ̳���selectable Ҳ����ʹ���䷽��

        resumeButton.animator.SetTrigger(buttonPressedParametersID);//ʹ�ù�ϣֵ ����ID ��ʹ�����Ƹ���У

        //OnResumeButtonClick();�������Ž���������ֵ��а󶨵��¼� �����ٵ�����
    }

    private void OnResumeButtonClick()
    {
        TimeController.Instance.Unpause();
        
        pauseMenuCanvas.enabled = false;
        hUDCanvas.enabled = true;

        playerInput.SWitchToFixedUpdateMode();
        playerInput.EnableGameplayInput();
    }

    private void OnOptionsButtonClick()
    {
        UIInput.Instance.SelectedUI(optionsButton);
        playerInput.EnablePauseMenuInput();
    }

    private void OnMainMenuButtonClick()
    {
        SceneLoadManager.Instance.LoadStartScene();
    }

}
