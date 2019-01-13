using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace STOLP
{
    public class Parser
    {
        public static List<Data> ParsLearningDataSet(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            List<Data> list = new List<Data>();
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "")
                    continue;
                string[] lineParts = lines[i].Split(',');
                Data.MaxAttributes = lineParts.Length - 2;
                double[] obj = new double[Data.MaxAttributes];
                for(int j = 0; j < Data.MaxAttributes; ++j)
                {
                    obj[j] = int.Parse(lineParts[j]);
                }
                list.Add(new Data(obj, int.Parse(lineParts[lineParts.Length-1])));
            }
            return list;
        }

        public static List<Data> ParseJson(string fileName)
        {
            string json = File.ReadAllText(fileName);

            JArray array = JArray.Parse(json);

            List<Data> list = new List<Data>();

            foreach (JObject obj in array.Children<JObject>())
            {
                double[] attrs = new double[Data.MaxAttributes];
                int className = 1;

                foreach (JProperty singleProp in obj.Properties())
                {
                    string name = singleProp.Name;
                    string value = singleProp.Value.ToString();

                    switch(name)
                    {
                        case "x": attrs[0] = double.Parse(value); break;
                        case "y": attrs[1] = double.Parse(value); break;
                        case "class": className = int.Parse(value); break;
                    }
                }
                list.Add(new Data(attrs, className));
            }

            return list;
        }

        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }
    }
}
