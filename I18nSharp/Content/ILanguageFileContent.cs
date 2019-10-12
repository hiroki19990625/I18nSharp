namespace I18nSharp
{
    public interface ILanguageFileContent<out T> where T : class
    {
        T Content { get; }
    }
}