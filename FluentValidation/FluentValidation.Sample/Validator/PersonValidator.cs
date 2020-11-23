using FluentValidation;
using FluentValidation.Sample.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentValidation.Sample.Validator
{
	public class PersonValidator : AbstractValidator<Person>
	{
		public PersonValidator()
		{
			RuleFor(x => x.Id).NotNull();
			RuleFor(x => x.Name).Length(0, 10);
			RuleFor(x => x.Email).EmailAddress();
			RuleFor(x => x.Age).InclusiveBetween(18, 120).WithMessage("年龄只能在18到120岁之间！");
		}
	}

}
