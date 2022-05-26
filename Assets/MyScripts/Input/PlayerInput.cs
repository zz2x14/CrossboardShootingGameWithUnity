using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static InputActions;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, IGameplayActions, IPauseMenuActions,IGameOverScreenActions
{
    private InputActions inputActions;

    public event UnityAction<Vector2> OnPlayerMove = delegate { };//�����¼�һ����ί�У������¼��Ͳ�����ˣ�����ʱ����Ҫ�����ж�
    public event UnityAction OnStopMove = delegate { };

    public event UnityAction OnStartFire = delegate { };
    public event UnityAction OnStopFire = delegate { };

    public event UnityAction OnPlayerDodge = delegate { };

    public event UnityAction OnPlayerOverdrive = delegate { };

    public event UnityAction OnGamePause = delegate { };
    public event UnityAction OnResumeGame = delegate { };

    public event UnityAction OnLaunch = delegate { };

    public event UnityAction OnGameOver = delegate { };

    private void OnEnable()
    {
        inputActions = new InputActions();

        inputActions.Gameplay.SetCallbacks(this);//��inputactions�еǼ�
        inputActions.PauseMenu.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);
    }

    private void OnDisable()
    {
        DisableAllInput();//�뼤�����Ӧ
    }

    public void SwitchInputAction(InputActionMap inputActionMap,bool isUIInput)
    {
        DisableAllInput();
        inputActionMap.Enable();//����Ŀ������

        if (!isUIInput)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;//UI����ʱ�����ָ���ʾ
        }
    }
    public void EnableGameplayInput() => SwitchInputAction(inputActions.Gameplay, false); 
    public void EnablePauseMenuInput() => SwitchInputAction(inputActions.PauseMenu, true);
    public void EnableGameOverInput() => SwitchInputAction(inputActions.GameOverScreen, false);

    //ֹͣtimescaleʱ ��Ҫ�л�����ϵͳ�ĸ���ģʽ - ������ͣ�����ٰ��»ص���Ϸ�У���Ϊ�޷����յ����������źţ�
    public void SwitchToDynamicMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void SWitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    public void DisableAllInput()
    {
        inputActions.Disable();//�������е�����
    }
   
    public void OnMove(InputAction.CallbackContext context)
    {
        //InputAction�Ľ׶�״̬ - cancel=input.getkeyup,performed=getkey,waiting=ɶҲ����,started=getkeydown,disableΪ���ø�inputactionʱ-û�м���ʱ
        if (context.phase == InputActionPhase.Performed)
        {
            OnPlayerMove(context.ReadValue<Vector2>());//��ȡ����Ҫ��Ŀ��ֵ - �ٴ���
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

    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnGameOver();
        }
    }
}
