using System.ComponentModel;

namespace TT.Core.Domain.Entities;

public class NotificationRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string To { get; set; }
    public Template Template { get; set; }

    public NotificationRequest(string title, string to, Template template)
    {
        Title = title;
        To = to;
        Template = template;
    }
}

public class Template
{
    public Template(TemplateType type, Dictionary<string, string> tags)
    {
        Type = type;
        Tags = tags;
    }

    public TemplateType Type { get; set; }
    public Dictionary<string, string> Tags { get; set; }
}

public enum TemplateType
{
    [Description("Default.html")]
    Default = 0,

    [Description("Active-account.html")]
    ActiveAccount = 1,

    [Description("Want-trade.html")]
    WantTrade = 2,

    [Description("Reset-password.html")]
    ResetPassword = 3,
}

public static class MyEnumExtensions
{
    public static string ToDescriptionString(this TemplateType type)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])type.GetType().GetField(type.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }
}
