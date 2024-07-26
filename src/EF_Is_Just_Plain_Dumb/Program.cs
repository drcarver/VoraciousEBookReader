
using System;

namespace EF_Is_Just_Plain_Dumb
{
#if DOC_HOW_TO_BUILD_DATABASE

Steps to create a new database with a new scheme

Update database schema
1. This only works in the x64 version -- the x86 does not work, ever, and will always crash.
2. Save your user data!
3. Add the new fields as needed
4. Arguably, delete the old migration files from the reader project and the ef_is_dumb project
5. Select the EF_IS_DUMB project as the startup in both places
6. In the Package Manager Console, run add-migration -Context BooksContext CreateBooks
   If you really are making a migration, don't call it CreateBooks!
7. Set the ebook reader as the startup project again
7. re-add the migrations
8. Turn off the auto-copy of the zipped database (InitializeFilesToGet.cs, CopyAssetDatabaseIfNeededAsync, var copyFileAsNeeded
9. VAR: localstate is directory C:\Users\toomr\AppData\Local\Packages\48425ShipwreckSoftware.558159B35D87A_mdsdtv3kgmcha\LocalState
   C:\Users\toomr\AppData\Local\Packages\48425ShipwreckSoftware.558159B35D87A_mdsdtv3kgmcha\LocalState
10. In localstate, delete the InitialBooks.db and .zip files and the Books.db file
11. Run the ebook reader. It will create a new tiny empty Books.db file
12. Run the Create Database extra command. The file to feed it is a rd-files.tar.zip file
    This makes a new InitialBooks.db file that includes all of the preinstalled books.
13. Exit the ebook reader
14. Undo the step 8 :-)
14. Delete the Books.db file
15. Rename InitialBooks.db to Books.db and Zip it to file InitialBooks.zip file (because unzipped, it's too big for github).
16. Copy the InitialBooks.db file into the Assets folder.
17. Delete the bookdb.db file from localstate
18. Run the ebook reader app. It will copy over the new book database
19. Re-install the user data!


C:\Users\toomr\AppData\Local\Packages\48425ShipwreckSoftware.558159B35D87A_mdsdtv3kgmcha\LocalState
InitialBooks.db --> .zip

#endif
    class Program
    {
        /// <summary>
        /// The program is never actually run.
        /// Run these commands in the Package Manager Console
        ///     add-migration -Context BooksContext CreateBooks
        ///     
        /// Add the new files (as links) in the SimpleEpubReader / Database / Migrations
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var bookdb = SimpleEPUBReader.Database.BooksContext.Get();
            bookdb.DoMigration();
            // Not needed any more? SimpleEpubReader.Database.UserDataContext.Initialize();

            Console.WriteLine("Hello World!");
        }
    }
}
