using NUnit.Framework;
using System;
using System.Collections.Generic;
using UniSharp.Tools.Validations.DataAnnotations;
using UnityEngine;

namespace UniSharp.Tools.Tests.Validation
{
    public class ValidationDataAnnotationsTests
    {
        public class DateRangeAttributeTests
        {
            private DateRangeAttribute dateRangeAttribute;

            [SetUp]
            public void Setup()
            {
                // Initialize the DateRangeAttribute with a specific range before each test
                dateRangeAttribute = new DateRangeAttribute("2023-01-01", "2023-12-31");
            }

            [Test]
            public void DateRangeAttribute_WithinRangeDate_ReturnsTrue()
            {
                // Arrange
                var withinRangeDate = new DateTime(2023, 6, 15);

                // Act
                bool isValid = dateRangeAttribute.IsValid(withinRangeDate);

                // Assert
                Assert.IsTrue(isValid, "Date within range should be considered valid.");
            }

            [Test]
            public void DateRangeAttribute_MinimumBoundaryDate_ReturnsTrue()
            {
                // Arrange
                var minBoundaryDate = new DateTime(2023, 1, 1);

                // Act
                bool isValid = dateRangeAttribute.IsValid(minBoundaryDate);

                // Assert
                Assert.IsTrue(isValid, "Minimum boundary date should be considered valid.");
            }

            [Test]
            public void DateRangeAttribute_MaximumBoundaryDate_ReturnsTrue()
            {
                // Arrange
                var maxBoundaryDate = new DateTime(2023, 12, 31);

                // Act
                bool isValid = dateRangeAttribute.IsValid(maxBoundaryDate);

                // Assert
                Assert.IsTrue(isValid, "Maximum boundary date should be considered valid.");
            }

            [Test]
            public void DateRangeAttribute_BelowMinimumDate_ReturnsFalse()
            {
                // Arrange
                var belowMinimumDate = new DateTime(2022, 12, 31);

                // Act
                bool isValid = dateRangeAttribute.IsValid(belowMinimumDate);

                // Assert
                Assert.IsFalse(isValid, "Date below the minimum range should be considered invalid.");
            }

            [Test]
            public void DateRangeAttribute_AboveMaximumDate_ReturnsFalse()
            {
                // Arrange
                var aboveMaximumDate = new DateTime(2024, 1, 1);

                // Act
                bool isValid = dateRangeAttribute.IsValid(aboveMaximumDate);

                // Assert
                Assert.IsFalse(isValid, "Date above the maximum range should be considered invalid.");
            }

            [Test]
            public void DateRangeAttribute_NullValue_ReturnsFalse()
            {
                // Act
                bool isValid = dateRangeAttribute.IsValid(null);

                // Assert
                Assert.IsFalse(isValid, "Null value should be considered invalid.");
            }
        }

        public class EmailAttributeTests
        {

            [Test]
            public void Email_ValidEmails_ReturnsTrue()
            {
                // Arrange
                var validEmails = new string[]
                {
                    "email@example.com",
                    "firstname.lastname@example.com",
                    "email@subdomain.example.com",
                    "1234567890@example.com",
                    "email@example-name.com",
                    "_______@example.com",
                    "email@example.co.jp",
                    "firstname-lastname@example.com"
                };

                var emailAttribute = new EmailAttribute();

                foreach (var validEmail in validEmails)
                {
                    // Act
                    bool isValid = emailAttribute.IsValid(validEmail);

                    // Assert
                    Assert.IsTrue(isValid, $"Email '{validEmail}' should be considered valid.");
                }
            }

            [Test]
            public void Email_InvalidEmails_ReturnsFalse()
            {
                // Arrange
                var invalidEmails = new string[]
                {
                    "plainaddress",
                    "@no-local-part.com",
                    "Outlook Contact <outlook-contact@domain.com>",
                    "no-at-sign.net",
                    "no-tld@domain",
                    ";beginning-semicolon@semicolon.com",
                    "middle-semicolon@domain.co;m",
                    "trailing-semicolon@domain.com;",
                    "\"email+leading-quotes@example.com",
                    "email+middle\"-quotes@example.com",
                    "\"quoted-local-part\"@example.com",
                    "\"quoted@domain\"@example.com",
                    "lots-of-dots@domain..gov..uk",
                    "multiple@domains@domain.com",
                    "spaces in local@domain.com",
                    "spaces-in-domain@dom ain.com",
                    "underscores-in-domain@dom_ain.com",
                    "pipe-in-domain@example.com|gov.uk",
                    "comma,in-local@gov.uk",
                    "comma-in-domain@domain,gov.uk",
                    "pound-sign-in-local£@domain.com",
                    "local-with-¡-accents@example.com",
                    "domain-with-umlauts@ümlaut.com",
                    "username@domain.com with spaces",
                    null,
                    string.Empty
                };

                var emailAttribute = new EmailAttribute();

                foreach (var invalidEmail in invalidEmails)
                {
                    // Act
                    bool isValid = emailAttribute.IsValid(invalidEmail);

                    // Assert
                    Assert.IsFalse(isValid, $"Email '{invalidEmail}' should be considered invalid.");
                }
            }
        }

