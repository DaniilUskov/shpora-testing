using FluentAssertions;
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

        // Перепишите код на использование Fluent Assertions.
        ClassicAssert.AreEqual(actualTsar.Name, expectedTsar.Name);
        ClassicAssert.AreEqual(actualTsar.Age, expectedTsar.Age);
        ClassicAssert.AreEqual(actualTsar.Height, expectedTsar.Height);
        ClassicAssert.AreEqual(actualTsar.Weight, expectedTsar.Weight);

        ClassicAssert.AreEqual(expectedTsar.Parent.Name, actualTsar.Parent.Name);
        ClassicAssert.AreEqual(expectedTsar.Parent.Age, actualTsar.Parent.Age);
        ClassicAssert.AreEqual(expectedTsar.Parent.Height, actualTsar.Parent.Height);
        ClassicAssert.AreEqual(expectedTsar.Parent.Parent, actualTsar.Parent.Parent);
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
     * Подход с Fluent Assertions лучше, потому что в случае с CheckCurrentTsar_WithCustomEquality при добавлении
     * новых свойств в классе Person нам пришлось бы добавлять новые условия сравнения в метод AreEqual, а в случае с
     * Fluent Assertions достаточно будет добавить их в тест, а сравнение будет происходить автоматически.
     *
     * В Fluent Assertions можно также указывать какие свойства не нужно сравнивать с помощью Fluent-api интерфейса.
     *
     * Также сильно повысится читаемость кода, из-за того же Fluent-api интерфейса, так как цепочку методов мы можем
     * читать как обычное предложение на английском языке.
     *
     * Также в случае с CheckCurrentTsar_WithCustomEquality мы по взгляду на тест не понимаем как именно мы сравниваем,
     * чтобы понять нужно перейти в метод AreEqual, а если у нас свойств будет много, то нам и код AreEqual будет разрастаться,
     * что приведет к трудностям в чтении кода.
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
