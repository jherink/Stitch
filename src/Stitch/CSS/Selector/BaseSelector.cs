﻿
// ReSharper disable once CheckNamespace
namespace Stitch.CSS
{
    public abstract class BaseSelector
    {
        public sealed override string ToString()
        {
            return ToString(false);
        }

        public abstract string ToString(bool friendlyFormat, int indentation = 0);
    }
}

