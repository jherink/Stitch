using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Elements
{
    public enum UnorderedListStyleType
    {
        [SetStyleType( "none" )]
        None,
        [SetStyleType( "disc" )]
        Disc,
        [SetStyleType( "circle" )]
        Circle,
        [SetStyleType( "square" )]
        Square
    };

    public enum OrderedListStyleType
    {
        [SetStyleType( "1" )]
        Numbered,
        [SetStyleType( "A" )]
        UppercaseLetter,
        [SetStyleType( "a" )]
        LowercaseLetter,
        [SetStyleType( "I" )]
        UppercaseRomanNumeral,
        [SetStyleType( "i" )]
        LowercaseRomanNumeral
    };

    public enum HeadingLevel
    {
        H1,
        H2,
        H3,
        H4,
        H5,
        H6
    };
}
