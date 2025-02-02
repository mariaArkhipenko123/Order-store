using Lab.Application.Utils.RegExp;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class ValidationUtilsTests
    {
        [Test]
        [TestCase("2024-12-25", ExpectedResult = true)]
        [TestCase("25/12/2024", ExpectedResult = true)]
        [TestCase("December 25, 2024", ExpectedResult = true)]
        [TestCase("Invalid Date", ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        public bool IsValidDate(string date)
        {
            return ValidationUtils.IsValidDate(date);
        }

        [Test]
        [TestCase("test@example.com", ExpectedResult = true)]
        [TestCase("user.name@domain.co", ExpectedResult = true)]
        [TestCase("user@sub.domain.com", ExpectedResult = true)]
        [TestCase("invalid-email", ExpectedResult = false)]
        [TestCase("user@.com", ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        public bool IsValidEmail(string email)
        {
            return ValidationUtils.IsValidEmail(email);
        }

        [Test]
        [TestCase("+1234567890", ExpectedResult = true)]
        [TestCase("1234567890", ExpectedResult = true)]
        [TestCase("+1 234 567 890", ExpectedResult = false)] 
        [TestCase("123", ExpectedResult = false)] 
        [TestCase("InvalidPhoneNumber", ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        public bool IsValidPhoneNumber(string phoneNumber)
        {
            return ValidationUtils.IsValidPhoneNumber(phoneNumber);
        }
    }
}
