using Hypergame.Entities.PlayerEntity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hypergame.UI
{
    public class ColorOptionBnt : MonoBehaviour
    {
        [HideInInspector] public Player player;
        [HideInInspector] public MenuController controller;
        [HideInInspector] public int id;

        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Image colorImg;
        [SerializeField] private GameObject selectedOutline;

        [SerializeField, HideInInspector] private int cost;
        [SerializeField, HideInInspector] private Color color;

        public int Cost => cost;
        public Color OptionColor => color;

        public void LoadBntInfo()
        {
            cost = int.Parse(label.text);
            color = colorImg.color;

            if (player.IsColorUnlocked(id))
            {
                cost = 0;
                label.text = "0";
            }
        }

        public void ToggleBnt(bool active) => selectedOutline.SetActive(active);

        public void ApplyColor()
        {
            if (player.Points < cost)
                return;

            ToggleBnt(true);
            player.UpdateColor(color, id, cost);
            controller.UpdateSelected(this);

            cost = 0;
            label.text = "0";
        }

        public void UpdateVisual()
        {
            color.a = cost >= player.Points ? 0.6f : 1;
            colorImg.color = color;
        }
    }
}