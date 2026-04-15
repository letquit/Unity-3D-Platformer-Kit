using System;
using System.Collections.Generic;

namespace Platformer
{
    /// <summary>
    /// 有限状态机。
    /// 负责管理状态的注册、切换以及转换条件的检测。
    /// </summary>
    public class StateMachine
    {
        private StateNode current; // 当前激活的状态节点
        private Dictionary<Type, StateNode> nodes = new(); // 所有已注册的状态节点字典
        private HashSet<ITransition> anyTransitions = new(); // 全局任意转换集合

        /// <summary>
        /// 每帧更新逻辑。
        /// 检测转换并更新当前状态。
        /// </summary>
        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
                ChangeState(transition.To);
            
            current.State?.Update();
        }
        
        /// <summary>
        /// 物理更新循环。
        /// 委托给当前状态处理物理逻辑。
        /// </summary>
        public void FixedUpdate()
        {
            current.State?.FixedUpdate();
        }

        /// <summary>
        /// 强制设置初始状态。
        /// </summary>
        public void SetState(IState state)
        {
            current = nodes[state.GetType()];
            current.State?.OnEnter();
        }

        /// <summary>
        /// 执行状态切换。
        /// 调用旧状态的 OnExit 和新状态的 OnEnter。
        /// </summary>
        private void ChangeState(IState state)
        {
            if (state == current.State) return;

            var previousState = current.State;
            var nextState = nodes[state.GetType()].State;
            
            previousState?.OnExit();
            nextState?.OnEnter();
            current = nodes[state.GetType()];
        }

        /// <summary>
        /// 获取满足条件的转换。
        /// 优先检查任意转换，其次检查当前状态的特定转换。
        /// </summary>
        private ITransition GetTransition()
        {
            // 检查全局任意转换（高优先级）
            foreach (var transition in anyTransitions)
                if (transition.Condition.Evaluate())
                    return transition;

            // 检查当前状态的特定转换
            foreach (var transition in current.Transitions)
                if (transition.Condition.Evaluate())
                    return transition;
            
            return null;
        }

        /// <summary>
        /// 添加特定状态之间的转换。
        /// </summary>
        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        /// <summary>
        /// 添加任意转换（可从任何状态跳转）。
        /// </summary>
        public void AddAnyTransition(IState to, IPredicate condition)
        {
            anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }

        /// <summary>
        /// 获取或添加状态节点（懒加载）。
        /// </summary>
        private StateNode GetOrAddNode(IState state)
        {
            var node = nodes.GetValueOrDefault(state.GetType());

            if (node == null)
            {
                node = new StateNode(state);
                nodes.Add(state.GetType(), node);
            }

            return node;
        }

        /// <summary>
        /// 状态节点内部类。
        /// 封装了状态对象及其拥有的转换集合。
        /// </summary>
        class StateNode
        {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to, IPredicate condition)
            {
                Transitions.Add(new Transition(to, condition));
            }
        }
    }
}