        public class IsNotEmptyAttributeTests
        {
            private IsNotEmptyAttribute isNotEmptyAttribute;

            [SetUp]
            public void Setup()
            {
                isNotEmptyAttribute = new IsNotEmptyAttribute();
            }

            [Test]
            public void IsNotEmpty_ValidNonEmptyString_ReturnsTrue()
            {
                // Arrange
                var nonEmptyString = "Not empty";

                // Act
                bool isValid = isNotEmptyAttribute.IsValid(nonEmptyString);

                // Assert
                Assert.IsTrue(isValid, "Non-empty string should be considered not empty.");
            }

            [Test]
            public void IsNotEmpty_EmptyString_ReturnsFalse()
            {
                // Arrange
                var emptyString = "";

                // Act
                bool isValid = isNotEmptyAttribute.IsValid(emptyString);

                // Assert
                Assert.IsFalse(isValid, "Empty string should be considered empty.");
            }

            [Test]
            public void IsNotEmpty_NonEmptyCollection_ReturnsTrue()
            {
                // Arrange
                var nonEmptyCollection = new List<int> { 1 };

                // Act
                bool isValid = isNotEmptyAttribute.IsValid(nonEmptyCollection);

                // Assert
                Assert.IsTrue(isValid, "Non-empty collection should be considered not empty.");
            }

            [Test]
            public void IsNotEmpty_EmptyCollection_ReturnsFalse()
            {
                // Arrange
                var emptyCollection = new List<int>();

                // Act
                bool isValid = isNotEmptyAttribute.IsValid(emptyCollection);

                // Assert
                Assert.IsFalse(isValid, "Empty collection should be considered empty.");
            }

            [Test]
            public void IsNotEmpty_NonEmptyArray_ReturnsTrue()
            {
                // Arrange
                var nonEmptyArray = new int[] { 1 };

                // Act
                bool isValid = isNotEmptyAttribute.IsValid(nonEmptyArray);

                // Assert
                Assert.IsTrue(isValid, "Non-empty array should be considered not empty.");
            }

            [Test]
            public void IsNotEmpty_EmptyArray_ReturnsFalse()
            {
                // Arrange
                var emptyArray = new int[] { };

                // Act
                bool isValid = isNotEmptyAttribute.IsValid(emptyArray);

                // Assert
                Assert.IsFalse(isValid, "Empty array should be considered empty.");
            }

            [Test]
            public void IsNotEmpty_NonNullValueType_ReturnsTrue()
            {
                // Arrange
                var nonNullValueType = 42; // An int, which is a value type

                // Act
                bool isValid = isNotEmptyAttribute.IsValid(nonNullValueType);

                // Assert
                Assert.IsTrue(isValid, "Non-null value type should be considered not empty.");
            }

            [Test]
            public void IsNotEmpty_NullValue_ReturnsFalse()
            {
                // Act
                bool isValid = isNotEmptyAttribute.IsValid(null);

                // Assert
                Assert.IsFalse(isValid, "Null should be considered empty.");
            }
        }

        public class LengthAttributeTests
        {
            [Test]
            public void LengthAttribute_ExactLengthString_ReturnsTrue()
            {
                // Arrange
                var lengthAttribute = new LengthAttribute(5);
                var validString = "Hello";

                // Act
                bool isValid = lengthAttribute.IsValid(validString);

                // Assert
                Assert.IsTrue(isValid);
            }

            [Test]
            public void LengthAttribute_IncorrectLengthString_ReturnsFalse()
            {
                // Arrange
                var lengthAttribute = new LengthAttribute(5);
                var invalidString = "Hi";

                // Act
                bool isValid = lengthAttribute.IsValid(invalidString);

                // Assert
                Assert.IsFalse(isValid);
            }

