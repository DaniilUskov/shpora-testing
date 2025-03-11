using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using TDD.Task;

namespace TDD;

[TestFixture]
public class GameTests
{
    [Test]
    [Explicit]
    public void HaveZeroScore_BeforeAnyRolls()
    {
        new Game()
            .GetScore()
            .Should().Be(0);
    }

    [Test]
    public void LeftFramesAreTen_BeforeAnyRolls()
    {
        var game = new Game();
        var framesLeft = game
            .GetType()
            .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(x => x.Name == "FramesLeft");
        
        framesLeft.GetValue(game).Should().Be(10);
    }

    [Test]
    public void LeftFramesProperty_IsPrivate()
    {
        var game = new Game();
        var privateProperties = game
            .GetType()
            .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
        
        privateProperties.Should().Contain(x => x.Name == "FramesLeft");
    }

    [Test]
    public void DecreaseFramesLeft_AfterRoll()
    {
        var game = new Game();
        var framesLeft = game
            .GetType()
            .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(x => x.Name == "FramesLeft");
        
        game.Roll(0);
        game.Roll(1);
        
        framesLeft.GetValue(game).Should().Be(9);
    }

    [Test]
    public void GetScore_ReturnsZero_BeforeAnyRolls()
    {
        new Game()
            .GetScore()
            .Should().Be(0);
    }

    [Test]
    public void CorrectScore_AfterSimpleFrame()
    {
        var game = new Game();
        game.Roll(5);
        game.Roll(2);
        
        game.GetScore().Should().Be(7);
    }

    [Test]
    public void CorrectScore_AfterSpareFrame()
    {
        var game = new Game();
        game.Roll(5);
        game.Roll(5);
        game.Roll(2);
        game.Roll(1);
        
        game.GetScore().Should().Be(15);
    }

    [Test]
    public void CorrectScore_AfterStrikeFrame()
    {
        var game = new Game();
        game.Roll(10);
        game.Roll(5);
        game.Roll(2);
        
        game.GetScore().Should().Be(24);
    }

    [Test]
    public void CorrectScore_AfterMultipleFrames()
    {
        var game = new Game();
        game.Roll(10);
        game.Roll(5);
        game.Roll(2);
        game.Roll(1);
        game.Roll(1);
        
        game.GetScore().Should().Be(26);
    }

    [Test]
    public void CorrectScore_AfterStrikeFrameOnLastFrame()
    {
        var game = new Game();

        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(10);
        game.Roll(10);
        
        game.GetScore().Should().Be(102);
    }

    [Test]
    public void CantRoll_AfterTenFramesWithoutStrikeOrSpare()
    {
        var game = new Game();
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        game.Roll(8);
        game.Roll(0);
        
        var action = new Action(() => game.Roll(1));
        
        action.Should().Throw<Exception>().WithMessage("Game is over!");
    }
}