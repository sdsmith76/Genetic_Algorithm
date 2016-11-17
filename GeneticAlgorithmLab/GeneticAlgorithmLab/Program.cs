using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GeneticAlgorithmLab
{
    class Program
    {
        //try changing these for the experiments
        // 1. fitness function
        public static int fitnessWeight = 2;
        // 2. change population size - done in the main function

        // 3. selection process
        static bool randomSelection = false; // change to false to get the best selections, true gives random selection
        // 4. crossover rate and type
        public static int crossoverRate = 2; // set this to 0 for a random crossover rate
        // 5. change mutation frequency
        public static int mutationPercent = 90; // must be between 0 and 100
        // 6. alphabet size / pool size



        public static string code = "AADCBBCDEBABCDE";
        static Random rnd = new Random();
        static char[] alphabet = { 'A', 'B', 'C', 'D', 'E' };

        static void Main(string[] args)
        {
            List<string> population = new List<string>();
            population.Add("ADDDDDDDDDDDCDE");
            population.Add("BBABABABABABABB");
            population.Add("AEAEAEAEAEAEAAB");
            population.Add("CDECDECDECDEDDE");
            population.Add("BDDADAEDAEDDEEE");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string answer = Genetic(population);
            sw.Stop();

            double seconds = sw.ElapsedMilliseconds;
            string time;
            if(seconds < 10000)
            {
                time = seconds.ToString() + " milli";
            }
            else
            {
                time = (seconds / 1000).ToString() + " ";
            }
            Console.WriteLine("Answer: " + answer + " = " + code + " in " + time + " milliseconds");
            Console.ReadKey();
        }

        static string Genetic(List<string> population)  //pg 129 in book
        {
            int generation = 0;
            while (true)
            {
                Console.WriteLine("Generation: {0}", generation++);
                List<string> newPopulation = new List<string>();
                foreach (string s in population)
                {
                    string x;
                    string y;

                    if (randomSelection)
                    {
                        x = RandomSelection(population);
                        y = RandomSelection(population);
                    }
                    else
                    {
                        // these two will grab based on the values, and it seems to make things way faster
                        x = MaxSelectionWithOffset(population, 0);
                        y = MaxSelectionWithOffset(population, 1);
                    }
                    string child = Reproduce(x, y);
                    if (rnd.Next(100) < mutationPercent)
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
            int i = rnd.Next(1, x.Length); // this gives us a random crossover rate
            if (crossoverRate > 1 && crossoverRate < x.Length - 1)
            {
                i = crossoverRate;
            }
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
                int value = Fitness_CharEquals(s) * fitnessWeight;
                for (int i = 0; i < value; i++)
                {
                    choices.Add(s);
                }
            }
            int r = rnd.Next(choices.Count);
            return choices[r];
        }

        static string MaxSelectionWithOffset(List<string> population, int numFromMax)
        {
            int value = 0;
            List<KeyValuePair<int, string>> choices = new List<KeyValuePair<int, string>>() { };
            foreach (string s in population)
            {
                value = Fitness_CharEquals(s) * fitnessWeight;
                choices.Insert(choices.Count(), new KeyValuePair<int, string>(value, s));
                
            }
            choices.Sort(Compare2);
            return choices[choices.Count() - 1 - numFromMax].Value;
        }

        static int Compare2(KeyValuePair<int, string> a, KeyValuePair<int, string> b)
        {
            return a.Key.CompareTo(b.Key);
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