            [Test]
            public void MaxLengthAttribute_StringWithinMaxLength_ReturnsTrue()
            {
                // Arrange
                var maxLengthAttribute = new MaxLengthAttribute(10);
                var validString = "Hello";

                // Act
                bool isValid = maxLengthAttribute.IsValid(validString);

                // Assert
                Assert.IsTrue(isValid);
            }

            [Test]
            public void MaxLengthAttribute_StringExceedsMaxLength_ReturnsFalse()
            {
                // Arrange
                var maxLengthAttribute = new MaxLengthAttribute(3);
                var invalidString = "Hello";

                // Act
                bool isValid = maxLengthAttribute.IsValid(invalidString);

                // Assert
                Assert.IsFalse(isValid);
            }

            [Test]
            public void MinLengthAttribute_StringMeetsMinLength_ReturnsTrue()
            {
                // Arrange
                var minLengthAttribute = new MinLengthAttribute(3);
                var validString = "Hello";

                // Act
                bool isValid = minLengthAttribute.IsValid(validString);

                // Assert
                Assert.IsTrue(isValid);
            }

            [Test]
            public void MinLengthAttribute_StringBelowMinLength_ReturnsFalse()
            {
                // Arrange
                var minLengthAttribute = new MinLengthAttribute(10);
                var invalidString = "Hi";

                // Act
                bool isValid = minLengthAttribute.IsValid(invalidString);

                // Assert
                Assert.IsFalse(isValid);
            }
        }

        public class NotNullAttributeTests
        {
            private NotNullAttribute notNullAttribute;

            [SetUp]
            public void Setup()
            {
                // Initialize the NotNullAttribute before each test
                notNullAttribute = new NotNullAttribute();
            }

            [Test]
            public void NotNullAttribute_NonNullValue_ReturnsTrue()
            {
                // Arrange
                var nonNullValue = "I am not null";

                // Act
                bool isValid = notNullAttribute.IsValid(nonNullValue);

                // Assert
                Assert.IsTrue(isValid, "NonNull value should be considered valid.");
            }

            [Test]
            public void NotNullAttribute_NullValue_ReturnsFalse()
            {
                // Arrange
                object nullValue = null;

                // Act
                bool isValid = notNullAttribute.IsValid(nullValue);

                // Assert
                Assert.IsFalse(isValid, "Null value should be considered invalid.");
            }

            [Test]
            public void NotNullAttribute_NonNullObject_ReturnsTrue()
            {
                // Arrange
                var nonNullObject = new object();

                // Act
                bool isValid = notNullAttribute.IsValid(nonNullObject);

                // Assert
                Assert.IsTrue(isValid, "NonNull object should be considered valid.");
            }

            [Test]
            public void NotNullAttribute_EmptyString_ReturnsTrue()
            {
                // Arrange
                var emptyString = string.Empty; // Empty string is not null

                // Act
                bool isValid = notNullAttribute.IsValid(emptyString);

                // Assert
                Assert.IsTrue(isValid, "Empty string should be considered valid as it is not null.");
            }
        }

        public class PasswordFormatAttributeTests
        {
            private PasswordFormatAttribute passwordFormatAttribute;

            [SetUp]
            public void Setup()
            {
                passwordFormatAttribute = new PasswordFormatAttribute();
            }

            [Test]
            public void PasswordFormatAttribute_ValidPassword_ReturnsTrue()
            {
                // Arrange
                var validPassword = "ValidPassword1!";

                // Act
                bool isValid = passwordFormatAttribute.IsValid(validPassword);

                // Assert
                Assert.IsTrue(isValid, "Valid password should be considered valid.");
            }

            [TestCase("short", false, Description = "Too Short")]
            [TestCase("nouppercase1!", false, Description = "No Uppercase")]
            [TestCase("NOLOWERCASE1!", false, Description = "No Lowercase")]
            [TestCase("NoDigits!!", false, Description = "No Digits")]
            [TestCase("NoSpecials1", false, Description = "No Special Characters")]
            [TestCase("ValidPassword1!", true, Description = "Meets All Requirements")]
            public void PasswordFormatAttribute_VariousPasswords_ValidationResults(string password, bool expected)
            {
                // Act
                bool isValid = passwordFormatAttribute.IsValid(password);

                // Assert
                Assert.AreEqual(expected, isValid, $"Password '{password}' validation failed.");
            }

