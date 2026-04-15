using System;

namespace Platformer
{
    /// <summary>
    /// 函数谓词。
    /// 一个基于委托的轻量级谓词实现。
    /// 允许使用 Lambda 表达式或方法来代替创建具体的谓词类。
    /// </summary>
    public class FuncPredicate : IPredicate
    {
        private readonly Func<bool> func; // 封装的布尔值返回函数

        /// <summary>
        /// 构造函数，注入判断逻辑。
        /// </summary>
        /// <param name="func">用于评估的函数</param>
        public FuncPredicate(Func<bool> func)
        {
            this.func = func;
        }

        /// <summary>
        /// 执行评估。
        /// 调用内部封装的函数并返回结果。
        /// </summary>
        /// <returns>评估结果（真或假）</returns>
        public bool Evaluate()
        {
            return func.Invoke();
        }
    }
}