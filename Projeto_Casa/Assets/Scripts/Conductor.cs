using System;

namespace AssemblyCSharp
{
	public class Conductor
	{
		private string conductor;
		private string type;
		private float offset;

		public Conductor (string conductor, string type, float offset)
		{
			conductor = conductor;
			type = type;
			offset = offset;
		}

		public Conductor(){
		}

		public string Print(){
			string s = conductor + "   " + type;
			return s;
		}

		public void SetType(string s){
			type = s;
		}

		public void SetConductor(string c){
			conductor = c;
		}

		public string GetConductor(){
			return conductor;
		}

		public string GetType(){
			return type;
		}
	}
}