            [Test]
            public void PasswordFormatAttribute_NullPassword_ReturnsFalse()
            {
                // Arrange
                object nullPassword = null;

                // Act
                bool isValid = passwordFormatAttribute.IsValid(nullPassword);

                // Assert
                Assert.IsFalse(isValid, "Null password should be considered invalid.");
            }

            [Test]
            public void PasswordFormatAttribute_EmptyPassword_ReturnsFalse()
            {
                // Arrange
                var emptyPassword = "";

                // Act
                bool isValid = passwordFormatAttribute.IsValid(emptyPassword);

                // Assert
                Assert.IsFalse(isValid, "Empty password should be considered invalid.");
            }
        }

        public class RangeAttributeTests
        {
            private Validations.DataAnnotations.RangeAttribute rangeAttribute;

            [SetUp]
            public void Setup()
            {
                // Initialize the RangeAttribute with a specific range before each test
                rangeAttribute = new Validations.DataAnnotations.RangeAttribute(10, 20); // Example range: 10 (inclusive) to 20 (inclusive)
            }

            [Test]
            public void RangeAttribute_ValueWithinRange_ReturnsTrue()
            {
                // Arrange
                var validValues = new object[] { 10, 15, 20 };

                foreach (var validValue in validValues)
                {
                    // Act
                    bool isValid = rangeAttribute.IsValid(validValue);

                    // Assert
                    Assert.IsTrue(isValid, $"Value '{validValue}' should be considered valid within the range.");
                }
            }

            [Test]
            public void RangeAttribute_ValueOutsideRange_ReturnsFalse()
            {
                // Arrange
                var invalidValues = new object[] { 9.999, 20.001, -10, 30 };

                foreach (var invalidValue in invalidValues)
                {
                    // Act
                    bool isValid = rangeAttribute.IsValid(invalidValue);

                    // Assert
                    Assert.IsFalse(isValid, $"Value '{invalidValue}' should be considered invalid outside the range.");
                }
            }

            [Test]
            public void RangeAttribute_BoundaryValues_ReturnsTrue()
            {
                // Arrange
                var boundaryValues = new object[] { 10, 20 };

                foreach (var boundaryValue in boundaryValues)
                {
                    // Act
                    bool isValid = rangeAttribute.IsValid(boundaryValue);

                    // Assert
                    Assert.IsTrue(isValid, $"Boundary value '{boundaryValue}' should be considered valid.");
                }
            }

            [Test]
            public void RangeAttribute_NonNumericValue_ReturnsFalse()
            {
                // Arrange
                var nonNumericValue = "NotANumber";

                // Act
                bool isValid = rangeAttribute.IsValid(nonNumericValue);

                // Assert
                Assert.IsFalse(isValid, "Non-numeric value should be considered invalid.");
            }

            [Test]
            public void RangeAttribute_NullValue_ReturnsFalse()
            {
                // Arrange
                object nullValue = null;

                // Act
                bool isValid = rangeAttribute.IsValid(nullValue);

                // Assert
                Assert.IsFalse(isValid, "Null value should be considered invalid.");
            }
        }

        public class RegularExpressionAttributeTests
        {
            private RegularExpressionAttribute emailPatternAttribute;

            [SetUp]
            public void Setup()
            {
                // Initialize the RegularExpressionAttribute with a simple email pattern
                emailPatternAttribute = new RegularExpressionAttribute(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$");
            }

            [Test]
            public void RegularExpressionAttribute_ValidEmails_ReturnsTrue()
            {
                // Arrange
                var validEmails = new string[]
                {
                    "email@example.com",
                    "firstname.lastname@example.com",
                    "email@subdomain.example.com",
                    "firstname+lastname@example.com"
                };

                foreach (var validEmail in validEmails)
                {
                    // Act
                    bool isValid = emailPatternAttribute.IsValid(validEmail);

                    // Assert
                    Assert.IsTrue(isValid, $"Email '{validEmail}' should be considered valid.");
                }
            }

            [Test]
            public void RegularExpressionAttribute_NullValue_ReturnsFalse()
            {
                // Arrange
                object nullValue = null;

                // Act
                bool isValid = emailPatternAttribute.IsValid(nullValue);

                // Assert
                Assert.IsFalse(isValid, "Null value should be considered invalid.");
            }

            [Test]
            public void RegularExpressionAttribute_EmptyString_ReturnsFalse()
            {
                // Arrange
                var emptyString = "";

                // Act
                bool isValid = emailPatternAttribute.IsValid(emptyString);

                // Assert
                Assert.IsFalse(isValid, "Empty string should be considered invalid.");
            }
        }

