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
	class State
	{
		//the amount of time for a variable to be updated
		float time = 0.0f;
		const float UPDATE_TIME = 0.4f;

		//the features of any state
		static List<string> features = new List<string>();
		public static string Features { set { features.Add(value); } }

		//the font
		static SpriteFont font;
		public static void loadFont(ContentManager content)
		{
			font = content.Load<SpriteFont>("font2");
		}
		static Vector2 textStartingPosition = new Vector2(500, 150);

		//the values for the individual state
		Dictionary<string, string> values = new Dictionary<string, string>();
		public List<Feature> Values
		{
			set
			{
				foreach (Feature feat in value)
				{
					if (values.ContainsKey(feat.Name))
						values[feat.Name] = feat.Value;
					else
						values.Add(feat.Name, feat.Value);
				}
			}
		}

		//create the state
		public State() { }
		public State(Dictionary<string, string> vals)
		{
			values = vals;
		}

		//get a value for a feature
		public string getValue(string key)
		{
			return values[key];
		}

		//update the state
		public void Update(GameTime time)
		{
			this.time += time.ElapsedGameTime.Milliseconds * 0.001f;

			//if we have passed the update time
			if (this.time < UPDATE_TIME)
				return;

			this.time = 0.0f;

			//get a list of all the variables that need to be updated
			Dictionary<string, string> updateKeys = new Dictionary<string, string>();

			foreach (string key in values.Keys)
			{
				int value;
				//if it is an int, it needs to be updated
				if (int.TryParse(values[key], out value))
					updateKeys.Add(key, "" + (value + 1));

			}

			//update all the values
			//this needed to be done this way because modifying values in the foreach statement broke the program.
			foreach (string key in updateKeys.Keys)
			{
				values[key] = updateKeys[key];
			}
		}

		//sets the state with the action's result
		public void setState(State other)
		{
			foreach (string feature in other.values.Keys)
			{
				if (values.ContainsKey(feature))
					values[feature] = other.values[feature];
				else
					values.Add(feature, other.values[feature]);
			}
		}

		//add's states together without changing the state ( for A*)
		public State addState(State other)
		{
			Dictionary<string, string> retVal = new Dictionary<string, string>(values);
			foreach (string feature in other.values.Keys)
			{
				if (retVal.ContainsKey(feature))
					retVal[feature] = other.values[feature];
				else
					retVal.Add(feature, other.values[feature]);
			}

			return new State(retVal);
		}

		//checks if the state fulfills the precondition
		public bool Equals(State precondition)
		{
			foreach (string feature in precondition.values.Keys)
			{
				//if any of the features do not exist or are not equal, it is not equal
				if (values.ContainsKey(feature) && !AreTheyEqual(values[feature], precondition.values[feature])) return false;
			}

			return true;
		}

		//checks if the value fulfulls the pre-condition
		public static bool AreTheyEqual(string val, string precondition)
		{
			int intparse = 0;
			//if the condition needs less than a value
			if (precondition[0] == '-')
			{
				return int.Parse(val) < int.Parse(precondition.Substring(1));
			}//if the condition needs more than a value
			else if (precondition[0] == '+')
			{
				return int.Parse(val) > int.Parse(precondition.Substring(1));
			}//if the condition just needs to be a number just compare it
			else if (int.TryParse(precondition, out intparse))
			{
				return intparse == int.Parse(val);
			}
			else
			{//compare the booleans
				return bool.Parse(val) == bool.Parse(precondition);
			}
		}

		public void Draw(SpriteBatch sb)
		{
			Vector2 position = textStartingPosition;
			foreach (string key in values.Keys)
			{
				//draw the values for each of the features
				sb.DrawString(font, key + " |  " + values[key], position, Color.Black);
				position.Y += font.MeasureString(key).Y + 10;
			}
		}
	}
}
