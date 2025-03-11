using Basic.Task.WordsStatistics.WordsStatistics;
using FluentAssertions;
using NUnit.Framework;

namespace Basic.Task.WordsStatistics;

// Документация по FluentAssertions с примерами : https://github.com/fluentassertions/fluentassertions/wiki

[TestFixture]
public class WordsStatisticsTests
{

    private IWordsStatistics wordsStatistics;

    [SetUp]
    public void SetUp()
    {

        wordsStatistics = CreateStatistics();
    }

    public virtual IWordsStatistics CreateStatistics()
    {
        // меняется на разные реализации при запуске exe
        return new WordsStatisticsImpl();
    }

    #region -- GetStatistics

    [Test]
    public void GetStatistics_IsEmpty_AfterCreation()
    {
        wordsStatistics.GetStatistics().Should().BeEmpty();
    }

    [Test]
    public void GetStatistics_ContainsItem_AfterAddition()
    {
        wordsStatistics.AddWord("abc");
        wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 1));
    }

    [Test]
    public void GetStatistics_ContainsManyItems_AfterAdditionOfDifferentWords()
    {
        wordsStatistics.AddWord("abc");
        wordsStatistics.AddWord("def");
        wordsStatistics.GetStatistics().Should().HaveCount(2);
    }

    [Test]
    public void GetStatistics_ItemsAreOrderedByCountInDescendingOrder_AfterAdditionOfDifferentWords()
    {
        wordsStatistics.AddWord("abc");
        wordsStatistics.AddWord("defg");
        wordsStatistics.AddWord("defg");
        wordsStatistics.GetStatistics().Should().Equal(new WordCount("defg", 2), new WordCount("abc", 1));
    }
    
    [Test]
    public void GetStatistics_ItemsAreOrderedLexicographicInDescendingOrder_AfterAdditionOfDifferentWords()
    {
        wordsStatistics.AddWord("defg");
        wordsStatistics.AddWord("defg");
        wordsStatistics.AddWord("abc");
        wordsStatistics.AddWord("abc");
        wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 2), new WordCount("defg", 2));
    }

    #endregion

    #region -- AddWord

    [Test]
    public void AddWord_ThrowsArgumentNullException_WhenWordIsNull()
    {
        var act = () => wordsStatistics.AddWord(null);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void AddWord_DoesNotAddWord_WhenWordIsEmpty()
    {
        wordsStatistics.AddWord(string.Empty);
        wordsStatistics.GetStatistics().Should().BeEmpty();
    }

    [Test]
    public void AddWord_DoesNotAddWord_WhenWordIsWhiteSpace()
    {
        wordsStatistics.AddWord(" ");
        wordsStatistics.GetStatistics().Should().BeEmpty();
    }

    [Test]
    public void AddWord_CountWordsCorrect_AfterAdditionOfSameWords()
    {
        wordsStatistics.AddWord("abc");
        wordsStatistics.AddWord("abc");
        wordsStatistics.AddWord("abc");
        wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 3));
    }

    [Test]
    public void AddWord_CountWordsCorrect_AfterAdditionSameWordsWithDifferentCases()
    {
        wordsStatistics.AddWord("abc");
        wordsStatistics.AddWord("ABC");
        wordsStatistics.AddWord("Abc");
        wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 3));
    }

    [Test]
    public void AddWord_DoesNotAddFullWord_WhenWordsLengthIsGreaterThan10()
    {
        wordsStatistics.AddWord("abcaaaaaaaaaaaaaaaaaaaaaaa");
        wordsStatistics.GetStatistics().Should().Equal(new WordCount("abcaaaaaaa", 1));
    }

    #endregion
}