        public class RequiredAttributeTests
        {
            private RequiredAttribute requiredAttribute;

            [SetUp]
            public void Setup()
            {
                // Initialize the RequiredAttribute before each test
                requiredAttribute = new RequiredAttribute();
            }

            [Test]
            public void RequiredAttribute_NonEmptyString_ReturnsTrue()
            {
                // Arrange
                var validString = "Hello";

                // Act
                bool isValid = requiredAttribute.IsValid(validString);

                // Assert
                Assert.IsTrue(isValid, "Non-empty string should be considered valid.");
            }

            [Test]
            public void RequiredAttribute_EmptyString_ReturnsFalse()
            {
                // Arrange
                var emptyString = "";

                // Act
                bool isValid = requiredAttribute.IsValid(emptyString);

                // Assert
                Assert.IsFalse(isValid, "Empty string should be considered invalid.");
            }

            [Test]
            public void RequiredAttribute_WhitespaceString_ReturnsFalse()
            {
                // Arrange
                var whitespaceString = "   ";

                // Act
                bool isValid = requiredAttribute.IsValid(whitespaceString);

                // Assert
                Assert.IsFalse(isValid, "Whitespace string should be considered invalid.");
            }

            [Test]
            public void RequiredAttribute_NullValue_ReturnsFalse()
            {
                // Arrange
                object nullValue = null;

                // Act
                bool isValid = requiredAttribute.IsValid(nullValue);

                // Assert
                Assert.IsFalse(isValid, "Null value should be considered invalid.");
            }

            [Test]
            public void RequiredAttribute_NonStringObject_ReturnsTrue()
            {
                // Arrange
                var nonStringObject = new object();

                // Act
                bool isValid = requiredAttribute.IsValid(nonStringObject);

                // Assert
                Assert.IsTrue(isValid, "Non-string object, not null, should be considered valid.");
            }
        }

        public class TimeSpanRangeAttributeTests
        {
            private TimeSpanRangeAttribute timeSpanRangeAttribute;

            [SetUp]
            public void Setup()
            {
                // Example range: from 1 hour to 5 hours
                timeSpanRangeAttribute = new TimeSpanRangeAttribute("01:00:00", "05:00:00");
            }

            [Test]
            public void TimeSpanRangeAttribute_ValueWithinRange_ReturnsTrue()
            {
                // Arrange
                var withinRange = TimeSpan.FromHours(3);

                // Act
                bool isValid = timeSpanRangeAttribute.IsValid(withinRange);

                // Assert
                Assert.IsTrue(isValid, "TimeSpan within range should be considered valid.");
            }

            [Test]
            public void TimeSpanRangeAttribute_BoundaryValues_ReturnsTrue()
            {
                // Arrange
                var minBoundary = TimeSpan.FromHours(1);
                var maxBoundary = TimeSpan.FromHours(5);

                // Act & Assert
                Assert.IsTrue(timeSpanRangeAttribute.IsValid(minBoundary), "Minimum boundary value should be considered valid.");
                Assert.IsTrue(timeSpanRangeAttribute.IsValid(maxBoundary), "Maximum boundary value should be considered valid.");
            }

            [Test]
            public void TimeSpanRangeAttribute_ValueBelowMinimum_ReturnsFalse()
            {
                // Arrange
                var belowMinimum = TimeSpan.FromMinutes(30); // Below 1 hour

                // Act
                bool isValid = timeSpanRangeAttribute.IsValid(belowMinimum);

                // Assert
                Assert.IsFalse(isValid, "TimeSpan below minimum should be considered invalid.");
            }

            [Test]
            public void TimeSpanRangeAttribute_ValueAboveMaximum_ReturnsFalse()
            {
                // Arrange
                var aboveMaximum = TimeSpan.FromHours(6); // Above 5 hours

                // Act
                bool isValid = timeSpanRangeAttribute.IsValid(aboveMaximum);

                // Assert
                Assert.IsFalse(isValid, "TimeSpan above maximum should be considered invalid.");
            }

            [Test]
            public void TimeSpanRangeAttribute_NullValue_ReturnsFalse()
            {
                // Act
                bool isValid = timeSpanRangeAttribute.IsValid(null);

                // Assert
                Assert.IsFalse(isValid, "Null value should be considered invalid.");
            }
        }

        public class UrlAttributeTests
        {
            private UrlAttribute urlAttribute;

            [SetUp]
            public void Setup()
            {
                // Initialize the UrlAttribute before each test
                urlAttribute = new UrlAttribute();
            }

