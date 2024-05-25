using NUnit.Framework;
using System;
using UniSharp.Tools.Validations;
using UniSharp.Tools.Validations.Attributes;
using UniSharp.Tools.Validations.Extensions;
using UniSharp.Tools.Validations.DataAnnotations;
using RangeAttribute = UniSharp.Tools.Validations.DataAnnotations.RangeAttribute;

namespace UniSharp.Tools.Tests.Validation
{
    public class ValidationProviderTests
    {
        [Validation(typeof(PlayerDefaultValiator))]
        public class Player
        {
            [Required]
            public string Name { get; set; }
            [Email]
            public string Email { get; set; }
            [Required]
            [Range(10, 20)]
            public int Age { get; set; }
            [DateRange("1990-01-01", "2023-01-01")]
            public DateTime Born { get; set; }
        }

        public class PlayerDefaultValiator : Validator<Player>
        {
            public override ValidationResult Validate(Player instance)
            {
                return base.Validate(instance);
            }
        }

        public class PlayerCostumValiator : Validator<Player>
        {
            public override ValidationResult Validate(Player instance)
            {
                var nameValidation = Required(instance.Name, nameof(instance.Name));
                var ageValidation = ValidateRange(instance.Age, 10, 20, nameof(instance.Name));
                var emailValidation = string.IsNullOrEmpty(instance.Name) ?
                    ValidationResult.Failed : ValidationResult.Ok;
                var bornValidation = instance.Born.IsInRange(new DateTime(1990, 1, 1), new DateTime(2023, 1, 1)) ?
                    ValidationResult.Ok : ValidationResult.Failed;

                return ValidateAll(nameValidation, ageValidation, emailValidation, bornValidation);
            }
        }

        [Test]
        public void GetValidatorPorperties_ReturnsTrue()
        {
            var player = new Player
            {
                Name = "Name",
                Email = "Name@Google.com",
                Age = 15,
                Born = new DateTime(2020, 1, 1)
            };

            var validationResult = ValidatorProvider.ValidateProperties(player);

            Assert.True(validationResult.IsValid);
        }

        [Test]
        public void GetValidatorPorperties_ReturnsFalse()
        {
            var player = new Player
            {
                Email = "Name@Google@.com",
                Age = 4,
                Born = new DateTime(1980, 1, 1)
            };

            var validationResult = ValidatorProvider.ValidateProperties(player);

            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void GetVlidatorAttributeClass_ReturnsTrue()
        {
            var player = new Player
            {
                Name = "Name",
                Email = "Name@Google.com",
                Age = 15,
                Born = new DateTime(2020, 1, 1)
            };

            var validationResult = ValidatorProvider.Validate(player, false);

            Assert.True(validationResult.IsValid);
        }

        [Test]
        public void GetVlidatorCustomClass_ReturnsTrue()
        {
            var player = new Player
            {
                Name = "Name",
                Email = "Name@Google.com",
                Age = 15,
                Born = new DateTime(2020, 1, 1)
            };

            var validator = new PlayerCostumValiator();

            var validationResult = validator.Validate(player);

            Assert.True(validationResult.IsValid);
        }

        [Test]
        public void ValidateProvider_ReturnsTrue()
        {
            var player = new Player
            {
                Name = "Name",
                Email = "Name@Google.com",
                Age = 15,
                Born = new DateTime(2020, 1, 1)
            };

            var validationResult = ValidatorProvider.Validate(player);

            Assert.True(validationResult.IsValid);
        }

        [Test]
        public void ValidateProvider_ReturnsFalse()
        {
            var player = new Player
            {
                Email = "Name@Google@.com",
                Age = 4,
                Born = new DateTime(1980, 1, 1)
            };

            var validationResult = ValidatorProvider.Validate(player);

            Assert.False(validationResult.IsValid);
        }

        [Test]
        public void FluentValidaion_ReturnsTrue()
        {
            var player = new Player
            {
                Name = "Name",
                Email = "Name@Google.com",
                Age = 20,
                Born = new DateTime(2020, 1, 1)
            };

            var validationResult = ValidatorProvider.For<Player>()
                .RuleFor(x => x.Name, name => !string.IsNullOrWhiteSpace(name), "Name is required")
                .RuleFor(x => x.Age, age => age > 18, "Must be over 18")
                .WithMessage("Age requirement not met")
                .Build()
                .Validate(player);

            Assert.True(validationResult.IsValid);
        }

        [Test]
        public void FluentValidaion_ReturnsFalse()
        {
            var player = new Player
            {
                Email = "Name@Google@.com",
                Age = 4,
                Born = new DateTime(1980, 1, 1)
            };

            var validationResult = ValidatorProvider.For<Player>()
                .RuleFor(x => x.Name, name => !string.IsNullOrWhiteSpace(name), "Name is required")
                .RuleFor(x => x.Age, age => age > 18, "Must be over 18")
                .WithMessage("Age requirement not met")
                .Build()
                .Validate(player);

            Assert.False(validationResult.IsValid);
        }
    }
}