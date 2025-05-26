using System;

[Serializable]
public class LeaderboardEntry : IComparable<LeaderboardEntry>
{
    public string userName;
    public int score;
    public LeaderboardEntry(string name, int score)
    {
        userName = name;
        this.score = score;
    }

    public int CompareTo(LeaderboardEntry comparePart)
    {
          // A null value means that this object is greater.
        if (comparePart == null)
            return 1;

        else
            return comparePart.score.CompareTo(this.score);
    }
}
