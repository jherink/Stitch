using System;
using System.Collections;
using System.Collections.Generic;
using Stitch.Fonts.Tables;

namespace Stitch.Fonts
{
    public sealed class GlyphNames : IReadOnlyList<string>
    {
        private readonly List<string> Names = new List<string>();

        internal GlyphNames( PostFontTable post )
        {
            if ( Math.Abs( post.Version - 1.0 ) < .0001 )
            {
                Names.AddRange( post.Names ); // this should be "standard names"
            }
            else if ( Math.Abs( post.Version - 2.0 ) < .0001 )
            {
                for ( var i = 0; i < post.NumGlyphs; i++ )
                {
                    if ( post.GlyphNameIndex[i] < Encodings.StandardNames.Length )
                    {
                        Names.Add( Encodings.StandardNames[post.GlyphNameIndex[i]] );
                    }
                    else
                    {
                        Names.Add( post.Names[post.GlyphNameIndex[i] - Encodings.StandardNames.Length] );
                    }
                }
            }
            else if ( Math.Abs( post.Version - 2.5 ) < .0001 )
            {
                for ( var i = 0; i < post.NumGlyphs; i++ )
                {
                    Names.Add( Encodings.StandardNames[i + post.GlyphNameIndex[i]] );
                }
            }
        }

        public int NameToGlyphIndex( string name )
        {
            return Names.IndexOf( name );
        }

        public string this[int index] { get { return Names[index]; } }

        public int Count { get { return Names.Count; } }

        public IEnumerator<string> GetEnumerator()
        {
            return Names.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
