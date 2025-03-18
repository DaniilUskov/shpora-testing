
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    [Description("Старый тест")]
    public void Test()
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(-1, 2, true));
        Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));
        Assert.Throws<ArgumentException>(() => new NumberValidator(-1, 2, false));
        Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));

        ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0.0"));
        ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("00.00"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("-0.00"));
        ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0.0"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("+0.00"));
        ClassicAssert.IsTrue(new NumberValidator(4, 2, true).IsValidNumber("+1.23"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("+1.23"));
        ClassicAssert.IsFalse(new NumberValidator(17, 2, true).IsValidNumber("0.000"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("-1.23"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("a.sd"));
    }

    #region -- Constructor tests

    [Test]
    public void Ctor_ShouldThrowArgumentException_WhenPrecisionIsLessThanZero()
    {
        var act = () => new NumberValidator(-1, 1, true);
        
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Ctor_ShouldGiveCorrectErrorMessage_WhenPrecisionIsLessThanZero()
    {
        var act = () => new NumberValidator(-1, 1, true);
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }

    [Test]
    public void Ctor_ShouldThrowArgumentException_WhenScaleIsLessThanZero()
    {
        var act = () => new NumberValidator(1, -1, true);
        
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Ctor_ShouldThrowArgumentException_WhenScaleIsMoreThanPrecision()
    {
        var act = () => new NumberValidator(1, 2, true);
        
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Ctor_ShouldGiveCorrectErrorMessage_WhenScaleIsLessThanZeroOrMoreThanPrecision()
    {
        var act = () => new NumberValidator(1, 2, true);
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

    #endregion

    #region -- IsValidNumber tests

    [Test]
    public void IsValidNumber_ShouldReturnFalse_WhenInputIsNull()
    {
        var validator = new NumberValidator(3, 2, true);
        
        validator.IsValidNumber(null!).Should().BeFalse();
    }

    [Test]
    [TestCase(3, "abc", false)]
    [TestCase(3, "a.bc", false)]
    [TestCase(3, "0.000", false)]
    [TestCase(3, "-1.23", false)]
    [TestCase(3, "+1.23", false)]
    [TestCase(3, "", false)]
    [TestCase(4, "0.0", true)]
    [TestCase(4, "+1.23", true)]
    public void IsValidNumber_ShouldReturnCorrectResult_OnDifferentInputs(int precision, string input, bool expected)
    {
        var validator = new NumberValidator(precision, 2, true);
        
        validator.IsValidNumber(input).Should().Be(expected);
    }

    #endregion
}