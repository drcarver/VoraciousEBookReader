using System;
using System.Collections.Generic;
using System.Text;

namespace VoraciousEBookReader.EPUB.Enum
{
    [Flags]
    enum LinqIncludes
    {
        None = 0,
        Notes = 0x01, 
        Review = 0x02, 
        DownloadData = 0x04, 
        NavigationData = 0x08,
        People = 0x10,
        LanguageExact = 0x20, 
        LanguageExactOrNull = 0x40,
        Files = 0x80,
        UserData = 0x0F, // don't include anything not in the Gutenberg catalog
        LanguagesFlags = 0x60, // all of the language flags; generally only do one!
    }
}
