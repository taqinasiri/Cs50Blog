using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Blog.Common.Extensions;
public static class ReflectionExtensions
{
    public static List<T> GetAllPublicConstantValues<T>(this Type type)
    => type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
           .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
           .Select(x => (T)x.GetRawConstantValue()).ToList();

    public static List<AreaControllerActionName> GetAreaControllerActionNames(this Assembly asm)
    {
        var controlleractionlist = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Select(x => new
                {
                    Controller = x.DeclaringType.Name,
                    Action = x.Name,
                    Area = x.DeclaringType.CustomAttributes.Where(c => c.AttributeType == typeof(AreaAttribute)),
                }).Distinct().ToList();

        var result = new List<AreaControllerActionName>();
        foreach (var item in controlleractionlist)
        {
            var newItem = new AreaControllerActionName()
            {
                Action = item.Action,
                Controller = item.Controller.Replace("Controller",null),
                Area = item.Area.Count() == 0 ? null : item.Area.Select(v => v.ConstructorArguments[0].Value.ToString()).FirstOrDefault()
            };
            if (!result.Any(r => r.Area == newItem.Area && r.Controller == newItem.Controller && r.Action == newItem.Action))
            {
                result.Add(newItem);
            }
        }
        return result.ToList();
    }
}

public class AreaControllerActionName
{
    public string Controller { get; set; }
    public string Action { get; set; }
    public string Area { get; set; }
    public string GetAll
    {
        get => $"{Area}|{Controller}|{Action}";
    }
}
