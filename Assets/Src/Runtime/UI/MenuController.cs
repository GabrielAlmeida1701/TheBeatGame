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

        private ColorOptionBnt selected;

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
            int cost = player.StackLimit * 2;
            if (player.Points < cost)
                return;

            player.IncreaseStackLimit(cost);
            UpdateStackLimitLabel();
        }

        private string UpdateStackLimitLabel() => stackLimit.text = $"Stack Limit: {player.StackLimit}\nCost: {player.StackLimit * 2}";
    }
}