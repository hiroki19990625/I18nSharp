using System;
using System.Globalization;
using System.IO;
using I18nSharp.Writer.Content;
using NUnit.Framework;

namespace I18nSharp.Tests
{
    [TestFixture]
    public class I18nSimpleTests
    {
        [SetUp]
        public void Setup()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        [Test, Order(0)]
        public void CreateLanguageFile()
        {
            Writer.LanguageFile languageFile = new Writer.LanguageFile();
            languageFile.LanguageFileDictionary.CultureString = CultureInfo.CurrentCulture.Name;
            languageFile.LanguageFileDictionary.LanguageFileContents["test_msg"] =
                new LanguageFileText("Hello World!!");
            languageFile.SaveFile(new FileInfo("Language-ja.json"));
        }

        [Test, Order(1)]
        public void LoadLanguageFile()
        {
            LanguageFile languageFile = new LanguageFile(new FileInfo("Language-ja.json"));
            Assert.True(
                (languageFile.LanguageFileDictionary.LanguageFileContents["test_msg"] as Content.LanguageFileText)
                ?.Content ==
                "Hello World!!");
        }
    }
}