using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace cgwo.Validation
{
	public class FileNameValidation : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			var filename = value as string;
			if (String.IsNullOrEmpty(filename))
				return ValidationResult.ValidResult;

			var badCharacters = System.IO.Path.GetInvalidFileNameChars();
			if(filename.Any(c => badCharacters.Contains(c)))
					return new ValidationResult(false, "");

			return ValidationResult.ValidResult;
		}
	}

	public class FolderPathValidation : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			var filename = value as string;
			if (String.IsNullOrEmpty(filename))
				return ValidationResult.ValidResult;

			var badCharacters = System.IO.Path.GetInvalidPathChars();
			if (filename.Any(c => badCharacters.Contains(c)))
					return new ValidationResult(false, "");

			return ValidationResult.ValidResult;
		}
	}
}
