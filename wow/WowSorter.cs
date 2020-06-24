using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wow
{
   class LetterAttribute
   {
      public char c;
      public uint instances;
      public LetterAttribute( char _c, uint _instances )
      {
         c = _c;
         instances = _instances;
      }
   }
   class WowSorter
   {
      public List<String> KeyWords = new List<String>();
      public List<String> ExtraWords = new List<String>();
      private MyDictionary MyHunDictionary;
      private String ScrambledLetters;
      private void LoadDictionary()
      {
         MyHunDictionary = new MyDictionary();
      }

      private String GetSpaceSeparatedListOfWords( List<String> words )
      {
         String text = "";
         words.GroupBy( match => match.Length )
            .OrderBy( x => x.FirstOrDefault().Length )
            .ToList()
            .ForEach( wordList =>
                   {
                      text += wordList.FirstOrDefault().Length + " | ";
                      wordList.ToList().ForEach( word => text += word + " " );

                      if( text.Any() )
                      {
                         text.Remove( text.Length - 1 );
                      }
                      text += "\n";
                   } );
         return text;
      }

      public String GetKeyWords()
      {
         return GetSpaceSeparatedListOfWords( KeyWords );
      }

      public String GetExtraWords()
      {
         return GetSpaceSeparatedListOfWords( ExtraWords );
      }

      private List<String> GetMatchesFrom( List<String> words )
      {
         List<String> matches = new List<String>();
         List<LetterAttribute> inputAttributes = GetAttributeOf( ScrambledLetters );

         words.ForEach( word =>
         {
            var wordAttributes = GetAttributeOf( word );
            if( DictionaryItemOKByAttrib( wordAttributes, inputAttributes ) )
            {
               matches.Add( word );
            }
         } );
         return matches;
      }
      private bool DictionaryItemOKByAttrib( List<LetterAttribute> checkedWord, List<LetterAttribute> setOfLetters )
      {
         if( checkedWord.Count > setOfLetters.Count )
         {
            return false; //több karakter a szótári szóban
         }
         if( checkedWord.Any( checkedChar => !setOfLetters.Any( setAttr => setAttr.c == checkedChar.c ) ) )
         {
            return false;
         }
         if( checkedWord.Any( checkedChar
            => ( setOfLetters.Find( setChar => setChar.c == checkedChar.c ).instances < checkedChar.instances ) ) )
         {
            return false;
         }
         return true;
      }
      private List<LetterAttribute> GetAttributeOf( String chars )
      {
         List<LetterAttribute> characterDb = new List<LetterAttribute>();
         foreach( var letter in chars )
         {
            var index = characterDb.FindIndex( c => c.c == letter );

            if( -1 == index )
            {
               characterDb.Add( new LetterAttribute( letter, 1 ) );
            }
            else
            {
               characterDb[ index ].instances++;
            }
         }

         characterDb = characterDb.OrderBy( c => c.c ).ToList();
         return characterDb;
      }
      public WowSorter()
      {
         LoadDictionary();
      }

      public void ProcessLetters( String letters )
      {
         ScrambledLetters = letters;
         KeyWords = GetMatchesFrom( MyHunDictionary.KeyWords );
         ExtraWords = GetMatchesFrom( MyHunDictionary.ExtraWords );
      }
   }
}
