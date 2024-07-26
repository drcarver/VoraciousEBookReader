namespace VoraciousEBookReader.EPUB.Enum
{
    public enum UserStatus
    {
        /// <summary>
        /// No user-set status. Might be downloaded, but might not (there's a file status to tell that)
        /// </summary>
        NoStatus,
        /// <summary>
        /// Being read in the Voracious Reader
        /// </summary>
        Reading,
        /// <summary>
        /// User completed reading the book to the end. The User's 'end' might not be the computer's end: they might well skip the index, or back cover, or gutengerb license.
        /// 
        /// </summary>
        Done,
        /// <summary>
        /// User doesn't want to see the book again, but they don't think they completed it -- e.g., because the book didn't match their taste, and time is too short to waste on reading a book you don't want to. 
        /// </summary>
        Abandoned,
        /// <summary>
        /// Book has been copied to e.g. a Nook. The user won't want to see it in Voracious (because they are reading it elsewhere)
        /// </summary>
        CopiedToEBookReader
    }
}
