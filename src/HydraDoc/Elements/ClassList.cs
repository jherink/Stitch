using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Elements
{
    public class ClassList : IEnumerable<string>, ICloneable
    {
        private readonly List<string> _classList = new List<string>();

        #region Constructors

        public ClassList() { }

        public ClassList( params string[] classes )
        {
            Add( classes );
        }

        #endregion

        #region ICollection Implementation

        public int Count
        {
            get
            {
                return _classList.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public ClassList Add( string item )
        {
            var classes = item.Split( new[] { " " }, StringSplitOptions.RemoveEmptyEntries );

            foreach (var cls in classes)
            {
                if (!Contains( cls )) _classList.Add( cls );
            }           

            return this;
        }
        
        public void Clear()
        {
            _classList.Clear();
        }

        public bool Contains( string item )
        {
            return _classList.Contains( item );
        }

        public void CopyTo( string[] array, int arrayIndex )
        {
            _classList.CopyTo( array, arrayIndex );
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _classList.GetEnumerator();
        }

        public ClassList Remove( string item )
        {
            _classList.Remove( item );
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public ClassList Add( params string[] classes )
        {
            foreach (var cls in classes) Add( cls );

            return this;
        }

        public string GetClassString()
        {
            return Count > 0 ? $"class=\"{ToString()}\"" : string.Empty;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < Count; i++)
            {
                builder.Append( (i != Count - 1) ? $"{ _classList[i]} " : $"{_classList[i]}" );
            }
            return builder.ToString();
        }

        #region ICloneable Implementation

        public object Clone()
        {
            var clsList = new ClassList();
            foreach (var cls in _classList) clsList.Add( cls );
            return clsList;
        }

        #endregion
    }
}
