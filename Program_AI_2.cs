using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;


namespace GeneticAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create basic parameters
            bool Enable_Mutation = true;
            bool Enable_CrossOver = true;
            double Mutation_Probability = 0.07;
            double CrossOver_Probability = 0.10;
            int Population_Size = 1000;
            int Iterations = 50;
            int SizeOfArray = 500;
            ArrayList Xprime = new ArrayList();

            double PercentageFitness = 0.8;
            double PercentageSupressed = 0.9;
            int TotalRuns = 50;
            TextWriter writehandle = new StreamWriter("Output.txt");

            //Prepare the output file
            writehandle.Write("Run Number");
            writehandle.Write(",");
            writehandle.Write("Iteration");
            writehandle.Write(",");
            writehandle.Write("Fitness");
            writehandle.Write(",");
            writehandle.WriteLine("Members");




            //Create the original population
            int[] X;
            X = new int[40] { 64, 92, 39, 97, 52, 85, 93, 30, 1, 53, 74, 87, 2, 24, 42, 89, 60, 95, 12, 79, 35, 27, 9, 18, 19, 32, 31, 94, 63, 36, 28, 41, 34, 72, 48, 46, 40, 25, 26, 84 };
            /*X = new int[50] {213, 144, 103, 46, 168, 202, 246, 143, 222, 87, 171, 130, 111, 163, 41, 76, 216, 127, 224, 21, 204, 
208, 144, 243, 9, 211, 127, 54, 12, 176, 118, 177, 83, 173, 238, 92, 116, 100, 208, 234, 47, 75, 70, 167, 
164, 61, 194, 163, 142, 242};*/
            /*X = new int[500] {1400,2147,1570,473,1756,564,576,751,325,397,31,1530,2042,2193,1875,1144,4,1568,555,541,357,1411,1829,1891,435,261,2358,1891,891,2175,89,876,1927,164,1113,249,1052,855,1451,2255,1019,1163,2092,1626,1283,2217,979,131,1899,1795,375,1399,2379,1480,2461,1712,1309,1067,2365,2173,1353,662,2401,1422,319,869,2245,1141,1472,2193,249,1001,1892,1401,1683,128,2112,912,756,1314,563,1516,2444,248,1561,915,215,1355,46,384,1379,2454,789,2354,2097,1296,1223,2333,594,1197,1286,2382,898,2403,1926,168,2251,1868,1549,1408,447,504,1301,181,1105,2053,1938,1207,1916,1275,1861,2262,1970,1856,395,1404,1048,630,1850,2486,1367,78,874,416,1112,42,2276,497,145,2318,1759,908,2042,2035,1650,1349,1688,868,1342,2332,2448,472,832,2179,2382,1987,1994,1807,1296,818,2265,953,1471,534,483,2344,1711,857,1060,2360,2120,2400,1008,1733,278,1064,1073,168,2479,514,1668,682,1830,1654,2140,648,1839,922,2257,1246,1458,1184,1692,847,1225,877,150,324,793,289,2280,1700,1442,1324,1167,646,220,1106,2214,2460,1932,139,1066,1940,611,1313,1275,1647,2061,539,917,
1899,328,668,1273,1175,583,2035,2454,271,784,840,779,1572,344,2467,1184,1886,1034,934,1879,1942,2118,632,2470,573,831,24,30,1316,2111,2359,1394,413,2064,427,719,1193,1632,692,2449,774,1826,1681,360,247,1099,756,1225,731,793,2268,119,1373,737,2381,2255,2337,1701,1145,1767,2424,547,1829,371,1728,804,1305,1651,835,944,2389,260,2060,198,2034,1871,2033,1714,1848,100,1731,262,1649,1190,1622,1965,1110,2227,1226,2074,2378,1708,145,1072,855,550,1329,1555,1096,264,2312,1788,471,1685,22,1037,2002,1204,667,859,189,2470,1335,118,1686,1877,2364,2213,2001,2078,2183,2225,2079,1251,1769,2357,1510,1821,479,1227,1769,2354,512,81,498,692,612,2140,418,1298,703,2490,1708,1191,2260,474,1220,1208,1594,1049,2203,2324,1823,1137,1513,19,1066,175,1153,558,1227,2387,1265,347,108,1972,1095,2006,2498,1459,1518,1510,2463,822,992,2465,1605,31,1013,763,913,70,655,1904,1084,841,493,1515,775,2039,506,1365,983,485,356,2382,2236,548,254,928,2261,142,84,1086,1381,1332,1858,2032,1482,1958,755,1762,1535,355,1823,1303,876,306,1545,1649,1556,1952,
1133,476,1033,1606,533,1086,896,163,1129,270,1323,679,279,1189,1823,46,1552,1859,1487,1212,1513,956,603,2042,2180,2109,50,837,587,1492,802,2295,2237,1609,978,553,747,888,2147,903,1212,1087,76,1935,346,1032,1777,2251,1517,1771,785,588,600,1801,2043,1248,1424};*/

            int ElementsChanged = System.Convert.ToInt32(X.Length * PercentageSupressed);


            // Begin the total Runs

            for (int CurrentRun = 0; CurrentRun < TotalRuns; CurrentRun++)
            {
                double OldValue = 99999999999;
                int lastiteration = 0;
                chromosome tempchromosome = new chromosome(X.Length);

                //Create the chromosemes
                Console.WriteLine("Generating the Population");
                chromosome[] chromosomes = new chromosome[Population_Size];
                for (int temp = 0; temp < Population_Size; temp++)
                {
                    chromosomes[temp] = new chromosome(X.Length);
                }

                // Change the values for certain percentages of the initial chromosomes to supress 
                // High values of X
                /*Console.WriteLine("Bias Certain number of chromosomes");
                for (int tempChange = 0; tempChange < ElementsChanged; tempChange++)
                {
                    changechromosome(chromosomes[tempChange], X, PercentageSupressed);
                }*/


                //Begin the main loop for certain numner of iterations
                for (int CurrentIteration = 0; CurrentIteration < Iterations; CurrentIteration++)
                {
                    //Calculate the fitness for every member in chromosomes
                    Console.WriteLine("Calculating the Fitness for each chromosome");
                    for (int temp = 0; temp < Population_Size; temp++)
                    {
                        chromosomes[temp].Fitness = GetFitness(chromosomes[temp], ref X);

                    }

                    //Perform the Crossover
                    Console.WriteLine("Performing CrossOver");
                    Xprime = CrossOver(CrossOver_Probability, ref chromosomes, ref Xprime);

                    //Perform the mutation
                    Console.WriteLine("Performing Mutation");
                    Xprime = Mutate(Mutation_Probability, Xprime);

                    // Calcultae Fitness for new Arraylist
                    for (int temp = 0; temp < Population_Size; temp++)
                    {
                        ((chromosome)Xprime[temp]).Fitness = GetFitness(((chromosome)Xprime[temp]), ref X);
                        //Xprime.Add(chromosomes[temp]);
                    }
                    //Select Fittest and put them in X
                    Console.WriteLine("Selecting the fittest");
                    SelectFittest(chromosomes, ref Xprime, Population_Size);
                    //SelectFittestImproved(chromosomes, ref Xprime, Population_Size, PercentageFitness);
                    //Clear the ArrayList
                    Xprime.Clear();

                    double tempvalue = chromosomes[0].Fitness;
                    chromosome C1 = chromosomes[0];
                    for (int temp = 0; temp < chromosomes.Length; temp++)
                    {
                        if (tempvalue > chromosomes[temp].Fitness)
                        {
                            tempvalue = chromosomes[temp].Fitness;
                            C1 = chromosomes[temp];
                        }
                    }
                    Console.WriteLine(tempvalue);
                    for (int temp = 0; temp < C1.Bits.Length; temp++)
                    {
                        Console.Write(C1.Bits[temp]);
                    }
                    Console.WriteLine("");

                    if (OldValue > C1.Fitness)
                    {
                        lastiteration = CurrentIteration;
                        tempchromosome = C1;

                    }
                    OldValue = C1.Fitness;
                    if (CurrentIteration == Iterations - 1)
                    {
                        writehandle.Write(CurrentRun);
                        writehandle.Write(",");
                        writehandle.Write(lastiteration);
                        writehandle.Write(",");
                        writehandle.Write(tempchromosome.Fitness);
                        writehandle.Write(",,");
                        for (int temp = 0; temp < tempchromosome.Bits.Length; temp++)
                        {
                            if (tempchromosome.Bits[temp] == 1)
                            {
                                writehandle.Write(X[temp]);
                                writehandle.Write(",");
                            }
                        }
                        writehandle.WriteLine("");
                    }
                }

            }

            writehandle.Close();
        }

        //Create the function that determines whether the Crossover or Mutation should happen for 
        //that specific instance, input is probability

        static bool GetRandomConfirmation(double probability)
        {
            Random RandomNumber = new Random();
            //System.Threading.Thread.Sleep(1);
            if ((RandomNumber.NextDouble()) <= probability)
            {
                return true;
            }
            
            return false;
        }

        //Create the function that selects certain percentage of members based 
        // on removing large numbers
        static void changechromosome(chromosome currentchromosome, int[] X, double PercentageSupressed)
        {
            int ElementsSupressed = System.Convert.ToInt32(X.Length * PercentageSupressed);
            int TempElementNumber = 0;
            int TempElement = 0;
            Random random = new Random();
            //Set the chromosome Bit Values to 1
            for (int temp = 0; temp < currentchromosome.Bits.Length; temp++)
            {
                currentchromosome.Bits[temp] = 1;
            }
            TempElement = X[0];
            for (int temp2 = 0; temp2 < ElementsSupressed; temp2++)
            {
                //find the largets element in X
                for (int temp3 = 0; temp3 < X.Length; temp3++)
                {
                    if (currentchromosome.Bits[temp3] == 1)
                    {
                        if (TempElement < X[temp3])
                        {
                            TempElement = X[temp3];
                            TempElementNumber = temp3;
                        }
                    }
                }
                if (random.NextDouble() < 0.5)
                {
                    currentchromosome.Bits[TempElementNumber] = 0;
                    TempElement = X[0];
                }
                System.Threading.Thread.Sleep(1);
                

            }
        }

        // Create the function that executes the cross over of two Chromosomes
        static ArrayList CrossOver(double CrossOver_Probability, ref chromosome[] X, ref ArrayList XPrime)
        {
            ArrayList tempXprime = new ArrayList();
            chromosome C1 = X[0];
            chromosome C2 = X[0];
            chromosome[] TempChromosomes = new chromosome[X.Length];
            Random Crossing = new Random();
            int CountofXPrime = X.Length;

            for (int temp2 = 0; temp2 < X.Length; temp2++)
            {
                TempChromosomes[temp2] = new chromosome(X[0].Bits.Length);
                Swap(X[temp2], TempChromosomes[temp2]);
                XPrime.Add(TempChromosomes[temp2]);
            }

            for (int temp = 0; temp < CountofXPrime/2; temp++)
            {
                //System.Threading.Thread.Sleep(1);
                int CrossingPoint = Crossing.Next(C1.Bits.Length);
                C1 = (chromosome)XPrime[Crossing.Next(XPrime.Count)];
                XPrime.Remove(C1);
                //System.Threading.Thread.Sleep(1);
                C2 = (chromosome)XPrime[Crossing.Next(XPrime.Count)];
                XPrime.Remove(C2);

                int tempvalue = 0;

                //if (GetRandomConfirmation(CrossOver_Probability))
                if (Crossing.NextDouble() < CrossOver_Probability)
                {
                    for (int temp2 = CrossingPoint; temp2 < C1.Bits.Length; temp2++)
                    {
                        tempvalue = C1.Bits[temp2];
                        C1.Bits[temp2] = C2.Bits[temp2];
                        C2.Bits[temp2] = tempvalue;
                    }
                }

                tempXprime.Add(C1);
                tempXprime.Add(C2);
            }

            XPrime.Clear();
            XPrime = tempXprime;
            return XPrime;
        }

        // create the function that performs the mutation on each chromosome
        static ArrayList Mutate(double Mutation_Probability, ArrayList XPrime)
        {
            Random random = new Random();
            ArrayList tempXPrime = new ArrayList();

            for (int temp = 0; temp < XPrime.Count; temp++)
            {
                chromosome C1 = (chromosome)XPrime[temp];

                for (int temp2 = 0; temp2 < C1.Bits.Length; temp2++)
                {
                    if (random.NextDouble() < Mutation_Probability)
                    {
                        if (C1.Bits[temp2] == 1)
                        {
                            C1.Bits[temp2] = 0;
                        }
                        if (C1.Bits[temp2] == 0)
                        {
                            C1.Bits[temp2] = 1;
                        }
                    }
                }
                tempXPrime.Add(C1);
            }

            XPrime.Clear();
            XPrime = tempXPrime;

            return XPrime;

        }

        // Create the function that calculates the fitness
        static double GetFitness(chromosome ChromoUnderTest, ref int[] XArray)
        {
            int c = 1000;
            int S = 0;
            int X = 0;
            double YPrime = 0;
            double ReturnValue = 0;

            //Calculate YPrime for this Chromosome
            for (int temp = 0; temp < XArray.Length; temp++)
            {
                if (ChromoUnderTest.Bits[temp] == 1)
                {
                    X++;
                    YPrime = YPrime + XArray[temp];
                }
            }

            S = XArray.Length - X;
            X = XArray.Length;
            if (X != S)
            {
                YPrime = YPrime / (X - S);
            }

            if (S == X)
            {
                return  (c*Math.Abs(S));
            }
     


            //Calculate the rest of the return value
            for (int temp = 0; temp < XArray.Length; temp++)
            {
                if (ChromoUnderTest.Bits[temp] == 1)
                {
                    ReturnValue = ReturnValue + Math.Pow(XArray[temp]-YPrime,2);
                }
            }

            ReturnValue = ReturnValue + c * Math.Abs(S);



            return ReturnValue;
        }

        // Create function that selects the fittest from a chromosome Object array 
        // and a new genration ArrayList
        static void SelectFittest(chromosome[] X, ref ArrayList XPrime, int Populationsize)
        {
            ArrayList AllObjects = new ArrayList();
            ArrayList FittestObjects = new ArrayList();
            AllObjects = XPrime;
            chromosome temp_chromosome = (chromosome)AllObjects[0];

            for (int temp = 0; temp < X.Length; temp++)
            {
                AllObjects.Add(X[temp]);
            }

            double Min_Fitness = ((chromosome)AllObjects[0]).Fitness;
            Populationsize = AllObjects.Count/2;
            for (int temp = 0; temp < Populationsize; temp++)
            {
                for (int temp2 = 0; temp2 < AllObjects.Count; temp2++)
                {
                    if (Min_Fitness > ((chromosome)AllObjects[temp2]).Fitness)
                    {
                        temp_chromosome = (chromosome)AllObjects[temp2];
                        Min_Fitness = ((chromosome)AllObjects[temp2]).Fitness;
                    }

                }
                FittestObjects.Add(temp_chromosome);
                AllObjects.Remove(temp_chromosome);
                Min_Fitness = ((chromosome)AllObjects[0]).Fitness;
            }

            for (int temp = 0; temp < X.Length; temp++)
            {
                (chromosome)X[temp] = (chromosome)FittestObjects[temp];
            }

            
        }

        // Create function that selects the fittest from a chromosome Object array 
        // and a new genration ArrayList based on some percentage passed, the rest are selected randomly
        static void SelectFittestImproved(chromosome[] X, ref ArrayList XPrime, int Populationsize, double PercentageFittest)
        {
            ArrayList AllObjects = new ArrayList();
            ArrayList FittestObjects = new ArrayList();
            AllObjects = XPrime;
            chromosome temp_chromosome = (chromosome)AllObjects[0];
            Random newItem = new Random();
            

            for (int temp = 0; temp < X.Length; temp++)
            {
                AllObjects.Add(X[temp]);
            }

            double Min_Fitness = ((chromosome)AllObjects[0]).Fitness;
            Populationsize = System.Convert.ToInt32(AllObjects.Count / 2 * PercentageFittest);
            int OriginalSize = AllObjects.Count / 2;
            for (int temp = 0; temp < Populationsize; temp++)
            {
                for (int temp2 = 0; temp2 < AllObjects.Count; temp2++)
                {
                    if (Min_Fitness > ((chromosome)AllObjects[temp2]).Fitness)
                    {
                        temp_chromosome = (chromosome)AllObjects[temp2];
                        Min_Fitness = ((chromosome)AllObjects[temp2]).Fitness;
                    }

                }
                FittestObjects.Add(temp_chromosome);
                AllObjects.Remove(temp_chromosome);
                Min_Fitness = ((chromosome)AllObjects[0]).Fitness;
            }

            for (int temp = 0; temp < (OriginalSize - Populationsize); temp++)
            {
                temp_chromosome = (chromosome)AllObjects[newItem.Next(AllObjects.Count)];
                FittestObjects.Add(temp_chromosome);
                AllObjects.Remove(temp_chromosome);
               
            }

            for (int temp = 0; temp < X.Length; temp++)
            {
                (chromosome)X[temp] = (chromosome)FittestObjects[temp];
            }


        }

        //Create the function that swaps the values between two chromosomes
        static void Swap(chromosome C1, chromosome C2)
        {
            for (int temp = 0; temp < C1.Bits.Length; temp++)
            {
                C2.Bits[temp] = C1.Bits[temp];
            }
            C2.NumberOfBits = C1.NumberOfBits;
            C2.Fitness = C1.Fitness;
        }
    }

    public class chromosome
    {
        public int NumberOfBits;
        public int[] Bits;
        public double Fitness = 0;

        public chromosome(int BitsNumber)
        {
            this.NumberOfBits = BitsNumber;
            Bits = new int[this.NumberOfBits];
            PopulateBits();
        }

        private void PopulateBits()
        {
            Random randomnumber = new Random();
            Troschuetz.Random.DiscreteUniformDistribution randomnumber2 = new Troschuetz.Random.DiscreteUniformDistribution();
            System.Threading.Thread.Sleep(1);
            for (int temp = 0; temp < Bits.Length; temp++)
            {
                double temprandom = randomnumber.NextDouble();
                temprandom = randomnumber2.NextDouble();
                
                if (temprandom < 0.5)
                {
                    Bits[temp] = 0;
                }
                else
                {
                    Bits[temp] = 1;
                }

                //Console.Write(Bits[temp]);
            }
            //Console.WriteLine(" ");
        }
    }
}
