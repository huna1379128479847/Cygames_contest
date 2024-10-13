using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    // ���j�b�g��\�������{�I�ȃC���^�[�t�F�[�X�B���j�b�g�̎�ނ�s�����`����B
    public interface IUnit
    {
        string Name { get; } // ���j�b�g�̖��O
        UnitType MyUnitType { get; } // ���j�b�g�̎��
        SkillHandler SkillHandler { get; } // �X�L�����Ǘ�����n���h���[
        void TurnBehavior(); // �^�[�����̍s�����`
    }

    // ���j�[�N�ȑ��݂�\���C���^�[�t�F�[�X�B�e�C���X�^���X�Ɉ�ӂ�ID����������B
    public interface IUniqueThing
    {
        Guid ID { get; } // ��ӂ�ID
    }

    // �v���C���[�I�������Ǘ����邽�߂̃C���^�[�t�F�[�X�B�I�����̐����ƑI��������S���B
    public interface IPlayerHandler
    {
        void GeneratePlayerOptions(); // �v���C���[�̑I�����𐶐�
        void HandlePlayerSelection(int selectionId); // �I�����ꂽ�I�v�V�����̏���
    }

    // �A�N�V���������s�����ǂ����������C���^�[�t�F�[�X�B�A�N�V�����̐i�s�󋵂��Ǘ�����B
    public interface IDoAction
    {
        bool InAction { get; } // �A�N�V���������ǂ���
    }

    // �G���j�b�g��\������C���^�[�t�F�[�X�B�s���p�^�[�����`�B
    public interface IEnemy
    {
        BehaviorPattern BehaviorPattern { get; } // �G�̍s���p�^�[��
    }

    // ���j�b�g�̃X�e�[�^�X��\������C���^�[�t�F�[�X�BHP��MP�Ȃǂ̃X�e�[�^�X�����B
    public interface IStatus
    {
        StatusBase MaxHP { get; } // �ő�HP
        StatusBase CurrentHP { get; } // ���݂�HP
        StatusBase MaxMP { get; } // �ő�MP
        StatusBase CurrentMP { get; } // ���݂�MP
        StatusBase MaxSpeed { get; } // �ő呬�x
        StatusBase CurrentSpeed { get; } // ���݂̑��x
        StatusBase Atk { get; } // �U����
        StatusBase Def { get; } // �h���
    }

    // �V�X�e����V�[���̓�����Ǘ����邽�߂̃C���^�[�t�F�[�X�B���s��Ԃ��Ǘ��B
    public interface IManager
    {
        bool IsRunning { get; set; } // �V�X�e�����ғ������ǂ���
    }

    // �V�[����؂�ւ��邽�߂̃C���^�[�t�F�[�X�B�V�[���̑J�ڂ��Ǘ��B
    public interface ISceneChanger
    {
        string FromSceneName { get; } // ���݂̃V�[����
        string ToSceneName { get; } // �J�ڐ�̃V�[����
        void Execute(string sceneName); // �V�[�������s���郁�\�b�h
    }

    // �f�[�^���i�[���A�擾�E�ݒ���s�����߂̃C���^�[�t�F�[�X�B�e�R���e�i�Ƃ̘A�g���\�B
    public interface IDataContainer
    {
        IDataContainer Parent { get; } // �e�R���e�i
        object GetData(); // �f�[�^���擾
        void SetData(object data); // �f�[�^��ݒ�
    }

    // ���ʁi�o�t��f�o�t�Ȃǁj���`����C���^�[�t�F�[�X�B���ʂ̓K�p��Ǘ����s���B
    public interface IEffect
    {
        string Name { get; } // ���ʂ̖��O
        int Duration { get; } // ���ʂ̎�������
        EffectTiming Timing { get; } // ���ʂ���������^�C�~���O
        EffectFlgs Flags { get; } // ���ʂ̃t���O�i����������j
        void Apply(); // ���ʂ�K�p
        void Remove(); // ���ʂ���菜��
        void UpdateEffect(); // �X�e�[�^�X�̉e�����X�V
        void DecreaseDuration(int time = 1); // ���ʂ̎c�莞�Ԃ����炷
        void ExecuteEffect(); // ���ʂ����s
    }

    // �o�g�����̃C�x���g���`����C���^�[�t�F�[�X�B����^�C�~���O�ł̏������Ǘ��B
    public interface IBattleEvent
    {
        int Priority { get; } // �C�x���g�̗D��x
        void Invoke(); // �C�x���g�𔭓�
    }

    /// <summary>
    /// �L�[�ƑΉ�����^��ێ����邽�߂̃C���^�[�t�F�[�X�B
    /// </summary>
    /// <typeparam name="T">�L�[�̌^�B�Q�ƌ^�łȂ���΂Ȃ�܂���B</typeparam>
    public interface IFactoryHolder<T> where T : class
    {
        /// <summary>
        /// �L�[�ƌ^�̑Ή���o�^���܂��B
        /// </summary>
        /// <param name="key">�L�[�ƂȂ�I�u�W�F�N�g�B</param>
        void RegisterFactory(T key);

        /// <summary>
        /// �w�肵���L�[�ɑΉ�����^���擾���܂��B
        /// </summary>
        /// <param name="key">�L�[�ƂȂ�I�u�W�F�N�g�B</param>
        /// <returns>�Ή�����^�B�L�[�����݂��Ȃ��ꍇ�� null�B</returns>
        IFactory GetFactoryForKey(T key);

        /// <summary>
        /// �w�肵���L�[�ɑΉ�����^����������폜���܂��B
        /// </summary>
        /// <param name="key">�폜����L�[�B</param>
        /// <returns>�폜�����������ꍇ�� true�A����ȊO�� false�B</returns>
        bool RemoveFactory(T key);

        /// <summary>
        /// �w�肵���L�[�������ɑ��݂��邩�ǂ������m�F���܂��B
        /// </summary>
        /// <param name="key">�m�F����L�[�B</param>
        /// <returns>�L�[�����݂���ꍇ�� true�A����ȊO�� false�B</returns>
        bool ContainsKey(T key);
    }

    public interface IFactoryHolders
    {
        Dictionary<Type, IFactoryHolder<IUseCutomClass>> FactoryHolders { get; }

        bool FinishedInit { get; }

        IFactory GetFactory(IUseCutomClass data);
    }

    /// <summary>
    /// �X�L����G�t�F�N�g������t�@�N�g���[�ł��邱�Ƃ��������߂̃C���^�[�t�F�[�X
    /// </summary>
    public interface IFactory
    {
        IUniqueThing CreateClass(DataBase data, IHandler parent);
    }

    public interface IHandler
    {
        UnitBase Parent { get; }
    }

    public interface IUseCutomClass
    {
        string ClassName { get; }
    }
}
