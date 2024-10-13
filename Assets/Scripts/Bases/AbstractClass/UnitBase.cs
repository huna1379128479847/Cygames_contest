using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// ユニットの基本動作を定義する抽象クラス。
    /// 各ユニットは、このクラスを継承して独自の行動やスキル処理を実装する。
    /// </summary>
    [RequireComponent(typeof(SkillHandler))]
    [RequireComponent(typeof(EffectHandler))]
    public abstract class UnitBase : MonoBehaviour, IUnit, IUniqueThing, IDoAction
    {
        // インスペクターからアタッチするデータ
        public UnitData unitData; // ユニットのデータ (ステータスやスキル)
        public EffectHandler effectHandler; // 効果管理 (バフ・デバフの処理)
        public StatusTracker statusTracker; // ステータス管理 (HP、MPの管理)

        // ユニークID
        private Guid id;

        // ユニットの名前
        [SerializeField]
        private string _name;

        // ユニットの種類
        private UnitType myUnitType;

        // スキルハンドラー
        private SkillHandler skillHandler;

        // アクション中かどうか
        private bool inAction;

        // 現在選択されているスキル
        private Skill selectedSkill;

        // ターン中かどうか
        private bool myTurn;

        // 初回ターン実行かどうか
        private bool firstExecute = true;

        // アクション完了を通知するイベント
        public event Action OnActionComplete;

        // デリゲート
        public delegate void PreApplyDamage(DamageInfo damageInfo);
        public delegate void PostApplyDamage(DamageInfo damageInfo);
        public delegate void Attack(List<DamageInfo> damageInfo);

        // イベント
        public event PreApplyDamage PreApplyDamageEvent;
        public event PostApplyDamage PostApplyDamageEvent;
        public event Attack AttackEvent;

        /// <summary>
        /// ユニークなIDを返すプロパティ。
        /// </summary>
        public Guid ID => id;

        /// <summary>
        /// ユニットが死亡しているかどうかを返すプロパティ。
        /// HPまたはMPが0なら死亡扱い。
        /// </summary>
        public virtual bool IsDead
        {
            get
            {
                return statusTracker.CurrentHP.IsDead || statusTracker.CurrentMP.IsDead;
            }
        }

        /// <summary>
        /// アクション中かどうかを返すプロパティ。
        /// </summary>
        public bool InAction => inAction;

        /// <summary>
        /// ユニットの種類 (味方、敵など) を返すプロパティ。
        /// </summary>
        public virtual UnitType MyUnitType
        {
            get
            {
                return myUnitType;
            }
        }

        /// <summary>
        /// スキル管理を返すプロパティ。
        /// </summary>
        public SkillHandler SkillHandler
        {
            get
            {
                return skillHandler;
            }
        }

        /// <summary>
        /// ユニットの名前を返すプロパティ。
        /// </summary>
        public virtual string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// ユニットが行動可能かどうかを返すプロパティ。
        /// 死亡しているか、コントロールされている場合は行動不可。
        /// </summary>
        public virtual bool CanAction
        {
            get
            {
                if (IsDead) return false;
                return !FLG.FLGCheck((uint)effectHandler.HaveFlgs, (uint)EffectFlgs.Controll);
            }
        }

        /// <summary>
        /// アクション完了を通知するメソッド。
        /// </summary>
        public void Notify_ActionComplete()
        {
            inAction = false;
            OnActionComplete?.Invoke();
        }

        /// <summary>
        /// ユニットの初期化処理。
        /// </summary>
        protected virtual void Awake()
        {
            id = Guid.NewGuid();
            statusTracker = new StatusTracker(this);
            statusTracker.CurrentHP.OnDeadEvent += DeadBehavior;
            effectHandler = GetComponent<EffectHandler>();
            if (effectHandler == null)
            {
                Debug.LogError("EffectHandler component is missing.");
            }
            effectHandler.Initialize(this);
            skillHandler = GetComponent<SkillHandler>();
            if (skillHandler == null)
            {
                Debug.LogError("SkillHandler component is missing.");
            }
            skillHandler.InitSkillList(unitData.SkillDatas);
            myUnitType = unitData.UnitType;
        }

        /// <summary>
        /// ユニットのターンを開始する処理。
        /// コルーチンを開始して行動を管理する。
        /// </summary>
        public void EnterTurn()
        {
            myTurn = true;
            StartCoroutine(TurnCoroutine());
        }

        /// <summary>
        /// ユニットのターンを終了する処理。
        /// </summary>
        public void ExitTurn()
        {
            myTurn = false;
        }

        /// <summary>
        /// ターン中の行動を管理するコルーチン。
        /// </summary>
        private IEnumerator TurnCoroutine()
        {
            if (!CanAction)
            {
                ExitTurn();
                Notify_ActionComplete();
                yield break;
            }

            if (firstExecute)
            {
                effectHandler.ExecuteEffects(EffectTiming.After);
                firstExecute = false;
            }

            // スキルを選択
            selectedSkill = Choice();
            if (selectedSkill == null)
            {
                // スキルが選択できない場合はターンを終了
                TurnBehavior(); // 具体的な行動は派生クラスで実装
                Notify_ActionComplete();
                yield break;
            }

            // スキルを実行するためにSkillSelectManagerを通じてターゲットを選択
            SkillSelectManager.instance.Init(selectedSkill);

            // アクションが完了するまで待機
            inAction = true;
            yield return new WaitUntil(() => !inAction);
            AttackEvent?.Invoke(selectedSkill.currentResult);

            // 行動が完了したらターンを終了
            ExitTurn();
            Notify_ActionComplete();
        }

        /// <summary>
        /// ユニットが死亡した際の処理。
        /// </summary>
        public virtual void DeadBehavior()
        {
            BattleSceneManager.instance.RemoveUnit(ID);
            // 必要に応じて死亡アニメーションやUIの更新を行う
        }

        /// <summary>
        /// ユニットのターン中の行動を定義する抽象メソッド。
        /// 必ず「TurnChange()」を呼び出してターンを終了させること。
        /// </summary>
        public abstract void TurnBehavior();

        /// <summary>
        /// ユニットが死亡したことを通知する。
        /// </summary>
        public virtual void Notify_Dead()
        {
            // 必要に応じて実装
        }

        /// <summary>
        /// ダメージを受ける前の処理を定義する。
        /// </summary>
        public virtual void Pre_TakeDamage()
        {
            // 必要に応じて実装
        }

        /// <summary>
        /// ダメージを受けた後の処理を定義する。
        /// </summary>
        public virtual void Post_TakeDamage()
        {
            // 必要に応じて実装
        }

        /// <summary>
        /// ユニットがダメージを受ける処理。
        /// </summary>
        /// <param name="info">ダメージ情報。</param>
        /// <param name="isFix">固定ダメージかどうか。</param>
        public virtual void TakeDamage(DamageInfo info)
        {
            Pre_TakeDamage();
            PreApplyDamageEvent?.Invoke(info);
            

            if (IsDead)
            {
                DeadBehavior();
            }
            Post_TakeDamage();
            PostApplyDamageEvent?.Invoke(info);
        }

        /// <summary>
        /// ユニットが使用するスキルを選択する処理。
        /// ランダムにスキルを選ぶ。
        /// </summary>
        /// <returns>選択されたスキル。</returns>
        public virtual Skill Choice()
        {
            return Helpers.RandomPick(skillHandler.skills);
        }

        /// <summary>
        /// ユニットが持つスキルデータのリストを取得する。
        /// </summary>
        /// <returns>スキルデータのリスト。</returns>
        public List<SkillData> GetSkillDatas()
        {
            return unitData.SkillDatas;
        }
    }
}
