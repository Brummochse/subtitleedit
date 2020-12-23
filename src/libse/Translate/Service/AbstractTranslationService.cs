﻿using System;
using System.Collections.Generic;
using System.Text;
using Nikse.SubtitleEdit.Core.Common;

namespace Nikse.SubtitleEdit.Core.Translate.Service
{
    public abstract class AbstractTranslationService : ITranslationStrategy
    {
        private bool? _initializationResult;

        /// <summary>
        /// can be used to transmit log messages (e.g. during initialization or translation)
        /// </summary>
        public event EventHandler<string> MessageLogEvent;

        public abstract List<TranslationPair> GetSupportedSourceLanguages();
        public abstract List<TranslationPair> GetSupportedTargetLanguages();
        public abstract string GetName();
        public abstract string GetUrl();
        public abstract int GetMaxTextSize();
        public abstract int GetMaximumRequestArraySize();

        /// <summary>
        /// possibility for initialization tasks.
        /// can be called directly (or gets called indirectly on first translation call)
        /// </summary>
        /// <returns>false, when initialization failed</returns>
        public bool Initialize()
        {
            if (_initializationResult == null)
            {
                _initializationResult = DoInitialize();
            }
            return _initializationResult.Value;
        }

        protected abstract bool DoInitialize();

        /// <exception cref="TranslationException">thrown when initialization failed or specific translation errors occur</exception>
        public List<string> Translate(string sourceLanguage, string targetLanguage, List<Paragraph> sourceParagraphs)
        {
            if (Initialize() == false)
            {
                throw new TranslationException("initialization failed");
            }
            return DoTranslate(sourceLanguage, targetLanguage, sourceParagraphs);
        }

        protected abstract List<string> DoTranslate(string sourceLanguage, string targetLanguage, List<Paragraph> sourceParagraphs);

        protected virtual void OnMessageLogEvent(string e)
        {
            MessageLogEvent?.Invoke(this, e);
        }
    }
}