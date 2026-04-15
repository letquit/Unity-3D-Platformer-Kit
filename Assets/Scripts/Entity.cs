using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 实体基类。
    /// 所有游戏世界中可交互或具有生命周期的对象（如玩家、敌人、物品）都应继承此类。
    /// 目前作为标记基类使用，未来可扩展共有属性（如生命值、状态等）。
    /// </summary>
    public abstract class Entity : MonoBehaviour
    {
        // 可以在这里添加所有实体共有的成员变量或方法
        // 例如：public int Health;
    }
}