using System;
using HydraDoc.Elements;
using HydraDoc.Elements.Interface;
using System.Collections.Generic;
using System.Text;

namespace HydraDoc
{
    public struct DOMString : IElement, ICloneable
    {
        public ClassList ClassList { get; set; }

        public StyleList StyleList { get; set; }

        public IDictionary<string, string> Attributes { get; private set; }

        public string ID { get; set; }

        public string Tag { get; set; }

        public string Title { get; set; }

        public string Text { get { return Render(); } }

        public ICollection<IElement> Elements { get; private set; }

        public string Render()
        {
            var builder = new StringBuilder();
            foreach (var element in Elements)
            {
                builder.Append( element.Render() );
            }
            return builder.ToString();
        }

        public DOMString( string content ) : this( new PlainText( content ) )
        {
        }

        public DOMString( int value ) : this( value.ToString() ) { }

        public DOMString( double value ) : this( value.ToString() ) { }

        public DOMString( params IElement[] content )
        {
            ID = string.Empty;
            Tag = string.Empty;
            Title = string.Empty;
            ClassList = new ClassList();
            StyleList = new StyleList();
            Attributes = new Dictionary<string, string>();
            Elements = new List<IElement>();
            foreach (var item in content)
            {
                Elements.Add( item );
            }
        }

        //public static DOMString Create()
        //{
        //    var str = new DOMString();
        //    str.ID = str.Tag = str.Title = string.Empty;
        //    str.ClassList = new ClassList();
        //    str.Elements = new List<IElement>();
        //    return str;
        //}

        public void Append( string content )
        {
            AppendElement( new PlainText( content ) );
        }

        public void AppendElement( IElement content )
        {
            if (Elements == null) Elements = new List<IElement>();
            Elements.Add( content );
        }


        #region Implicit Operators With string

        public static implicit operator DOMString( string str )
        {
            return new DOMString( str );
        }

        public static implicit operator string( DOMString str )
        {
            return str.Render();

            // For whatever reason I got an Overflow exception
            // when checkin if str was null.  Do this and hope for the 
            // best...
            //return str != default( DOMString ) ? str.Render() : 
            //                                     default( string );
        }

        public static implicit operator DOMString( int value )
        {
            return new DOMString( value );
        }

        public static implicit operator DOMString( double value )
        {
            return new DOMString( value );
        }


        #endregion

        #region Operator Overloads

        public static DOMString operator +( DOMString left, DOMString right )
        {
            return new DOMString( left.Text + right.Text );
        }

        public static DOMString operator +( DOMString left, IElement right )
        {
            var result = (DOMString)left.Clone();
            result.Elements.Add( right );
            return result;
        }

        public static bool operator ==( DOMString left, string right )
        {
            return left.Render() == right;
        }

        public static bool operator !=( DOMString left, string right )
        {
            return left.Render() != right;
        }

        public static bool operator ==( string left, DOMString right )
        {
            return left == right.Render();
        }

        public static bool operator !=( string left, DOMString right )
        {
            return left != right.Render();
        }

        #endregion

        #region object overrides

        public override string ToString()
        {
            return Text;
        }

        public override bool Equals( object obj )
        {
            try
            {
                var o = (DOMString)obj;
                if (o != null)
                {
                    return o.Text.Equals( Text );
                }
            }
            catch (InvalidCastException) { }

            var o2 = obj as string;
            if (o2 != null)
            {
                return o2.Equals( Text );
            }

            if (obj != null)
            {
                var o3 = obj.ToString();
                return o3.Equals( Text );
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }

        #endregion

        #region ICloneable Implementation

        public object Clone()
        {
            // this can happen if initialized from default struct constructor. :(
            if (Elements == null)
            {
                Elements = new List<IElement>();
            }
            if (ClassList == null)
            {
                ClassList = new ClassList();
            }

            var domString = (DOMString)MemberwiseClone();
            domString.ID = string.Empty;
            domString.Elements = new List<IElement>();
            domString.ClassList = new ClassList();
            foreach (var element in Elements)
            {
                domString.Elements.Add( element.Clone() as IElement );
            }
            foreach (var cls in ClassList)
            {
                domString.ClassList.Add( cls );
            }
            return domString;
        }

        #endregion
    }

}
