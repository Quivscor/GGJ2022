public class SpellcastEventData
{
    public SpellType type;

    public SpellcastEventData()
    {
        type = SpellType.Unknown;
    }

    public SpellcastEventData(SpellType type)
    {
        this.type = type;
    }
}