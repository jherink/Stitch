using Stitch.Attributes;

namespace Stitch.Elements
{
    public enum ListStyleType
    {
        [StitchCssResource( "stitch-list-type-decmial" )]
        Numbered,
        [StitchCssResource( "stitch-list-type-decimal-leading-zero" )]
        DecimalLeadingZero,
        [StitchCssResource( "stitch-list-type-upper-alpha" )]
        UppercaseLetter,
        [StitchCssResource( "stitch-list-type-lower-alpha" )]
        LowercaseLetter,
        [StitchCssResource( "stitch-list-type-upper-roman" )]
        UppercaseRomanNumeral,
        [StitchCssResource( "stitch-list-type-lower-roman" )]
        LowercaseRomanNumeral,
        [StitchCssResource( "stitch-list-type-none" )]
        None,
        [StitchCssResource( "stitch-list-type-lower-greek" )]
        LowercaseGreek,
        [StitchCssResource( "stitch-list-type-upper-greek" )]
        UppercaseGreek,
        [StitchCssResource( "stitch-list-type-armenian" )]
        Armenian,
        [StitchCssResource( "stitch-list-type-georgian" )]
        Georgian,
        [StitchCssResource( "stitch-list-type-disc" )]
        Disc,
        [StitchCssResource( "stitch-list-type-circle" )]
        Circle,
        [StitchCssResource( "stitch-list-type-square" )]
        Square,
    }

    public enum UnorderedListStyleType
    {
        //[SetStyleType( "none" )]
        [StitchCssResource( "stitch-list-type-none" )]
        None,
        //[SetStyleType( "disc" )]
        [StitchCssResource( "stitch-list-type-disc" )]
        Disc,
        //[SetStyleType( "circle" )]
        [StitchCssResource( "stitch-list-type-circle" )]
        Circle,
        //[SetStyleType( "square" )]
        [StitchCssResource( "stitch-list-type-square" )]
        Square,
        [StitchCssResource( "stitch-list-type-upper-alpha" )]
        UppercaseLetter,
    };

    public enum OrderedListStyleType
    {
        //[SetStyleType( "1" )]
        [StitchCssResource( "stitch-list-type-decmial" )]
        Numbered,
        [StitchCssResource( "stitch-list-type-decimal-leading-zero" )]
        DecimalLeadingZero,
        //[SetStyleType( "A" )]
        [StitchCssResource( "stitch-list-type-upper-alpha" )]
        UppercaseLetter,
        //[SetStyleType( "a" )]
        [StitchCssResource( "stitch-list-type-lower-alpha" )]
        LowercaseLetter,
        //[SetStyleType( "I" )]
        [StitchCssResource( "stitch-list-type-upper-roman" )]
        UppercaseRomanNumeral,
        //[SetStyleType( "i" )]
        [StitchCssResource( "stitch-list-type-lower-roman" )]
        LowercaseRomanNumeral,
        //[SetStyleType( "none" )]
        [StitchCssResource( "stitch-list-type-none" )]
        None,
        [StitchCssResource( "stitch-list-type-lower-greek" )]
        LowercaseGreek,
        [StitchCssResource( "stitch-list-type-upper-greek" )]
        UppercaseGreek,
        [StitchCssResource( "stitch-list-type-armenian" )]
        Armenian,
        [StitchCssResource( "stitch-list-type-georgian" )]
        Georgian,

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
