
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
    public void IsValidNumber_ShouldReturnFalse_WhenInputIsEmpty()
    {
        var validator = new NumberValidator(3, 2, true);
        
        validator.IsValidNumber(string.Empty).Should().BeFalse();
    }

    [Test]
    [TestCase("abc")]
    [TestCase("a.bc")]
    public void IsValidNumber_ShouldReturnFalse_WhenInputIsNotNumber(string input)
    {
        var validator = new NumberValidator(3, 2, true);
        
        validator.IsValidNumber(input).Should().BeFalse();
    }

    [Test]
    [TestCase("0.0")]
    [TestCase("+1.23")]
    public void IsValidNumber_ShouldReturnTrue_WhenInputIsValid(string input)
    {
        var validator = new NumberValidator(4, 2, true);
        
        validator.IsValidNumber(input).Should().BeTrue();
    }

    [Test]
    [TestCase("0.000")]
    [TestCase("-1.23")]
    [TestCase("+1.23")]
    public void IsValidNumber_ShouldReturnFalse_WhenInputIsNotValid(string input)
    {
        var validator = new NumberValidator(3, 2, true);
        
        validator.IsValidNumber(input).Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_ShouldReturnFalse_WhenOnlyPositiveIsTrueAndInputIsNegative()
    {
        var validator = new NumberValidator(3, 2, true);
        
        validator.IsValidNumber("-1.23").Should().BeFalse();
    }

    #endregion
}