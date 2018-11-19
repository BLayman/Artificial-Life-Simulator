using System;
using System.Collections;
using System.Reflection;

public class Utility
{

    public static object getDeepCopy(object toCopy)
    {
        Type sourceType = toCopy.GetType();
        object instance = Activator.CreateInstance(sourceType);
        PropertyInfo[] properties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (PropertyInfo property in properties)
        {
            if (property.CanWrite)
            {
                if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))
                {
                    property.SetValue(instance, property.GetValue(toCopy, null), null);
                }
                else
                {
                    object propertyVal = property.GetValue(toCopy, null);
                    if (propertyVal == null)
                    {
                        property.SetValue(instance, null, null);
                    }
                    else
                    {
                        property.SetValue(instance, getDeepCopy(propertyVal), null);
                    }
                }
            }
        }
        return instance;
    }


}
