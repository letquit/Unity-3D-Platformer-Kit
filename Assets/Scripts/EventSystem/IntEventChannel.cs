using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 整数事件通道。
    /// 专门用于传递 int 类型数据的事件（如：金币数、弹药量、关卡索引）。
    /// 可以在右键菜单中创建：Assets -> Create -> Events -> IntEventChannel
    /// </summary>
    [CreateAssetMenu(menuName = "Events/IntEventChannel")]
    public class IntEventChannel : EventChannel<int> { }
}