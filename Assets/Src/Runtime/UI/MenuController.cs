using Hypergame.Entities.PlayerEntity;
using TMPro;
using UnityEngine;

namespace Hypergame.UI
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField, HideInInspector] private ColorOptionBnt[] colorOptions;

        [SerializeField] private GameObject gameplayMenu;
        [SerializeField] private GameObject shopMenu;
        [SerializeField] private TextMeshProUGUI stackLimit;
        [SerializeField] private Transform colorParent;
        [SerializeField] private Player player;
        [SerializeField] private float stackCostIncrease = 1.5f;

        private ColorOptionBnt selected;

        private int StackIncreaseCost => (int)(player.StackLimit * stackCostIncrease);

        private void Start()
        {
            colorOptions = GetComponentsInChildren<ColorOptionBnt>();

            int i = 0;
            foreach(var bnt in colorOptions)
            {
                bnt.controller = this;
                bnt.player = player;
                bnt.id = i;
                bnt.LoadBntInfo();
                bnt.ToggleBnt(false);
                i++;
            }

            selected = colorOptions[player.ActiveColorId];
            selected.ToggleBnt(true);

            player.UpdateColor(selected.OptionColor, player.ActiveColorId, 0);

            CloseShop();
        }

        public void OpenShop()
        {
            gameplayMenu.SetActive(false);
            shopMenu.SetActive(true);

            UpdateStackLimitLabel();
            foreach (var bnt in colorOptions)
                bnt.UpdateVisual();
        }

        public void CloseShop()
        {
            gameplayMenu.SetActive(true);
            shopMenu.SetActive(false);
        }

        public void UpdateSelected(ColorOptionBnt bnt)
        {
            selected.ToggleBnt(false);
            selected = bnt;

            foreach (var opt in colorOptions)
                opt.UpdateVisual();
        }

        public void AddStackLimit()
        {
            if (player.Points < StackIncreaseCost)
                return;

            player.IncreaseStackLimit(StackIncreaseCost);
            UpdateStackLimitLabel();
        }

        private string UpdateStackLimitLabel() => stackLimit.text = $"Stack Limit: {player.StackLimit}\nCost: {StackIncreaseCost}";
    }
}