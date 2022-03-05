using UnityEngine.Serialization;

namespace entities
{
    [System.Serializable]
    public class ColorEntity
    {
        public float r=0f;
        public float g=0f;
        public float b=0f;
        public float alpha=0f;

        public ColorEntity(float r, float g, float b) {
            this.r=r;
            this.g=g;
            this.b=b;
            alpha=1f;
        }

        public override string ToString() { 
            return $"(r:{r},g:{g},b:{b},a:{alpha})";
        }
    }
}