using System;
using System.Collections.Generic;

namespace Hypergame.Entities.PlayerEntity
{
    [Serializable]
    public struct PlayerData
    {
        public int points;
        public int stackLimit;
        public int activeColor;
        public List<int> colors;

        public PlayerData Clone()
        {
            var data = new PlayerData() {
                points = points,
                stackLimit = stackLimit,
                activeColor = activeColor,
                colors = new List<int>()
            };

            foreach (int color in colors)
                data.colors.Add(color);

            return data;
        }
    }
}
