using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace FEARA
{
	class Action
	{
		//the name of the action
		string name;
		public string Name { get { return name; } set { name = value; } }

		//the precondition to be met for the action
		State precondition;
		public State Precondition { get { return precondition; } set { precondition = value; } }

		//the result of the action
		State result;
		public State Result { get { return result; } set { result = value; } }

		//either something to move to, or null
		EnvironmentObject moveTo;
		public EnvironmentObject MoveTo { get { return moveTo; } set { moveTo = value; } }
		//an object to add, or null
		EnvironmentObject add;
		public EnvironmentObject Add { get { return add; } set { add = value; } }
		//an object to remove, or null
		bool remove;
		public bool Remove { get { return remove; } set { remove = value; } }

		//the cost of the action
		float cost;
		public float Cost { get { return cost; } set { cost = value; } }

		//returns true if the action is complete (if the player is where he was meant to go
		public bool isComplete(Character self)
		{
			return Vector2.Distance(self.Position, moveTo.Position) < 2;
		}

		//completes the action
		public void Complete(Character self)
		{
			//if we need to remove our object, remove it
			if (remove)
			{
				self.Carrying.Position = new Vector2(-50, -50);
				self.Carrying = null;
			}

			//if we need to add something, add it
			if (add != null)
				self.Carrying = add;

			//add the result
			self.state.setState(result);
		}
	}
}
