﻿//Copyright (C) 2016 Amaia Industries
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
//associated documentation files (the "Software"), to deal in the Software without restriction, 
//including without limitation the rights to use, copy, modify, merge, publish, and distribute 
//copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//	Notice:
//	The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
//	Modifications:
//	Any modified version or derivative of this software must include a link to the original source code.  Distributing the original source code with the 
//	release of the modified version shall be construed to satisfy this clause.
//
//	Attribution:
//	Any modified version or derivative of this software must include attribution to the original author.  
//
//	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//	INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
//	PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE Amaia Industries 
//	BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//	TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE 
//	USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//	Except as contained in this notice, the name of Amaia Industries shall not be used in advertising or 
//	otherwise to promote the sale, use or other dealings in this Software without prior written authorization from Amaia Industries.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalTherapist.Accumulators {
	public class SpecificAccumulator {
		public string Name { get; set; }

		public bool IsKerman { get; set; }

		public bool RandName { get; set; }

		public bool Female { get; set; }

		public string Type { get; set; }

		public string Trait { get; set; }

		public bool Badass { get; set; }

		public bool Tourist { get; set; }

		public float Brave { get; set; }

		public float Dumb { get; set; }

		public bool RandBrave { get; set; }

		public bool RandDumb { get; set; }

		public SpecificAccumulator ( ) {
			//set default values.
			Name = "";
			IsKerman = false;
			RandName = false;
			Female = true;
			Type = "";
			Trait = "";
			Badass = false;
			Tourist = false;
			Brave = 0f;
			Dumb = 0f;
			RandBrave = false;
			RandDumb = false;

		}

		public SpecificAccumulator Reset ( ) {
			//just in case
			//throw this out and send back a clean, brand new accumulator.
			return new SpecificAccumulator ( );
		}
	}
}
