namespace TDD.Task;
public class Game
{
    private int FramesInGame { get; set; } = 10;
    
    private int FramesLeft { get; set; } = 10;

    private List<(int FirstRoll, int SecondRoll)> Rolls { get; set; } =
    [
        (FirstRoll: -1, SecondRoll: -1),
        (FirstRoll: -1, SecondRoll: -1),
        (FirstRoll: -1, SecondRoll: -1),
        (FirstRoll: -1, SecondRoll: -1),
        (FirstRoll: -1, SecondRoll: -1),
        (FirstRoll: -1, SecondRoll: -1),
        (FirstRoll: -1, SecondRoll: -1),
        (FirstRoll: -1, SecondRoll: -1),
        (FirstRoll: -1, SecondRoll: -1),
        (FirstRoll: -1, SecondRoll: -1),
        (FirstRoll: -1, SecondRoll: -1)
    ];

    private void IncreaseFramesInGameAfterStrikeOrSpare()
    {
        FramesInGame += 1;
        FramesLeft += 1;
    }

    public void Roll(int pins)
    {
        var index = FramesInGame - FramesLeft;
        if (index > FramesInGame - 1)
        {
            throw new Exception("Game is over!");
        }
        
        var currentRoll = Rolls[index];

        if (currentRoll.FirstRoll == -1)
        {
            currentRoll.FirstRoll = pins;
            if (pins == 10)
            {
                currentRoll.SecondRoll = 0;
                FramesLeft--;
            }
        }
        else
        {
            currentRoll.SecondRoll = pins;
            FramesLeft--;
        }
        
        Rolls[index] = currentRoll;

        if (index == FramesInGame - 1 
            && currentRoll.FirstRoll + currentRoll.SecondRoll == 10
            && FramesInGame == 10)
        {
            IncreaseFramesInGameAfterStrikeOrSpare();
        }
    }

    public int GetScore()
    {
        var score = 0;
        var isSpare = false;
        var isStrike = false;
        for (var i = 0; i < FramesInGame - FramesLeft; i++)
        {
            if (isStrike)
            {
                score += Rolls[i].FirstRoll * 2 + Rolls[i].SecondRoll * 2;
                isStrike = false;
            }
            else if (isSpare)
            {
                score += Rolls[i].FirstRoll * 2 + Rolls[i].SecondRoll;
                isSpare = false;
            }
            else
            {
                score += Rolls[i].FirstRoll + Rolls[i].SecondRoll;
            }
            
            if (Rolls[i].FirstRoll == 10 || Rolls[i].SecondRoll == 10)
            {
                isStrike = true;
            }
            else if (Rolls[i].FirstRoll + Rolls[i].SecondRoll == 10)
            {
                isSpare = true;
            }
        }

        return score;
    }
}


