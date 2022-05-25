using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static InputActions;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, IGameplayActions, IPauseMenuActions
{
    private InputActions inputActions;

    public event UnityAction<Vector2> OnPlayerMove = delegate { };//给予事件一个空委托，这样事件就不会空了，调用时不需要再做判断
    public event UnityAction OnStopMove = delegate { };

    public event UnityAction OnStartFire = delegate { };
    public event UnityAction OnStopFire = delegate { };

    public event UnityAction OnPlayerDodge = delegate { };

    public event UnityAction OnPlayerOverdrive = delegate { };

    public event UnityAction OnGamePause = delegate { };
    public event UnityAction OnResumeGame = delegate { };

    public event UnityAction OnLaunch = delegate { };

    private void OnEnable()
    {
        inputActions = new InputActions();

        inputActions.Gameplay.SetCallbacks(this);//在inputactions中登记
        inputActions.PauseMenu.SetCallbacks(this);
    }

    private void OnDisable()
    {
        DisableAllInput();//与激活相对应
    }

    public void SwitchInputAction(InputActionMap inputActionMap,bool isUIInput)
    {
        DisableAllInput();
        inputActionMap.Enable();//启用目标输入

        if (!isUIInput)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;//UI输入时将鼠标恢复显示
        }
    }
    public void EnableGameplayInput() => SwitchInputAction(inputActions.Gameplay, false); 
    public void EnablePauseMenuInput() => SwitchInputAction(inputActions.PauseMenu, true);

    //停止timescale时 需要切换输入系统的更新模式 - 否则暂停后不能再按下回到游戏中（因为无法接收到按键按下信号）
    public void SwitchToDynamicMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void SWitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    public void DisableAllInput()
    {
        inputActions.Disable();//禁用所有的输入
    }
   
    public void OnMove(InputAction.CallbackContext context)
    {
        //InputAction的阶段状态 - cancel=input.getkeyup,performed=getkey,waiting=啥也不做,started=getkeydown,disable为禁用该inputaction时-没有激活时
        if (context.phase == InputActionPhase.Performed)
        {
            OnPlayerMove(context.ReadValue<Vector2>());//读取所需要的目标值 - 再传入
        }
       if(context.phase == InputActionPhase.Canceled)
        {
            OnStopMove();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnStartFire();
        }
        if (context.canceled)
        {
            OnStopFire();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnPlayerDodge();
        }
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnPlayerOverdrive();
        }
    }

    public void OnPauseGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnGamePause();
        }
    }

    public void OnResumeGameFromMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnResumeGame();
        }
    }

    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnLaunch();
        }
    }
}
