using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Contest
{
    public class BattleSceneManager : SingletonBehavior<BattleSceneManager>, IManager
    {
        // デバッグ用 フィールド
        private int turnCount = 0;

        // フィールド
        [SerializeField] private Transform _canvas;
        [SerializeField] private Transform _playerFrame;
        [SerializeField] private Transform _enemiesFrame;

        private Turn _currentTurn = Turn.Friend;
        private bool _isRunning = false;
        private IFactoryHolders _factoryHolders;

        private readonly List<GameObject> _units = new List<GameObject>();
        private readonly List<UnitBase> _unitBases = new List<UnitBase>();
        private List<InBattleEvent> _battleEvent = new List<InBattleEvent>();

        private int _friendCount = 0;
        private int _enemyCount = 0;

        // イベント
        public delegate void OnDamage(DamageInfo info);
        public event OnDamage OnDamageEvent;

        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    // 必要に応じてイベントを追加
                }
            }
        }

        public bool InAction => _unitBases.Count > 0 && _unitBases[0].InAction;
        public bool InSelecting => SkillSelectManager.instance.InSelecting;
        public Turn CurrentTurn => _currentTurn;
        public IReadOnlyList<UnitBase> AllUnits => _unitBases.AsReadOnly();
        public int FriendCount => _friendCount;
        public int EnemyCount => _enemyCount;

        public IFactoryHolders FactoryHolders => _factoryHolders;

        /// <summary>
        /// バトルの実行を開始します。
        /// </summary>
        /// <param name="datas">バトルに参加するユニットのGameObjectリスト</param>
        /// <param name="battleEvent">バトルイベント</param>
        /// <param name="factoryHolders">ファクトリーホルダー</param>
        public void Execute(List<GameObject> datas, List<InBattleEvent> battleEvent, IFactoryHolders factoryHolders = null)
        {
            _factoryHolders = factoryHolders ?? new InBattleFactoryHolder();
            _battleEvent = battleEvent;
            InitializeBattle(datas);
        }

        /// <summary>
        /// バトルの初期化を行う。
        /// </summary>
        /// <param name="datas">バトルに参加するユニットのGameObjectリスト</param>
        private void InitializeBattle(List<GameObject> datas)
        {
            turnCount = 1;
            Notify_StartInitialize();

            foreach (var obj in datas)
            {
                var instantiatedObj = Instantiate(obj, Vector3.zero, Quaternion.identity, _canvas);
                if (instantiatedObj.TryGetComponent<UnitBase>(out var unitBase))
                {
                    _units.Add(instantiatedObj);
                    _unitBases.Add(unitBase);
                    ChangeUnitCount(unitBase);
                }
                else
                {
                    Destroy(instantiatedObj);
                    Debug.LogError("UnitBaseがアタッチされていないGameObjectが存在します");
                }
            }

            _factoryHolders.SetData(_unitBases);
            foreach (var unit in _unitBases)
            {
                unit.InitUnit(this);
            }

            Notify_FinishInitialize();

            // ターン管理を開始
            if (!IsRunning)
            {
                StartCoroutine(TurnFlow());
            }
        }

        /// <summary>
        /// ターンフローを管理するコルーチン。
        /// </summary>
        /// <returns></returns>
        private IEnumerator TurnFlow()
        {
            IsRunning = true;

            while (_friendCount > 0 && _enemyCount > 0)
            {
                // 各ユニットの行動を順番に処理
                for (int i = 0; i < _unitBases.Count; i++)
                {
                    var unit = _unitBases[i];
                    if (unit == null || !FLG.FLGCheckHaving((uint)unit.MyUnitType, (uint)_currentTurn))
                    {
                        continue;
                    }

                    yield return StartCoroutine(HandleUnitAction(unit));

                    // バトル終了の可能性をチェック
                    if (_friendCount <= 0 || _enemyCount <= 0)
                    {
                        break;
                    }
                }

                // ターン終了後にターンを切り替え
                ChangeTurn();
                yield return null; // 次のターンへ移行する前にフレームを分ける
            }

            IsRunning = false;
            EndBattle();
        }

        /// <summary>
        /// ユニットのアクションを処理するコルーチン。
        /// </summary>
        /// <param name="unit">処理するユニット</param>
        /// <returns></returns>
        private IEnumerator HandleUnitAction(UnitBase unit)
        {
            if (unit == null)
            {
                yield break;
            }

            bool actionCompleted = false;

            void OnActionCompleteHandler()
            {
                actionCompleted = true;
            }

            unit.OnActionComplete += OnActionCompleteHandler;

            if (!unit.gameObject.activeInHierarchy)
            {
                unit.gameObject.SetActive(true);
            }

            unit.EnterTurn();

            while (!actionCompleted)
            {
                yield return null;
            }

            unit.OnActionComplete -= OnActionCompleteHandler;
            unit.ExitTurn();
        }

        /// <summary>
        /// ターンを切り替えます（友軍 ⇄ 敵軍）。
        /// </summary>
        private void ChangeTurn()
        {
            _currentTurn ^= Turn.All; // 味方と敵のターンを切り替え
            Debug.Log($"{turnCount}, Turn changed to: {_currentTurn}");
            turnCount++;
        }

        /// <summary>
        /// バトル終了時の処理を行います。
        /// </summary>
        private void EndBattle()
        {
            if (_friendCount > 0 && _enemyCount <= 0)
            {
                Debug.Log("Friends Win!");
            }
            else if (_enemyCount > 0 && _friendCount <= 0)
            {
                Debug.Log("Enemies Win!");
            }
            else
            {
                Debug.Log("Battle Ended");
            }
            Debug.Log($"\nfriend: {_friendCount}\nenemy: {_enemyCount}");

            // 必要に応じてバトル終了後の処理を追加
            // 例: 結果画面の表示、リトライオプションの提供など
        }

        /// <summary>
        /// ユニットのカウントを変更します。
        /// </summary>
        /// <param name="unitBase">カウントを変更するユニット</param>
        /// <param name="count">変更する数（デフォルトは1）</param>
        private void ChangeUnitCount(UnitBase unitBase, int count = 1)
        {
            if (FLG.FLGCheckHaving((uint)unitBase.UnitData.UnitType, (uint)(UnitType.Enemy | UnitType.EnemyAI)))
            {
                _enemyCount += count;
                unitBase.transform.SetParent(_enemiesFrame);
            }
            else if (FLG.FLGCheckHaving((uint)unitBase.UnitData.UnitType, (uint)(UnitType.Friend | UnitType.FriendAI)))
            {
                _friendCount += count;
                unitBase.transform.SetParent(_playerFrame);
                unitBase.transform.position = _playerFrame.position;
            }
            else
            {
                Debug.LogWarning($"Unknown UnitType: {unitBase.UnitData.UnitType}");
            }
        }

        /// <summary>
        /// バトルが終了しているかをチェックし、必要な処理を行います。
        /// </summary>
        private void CheckBattleEnd()
        {
            if (_friendCount <= 0 || _enemyCount <= 0)
            {
                IsRunning = false;
                StopAllCoroutines();
                EndBattle();
            }
        }

        /// <summary>
        /// ユニットのアクションが終了した際に呼び出されます。
        /// </summary>
        public void Notify_EndUnitAction()
        {
            // 将来的な拡張のために保持
        }

        public void ApplyDamageInfo(DamageInfo info)
        {
            OnDamageEvent?.Invoke(info);
            info.damageTaker.TakeDamage(info);
        }
        /// <summary>
        /// バトルの初期化が開始された際に呼び出されます。
        /// </summary>
        protected virtual void Notify_StartInitialize()
        {
            Debug.Log("Battle Initialization Started.");
            // 初期化開始時の処理をここに追加
        }

        /// <summary>
        /// バトルの初期化が完了した際に呼び出されます。
        /// </summary>
        protected virtual void Notify_FinishInitialize()
        {
            Debug.Log("Battle Initialization Finished.");
            // 初期化完了時の処理をここに追加
        }

        /// <summary>
        /// ユニットをバトルから削除します。
        /// </summary>
        /// <param name="id">削除するユニットのID</param>
        public void RemoveUnit(Guid id)
        {
            int index = _unitBases.FindIndex(unit => unit.ID == id);
            if (index != -1)
            {
                var unit = _unitBases[index];
                ChangeUnitCount(unit, -1);

                Destroy(_units[index]);
                _units.RemoveAt(index);
                _unitBases.RemoveAt(index);

                Debug.Log($"Unit {id} removed from battle.");

                CheckBattleEnd();
            }
            else
            {
                Debug.LogWarning($"Unit with ID {id} not found.");
            }
        }
    }
}
