using UnityEngine;
using UnityEngine.Events;

namespace Hypergame.Entities.PlayerEntity
{
    public class CharacterAnimationEventReciver : MonoBehaviour
    {
        public UnityEvent onAttackHit;

        public void OnAttackHit() => onAttackHit?.Invoke();
    }
}