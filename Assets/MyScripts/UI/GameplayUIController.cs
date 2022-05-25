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

    [Header("暂停回到音效")]
    [SerializeField] private AudioData pauseGameAuioData;
    [SerializeField] private AudioData resumeGameAuioData;

    private int buttonPressedParametersID = Animator.StringToHash("Pressed");

    private void OnEnable()
    {
        playerInput.OnGamePause += PauseGame;
        playerInput.OnResumeGame += ResumeGame;
        
        //将点击事件加入字典 - 把事件行为交给动画逻辑执行 - 也不需要再为button单独添加点击事件了
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
        //注意：此处一开始忘了注销事件 - 场景重新再加载后再使用出现了报错 - 原本事件订阅的内容已被消除但未注销（已经为null确还要使用它）
        //是否未自己所理解的这个意思 TODO：

        //resumeButton.onClick.RemoveAllListeners();
        //optionsButton.onClick.RemoveAllListeners();
        //mainMenuButton.onClick.RemoveAllListeners();//有加必有减

        ButtonPressedBehavior.buttonActionTable.Clear();//同样的有加必有减 - 转到新的场景后如若没有该物体就该注销掉
    }

    public void PauseGame()
    {
        TimeController.Instance.Pause();

        AudioManager.Instance.PlaySFX(pauseGameAuioData);

        hUDCanvas.enabled = false;
        pauseMenuCanvas.enabled = true;

        playerInput.SwitchToDynamicMode();
        playerInput.EnablePauseMenuInput();

        UIInput.Instance.SelectedUI(resumeButton);//暂停游戏时 第一个按钮（回到游戏按钮）默认为选中状态
    }

    public void ResumeGame()
    {
        AudioManager.Instance.PlaySFX(resumeGameAuioData);

        //无论之前选中了哪个按钮 按下回到游戏按钮后 - 播放退出按钮的播放动画 - 执行逻辑
        resumeButton.Select();//button等继承了selectable 也可以使用其方法

        resumeButton.animator.SetTrigger(buttonPressedParametersID);//使用哈希值 传入ID 比使用名称更高校

        //OnResumeButtonClick();动画播放结束会调用字典中绑定的事件 无需再调用了
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
