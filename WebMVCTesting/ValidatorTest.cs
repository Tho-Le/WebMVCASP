using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using WebMVCDemo.Validator;

namespace WebMVC.Core.Test
{
    public class ValidatorTest
    {
        [Fact]
        public void ValidateNoUpperCaseLetterTest()
        {
            string password  = "password012";

            bool expected = false;

            bool actual = Validator.ValidatePassword(password);

            Assert.Equal(expected, actual);

        }

        [Fact]
        public void ValidateNoDigitTest()
        {
            string password = "lkeksjcidjnejAAAA";

            bool expected = false;

            bool actual = Validator.ValidatePassword(password);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ValidateLessThan8CharsTest()
        {
            string password = "1233Psd";

            bool expected = false;

            bool actual = Validator.ValidatePassword(password);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ValidateCorrectPasswordTest()
        {
            string password = "sowD20pswe";

            bool expected = true;

            bool actual = Validator.ValidatePassword(password);

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void ValidateSamePasswordTest()
        {
            string password1 = "ole2212PP";
            string password2 = "ole2212PP";

            bool expected = true;

            bool actual = Validator.SamePassword(password1, password2);

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void ValidateNotSamePasswordTest()
        {
            string password1 = "kieppp1P";
            string password2 = "ke92hhsncM";

            bool expected = false;

            bool actual = Validator.SamePassword(password1, password2);

            Assert.Equal(expected, actual);
        }
    }
}
