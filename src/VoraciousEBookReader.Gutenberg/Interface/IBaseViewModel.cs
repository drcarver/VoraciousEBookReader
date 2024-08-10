using System;
using System.Collections.Generic;
using System.Text;

namespace VoraciousEBookReader.Gutenberg.Interface;

public interface IBaseViewModel
{
    /// <summary>
    /// The page title
    /// </summary>
    string PageTitle { get; set; }
}
