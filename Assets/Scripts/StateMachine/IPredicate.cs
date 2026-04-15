namespace Platformer
{
    /// <summary>
    /// 谓词接口。
    /// 定义了逻辑判断的标准契约。
    /// 任何实现此接口的类都必须能够评估一个条件并返回真或假。
    /// </summary>
    public interface IPredicate
    {
        /// <summary>
        /// 执行评估。
        /// </summary>
        /// <returns>评估结果（真或假）</returns>
        bool Evaluate();
    }
}