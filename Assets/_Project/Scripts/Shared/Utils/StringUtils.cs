using System.IO;
using System.Linq;

namespace TriplanoTest.Shared.Utils
{
    public static class StringUtils
    {
        public static int GetNumberPart(string text)
        {
            string fullNumber = text
                .Where(character => int.TryParse(character.ToString(), out int _))
                .Aggregate(string.Empty, (current, character) => current + character);

            return int.Parse(fullNumber);
        }

        public static string CombinePaths(params string[] paths)
        {
            char separator = Path.DirectorySeparatorChar;
            string finalPath = string.Empty;

            for (int index = 0; index < paths.Length; index++)
            {
                string path = paths[index];
                finalPath += path;
                
                if(index != paths.Length - 1)
                    finalPath += separator;
            }

            return finalPath;
        }
    }
}