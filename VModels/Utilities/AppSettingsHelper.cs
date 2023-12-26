using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VModels.Utilities;
public class AppSettingsHelper
{
    #region Sections
    public const string ConnectionString = "ConnectionString";
    #endregion

    #region Property
    public const string DefaultConnection = "DefaultConnection";
    #endregion

    public static List<string> GetKeys()
    {
        Type type = typeof(AppSettingsHelper);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

        List<string> constantValues = new List<string>();

        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(string) && field.IsLiteral && !field.IsInitOnly)
            {
                string constantValue = (string)field.GetValue(null);
                constantValues.Add(constantValue);
            }
        }

        return constantValues;
    }

    public static bool UpdateSection(string filePath, string section, string newValue, string key)
    {
        string jsonData = File.ReadAllText(filePath);
        JObject jsonSettings = JObject.Parse(jsonData);
        UpdateJsonProperty(jsonSettings, section, key, newValue);
        string updatedJsonData = jsonSettings.ToString();
        File.WriteAllText(filePath, updatedJsonData);

        return true;
    }

    private static void UpdateJsonProperty(JObject json, string section, string key, string value)
    {
        JToken sectionToken = json.SelectToken(section) ?? throw new ArgumentException($"Section '{section}' not found in the JSON.");
        JToken token = sectionToken.SelectToken(key) ?? throw new ArgumentException($"Property with key '{key}' not found in the JSON.");
        token.Replace(new JValue(value));
    }
}
