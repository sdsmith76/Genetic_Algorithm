using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmLab
{
    class Program
    {
        public static string code = "BAB";
        static Random rnd = new Random();
        static char[] alphabet = { 'A', 'B' };

        static void Main(string[] args)
        {
            List<string> population = new List<string>();
            population.Add("AAA");
            population.Add("BBB");
            Console.WriteLine(Genetic(population));
            Console.Read();
        }

        static string Genetic(List<string> population)  //pg 129 in book
        {
            while (true)
            {
                List<string> newPopulation = new List<string>();
                foreach (string s in population)
                {
                    string x = RandomSelection(population);
                    string y = RandomSelection(population);
                    string child = Reproduce(x, y);
                    if (rnd.Next(100) < 20)
                    {
                        child = Mutate(child);
                    }
                    if (child == code)
                        return child;
                    newPopulation.Add(child);
                }
                foreach (string s in newPopulation)
                {
                    population.Add(s);
                }
            }
        }

        static string Mutate(string child)
        {
            System.Text.StringBuilder strBuilder = new System.Text.StringBuilder(child);
            strBuilder[rnd.Next(child.Length)] = alphabet[rnd.Next(alphabet.Length)];
            return strBuilder.ToString();
        }

        static string Reproduce(string x, string y)
        {
            int i = rnd.Next(1, x.Length);
            string[] xSplit = new string[2];
            string[] ySplit = new string[2];
            xSplit[0] = x.Substring(0, i);
            xSplit[1] = x.Substring(i);
            ySplit[0] = y.Substring(0, i);
            ySplit[1] = y.Substring(i);
            string[] children = new string[2];
            children[0] = xSplit[0] + ySplit[1];
            children[1] = ySplit[0] + xSplit[1];
            return children[rnd.Next(2)];
        }
        

        static string RandomSelection(List<string> population)
        {
            List<string> choices = new List<string>();
            foreach (string s in population)
            {
                int value = Fitness_CharEquals(s);
                for (int i = 0; i < value; i++)
                {
                    choices.Add(s);
                }
            }
            int r = rnd.Next(choices.Count);
            return choices[r];
        }

        static int Fitness_CharEquals(string s)    //counts number of correct characters
        {
            int value = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == code[i])
                {
                    value++;
                }
            }
            return value;
        } 
    }
}
