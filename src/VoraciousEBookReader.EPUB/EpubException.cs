using System;

namespace VoraciousEBookReader.EPUB
{
    public class EPUBException : Exception
    {
        public EPUBException(string message) : base(message) { }
    }

    public class EPUBParseException : EPUBException
    {
        public EPUBParseException(string message) : base($"EPUB parsing error: {message}") { }
    }

    public class EPUBWriteException : EPUBException
    {
        public EPUBWriteException(string message) : base($"EPUB write error: {message}") { }
    }
}
