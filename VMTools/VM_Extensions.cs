using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;

namespace VirtualMachine.VMTools
{
    public static class VM_Extensions
    {
        public static int EnumCount<T>() => Enum.GetValues(typeof(T)).Length;

        public static List<T> GetEnums<T>() => Enum.GetValues(typeof(T)).Cast<T>().ToList();

        public static List<TFinal> ZipEnums<TFirst, TSecond, TFinal>(int i = -1) =>
            GetEnums<TFirst>().Zip(Enumerable.Repeat((TSecond)Activator.CreateInstance(typeof(TSecond)), i > 0 ? i : EnumCount<TFirst>()), (e, o) =>
                (TFinal)Activator.CreateInstance(typeof(TFinal), e, o)).ToList();

        public static Dictionary<TFirst, TSecond> ZipEntries<TFirst, TSecond>(int i = -1) =>
            GetEnums<TFirst>().Zip(Enumerable.Repeat((TSecond)Activator.CreateInstance(typeof(TSecond)), i > 0 ? i : EnumCount<TFirst>()), (k, v) =>
                new { k, v }).ToDictionary(kvp => kvp.k, kvp => kvp.v);


        public static bool IsIn<T>(this T obj, params T[] collection) => collection.Contains(obj);

        public static bool Is<T>(this T obj, params T[] collection) => collection.Any(o => o.Equals(obj));

        public static T[] SubArray<T>(this T[] data, int index, int length) => data.Skip(index).Take(length).ToArray();

        public static List<Attribute> GetCustomAttributes<T>(T obj) => GetAttributes(obj).GetCustomAttributes().ToList();
        public static List<Attribute> GetCustomAttributes(object obj) => GetAttributes(obj).GetCustomAttributes().ToList();
        public static IEnumerable<CustomAttributeData> GetCustomAttributeData(object obj) => GetAttributes(obj).CustomAttributes;
        public static FieldInfo GetAttributes(object obj) => obj.GetType().GetField(obj.ToString());
        public static FieldInfo GetAttributes<T>(T obj) => obj.GetType().GetField(obj.ToString());
        public static List<T> FilterByCustomAttribute<T>(List<T> obj, string description) => obj.Where(o => SelectByCustomAttribute(o, description)).ToList();
        public static T[] FilterByCustomAttribute<T>(string description) => GetEnums<T>().Where(o => SelectByCustomAttribute(o, description)).ToArray();
        //public static List<T> FilterByCustomAttributeAndDescription<T>(string category, string description) =>
        //    FilterByCustomAttribute<T>(category).Where(e => GetAttributes(e).CustomAttributes.Any(a => a.ConstructorArguments.Any(c => ((string)c.Value).Contains(description)))).ToList();
        
        public static T[] FilterByCustomAttributeAndDescription<T>(string category, string description) => 
            FilterByCustomAttribute<T>(category).Where(e => SelectByCustomDescription(e, description)).ToArray();

        public static bool SelectByCustomDescription<T>(T obj, string description) => GetCustomAttributeData(obj).Any(o => HasDescription(o, description));

        public static List<List<Attribute>> GetCustomAttributes<T>(List<T> obj) => obj.Select(o => GetCustomAttributes(o)).ToList();

        public static bool HasDescription(CustomAttributeData data, string description) => data.ConstructorArguments.Any(c => ((string)c.Value).Contains(description));
        public static bool SelectByCustomAttribute<T>(T obj, string description) => GetCustomAttributes(obj).Any(a => a.GetType().Name.Contains(description));   

    
    }
}
