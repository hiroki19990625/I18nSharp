using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace I18nSharp.Content
{
    public static class LanguageFileContentFactory
    {
        private static readonly Dictionary<string, Func<LanguageFileContent>> _factory =
            new Dictionary<string, Func<LanguageFileContent>>();

        static LanguageFileContentFactory()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type[] targets = assemblies.SelectMany(asm => asm.GetTypes())
                .Where(t => t.BaseType == typeof(LanguageFileContent)).ToArray();
            foreach (Type type in targets)
            {
                _factory[type.Name] = Expression
                    .Lambda<Func<LanguageFileContent>>(
                        Expression.New(
                            type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes,
                                null) ??
                            throw new InvalidOperationException()))
                    .Compile();
            }
        }

        public static LanguageFileContent GetLanguageFileContent(string name)
        {
            return _factory[name]();
        }

        public static LanguageFileContent GetLanguageFileContent(Type type)
        {
            return _factory[type.Name]();
        }

        public static T GetLanguageFileContent<T>(string name) where T : LanguageFileContent
        {
            return (T) _factory[name]();
        }

        public static T GetLanguageFileContent<T>() where T : LanguageFileContent
        {
            return (T) _factory[typeof(T).Name]();
        }
    }
}