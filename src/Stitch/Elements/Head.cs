using Stitch.CSS;
using Stitch.Elements.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Elements
{
    public class Head : IHeadElement
    {
        public string Tag { get { return "head"; } }

        public ICollection<IMetaElement> Metas { get; set; } = new List<IMetaElement>();

        public ICollection<IStyleElement> Styles { get; set; } = new List<IStyleElement>();

        public ClassList ClassList { get; set; } = new ClassList();

        public StyleList StyleList { get; set; } = new StyleList();

        public IDictionary<string, string> Attributes { get; private set; } = new Dictionary<string, string>();

        public string ID { get; set; }

        public string Title { get; set; }

        public string Render()
        {
            throw new NotImplementedException();
        }

        public string Render( IEnumerable<string> tags, IEnumerable<string> classes )
        {
            var builder = new StringBuilder();
            builder.AppendLine( $"<{Tag}>" );
            builder.AppendLine( $"<title>{Title}</title>" );

            // render metas and styles on multiple threads
            // because they can be large, especially styles.
            // However, because CSS order matters we must wait
            // for all renderings to complete and append them in order.
            var tasks = new List<Task<string>>();
            foreach (var meta in Metas)
            {
                tasks.Add( Task.Factory.StartNew( () => { return meta.Render(); } ) );
            }

            foreach (var style in Styles)
            {
                tasks.Add( Task.Factory.StartNew( () => { return style.Render( tags, classes ); } ) );
            }

            Task.WaitAll( tasks.ToArray() ); // wait for renderings to complete.
            foreach (var task in tasks) builder.Append( task.Result );

            builder.AppendLine( $"</{Tag}>" );
            return builder.ToString();
        }

        public object Clone()
        {
            var clone = MemberwiseClone() as IHeadElement;
            clone.ID = string.Empty;
            var cloneTasks = new Task[3];
            clone.Metas = new List<IMetaElement>();
            clone.Styles = new List<IStyleElement>();
            clone.ClassList = new ClassList();
            int i = 0;
            // metas
            cloneTasks[i++] = Task.Factory.StartNew( () =>
             {
                 foreach (var meta in Metas)
                 {
                     clone.Metas.Add( (IMetaElement)meta.Clone() );
                 }
             } );
            // styles
            cloneTasks[i++] = Task.Factory.StartNew( () =>
             {
                 foreach (var style in Styles)
                 {
                     clone.Styles.Add( (IStyleElement)style.Clone() );
                 }
             } );
            // classes
            cloneTasks[i++] = Task.Factory.StartNew( () =>
             {
                 foreach (var cls in ClassList)
                 {
                     clone.ClassList.Add( cls );
                 }
             } );

            Task.WaitAll( cloneTasks );

            return clone;
        }
    }

    public class Meta : IMetaElement
    {
        public Meta() { }

        public Meta( string name ) : this()
        {
            Name = name;
        }

        public Meta( string name, string content, string httpEquiv = "" ) : this( name )
        {
            Content = content;
            HttpEquiv = httpEquiv;
        }

        public string Tag { get { return "meta"; } }

        public ClassList ClassList { get; set; } = new ClassList();

        public StyleList StyleList { get; set; } = new StyleList();

        public IDictionary<string, string> Attributes { get; private set; } = new Dictionary<string, string>();
        
        public string Content { get; set; }

        public string HttpEquiv { get; set; }

        public string ID { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Render()
        {
            var builder = new StringBuilder();
            builder.Append( "<meta" );
            if (!string.IsNullOrWhiteSpace( Name ))
            {
                builder.Append( $" name=\"{Name}\"" );
            }
            if (!string.IsNullOrWhiteSpace( Content ))
            {
                builder.Append( $" content=\"{Content}\"" );
            }
            if (!string.IsNullOrWhiteSpace( HttpEquiv ))
            {
                builder.Append( $" http-equiv=\"{HttpEquiv}\"" );
            }
            if (Attributes.Count > 0) foreach (var att in Attributes) builder.Append( $" {att.Key}=\"{att.Value}\"" );

            builder.Append( " />" );
            return builder.ToString();
        }

        public object Clone()
        {
            var clone = (IMetaElement)MemberwiseClone();
            clone.ID = string.Empty;
            clone.ClassList = new ClassList();
            foreach (var cls in ClassList)
            {
                clone.ClassList.Add( cls );
            }
            return clone;
        }
    }

    public class Style : IStyleElement
    {
        public string Tag { get { return "style"; } }

        public ClassList ClassList { get; set; } = new ClassList();

        public StyleList StyleList { get; set; } = new StyleList();

        public IDictionary<string, string> Attributes { get; private set; } = new Dictionary<string, string>();
        
        public bool Disabled { get; set; }

        public string ID { get; set; }

        public string Media { get; set; }

        public string Title { get; set; }

        public string Type { get; set; } = "text/css";

        public StyleSheet StyleSheet { get; set; }

        public string Render()
        {
            throw new NotImplementedException();
        }

        public string Render( IEnumerable<string> tags, IEnumerable<string> classes )
        {

            var builder = new StringBuilder();
            builder.Append( "<style" );
            if (!string.IsNullOrWhiteSpace( Type ))
            {
                builder.Append( $" type=\"{Type}\"" );
            }
            if (Attributes.Count > 0) foreach (var att in Attributes) builder.Append( $" {att.Key}=\"{att.Value}\"" );

            builder.AppendLine( ">" );

            foreach (var ff in StyleSheet.FontFaceDirectives)
            { // import all fonts. :/
                builder.Append( ff.ToString() );
            }


            foreach (var rule in StyleSheet.StyleRules)
            {
                if (StyleRuleApplies( rule.Selector, tags, classes ))
                {
                    builder.AppendLine( rule.Value + "{ " + rule.Declarations.ToString() + " }" );
                }
            }


            // add all media queries since they may influence the layout
            // based on screen size.  These must be done after the style rules are defined.
            foreach (var m in StyleSheet.MediaDirectives)
            {
                var rules = new List<string>();
                var styleRules = m.RuleSets.Where( t => t.RuleType == RuleType.Style ).Cast<StyleRule>();
                if (styleRules.Any( s => StyleRuleApplies( s.Selector, tags, classes ) ))
                {
                    builder.Append( m.ToString() );
                }
            }

            builder.AppendLine( "</style>" );
            return builder.ToString();
        }

        private bool StyleRuleApplies( BaseSelector ruleSelector, IEnumerable<string> tags, IEnumerable<string> classes )
        {
            var comp = ruleSelector.ToString();

            if ((ruleSelector as SimpleSelector) != null)
            {
                var simpleSelector = ruleSelector as SimpleSelector;
                if (comp.StartsWith( "*" ) ||
                   (tags.Any( t => t == comp ) || classes.Any( t => $".{t}" == comp )))
                {
                    return true;
                    //builder.AppendLine( rule.Value + "{ " + rule.Declarations.ToString() + " }" );
                }
            }
            else if ((ruleSelector as AggregateSelectorList) != null)
            {
                var aggSelector = ruleSelector as AggregateSelectorList;
                foreach (var s in aggSelector)
                {
                    comp = s.ToString();
                    if (comp.StartsWith( "*" ) ||
                        (tags.Any( t => t == comp ) ||
                         classes.Any( t =>
                         {
                             var classTag = $".{t}";
                             return classTag == comp || comp.Contains( classTag );
                         } )))
                    {
                        return true;
                        //builder.AppendLine( rule.Value + "{ " + rule.Declarations.ToString() + " }" );
                    }
                }
            }
            else if ((ruleSelector as ComplexSelector) != null)
            {
                var complexSelector = ruleSelector as ComplexSelector;
                foreach (var s in complexSelector)
                {
                    if (StyleRuleApplies( s.Selector, tags, classes )) return true;
                }
            }
            else
            {

            }

            return false;
        }

        public Style( string css )
        {
            var parser = new Parser();
            StyleSheet = parser.Parse( css );
        }

        public Style() { }

        public Style( Uri address )
        {
            var resolver = new CSSResourceResolver( address );
            StyleSheet = resolver.StyleSheet;
        }

        public object Clone()
        {
            var clone = (IStyleElement)MemberwiseClone();
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
