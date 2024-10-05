using System;

namespace Contest
{
    /// <summary>
    /// ユニットのステータスを管理するクラス。
    /// HPやMP、スピード、攻撃力、防御力などを追跡する。
    /// </summary>
    public class StatusTracker : IStatus
    {
        // ステータスを持つユニット
        public UnitBase Parent { get; private set; }

        // 各ステータスを管理するフィールド
        private StatusBase maxHp;
        private StatusBase hp;
        private StatusBase maxMp;
        private StatusBase mp;
        private StatusBase maxSpeed;
        private StatusBase speed;
        private StatusBase atk;
        private StatusBase def;

        // プロパティを通じて各ステータスを公開
        public StatusBase MaxHP => maxHp;               // 最大HP
        public StatusBase CurrentHP => hp;              // 現在のHP
        public StatusBase MaxMP => maxMp;               // 最大MP
        public StatusBase CurrentMP => mp;              // 現在のMP
        public StatusBase MaxSpeed => maxSpeed;         // 最大スピード
        public StatusBase CurrentSpeed => speed;        // 現在のスピード
        public StatusBase Atk => atk;                   // 攻撃力
        public StatusBase Def => def;                   // 防御力

        /// <summary>
        /// ステータストラッカーを初期化します。
        /// </summary>
        /// <param name="parent">このステータスが関連付けられているユニット。</param>
        /// <exception cref="ArgumentNullException">parent が null の場合。</exception>
        /// <exception cref="InvalidOperationException">unitData が null の場合。</exception>
        public StatusTracker(UnitBase parent)
        {
            // parent が null の場合、例外をスロー
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));

            // ユニットデータが存在しない場合、例外をスロー
            UnitData unitData = parent.unitData;
            if (unitData == null)
                throw new InvalidOperationException("UnitData cannot be null");

            // 各ステータスをUnitDataに基づいて初期化
            maxHp = new StatusBase(unitData.HP);
            maxMp = new StatusBase(unitData.MP);
            maxSpeed = new StatusBase(unitData.Speed);
            atk = new StatusBase(unitData.Atk);
            def = new StatusBase(unitData.Def);
            hp = new StatusBase(unitData.HP, true);
            mp = new StatusBase(unitData.MP, true);
        }
    }
}
