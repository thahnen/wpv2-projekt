using System;
using System.IO;
using Newtonsoft.Json;


namespace Graphsky {
    static class JSONHandler {
        /**
         *  Loads a graph from a JSON file
         *  
         *  @param filepath     path to the input json file
         *  @param input        where to store the loaded graph
         *  @return             true if loading was sucessfull
         */
        public static bool loadFromFile(string filePath, ref Graph input) {
            if (!File.Exists(filePath)) {
                return false;
            }

            using (StreamReader reader = File.OpenText(filePath)) {
                JsonSerializer s = new JsonSerializer();

                try {
                    input = (Graph)s.Deserialize(reader, typeof(Graph));
                    return true;
                } catch (Exception e) {
                    return false;
                }
            }
        }


        /**
         *  Writes a given graph to a JSON file
         *
         *  @param filepath     path to the output json file
         *  @param output       which graph should be written to file
         *  @return             true if saving was sucessfull
         */
        public static bool saveGraphToFile(string filepath, ref Graph output) {
            using (JsonWriter writer = new JsonTextWriter(File.CreateText(filepath))) {
                JsonSerializer s = new JsonSerializer();

                try {
                    s.Serialize(writer, new OGraph(ref output));
                    return true;
                } catch (Exception e) {
                    return false;
                }
            }
        }
    }
}
