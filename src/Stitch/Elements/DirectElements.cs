using HydraDoc.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Elements
{
    // This file contains classes that directly inherit from IElement
    // The only element not here is Head

    public abstract class SpecialText : DOMString, IElement
    {
        public abstract string Tag { get; }

        public ClassList ClassList { get; set; } = new ClassList();

        public string ID { get; set; }

        public string Title { get; set; }

        public string Render()
        {
            return $"<{Tag}>{Text}</{Tag}>";
        }

        public override string ToString()
        {
            return Render();
        }
    }

    public class ItalixText : SpecialText
    {
        public override string Tag { get { return "i"; } }
        public ItalixText() { }
        public ItalixText( string text ) { Text = text; }
    }
    public class BoldText : SpecialText
    {
        public override string Tag { get { return "b"; } }
        public BoldText() { }
        public BoldText( string text ) { Text = text; }
    }
    public class UnderlineText : SpecialText
    {
        public override string Tag { get { return "u"; } }
        public UnderlineText() { }
        public UnderlineText( string text ) { Text = text; }
    }
    public class SubText : SpecialText
    {
        public override string Tag { get { return "sub"; } }
        public SubText() { }
        public SubText( string text ) { Text = text; }
    }
    public class SupText : SpecialText
    {
        public override string Tag { get { return "sup"; } }
        public SupText() { }
        public SupText( string text ) { Text = text; }
    }
    public class StrikeText : SpecialText
    {
        public override string Tag { get { return "strike"; } }
        public StrikeText() { }
        public StrikeText( string text ) { Text = text; }
    }
    public class BigText : SpecialText
    {
        public override string Tag { get { return "big"; } }
        public BigText() { }
        public BigText( string text ) { Text = text; }
    }
    public class SmallText : SpecialText
    {
        public override string Tag { get { return "small"; } }
        public SmallText() { }
        public SmallText( string text ) { Text = text; }
    }
}
