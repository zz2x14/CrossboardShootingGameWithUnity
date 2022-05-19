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

    public event UnityAction<Vector2> OnPlayerMove = delegate { };//给予事件一个空委托，这样事件就不会空了，调用时不需要再做判断
    public event UnityAction OnStopMove = delegate { };

    private void OnEnable()
    {
        inputActions = new InputActions();

        inputActions.Gameplay.SetCallbacks(this);//在inputactions中登记
    }

    private void OnDisable()
    {
        inputActions.Gameplay.Disable();//与激活相对应
    }

    public void EnableGameplayInput()//提供激活启用的方法
    {
        inputActions.Gameplay.Enable();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
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
}
