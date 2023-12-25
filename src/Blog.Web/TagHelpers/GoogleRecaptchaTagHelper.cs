using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Blog.Web.TagHelpers;

[HtmlTargetElement("GoogleRecaptchaHelper")]
public class GoogleRecaptchaTagHelper : TagHelper
{
    private readonly IConfiguration _configuration;

    [HtmlAttributeName("asp-id")]
    public string Id { get; set; }

    [HtmlAttributeName("asp-callback")]
    public string Callback { get; set; }
    public GoogleRecaptchaTagHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var clienKeyValue = _configuration["GoogleReCaptcha:ClientKey"];
        if (context is null) throw new ArgumentNullException(nameof(context));
        if (output is null) throw new ArgumentNullException(nameof(output));
        if (Id is null) throw new ArgumentNullException(nameof(Id));
        if (Callback is null) throw new ArgumentNullException(nameof(Callback));

        output.TagName = "";
        output.TagMode = TagMode.StartTagAndEndTag;

        var loadingTagBuilder = new TagBuilder("div");
        loadingTagBuilder.AddCssClass("loader");
        loadingTagBuilder.MergeAttribute("Id", "captcha-Loading");

        var captchaTagBuilder = new TagBuilder("div");
        captchaTagBuilder.MergeAttribute("data-sitekey", clienKeyValue);
        captchaTagBuilder.MergeAttribute("data-callback", Callback);
        captchaTagBuilder.GenerateId(Id, string.Empty);
        captchaTagBuilder.AddCssClass("g-recaptcha");

        var spanTagBuilder = new TagBuilder("span");
        spanTagBuilder.AddCssClass("text-danger");
        spanTagBuilder.MergeAttribute("data-valmsg-for", "GoogleReCaptchaResponse");

        output.Content.AppendHtml(loadingTagBuilder);
        output.Content.AppendHtml(captchaTagBuilder);
        output.Content.AppendHtml(spanTagBuilder);
    }
}
