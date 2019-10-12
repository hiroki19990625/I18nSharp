namespace I18nSharp.Writer.Content
{
    public interface ILanguageFileContent<T> where T : class
    {
        T Content { get; set; }
    }
}