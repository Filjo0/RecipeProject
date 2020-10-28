using System;
using System.Globalization;
using System.Windows.Controls;

namespace RecipeAssignment.Utils.ValidationRules
{
    public class RequiredValidationRule : ValidationRule
    {
        private readonly string _recipeName;

        private string _description;

        private TimeSpan _cookingTime;

        private bool _isFavorite;

        private string _ingredients;

        public RequiredValidationRule(string fieldName, string recipeName, string description, bool isFavorite, string ingredients)
        {
            FieldName = fieldName;
            _recipeName = recipeName;
            _description = description;
            _isFavorite = isFavorite;
            _ingredients = ingredients;
        }

        private const int Max = 100;

        private static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            var errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = $"You cannot leave the {fieldName} field empty.";
            if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                errorMessage = $"You cannot leave the {fieldName} field empty.";
            return errorMessage;
        }

        private string FieldName { get; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (value != null)
                {
                    if (((string) value).Length > 0)
                    {
                        _cookingTime = TimeSpan.Parse((string) value);
                    }
                }
                else
                {
                    return new ValidationResult(false, "Illegal input for Cooking Time. (eg.: 00:30)");
                }
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Illegal input for Cooking Time. (eg.: 00:30)");
            }

            try
            {
                if (_recipeName != null)
                {
                    if (_recipeName.Length > Max)
                    {
                        return new ValidationResult(false, "Illegal input. Too long!");
                    }
                }
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Illegal input for Cooking Time. (eg.: 00:30)");
            }

            var error = GetErrorMessage(FieldName, value);
            return !string.IsNullOrEmpty(error) ? new ValidationResult(false, error) : ValidationResult.ValidResult;
        }
    }
}