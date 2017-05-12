using Stitch.Elements.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Stitch.Elements
{
    // This file contains classes that directly inherit from IElement
    // The only element not here is Head

    public abstract class FormattedElement : IElement
    {
        public abstract string Tag { get; }

        public ClassList ClassList { get; set; } = new ClassList();

        public StyleList StyleList { get; set; } = new StyleList();

        public IDictionary<string, string> Attributes { get; private set; } = new Dictionary<string, string>();

        public string Text { get; protected set; }

        public string ID { get; set; }

        public string Title { get; set; }

        protected FormattedElement() { }

        protected FormattedElement( string text ) { Text = text; }

        protected FormattedElement( IElement element ) { Text = element.Render(); }

        public virtual string Render()
        {
            var builder = new StringBuilder();
            builder.Append( $"<{Tag}" );
            if (!string.IsNullOrWhiteSpace( ID )) builder.Append( $" id=\"{ID}\"" );
            if (ClassList.Count > 0) builder.Append( $" {ClassList.GetClassString()}" );
            if (StyleList.Count > 0) builder.Append( $" {StyleList.GetStyleString()}" );
            if (Attributes.Count > 0) foreach (var att in Attributes) builder.Append( $" {att.Key}=\"{att.Value}\"" );

            builder.Append( $">{Text}</{Tag}>" );
            return builder.ToString();
        }

        public override string ToString()
        {
            return Render();
        }

        public object Clone()
        {
            var clone = (IElement)MemberwiseClone();
            clone.ID = string.Empty;
            clone.ClassList = new ClassList();
            foreach (var cls in ClassList)
            {
                clone.ClassList.Add( cls );
            }
            return clone;
        }
    }
}
