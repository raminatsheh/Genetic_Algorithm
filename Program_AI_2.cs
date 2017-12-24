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
