using System;

namespace Puya.Localization
{
    public interface ILanguageProvider
    {
        string GetCurrent();
        string[] GetSupported();
        string[] GetAll();
    }
}
