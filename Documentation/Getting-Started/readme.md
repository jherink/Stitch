# Getting Started

~~~cs
// Step 1: Create new StitchDocument
var doc = new StitchDocument();

// Step 2: Add elements to the document
doc.Add( new Paragraph( "Hello World!" ) );

// Step 3: Render or the document
var html = doc.Render();
~~~
