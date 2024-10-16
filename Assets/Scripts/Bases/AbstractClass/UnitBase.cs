using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// ユニットの基本動作を定義する抽象クラス。
    /// 各ユニットは、このクラスを継承して独自の行動やスキル処理を実装します。
    /// </summary>
    [RequireComponent(typeof(SkillHandler))]
    [RequireComponent(typeof(EffectHandler))]
    public abstract class UnitBase : MonoBehaviour, IUnit, IUniqueThing, IDoAction
    {
        // インスペクターからアタッチするデータ
        [SerializeField] private UnitData _unitData; // ユニットのデータ (ステータスやスキル)
        [HideInInspector] public EffectHandler EffectHandler { get; private set; } // 効果管理 (バフ・デバフの処理)
        public StatusTracker StatusTracker { get; private set; } // ステータス管理 (HP、MPの管理)

        // ユニークID
        private Guid _id;

        // ユニットの種類
        private UnitType _myUnitType;

        // スキルハンドラー
        private SkillHandler _skillHandler;

        // アクション中かどうか
        private bool _inAction;

        // 現在選択されているスキル
        private Skill _selectedSkill;

        // ターン中かどうか
        private bool _isMyTurn;

        // 初回ターン実行かどうか
        private bool _isFirstExecute = true;

        // アクション完了を通知するイベント
        public event Action OnActionComplete;

        // デリゲート
        public delegate void PreApplyDamageHandler(DamageInfo damageInfo);
        public delegate void PostApplyDamageHandler(DamageInfo damageInfo);
        public delegate void AttackHandler(List<DamageInfo> damageInfoList);

        // イベント
        public event PreApplyDamageHandler PreApplyDamageEvent;
        public event PostApplyDamageHandler PostApplyDamageEvent;
        public event AttackHandler AttackEvent;

        /// <summary>
        /// ユニークなIDを返すプロパティ。
        /// </summary>
        public Guid ID => _id;

        /// <summary>
        /// ユニットが死亡しているかどうかを返すプロパティ。
        /// HPまたはMPが0なら死亡扱い。
        /// </summary>
        public virtual bool IsDead
        {
            get
            {
                return StatusTracker.CurrentHP.IsDead || StatusTracker.CurrentMP.IsDead;
            }
        }

        /// <summary>
        /// アクション中かどうかを返すプロパティ。
        /// </summary>
        public bool InAction => _inAction;

        /// <summary>
        /// ユニットの種類 (味方、敵など) を返すプロパティ。
        /// </summary>
        public virtual UnitType MyUnitType => _myUnitType;

        /// <summary>
        /// スキル管理を返すプロパティ。
        /// </summary>
        public SkillHandler SkillHandler => _skillHandler;

        /// <summary>
        /// ユニットの名前を返すプロパティ。
        /// </summary>
        public virtual string Name => _unitData.Name;

        /// <summary>
        /// ユニットが行動可能かどうかを返すプロパティ。
        /// 死亡しているか、コントロールされている場合は行動不可。
        /// </summary>
        public virtual bool CanAction
        {
            get
            {
                if (IsDead) return false;
                return !FLG.FLGCheck((uint)EffectHandler.HaveFlgs, (uint)EffectFlgs.Controll);
            }
        }

        public UnitData UnitData => _unitData;
        /// <summary>
        /// アクション完了を通知するメソッド。
        /// </summary>
        public void NotifyActionComplete()
        {
            _inAction = false;
            OnActionComplete?.Invoke();
            _skillHandler.currentSkill = null;
        }

        /// <summary>
        /// ユニットの初期化処理。
        /// </summary>
        public virtual void InitUnit()
        {
            _id = Guid.NewGuid();
            StatusTracker = new StatusTracker(this);
            StatusTracker.CurrentHP.OnDeadEvent += DeadBehavior;

            EffectHandler = GetComponent<EffectHandler>();
            if (EffectHandler == null)
            {
                Debug.LogError("EffectHandlerコンポーネントが見つかりません。");
                return;
            }
            EffectHandler.Initialize(this);

            _skillHandler = GetComponent<SkillHandler>();
            if (_skillHandler == null)
            {
                Debug.LogError("SkillHandlerコンポーネントが見つかりません。");
                return;
            }
            _skillHandler.InitSkillList(_unitData.SkillDatas);

            _myUnitType = _unitData.UnitType;
        }

        /// <summary>
        /// ユニットのターンを開始する処理。
        /// コルーチンを開始して行動を管理します。
        /// </summary>
        public void EnterTurn()
        {
            _isMyTurn = true;
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }
            StartCoroutine(TurnCoroutine());
        }

        /// <summary>
        /// ユニットのターンを終了する処理。
        /// </summary>
        public void ExitTurn()
        {
            _isMyTurn = false;
        }

        /// <summary>
        /// ターン中の行動を管理するコルーチン。
        /// </summary>
        /// <returns>コルーチンの列挙子。</returns>
        private IEnumerator TurnCoroutine()
        {
            Debug.Log($"{_unitData.Name}のターン開始");

            if (!CanAction)
            {
                ExitTurn();
                NotifyActionComplete();
                yield break;
            }

            if (_isFirstExecute)
            {
                EffectHandler.ExecuteEffects(EffectTiming.After);
                _isFirstExecute = false;
            }

            yield return StartCoroutine(TurnBehavior());

            // アクションが完了するまで待機
            _inAction = true;
            yield return new WaitUntil(() => !_inAction);
            AttackEvent?.Invoke(_selectedSkill?.currentResult ?? new List<DamageInfo>());

            // 行動が完了したらターンを終了
            ExitTurn();
            NotifyActionComplete();
        }

        /// <summary>
        /// ユニットが死亡した際の処理。
        /// </summary>
        public virtual void DeadBehavior()
        {
            if (BattleSceneManager.instance != null)
            {
                BattleSceneManager.instance.RemoveUnit(ID);
            }
            // 必要に応じて死亡アニメーションやUIの更新を行います。
            gameObject.SetActive(false);
        }

        /// <summary>
        /// ユニットのターン中の行動を定義する抽象メソッド。
        /// 必ず「TurnChange()」を呼び出してターンを終了させること。
        /// </summary>
        /// <returns>コルーチンの列挙子。</returns>
        public abstract IEnumerator TurnBehavior();

        /// <summary>
        /// ユニットが死亡したことを通知する。
        /// </summary>
        public virtual void NotifyDead()
        {
            DeadBehavior();
        }

        /// <summary>
        /// ダメージを受ける前の処理を定義する。
        /// </summary>
        public virtual void PreTakeDamage()
        {
            // 必要に応じて実装
        }

        /// <summary>
        /// ダメージを受けた後の処理を定義する。
        /// </summary>
        public virtual void PostTakeDamage()
        {
            // 必要に応じて実装
            Debug.Log($"最大HP：{StatusTracker.MaxHP.CurrentAmount} / 残りHP：{StatusTracker.CurrentHP.CurrentAmount}");
        }

        /// <summary>
        /// ユニットがダメージを受ける処理。
        /// </summary>
        /// <param name="info">ダメージ情報。</param>
        /// <param name="isFixedDamage">固定ダメージかどうか。</param>
        public virtual void TakeDamage(DamageInfo info, bool isFixedDamage = false)
        {
            if (info == null)
            {
                Debug.LogError("DamageInfoがnullです。");
                return;
            }
            Debug.Log("与えたダメージ（回復量）："+info.amount + " 対象：" + info.damageTaker.Name);
            PreTakeDamage();
            PreApplyDamageEvent?.Invoke(info);

            // ダメージ計算やステータスの更新をここに実装
            StatusTracker.CurrentHP.CurrentAmount += info.amount;
            if (IsDead)
            {
                DeadBehavior();
            }

            PostTakeDamage();
            PostApplyDamageEvent?.Invoke(info);
        }

        /// <summary>
        /// ユニットが使用するスキルを選択する処理。
        /// ランダムにスキルを選びます。
        /// </summary>
        /// <returns>選択されたスキル。</returns>
        public virtual Skill ChooseSkill()
        {
            return Helpers.RandomPick(_skillHandler.skills);
        }

        /// <summary>
        /// ユニットが持つスキルデータのリストを取得します。
        /// </summary>
        /// <returns>スキルデータのリスト。</returns>
        public List<SkillData> GetSkillDatas()
        {
            return _unitData.SkillDatas;
        }

        /// <summary>
        /// オブジェクトが破棄される際に呼び出されます。
        /// イベントハンドラーの解除を行います。
        /// </summary>
        private void OnDestroy()
        {
            StatusTracker.CurrentHP.OnDeadEvent -= DeadBehavior;
            PreApplyDamageEvent = null;
            PostApplyDamageEvent = null;
            AttackEvent = null;
            OnActionComplete = null;
        }
    }
}
