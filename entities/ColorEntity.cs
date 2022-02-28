[System.Serializable]
public class ColorEntity
{
    public float red=0f;
    public float green=0f;
    public float blue=0f;
    public float alpha=0f;

    public ColorEntity(float Red, float Green, float Blue) {
        red=Red;
        green=Green;
        blue=Blue;
        alpha=1f;
    }
}