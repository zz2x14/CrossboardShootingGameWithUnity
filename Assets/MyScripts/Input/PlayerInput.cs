using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static InputActions;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, IGameplayActions
{
    private InputActions inputActions;

    public event UnityAction<Vector2> OnPlayerMove = delegate { };//�����¼�һ����ί�У������¼��Ͳ�����ˣ�����ʱ����Ҫ�����ж�
    public event UnityAction OnStopMove = delegate { };

    private void OnEnable()
    {
        inputActions = new InputActions();

        inputActions.Gameplay.SetCallbacks(this);//��inputactions�еǼ�
    }

    private void OnDisable()
    {
        inputActions.Gameplay.Disable();//�뼤�����Ӧ
    }

    public void EnableGameplayInput()//�ṩ�������õķ���
    {
        inputActions.Gameplay.Enable();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
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
}
