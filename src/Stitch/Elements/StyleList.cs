using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Elements
{
    public class StyleList : IDictionary<string, string>, ICloneable
    {

        private readonly Dictionary<string, string> _styles = new Dictionary<string, string>();

        public void Add( string styleString )
        {
            var styleRuleSplit = styleString.Split( new[] { ';' }, StringSplitOptions.RemoveEmptyEntries );
            foreach (var style in styleRuleSplit)
            {
                var rule = style.Split( ':' );
                // there should be two.
                if (rule != null && rule.Length >= 2)
                {
                    var cssProperty = rule[0].Trim();
                    var cssRule = rule[1].Trim();
                    Add( cssProperty, cssRule );
                }
            }
        }

        public string GetStyleString()
        {
            return Count > 0 ? $"style=\"{ToString()}\"" : string.Empty;
        }

        public override string ToString()
        {
            var builer = new StringBuilder();
            foreach (var style in _styles)
            {
                builer.Append( $"{style.Key}:{style.Value};" );
            }
            return builer.ToString();
        }

        #region IDictionary<string, string> Implementation

        public string this[string cssProperty]
        {
            get
            {
                return _styles[cssProperty];
            }

            set
            {
                if (!ContainsKey( cssProperty ))
                {
                    Add( cssProperty, string.Empty );
                }
                _styles[cssProperty] = value;
            }
        }

        public int Count
        {
            get
            {
                return _styles.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return _styles.Keys;
            }
        }

        public ICollection<string> Values
        {
            get
            {
                return _styles.Values;
            }
        }

        public void Add( string cssProperty, string cssRule )
        {
            if (ContainsKey( cssProperty ))
            {
                _styles[cssProperty] = cssRule;
            }
            else
            {
                _styles.Add( cssProperty, cssRule );
            }
        }

        public void Add( KeyValuePair<string, string> rule )
        {
            _styles.Add( rule.Key, rule.Value );
        }

        public void Clear()
        {
            _styles.Clear();
        }

        public bool Contains( KeyValuePair<string, string> rule )
        {
            return ContainsKey( rule.Key ) && _styles[rule.Key] == rule.Value;
        }

        public bool ContainsKey( string cssProperty )
        {
            return _styles.ContainsKey( cssProperty );
        }

        public void CopyTo( KeyValuePair<string, string>[] array, int arrayIndex )
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _styles.GetEnumerator();
        }

        public bool Remove( KeyValuePair<string, string> rule )
        {
            return Remove( rule.Key );
        }

        public bool Remove( string cssProperty )
        {
            return _styles.Remove( cssProperty );
        }

        public bool TryGetValue( string cssProperty, out string cssValue )
        {
            return _styles.TryGetValue( cssProperty, out cssValue );
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _styles.GetEnumerator();
        }

        #endregion

        #region ICloneable Implementation

        public object Clone()
        {
            var styleList = new StyleList();
            foreach (var style in _styles) styleList.Add( style );

            return styleList;
        }

        #endregion
    }
}