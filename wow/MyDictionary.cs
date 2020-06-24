using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace wow
{

   class MyDictionary
   {
      private const String JsonFile = "hungarian.json";
      private const String TxtFile = "hungarian.txt";
      public List<String> KeyWords = new List<String>();
      public List<String> ExtraWords = new List<String>();

      public MyDictionary()
      {
         Init();
      }

      private void Init()
      {
         if( !File.Exists( JsonFile ) )
         {
            LoadFromTxt();
            SaveToJson();
         }
         else
         {
            LoadFromJson();
         }
      }

      private void LoadFromJson()
      {
         dynamic i = JsonConvert.DeserializeObject( File.ReadAllText( JsonFile ) );
         foreach( var keyWord in i.KeyWords )
         {
            this.KeyWords.Add( keyWord.ToObject<String>() );
         }
         foreach( var extraWord in i.ExtraWords )
         {
            this.ExtraWords.Add( extraWord.ToObject<String>() );
         }
      }

      private void SaveToJson()
      {
         string json = JsonConvert.SerializeObject( this, Formatting.Indented );
         using( var fW = new StreamWriter( JsonFile ) )
         {
            fW.WriteLine( json );
         }
      }

      private List<String> GetWordsWithFilterOneFrom( List<String> lines, bool filter )
      {
         List<String> wordList = new List<String>();
         lines.ForEach( line =>
         {
            if( filter )
            {
               if( !line.Contains( "" ) )
               {
                  return;
               }
               line = line.Remove( line.IndexOf( "" ) );
            }

            var words = line.Split().ToList();
            words.ForEach( word =>
            {
               if( word.Length < 3 )
               {
                  return;
               }
               if( word.Any( c => Char.IsDigit( c ) ) )
               {
                  return;
               }

               while( word.Any( c => @"’~…„[…]»”".Contains( c ) ) )
               {
                  word = word.Remove( word.IndexOfAny( @"’~…„[…]»”".ToCharArray() ) );
               }
               if( word.Any( c => c < 65 ) )
               {
                  word = new String( word.Select( c => c ).Where( c => c >= 65 ).ToArray() );
               }
               if( word.Contains( "|" ) ) //tartoz|ik -> tartozik
               {
                  word = word.Remove( word.IndexOf( "|" ), 1 );
               }

               if( word.Length >= 3 ) // check for length again
               {
                  wordList.Add( word.ToLower() );
               }
            } );
         } );

         wordList = wordList.Distinct().ToList();
         wordList.Sort();
         return wordList;
      }

      public void LoadFromTxt()
      {
         var myShit = File.ReadAllLines( TxtFile ).ToList();

         KeyWords = GetWordsWithFilterOneFrom( myShit, true );
         var AllWords = GetWordsWithFilterOneFrom( myShit, false );

         ExtraWords = AllWords;
         for( int i = 0; i < KeyWords.Count; i++ )
         {
            String s = KeyWords[ i ];
            var index = ExtraWords.FindIndex( word => word == s );
            if( index != -1 )
            {
               ExtraWords.RemoveAt( index );
            }
         }
      }
   }
}
