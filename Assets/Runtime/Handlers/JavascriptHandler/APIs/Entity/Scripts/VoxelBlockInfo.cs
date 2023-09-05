using System.Collections.Generic;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Class for a voxel block definition.
    /// </summary>
    public class VoxelBlockInfo
    {
        public int id;

        public Dictionary<int, VoxelBlockSubType> subTypes = new Dictionary<int, VoxelBlockSubType>();

        public VoxelBlockInfo(int id)
        {
            this.id = id;
        }

        public void AddSubType(int id, bool invisible, string topTexture, string bottomTexture,
            string leftTexture, string rightTexture, string frontTexture, string backTexture)
        {
            subTypes[id] = new VoxelBlockSubType()
            {
                id = id,
                invisible = invisible,
                topTex = topTexture,
                bottomTex = bottomTexture,
                leftTex = leftTexture,
                rightTex = rightTexture,
                frontTex = frontTexture,
                backTex = backTexture
            };
        }
    }
}