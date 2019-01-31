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
            //string input = "a,b,d,e,i,l,n,o,r,s,t";
            string input = "l,q,s";
        


            var list = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("A", "a,b,c,d,e,f,g,h"),
                new KeyValuePair<string, string>("B", "d,e,f,g,h,i,j,k"),
                new KeyValuePair<string, string>("C", "g,h,i,j,k,l,m,n"),
                new KeyValuePair<string, string>("D", "k,l,m,n,o,p,q,r"),
                new KeyValuePair<string, string>("E", "n,o,p,q,r,s,t,u"),
                new KeyValuePair<string, string>("F", "a,b,c,r,s,t,u,v,w,x,y,z"),
                new KeyValuePair<string, string>("G", "a,e,o,u,y"),
                new KeyValuePair<string, string>("H", "b,c,e,g,k,m,q,s,x")
            };

            List<string> groups = GenerateGroups(list, input);
        }

        static List<String> GenerateGroups(List<KeyValuePair<string, string>> list, string input)
        {
            List<String> req_areas = input.Split(',').ToList();
            List<List<String>> foundGroups = new List<List<string>>();
            
            bool fin = false;
            while (!fin)
            {
                //Lav liste over alle relevante personer (i.e. personer med givne kvalifikationer).
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
                foreach(KeyValuePair<String, String> person in rel_persons)
                {
                    rel_keys += person.Key;
                }
                //Lav powerset af alle relevante keys
                List<string> pe = PowerSet(rel_keys);
                //Lav liste til at lagre returnerede grupper
                List<String> groups = new List<string>();
                //Sortér af længden af power set værdier, således at de mindste (minimale) kommer først
                foreach (string s in SortByLength(pe))
                {
                    //Console.WriteLine(s);
                    //Lav en concat af alle kvalifikationer fra fundne subset
                    string quals = "";
                    string keys = "";
                    foreach(char c in s)
                    {
                        var p = rel_persons.Find(x => x.Key == c.ToString());
                        //Tilføj personers key til keys
                        keys += p.Key;
                        //Tilføj personers kvalifikationer til conc
                        quals += p.Value;
                    }

                    //Tjek om nuværende liste overholder kravene
                    if (CheckGroup(quals, input))
                    {
                        bool isTaken = false;
                        foreach (string g in groups)
                        {
                            if (g.All(x => keys.Contains(g)))
                            {
                                isTaken = true;
                            }
                        }
                        if (!isTaken)
                        {
                            groups.Add(s);
                            //Console.WriteLine("#");
                        }
                    }
                }

                //Lav alle mulige kombinationer, startende med de mindste
                /*for (int i = 0; i < rel_persons.Count()-1; i++)
                {
                    for (int j = 0; j < rel_persons.Count(); j++)
                    {
                        for (int k = 0; k < rel_persons.Count(); k++)
                        {
                            if (k != j)
                            {
                                string conc = "";
                                //TODO: increase search width (rel_person[k+0,1,2,3 osv]).
                                conc += rel_persons[j].Value;
                                conc += rel_persons[k].Value;

                                //Tjek, om gruppen indeholder alle krævede kvalifikationer
                                if (CheckGroup(conc, input))
                                {
                                    bool isTaken = false;
                                    foreach (string g in groups)
                                    {
                                        //Hvis et subset af gruppen allerede er en gruppe, lad da vær med at tilføje den, da den ikke er minimal
                                        if (g.Contains(rel_persons[j].Key) && g.Contains(rel_persons[k].Key))
                                        {
                                            isTaken = true;
                                        }
                                    }
                                    if (!isTaken)
                                    {
                                        groups.Add(rel_persons[j].Key + "," + rel_persons[k].Key);
                                    }
                                }
                            }
                        }
                    }
                }*/

                foreach (string g in groups)
                {
                    Console.WriteLine(g);
                }


                    //--------------------------------//
                    Console.WriteLine("____________________________");
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
                fin = true;
            }
            return null;
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
            // Use LINQ to sort the array received and return a copy.
            var sorted = from s in e
                         orderby s.Length ascending
                         select s;
            return sorted;
        }
    }
}

//Legacy code
//For hver person, fjern fra listen, tjek om kravene stadig holder
/*                int ite = 0;
                bool newGroup = false;
for (int i = 0; i<rel_persons.Count(); i++)
                {
                    List<KeyValuePair<String, String>> groups = new List<KeyValuePair<string, string>>();
                    string checkedKeys = "";
                    string qual = "";
                    for (int j = 0; j < rel_persons.Count(); j++)
                    {
                        groups.Add(rel_persons[j]);
                        
                        qual += rel_persons[j].Value;
                        if (CheckComplete(qual, input))
                        {
                            
                            var tmp = groups.ToList();
                            for (int k = 0; k < groups.Count(); k++)
                            {
                                newGroup = false;
                                if(k==i || checkedKeys.Contains(groups[k].Key))
                                {
                                    continue;
                                }
                                else
                                {
                                    checkedKeys += rel_persons[k].Key;
                                }
                                tmp.Remove(rel_persons[k]);

                                string tmpQuals = "";
                                foreach(KeyValuePair<String, String> p in tmp)
                                {
                                    tmpQuals += p.Value;
                                }
                                if (!CheckComplete(tmpQuals, input))
                                {
                                    tmp.Add(rel_persons[k]);
                                    groups.Remove(rel_persons[k]);
                                    newGroup = true;
                                }
                                
                            }
                            if (newGroup)
                            {
                                foreach (KeyValuePair<String, String> k in tmp)
                                {
                                    Console.Write(k.Key);
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                    qual = "";

                    //---Print result---//
                    /*foreach (KeyValuePair<string, string> pe in tmp)
                    {
                        Console.Write(pe.Key);
                    }
                    Console.WriteLine();

                    ite++;
                }*/
