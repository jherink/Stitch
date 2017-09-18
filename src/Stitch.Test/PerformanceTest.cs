using System;
using Stitch.Elements;
using System.Collections.Generic;
using System.Linq;
using Stitch.Elements.Interface;
using Xunit;

namespace Stitch.Tests
{
    public class PerformanceTest
    {
        [Fact]
        public void ManyElementTest()
        {
            var runs = new List<double>();
            var watch = new System.Diagnostics.Stopwatch();

            for (int j = 0; j < 10; j++)
            {
                watch.Reset();
                watch.Start();
                var doc = new StitchDocument();
                for (int i = 0; i < 10000; i++)
                {
                    doc.Add( new Div() );
                }

                Assert.False( string.IsNullOrWhiteSpace( doc.Render() ) );
                watch.Stop();
                runs.Add( watch.ElapsedMilliseconds );
            }

            Console.WriteLine( $"Average Time {runs.Average()}ms" );
        }

        [Fact]
        public void DeeplyNestedElementTest()
        {
            var runs = new List<double>();
            var watch = new System.Diagnostics.Stopwatch();

            for (int j = 0; j < 10; j++)
            {
                watch.Reset();
                watch.Start();
                var doc = new StitchDocument();
                var lastDiv = new Div();
                doc.Add( lastDiv );
                for (int i = 0; i < 3000; i++)
                {
                    var newDiv = new Div();
                    lastDiv.Add( newDiv );
                    lastDiv = newDiv;
                }

                Assert.False( string.IsNullOrWhiteSpace( doc.Render() ) );
                watch.Stop();
                runs.Add( watch.ElapsedMilliseconds );
            }

            Console.WriteLine( $"Average Time {runs.Average()}ms" );
        }

        [Fact]
        public void DeeplyNestedAndManyElementTest()
        {
            var runs = new List<double>();
            var watch = new System.Diagnostics.Stopwatch();

            for (int j = 0; j < 10; j++)
            {
                watch.Reset();
                watch.Start();
                var doc = new StitchDocument();
                for (int k = 0; k < 100; k++)
                {
                    var lastDiv = new Div();
                    doc.Add( lastDiv );
                    for (int i = 0; i < 100; i++)
                    {
                        var newDiv = new Div();
                        lastDiv.Add( newDiv );
                        lastDiv = newDiv;
                    }
                }
                Assert.False( string.IsNullOrWhiteSpace( doc.Render() ) );
                watch.Stop();
                runs.Add( watch.ElapsedMilliseconds );
            }

            Console.WriteLine( $"Average Time {runs.Average()}ms" );
        }

        [Fact]
        public void FractalNestedTest()
        {
            var runs = new List<double>();
            var watch = new System.Diagnostics.Stopwatch();

            for (int j = 0; j < 10; j++)
            {
                watch.Reset();
                watch.Start();
                var doc = new StitchDocument();
                for (int k = 0; k < 100; k++)
                { // 1000 linear
                    AddChildren( 5, doc.Body ); // should be 120 children
                }
                Assert.False( string.IsNullOrWhiteSpace( doc.Render() ) );
                watch.Stop();
                runs.Add( watch.ElapsedMilliseconds );
            }

            Console.WriteLine( $"Average Time {runs.Average()}ms" );
        }

        private void AddChildren( int children, IParentElement parent )
        {
            for (int i = 0; i < children; i++)
            {
                var child = new Div();
                AddChildren( children - 1, child );
                parent.Children.Add( child );
            }
        }
    }
}
