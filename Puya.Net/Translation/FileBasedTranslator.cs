using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Puya.Caching;
using Puya.Localization;
using Puya.Logging;

namespace Puya.Translation
{
    public class FileBasedTranslator : BaseTranslator
    {
        public FileBasedTranslator()
        { }
        public FileBasedTranslator(ICache cache): this(cache, null)
        { }
        public FileBasedTranslator(ILogger logger) : this(null, logger)
        { }
        public FileBasedTranslator(ICache cache, ILogger logger) : this(cache, logger, null)
        { }
        public FileBasedTranslator(ICache cache, ILogger logger, ILanguageProvider languageProvider) : base(cache, logger, languageProvider)
        { }
        private string _basePath;
        public string BasePath
        {
            get
            {
                return _basePath;
            }
            set
            {
                _basePath = value;
                loaded = false;
            }
        }
        protected virtual string GetCategoryOf(string filenameAndPath)
        {
            return Path.GetFileNameWithoutExtension(filenameAndPath);
        }
        protected override void LoadInternal()
        {
            Logger.Debug($"Loading Path: {BasePath}");

            var files = Directory.GetFiles(BasePath, $"*{Options.CultureDependentTextExtension}", SearchOption.AllDirectories);

            Logger.Debug($"culture dependent files: {files.Length}");

            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                var category = GetCategoryOf(file);

                Logger.Debug($"Loading File: {file}");

                try
                {
                    ProcessCultureDependentText(category, content);
                }
                catch (Exception e)
                {
                    Logger.Debug($"error loading {file}");
                    Logger.Danger(e, $"{Name}.Load(): ProcessCultureDependentText('{file}')");
                }
            }

            files = Directory.GetFiles(BasePath, $"*{Options.CultureIndependentTextExtension}", SearchOption.AllDirectories);

            Logger.Debug($"culture independent files: {files.Length}");

            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                var category = GetCategoryOf(file);

                Logger.Debug($"Loading File: {file}");

                try
                {
                    ProcessCultureIndependentText(category, content);
                }
                catch (Exception e)
                {
                    Logger.Debug($"error loading {file}");
                    Logger.Danger(e, $"{Name}.Load(): ProcessCultureIndependentText('{file}')");
                }
            }
        }
    }
}
