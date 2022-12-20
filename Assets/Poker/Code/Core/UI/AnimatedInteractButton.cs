using UnityEngine;
using UnityEngine.UI;

namespace Poker.Code.Core.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class AnimatedInteractButton : Button
    {
        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        private readonly int _switchHash = Animator.StringToHash("Switch");
        
        public void SetInteractable(bool interactable)
        {
            _animator.SetTrigger(_switchHash);
            this.interactable = interactable;
        }
    }
}