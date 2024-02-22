// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Utilities;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Class for a terrain entity layer mask.
    /// </summary>
    public class TerrainEntityLayerMask
    {
        /// <summary>
        /// Heights for the layer mask.
        /// </summary>
        public float[,] heights;

        /// <summary>
        /// Terrain entity layer mask.
        /// </summary>
        /// <param name="heights">Heights for the layer mask.</param>
        public TerrainEntityLayerMask(float[][] heights)
        {
            if (heights == null || heights[0] == null)
            {
                Logging.LogWarning("[TerrainEntityLayerMask] Invalid heights array.");
                return;
            }

            float[,] list = new float[heights.Length,heights[0].Length];
            for (int i = 0; i < heights.Length; i++)
            {
                for (int j = 0; j < heights[0].Length; j++)
                {
                    list[i, j] = heights[i][j];
                }
            }
            this.heights = list;
        }
    }
}