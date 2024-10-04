using System;
using UnityEngine;

namespace Contest
{
    public static class AnimationHandler
    {
        public static void InvokeAnimation(GameObject obj, AnimationType type)
        {
            if (obj == null || !obj.TryGetComponent<Animator>(out Animator animator)) return;
            Invoke(animator, type.ToString());
        }

        private static void Invoke(Animator animator, string type)
        {
            try
            {
                animator.SetBool(type, true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Invoke: オブジェクト '{animator.gameObject.name}' でアニメーション '{type}' のセット中にエラーが発生しました。詳細: {ex.Message}");
            }
        }
    }
}
