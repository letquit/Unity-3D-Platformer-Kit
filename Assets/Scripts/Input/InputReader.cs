using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Platformer
{
    /// <summary>
    /// 输入读取器。
    /// 负责读取玩家输入并通过事件广播，实现输入与逻辑的解耦。
    /// 实现了由 InputSystem 自动生成的 IPlayerActions 接口。
    /// </summary>
    [CreateAssetMenu(fileName = "InputReader", menuName = "Platformer/InputReader")]
    public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions
    {
        // --- 输入事件定义 ---
        // 使用 UnityAction 允许在 Inspector 中绑定，也支持代码订阅
        
        public event UnityAction<Vector2> Move = delegate { }; // 移动输入 (WASD/摇杆)
        public event UnityAction<Vector2, bool> Look = delegate { }; // 视角输入 (鼠标/右摇杆), bool 表示是否为鼠标
        public event UnityAction EnableMouseControlCamera = delegate { }; // 启用鼠标控制摄像机
        public event UnityAction DisableMouseControlCamera = delegate { }; // 禁用鼠标控制摄像机
        public event UnityAction<bool> Jump = delegate { }; // 跳跃输入 (bool 表示是否按下)
        public event UnityAction<bool> Dash = delegate { }; // 冲刺输入 (bool 表示是否按下)
        public event UnityAction Attack = delegate { }; // 攻击输入 (仅触发一次)

        private PlayerInputActions inputActions; // 自动生成的 InputActionAsset 包装类

        /// <summary>
        /// 获取当前的移动方向值（只读属性）。
        /// </summary>
        public Vector3 Direction => inputActions.Player.Move.ReadValue<Vector2>();

        private void OnEnable()
        {
            if (inputActions == null)
            {
                // 初始化输入映射
                inputActions = new PlayerInputActions();
                // 将本类注册为回调接收者
                inputActions.Player.SetCallbacks(this);
            }
        }

        /// <summary>
        /// 启用玩家输入映射。
        /// 通常在角色生成或游戏开始时调用。
        /// </summary>
        public void EnablePlayerActions()
        {
            inputActions.Enable();
        }

        // --- 接口实现：输入回调 ---
        // 这些方法由 InputSystem 在检测到输入时自动调用

        public void OnMove(InputAction.CallbackContext context)
        {
            // 广播移动向量
            Move.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            // 广播视角向量，并检测当前设备是否为鼠标
            Look.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
        }

        /// <summary>
        /// 判断当前输入设备是否为鼠标。
        /// </summary>
        private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

        public void OnFire(InputAction.CallbackContext context)
        {
            // 仅在按键按下的瞬间触发攻击
            if (context.phase == InputActionPhase.Started)
            {
                Attack.Invoke();
            }
        }

        public void OnMouseControlCamera(InputAction.CallbackContext context)
        {
            // 根据按键状态切换鼠标控制摄像机的开关
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    EnableMouseControlCamera.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    DisableMouseControlCamera.Invoke();
                    break;
            }
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            // 处理冲刺（按下开始，松开结束）
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Dash.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Dash.Invoke(false);
                    break;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            // 处理跳跃（按下开始，松开结束）
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Jump.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Jump.Invoke(false);
                    break;
            }
        }
    }
}