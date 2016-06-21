using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVote;

namespace HWVote.Models.Validators
{
    public class CustomerValidator : AbstractValidator<UserInfo>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.UserName).NotNull().WithMessage("客户名称不能为空");
            RuleFor(customer => customer.Mobile)
                .NotEmpty().WithMessage("邮箱不能为空")
                .EmailAddress().WithMessage("邮箱格式不正确");
            RuleFor(customer => customer.VsID)
                .NotEqual(0)
                .When(customer => customer.Mobile=="0");
            RuleFor(customer => customer.Hospital)
                .NotEmpty()
                .WithMessage("地址不能为空")
                .Length(20, 50)
                .WithMessage("地址长度范围为20-50字节");
        }
    }
}