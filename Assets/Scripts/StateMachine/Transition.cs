namespace Platformer
{
    /// <summary>
    /// 状态转换类。
    /// ITransition 接口的具体实现。
    /// 用于定义从一个状态跳转到另一个状态的规则。
    /// </summary>
    public class Transition : ITransition
    {
        /// <summary>
        /// 目标状态。
        /// 如果条件满足，状态机将转换到此状态。
        /// </summary>
        public IState To { get; }
        
        /// <summary>
        /// 触发条件。
        /// 一个返回布尔值的谓词，用于判断是否应该进行状态转换。
        /// </summary>
        public IPredicate Condition { get; }
        
        /// <summary>
        /// 构造函数，注入目标状态和条件。
        /// </summary>
        /// <param name="to">目标状态</param>
        /// <param name="condition">触发条件</param>
        public Transition(IState to, IPredicate condition)
        {
            To = to;
            Condition = condition;
        }
    }
}