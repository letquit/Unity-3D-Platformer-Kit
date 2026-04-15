using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 浮点数事件通道。
    /// 专门用于传递 float 类型数据的事件（如：生命值、分数、时间）。
    /// 可以在右键菜单中创建：Assets -> Create -> Events -> FloatEventChannel
    /// </summary>
    [CreateAssetMenu(menuName = "Events/FloatEventChannel")]
    public class FloatEventChannel : EventChannel<float> { }
}