using System;

[Serializable]
public class Player
{
    public float score;
    public string name;

    public Player(float score, string name = "anonimo")
    {
        this.score = score;
        this.name = name;
    }
}
