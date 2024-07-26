using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB.Interfaces
{
    public interface IPerson
    {
        /// <summary>
        /// The person Id
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// The person name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Aliases is stored as a series of + separated names
        /// So it could be "john jones" or "john jones+samantha sams"
        /// Never fiddle with the value directly! Use the AddAlias to add each alias.
        /// </summary>
        string Aliases { get; set; }

        /// <summary>
        /// The relation of the person to the book
        /// e.g. aut=author ill=illustrator from id.
        /// loc.gov/vocabulary/relators.html
        /// </summary>
        Relator PersonType { get; set; }

        /// <summary>
        /// The person's date of birth
        /// </summary>
        int BirthDate { get; set; }

        /// <summary>
        /// The date of th persons death
        /// </summary>
        int DeathDate { get; set; }

        /// <summary>
        /// The web page for the person
        /// </summary>
        string Webpage { get; set; }

        /// <summary>
        /// Return the importance of the person to the EPUB
        /// </summary>
        /// <returns>The integer importance of the person to the EPUB</returns>
        int GetImportance();
    }
}