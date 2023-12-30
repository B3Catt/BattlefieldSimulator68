using UnityEngine;


namespace BattlefieldSimulator
{
    /// <summary>
    /// Unit; the object which Users can control;
    /// TODO: 完成对 Unit 类的设计
    /// </summary>
    public class Unit : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private HexPosition position;

        /// <summary>
        /// 
        /// </summary>
        public enum State
        {
            Move,
            Attack,
            Wait
        }

        /// <summary>
        /// 
        /// </summary>
        private State state = State.Move;

        /// <summary>
        /// 
        /// </summary>
        public int Player;

        /// <summary>
        /// 
        /// </summary>
        public int MaxHP;

        /// <summary>
        /// 
        /// </summary>
        private int hp;

        /// <summary>
        /// 
        /// </summary>
        public float Strength;

        /// <summary>
        /// 
        /// </summary>
        public float Variation;

        /// <summary>
        /// 
        /// </summary>
        public int Speed;

        /// <summary>
        /// 
        /// </summary>
        public int Range;

        /// <summary>
        /// 
        /// </summary>
        private int hpBarWidth = 64;

        /// <summary>
        /// 
        /// </summary>
        private int hpBarHeight = 16;

        /// <summary>
        /// 
        /// </summary>
        private bool moving = false;

        /// <summary>
        /// 
        /// </summary>
        private float t;

        /// <summary>
        /// 
        /// </summary>
        private Vector3[] path;

        /// <summary>
        /// position on the path
        /// </summary>
        private int iteratorP;

        /// <summary>
        /// 
        /// </summary>
        private const float MOTION_SPEED = 0.05f;

        /// <summary>
        /// 
        /// </summary>
        private IPlayerInterface player = null;

        /// <summary>
        /// 
        /// </summary>
        private bool localPlayer = false;
    }
}

