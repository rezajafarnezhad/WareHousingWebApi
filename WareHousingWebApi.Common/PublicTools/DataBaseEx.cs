using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.AccessControl;

namespace WareHousingWebApi.Common.PublicTools;

public static class DataBaseEx
{
    //public static void RegisterAllEntities(this ModelBuilder builder, Type type)
    //{
    //    var entities = type.Assembly.GetTypes().Where(x => x.BaseType == type);
    //    foreach (var entity in entities)
    //        builder.Entity(entity);
    //}
    public static void VerifyEntities<BaseType>(this ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        //Refilection
        IEnumerable<Type> types = assemblies.SelectMany(a => a.GetExportedTypes())
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(BaseType).IsAssignableFrom(c));

        foreach (var type in types)
            modelBuilder.Entity(type);
    }



    public static string GetEnumDisplayName(this Enum enumValue)
    {
        if (enumValue is null)
            return "";

        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            ?.GetName();
    }

}