using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobindex_programmeringsopgave
{
    static class Program
    {
        static void Main(string[] args)
        {
            string input = "a,b,d,e,i,l,n,o,r,s,t";

            var list = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("A", "a,b,c,d,e,f,g,h"),
                new KeyValuePair<string, string>("B", "d,e,f,g,h,i,j,k"),
                new KeyValuePair<string, string>("C", "g,h,i,j,k,l,m,n"),
                new KeyValuePair<string, string>("D", "k,l,m,n,o,p,q,r"),
                new KeyValuePair<string, string>("E", "n,o,p,q,r,s,t,u"),
                new KeyValuePair<string, string>("F", "a,b,c,r,s,t,u,v,w,x,y,z"),
                new KeyValuePair<string, string>("G", "a,e,i,o,u,y"),
                new KeyValuePair<string, string>("H", "b,c,e,g,k,m,q,s,x")
            };

            List<String> groups = GenerateGroups(list, input);

            //Print grupper
            Console.WriteLine("Minimal groups: ");
            foreach(string group in groups)
            {
                Console.WriteLine(group);
            }
        }

        //Genererer grupper og returnerer dem som strings i en liste
        static List<String> GenerateGroups(List<KeyValuePair<string, string>> list, string input)
        {
            List<String> req_areas = input.Split(',').ToList();
            List<String> foundGroups = new List<string>();

            //Lav liste over alle relevante personer (i.e. personer med mindst én af de givne kvalifikationer).
            List<KeyValuePair<string, string>> rel_persons = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<string, string> person in list)
            {
                List<string> rel_ares = req_areas.Where(x => person.Value.Contains(x)).ToList();
                if (rel_ares.Count() > 0)
                {
                    rel_persons.Add(person);
                }
            }
            //Lav string af keys fra alle relevante personer
            String rel_keys = "";
            foreach (KeyValuePair<String, String> person in rel_persons)
            {
                rel_keys += person.Key;
            }
            //Lav powerset af alle relevante keys
            List<string> ps = PowerSet(rel_keys);
            //Lav liste til at lagre returnerede grupper
            List<String> groups = new List<string>();
            //Sortér af længden af power set værdier, således at de mindste (minimale) kommer først
            foreach (string s in SortByLength(ps))
            {
                //Lav en string af alle kvalifikationer fra fundne subset
                string quals = "";
                string keys = "";
                foreach (char c in s)
                {
                    var p = rel_persons.Find(x => x.Key == c.ToString());
                    //Tilføj personers key til keys
                    keys += p.Key;
                    //Tilføj personers kvalifikationer til quals
                    quals += p.Value;
                }

                //Tjek om nuværende liste overholder kravene
                if (CheckGroup(quals, input))
                {
                    bool isTaken = false;
                    foreach (string g in groups)
                    {
                        bool isFound = false;
                        foreach (char c in g)
                        {
                            if (keys.Contains(c))
                            {
                                isFound = true;
                            }
                            else
                            {
                                isFound = false;
                                break;
                            }
                        }
                        if (!isFound) { isTaken = false; }
                        else
                        {
                            isTaken = true;
                            //Console.WriteLine(keys + " Is a subset of: " + g);
                            break;
                        }
                    }
                    if (!isTaken)
                    {
                        groups.Add(s);
                    }
                }
            }

            //Tilføj grupper til liste 
            foreach (string g in groups)
            {
                foundGroups.Add(g);
            }


            //--------------------------------//
            Console.WriteLine("RELEVANT PEOPLE FOUND:");
            foreach (KeyValuePair<string, string> person in list)
            {
                List<string> f = req_areas.Where(x => person.Value.Contains(x)).ToList();
                Console.Write(person.Key + ": ");
                foreach (string s in f)
                {
                    Console.Write(s);
                }
                Console.WriteLine();
            }
            Console.WriteLine("____________________________");

            return foundGroups;
        }

        static bool CheckGroup(string toTest, string input)
        {
            foreach(char c in input.ToCharArray())
            {
                if (!toTest.Contains(c))
                {
                    return false;
                }
            }
            return true;
        }

        private static List<string> PowerSet(string input)
        {
            int n = input.Length;
            // Power set contains 2^N subsets.
            int powerSetCount = 1 << n;
            var ans = new List<string>();

            for (int setMask = 0; setMask < powerSetCount; setMask++)
            {
                var s = new StringBuilder();
                for (int i = 0; i < n; i++)
                {
                    // Checking whether i'th element of input collection should go to the current subset.
                    if ((setMask & (1 << i)) > 0)
                    {
                        s.Append(input[i]);
                    }
                }
                ans.Add(s.ToString());
            }
            return ans;
        }

        static IEnumerable<string> SortByLength(IEnumerable<string> e)
        {
            // Ved hjælp af LINQ, sortér listen via længde
            var sorted = from s in e
                         orderby s.Length ascending
                         select s;
            return sorted;
        }
    }
}