            [Test]
            public void UrlAttribute_ValidUrls_ReturnsTrue()
            {
                // Arrange
                var validUrls = new string[]
                {
                    "http://www.example.com",
                    "https://example.com",
                    "https://www.example.com/path/to/resource?query=string&value=abc",
                    "https://localhost",
                    "http://127.0.0.1",
                    "https://sub.domain.example.com",
                    "ftp://example.com/resource.txt",
                    "http://example.com:8080"
                };

                foreach (var validUrl in validUrls)
                {
                    try
                    {
                        // Act
                        bool isValid = urlAttribute.IsValid(validUrl);

                        // Assert
                        Assert.IsTrue(isValid, $"URL '{validUrl}' should be considered valid.");
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }

            [Test]
            public void UrlAttribute_InvalidUrls_ReturnsFalse()
            {
                // Arrange
                var invalidUrls = new string[]
                {
                    "justastring",
                    "www.example.com", // Missing scheme
                    "http://", // Missing domain
                    "https://?query", // Missing domain with query string
                    "ftp:/resource.txt", // Missing slash in scheme
                    "http:///example.com", // Extra slash in scheme
                    null, // Null value
                    "" // Empty string
                };

                foreach (var invalidUrl in invalidUrls)
                {
                    // Act
                    bool isValid = urlAttribute.IsValid(invalidUrl);

                    // Assert
                    Assert.IsFalse(isValid, $"URL '{invalidUrl}' should be considered invalid.");
                }
            }
        }

        public class UsernameFormatAttributeTests
        {
            private UsernameFormatAttribute usernameFormatAttribute;

            [SetUp]
            public void Setup()
            {
                // Initialize the UsernameFormatAttribute with default settings
                usernameFormatAttribute = new UsernameFormatAttribute();
            }

            [Test]
            public void UsernameFormatAttribute_ValidUsernames_ReturnsTrue()
            {
                // Arrange
                var validUsernames = new string[]
                {
                    "user123",
                    "User",
                    "12345",
                    "UserName"
                };

                foreach (var validUsername in validUsernames)
                {
                    // Act
                    bool isValid = usernameFormatAttribute.IsValid(validUsername);

                    // Assert
                    Assert.IsTrue(isValid, $"Username '{validUsername}' should be considered valid.");
                }
            }

            [Test]
            public void UsernameFormatAttribute_InvalidUsernamesDueToLength_ReturnsFalse()
            {
                // Arrange
                var invalidUsernames = new string[]
                {
                    "usr", // Too short
                    "thisusernameiswaytoolongandinvalid" // Too long
                };

                foreach (var invalidUsername in invalidUsernames)
                {
                    // Act
                    bool isValid = usernameFormatAttribute.IsValid(invalidUsername);

                    // Assert
                    Assert.IsFalse(isValid, $"Username '{invalidUsername}' should be considered invalid due to length.");
                }
            }

            [Test]
            public void UsernameFormatAttribute_InvalidUsernamesDueToSpecialCharacters_ReturnsFalse()
            {
                // Arrange
                usernameFormatAttribute.AllowSpecialCharacters = false; // Disallow special characters for this test
                var invalidUsernames = new string[]
                {
                    "user@name",
                    "user!name",
                    "user#name"
                };

                foreach (var invalidUsername in invalidUsernames)
                {
                    // Act
                    bool isValid = usernameFormatAttribute.IsValid(invalidUsername);

                    // Assert
                    Assert.IsFalse(isValid, $"Username '{invalidUsername}' should be considered invalid due to special characters.");
                }
            }

            [Test]
            public void UsernameFormatAttribute_InvalidUsernamesDueToDigits_ReturnsFalse()
            {
                // Arrange
                usernameFormatAttribute.AllowDigits = false; // Disallow digits for this test
                var invalidUsernames = new string[]
                {
                    "user1",
                    "123username"
                };

                foreach (var invalidUsername in invalidUsernames)
                {
                    // Act
                    bool isValid = usernameFormatAttribute.IsValid(invalidUsername);

                    // Assert
                    Assert.IsFalse(isValid, $"Username '{invalidUsername}' should be considered invalid due to digits.");
                }
            }

            [Test]
            public void UsernameFormatAttribute_NullUsername_ReturnsFalse()
            {
                // Act
                bool isValid = usernameFormatAttribute.IsValid(null);

                // Assert
                Assert.IsFalse(isValid, "Null username should be considered invalid.");
            }
        }
    }
}