using System;

namespace Contest
{
    public class StatusTracker : IStatus
    {
        public UnitBase Parent { get; private set; }

        private StatusBase maxHp;
        private StatusBase hp;
        private StatusBase maxMp;
        private StatusBase mp;
        private StatusBase maxSpeed;
        private StatusBase speed;
        private StatusBase atk;
        private StatusBase def;

        public StatusBase MaxHP => maxHp;
        public StatusBase CurrentHP => hp;
        public StatusBase MaxMP => maxMp;
        public StatusBase CurrentMP => mp;
        public StatusBase MaxSpeed => maxSpeed;
        public StatusBase CurrentSpeed => speed;
        public StatusBase Atk => atk;
        public StatusBase Def => def;

        /// <summary>
        /// ステータストラッカーを初期化します。
        /// </summary>
        /// <param name="parent">このステータスが関連付けられているユニット。</param>
        /// <exception cref="ArgumentNullException">parent が null の場合。</exception>
        /// <exception cref="InvalidOperationException">unitData が null の場合。</exception>
        public StatusTracker(UnitBase parent)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));

            UnitData unitData = parent.unitData;
            if (unitData == null)
                throw new InvalidOperationException("UnitData cannot be null");

            maxHp = new StatusBase(unitData.HP);
            maxMp = new StatusBase(unitData.MP);
            maxSpeed = new StatusBase(unitData.Speed);
            atk = new StatusBase(unitData.Atk);
            def = new StatusBase(unitData.Def);
            hp = new StatusBase(unitData.HP);
            mp = new StatusBase(unitData.MP);
        }
    }
}
