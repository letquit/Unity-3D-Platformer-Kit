namespace Platformer
{
    /// <summary>
    /// 状态转换接口。
    /// 定义了从一个状态跳转到另一个状态的规则。
    /// 它包含目标状态和触发跳转的条件。
    /// </summary>
    public interface ITransition
    {
        /// <summary>
        /// 获取目标状态。
        /// 如果条件满足，状态机将转换到此状态。
        /// </summary>
        IState To { get; }

        /// <summary>
        /// 获取触发条件。
        /// 一个返回布尔值的谓词，用于判断是否应该进行状态转换。
        /// </summary>
        IPredicate Condition { get; }
    }
}