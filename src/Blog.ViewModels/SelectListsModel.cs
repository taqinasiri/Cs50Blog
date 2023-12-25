using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.ViewModels;
public static class SelectListsModel
{
    public static List<SelectListItem> AllTrueFalse(string allDisplayName, string trueDisplayName, string falseDisplayName)
    {
        return new List<SelectListItem>()
        {
            new SelectListItem(allDisplayName,AllTrueFalseEnum.All.ToString()),
            new SelectListItem(trueDisplayName,AllTrueFalseEnum.Yes.ToString()),
            new SelectListItem(falseDisplayName,AllTrueFalseEnum.No.ToString())
        };
    }
}
public enum AllTrueFalseEnum { All, Yes, No }