using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

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
	static class EnvironmentLoader
	{
		public static Environment Load(Game1 game, string filename)
		{
			//set up the object lists
			List<EnvironmentObject> objs = new List<EnvironmentObject>();
			List<Character> chars = new List<Character>();
			List<Goal> goals = new List<Goal>();

			//get the document
			XDocument doc = XDocument.Load(TitleContainer.OpenStream(filename));

			foreach (string feature in doc.Descendants("stateFeature"))
			{
				State.Features = feature;
			}

			//select all the environment objects
			objs = (from EnviroObject in doc.Descendants("EnvironmentObject")
					select new EnvironmentObject()
					{
						Name = EnviroObject.Element("name").Value,
						Game = game,
						Texture = EnviroObject.Element("texturename").Value,
						Position = new Vector2(float.Parse(EnviroObject.Element("locationx").Value), float.Parse(EnviroObject.Element("locationy").Value)),
						Scale = float.Parse(EnviroObject.Element("scale").Value)
					}).ToList();


			//load all the characters
			chars = (from Charcat in doc.Descendants("Character")
					 select new Character()
					 {
						 Game = game,
						 Name = Charcat.Element("name").Value,
						 Texture = Charcat.Element("texturename").Value,
						 Position = new Vector2(float.Parse(Charcat.Element("locationx").Value), float.Parse(Charcat.Element("locationy").Value)),
						 Scale = float.Parse(Charcat.Element("scale").Value),
						 state = new State()
						 {
							 Values = (from vals in Charcat.Element("state").Descendants("feature")
									   select new Feature()
									   {
										   Name = vals.Element("name").Value,
										   Value = vals.Element("value").Value
									   }).ToList()
						 },
						 AI = new AI()
						 {
							 Actions = (from act in Charcat.Descendants("action")
										select new Action()
										{
											Name = act.Element("name").Value,
											Precondition = new State()
											{
												Values = (from val in act.Element("precondition").Element("state").Descendants("feature")
														  select new Feature()
														  {
															  Name = val.Element("name").Value,
															  Value = val.Element("value").Value
														  }).ToList()
											},
											Result = new State()
											{
												Values = (from val in act.Element("result").Element("state").Descendants("feature")
														  select new Feature()
														  {
															  Name = val.Element("name").Value,
															  Value = val.Element("value").Value
														  }).ToList()
											},
											MoveTo = EnvironmentObject.getObjectByName(act.Element("moveTo").Value),
											Add = EnvironmentObject.getObjectByName(act.Element("add").Value),
											Remove = bool.Parse(act.Element("remove").Value),
											Cost = float.Parse(act.Element("cost").Value)
										}).ToList(),
							 Goals = (from goal in doc.Descendants("goal")
									  select new Goal()
									  {
										  Name = goal.Element("name").Value,
										  GoalState = new State()
										  {
											  Values = (from val in goal.Element("goalState").Descendants("feature")
														select new Feature()
														{
															Name = val.Element("name").Value,
															Value = val.Element("value").Value
														}).ToList()
										  },
										  WeightFeature = goal.Element("weightFeature").Value,
										  WeightLimit = int.Parse(goal.Element("weightLimit").Value)
									  }).ToList()
						 }
					 }).ToList();


			return new Environment(objs, chars);
		}
	}
}
