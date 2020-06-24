using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace wow
{
   class Program
   {
      static void Main( string[] args )
      {
         var wowSorter = new WowSorter();

         ConsoleKey response;
         do
         {
            Console.Clear();
            Console.Write( "Pls provide scrambled letters: " );
            wowSorter.ProcessLetters( Console.ReadLine() );
            Console.WriteLine();
            Console.WriteLine( "Keywords:" );
            Console.WriteLine( wowSorter.GetKeyWords() );
            Console.WriteLine( "Extra words:" );
            Console.WriteLine( wowSorter.GetExtraWords() );

            do //https://stackoverflow.com/questions/37359161/how-would-i-make-a-yes-no-prompt-in-console-using-c
            {
               Console.Write( "Would you like to start a new search? [y/n] " );
               response = Console.ReadKey( false ).Key;   // true is intercept key (dont show), false is show
               if( response != ConsoleKey.Enter )
               {
                  Console.WriteLine();
               }
            } while( response != ConsoleKey.Y && response != ConsoleKey.N );
            Console.WriteLine();
         } while( response == ConsoleKey.Y );
      }
   }
}
