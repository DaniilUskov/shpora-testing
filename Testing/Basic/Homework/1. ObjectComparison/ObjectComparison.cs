using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        using (new AssertionScope())
        {
            actualTsar.Name.Should().Be(expectedTsar.Name);
            actualTsar.Age.Should().Be(expectedTsar.Age);
            actualTsar.Height.Should().Be(expectedTsar.Height);
            actualTsar.Weight.Should().Be(expectedTsar.Weight);
            
            actualTsar.Parent.Name.Should().Be(expectedTsar.Parent.Name);
            actualTsar.Parent.Age.Should().Be(expectedTsar.Parent.Age);
            actualTsar.Parent.Height.Should().Be(expectedTsar.Parent.Height);
            actualTsar.Parent.Parent.Should().Be(expectedTsar.Parent.Parent);
            
        }
    }

    #region -- Simple Tsar without parents tests

    [Test]
    [TestCase("Ivan IV The Terrible")]
    public void TsarName_IsCorrect_AfterInitialization(string expectedTsarName)
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        actualTsar.Name.Should().BeEquivalentTo(expectedTsarName);
    }

    [Test]
    [TestCase(54)]
    public void TsarAge_IsCorrect_AfterInitialization(int expectedTsarAge)
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        actualTsar.Age.Should().Be(expectedTsarAge);
    }

    [Test]
    [TestCase(170)]
    public void TsarHeight_IsCorrect_AfterInitialization(int expectedTsarHeight)
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        actualTsar.Height.Should().Be(expectedTsarHeight);
    }

    [Test]
    [TestCase(70)]
    public void TsarWeight_IsCorrect_AfterInitialization(int expectedTsarWeight)
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        actualTsar.Weight.Should().Be(expectedTsarWeight);
    }

    #endregion

    #region -- Tsar with parents tests

    [Test]
    public void TsarParents_IsCorrect_AfterInitialization()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        actualTsar.Parent
            .Should()
            .BeEquivalentTo(expectedTsar.Parent, options => options.IncludingNestedObjects().Excluding(x => x.Id));
    }

    #endregion
    
    /*
     * Подход с AssertionScope лучше, потому что если у нас например ошибка в первом сравнении, то проверки не прекратятся
     * и в консоли тестов будет выведена более информативная ошибка. А в случае подхода с ClassicAssert у нас упадет сразу после первой
     * ошибки, даже если следующие поля были верны.
     */

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
