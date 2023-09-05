using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Physical properties for an entity.
    /// </summary>
    public struct EntityPhysicalProperties
    {
        /// <summary>
        /// Angular drag of the entity.
        /// </summary>
        public float angularDrag;

        /// <summary>
        /// Center of mass of the entity.
        /// </summary>
        public Vector3 centerOfMass;

        /// <summary>
        /// Drag of the entity.
        /// </summary>
        public float drag;

        /// <summary>
        /// Whether or not the entity is gravitational.
        /// </summary>
        public bool gravitational;

        /// <summary>
        /// Mass of the entity.
        /// </summary>
        public float mass;
    }
}