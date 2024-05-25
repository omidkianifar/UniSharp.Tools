using System;
using UniSharp.Tools.Validations;
using UniSharp.Tools.Validations.Extensions;
using UniSharp.Tools.Validations.Attributes;
using UnityEngine;
using UniSharp.Tools.Validations.DataAnnotations;
using RangeAttribute = UniSharp.Tools.Validations.DataAnnotations.RangeAttribute;

namespace UniSharp.Tools.Tests.Validation
{
    public class Usage
    {
        public class Player
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public int Age { get; set; }
            public DateTime Born { get; set; }
        }

        public class PlayerValidator : Validator<Player>
        {
            public static readonly DateTime MinBorn = new(1990, 1, 1);
            public static readonly DateTime MaxBorn = new(2023, 1, 1);

            public override ValidationResult Validate(Player instance)
            {
                var nameValidation = Required(instance.Name, nameof(instance.Name));

                var ageValidation = ValidateRange(instance.Age, 10, 20, nameof(instance.Name));

                var emailValidation = string.IsNullOrEmpty(instance.Name) ?
                    ValidationResult.Failed :
                    instance.Name.IsEmail();

                var bornValidation = instance.Born.IsInRange(MinBorn, MaxBorn) ?
                    ValidationResult.Ok :
                    ValidationResult.Failed;

                return ValidateAll(nameValidation, ageValidation, emailValidation, bornValidation);
            }
        }

        public void Validate_Manually()
        {
            var player = new Player
            {
                Name = "Nick",
                Email = "Test@Google.com",
                Age = 20,
                Born = new DateTime(2020, 1, 1)
            };

            var validator = new PlayerValidator();
            var validationResult = validator.Validate(player);

            if (validationResult.IsValid)
            {
                // do somethings ...
            }
            else
            {
                Debug.Log(validationResult.Errors);
            }
        }

        public void Validate_Manually_Fluent()
        {
            var player = new Player
            {
                Name = "Nick",
                Email = "Test@Google.com",
                Age = 20,
                Born = new DateTime(2020, 1, 1)
            };

            var validationResult = ValidatorProvider.For<Player>()
                .RuleFor(x => x.Name, name => !string.IsNullOrWhiteSpace(name), "Name is required")
                .RuleFor(x => x.Age, age => age > 18, "Must be over 18")
                .WithMessage("Age requirement not met")
                .Build()
                .Validate(player);

            if (validationResult.IsValid)
            {
                // do somethings ...
            }
            else
            {
                Debug.Log(validationResult.Errors);
            }
        }

        public void Validate_Via_ValidatorProvider()
        {
            var player = new Player
            {
                Name = "Nick",
                Email = "Test@Google.com",
                Age = 20,
                Born = new DateTime(2020, 1, 1)
            };

            var validationResult = ValidatorProvider.Validate(player, typeof(PlayerValidator));

            if (validationResult.IsValid)
            {
                // do somethings ...
            }
            else
            {
                Debug.Log(validationResult.Errors);
            }
        }

        [Validation(typeof(StudentValidator1))]
        [Validation(typeof(StudentValidator2))]
        public class Student
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public int Age { get; set; }
            public DateTime Born { get; set; }
        }

        public class StudentValidator1 : Validator<Student>
        {
            public override ValidationResult Validate(Student instance)
            {
                var nameValidation = Required(instance.Name, nameof(instance.Name));

                var ageValidation = ValidateRange(instance.Age, 10, 20, nameof(instance.Name));

                var emailValidation = string.IsNullOrEmpty(instance.Name) ?
                    ValidationResult.Failed :
                    instance.Name.IsEmail();

                return ValidateAll(nameValidation, ageValidation, emailValidation);
            }
        }

        public class StudentValidator2 : Validator<Student>
        {
            public static readonly DateTime MinBorn = new(1990, 1, 1);
            public static readonly DateTime MaxBorn = new(2023, 1, 1);

            public override ValidationResult Validate(Student instance)
            {
                return instance.Born.IsInRange(MinBorn, MaxBorn) ?
                    ValidationResult.Ok :
                    ValidationResult.Failed;
            }
        }

        public void Validate_Via_ValidatorProvider_By_ValidatorAttribute()
        {
            var student = new Student
            {
                Name = "Nick",
                Email = "Test@Google.com",
                Age = 20,
                Born = new DateTime(2020, 1, 1)
            };

            var validationResult = ValidatorProvider.Validate(student, false);

            if (validationResult.IsValid)
            {
                // do somethings ...
            }
            else
            {
                Debug.Log(validationResult.Errors);
            }
        }

        public class Customer
        {
            [Required]
            public string Name { get; set; }
            [Required]
            [Email]
            public string Email { get; set; }
            [Range(10, 20)]
            public int Age { get; set; }
            [DateRange("1990-01-01", "2023-01-01")]
            public DateTime Born { get; set; }
        }

        public void Validate_Via_ValidatorProvider_By_PropertyAttribute()
        {
            var customer = new Customer
            {
                Name = "Nick",
                Email = "Test@Google.com",
                Age = 20,
                Born = new DateTime(2020, 1, 1)
            };

            var validationResult = ValidatorProvider.Validate(customer);

            if (validationResult.IsValid)
            {
                // do somethings ...
            }
            else
            {
                Debug.Log(validationResult.Errors);
            }
        }
    }




}
