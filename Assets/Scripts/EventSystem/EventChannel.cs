using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 泛型事件通道基类。
    /// 基于 ScriptableObject 实现，用于在不同脚本之间传递事件和数据，实现解耦。
    /// 遵循观察者模式。
    /// </summary>
    /// <typeparam name="T">事件携带的数据类型</typeparam>
    public abstract class EventChannel<T> : ScriptableObject
    {
        // 使用 HashSet 存储监听者，防止重复添加同一个观察者
        private readonly HashSet<EventListener<T>> observers = new();

        /// <summary>
        /// 触发事件。
        /// 通知所有注册的观察者。
        /// </summary>
        /// <param name="value">要传递的数据</param>
        public void Invoke(T value)
        {
            foreach (var observer in observers)
            {
                observer.Raise(value);
            }
        }
        
        /// <summary>
        /// 注册监听者。
        /// </summary>
        public void Register(EventListener<T> observer) => observers.Add(observer);

        /// <summary>
        /// 取消注册监听者。
        /// </summary>
        public void Deregister(EventListener<T> observer) => observers.Remove(observer);
    }

    /// <summary>
    /// 空数据结构。
    /// 用于作为无参事件的泛型参数占位符。
    /// </summary>
    public readonly struct Empty { }
    
    /// <summary>
    /// 无参事件通道。
    /// 继承自 EventChannel<Empty>，用于不需要传递数据的事件（如“游戏开始”、“玩家死亡”）。
    /// 可以在右键菜单中创建：Assets -> Create -> Events -> EmptyEventChannel
    /// </summary>
    [CreateAssetMenu(menuName = "Events/EmptyEventChannel")]
    public class EventChannel : EventChannel<Empty> { }
}