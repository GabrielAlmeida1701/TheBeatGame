using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Hypergame.Entities.PlayerEntity.Input
{
    public class InputManager : MonoBehaviour
    {
        public PlayerInput input;
        public InputActionAsset inputActions;

        public UnityEvent<Vector2> onMoviment;

        #region Globa references
        private static InputManager _instance;
        public static InputManager Actions
        {
            get
            {
                if (!_instance) _instance = FindObjectOfType<InputManager>();
                return _instance;
            }
        }

        public static void SwitchToUI() => Actions.SwitchActionMap("ui");

        public static void SwitchToGameplay() => Actions.SwitchActionMap("gameplay");

        private void SwitchActionMap(string map) => input.currentActionMap = inputActions.FindActionMap(map);
        #endregion

        public void OnMoviment(InputValue value)
        {
            //Update UI
            onMoviment?.Invoke(value.Get<Vector2>());
        }
    }
}
