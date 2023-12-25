namespace Blog.Common.Constants;

public static class AttributesErrorMessages
{
    public const string RequiredMessage = "لطفا {0} را وارد نمایید";
    public const string MinLengthMessage = "{0} نباید کمتر از {1} کاراکتر باشد";
    public const string MaxLengthMessage = "{0} نباید بیشتر از {1} کاراکتر باشد";
    public const string StringLengthMessage = "{0} باید بین {2} کاراکتر تا {1} کاراکتر باشد";
    public const string RegularExpressionMessage = "{0} را به درستی وارد نمایید";
    public const string RemoteMessage = "{0} قبلا در سیستم ثبت شده است";
    public const string CompareMessage = "{1} با تکرار آن تطابق ندارد";
    public const string RangeMessage = "{0} باید در بازه {1} و {2} باشد";
    public const string EmailAddressMessage = "ایمیل وارد شده نامعتبر است";
    public const string ValidUrl = "لطفا یک Url درست وارد کنید";

    public const string RequiredAcceptRegulationMessage = "پذیرش قوانین و مقررات اجباری است";

    public const string UserNameRegularExpressionMessage = "{0} میتوان شامل حروف انگلیسی، اعداد _ و . باشد";
    public const string PasswordRegularExpressionMessage = "{0} باید شامل یک حرف بزرگ، یک حرف کوچیک، یک عدد، یک کاراکتر خاص و حداقل 8 کاراکتر داشته باشد";
}
