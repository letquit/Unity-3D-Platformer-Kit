using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 生成点策略接口。
    /// 定义了选择下一个生成点的标准契约。
    /// 实现此接口的类可以决定生成点是按顺序、随机还是其他方式选择。
    /// </summary>
    public interface ISpawnPointStrategy
    {
        /// <summary>
        /// 获取下一个生成点的位置。
        /// </summary>
        /// <returns>下一个生成点的 Transform 组件</returns>
        Transform NextSpawnPoint();
    }
}